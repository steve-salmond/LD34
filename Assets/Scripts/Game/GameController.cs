using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameController : Singleton<GameController>
{

    // Properties
    // -----------------------------------------------------

    /** The game's current state. */
    public GameState State
    { get; private set; }

    /** Player's username. */
    public string UserName
    { get; private set; }

    /** Whether player is logged in. */
    public bool LoggedIn
    { get { return !string.IsNullOrEmpty(UserName); } }

    /** The current work day. */
    public int Day
    { get; private set; }

    /** Pods spawned today. */
    public int PodCount
    { get; private set; }

    /** Total pod count for today's shift (seconds). */
    public int PodTotalCount
    { get; private set; }

    /** Number of pods yet to be delivered today. */
    public int PodsToDeliver
    { get; private set; }

    /** Number of pods successfully delivered. */
    public int PodGoodCount
    { get; private set; }

    /** Number of pods unsuccessfully delivered. */
    public int PodBadCount
    { get; private set; }

    /** Pod quota for the current day. */
    public int PodQuota
    { get; private set; }

    /** Player's score for today. */
    public int Score
    { get; private set; }
    
    /** Player's total score to date. */
    public int TotalScore
    { get; private set; }

    /** Whether player is working at the moment. */
    public bool IsWorking
    { get { return State == GameState.Working; } }


    // Configuration
    // -----------------------------------------------------

    /** Curve indicating the initial time allocated on a given day. */
    public AnimationCurve PodTotalCurve;

    /** Curve indicating the initial pod count per day. */
    public AnimationCurve PodQuotaCurve;

    /** Curve indicating the interval between successive pods on a given day. */
    public AnimationCurve PodIntervalCurve;

    /** Curve indicating the time-step speed of pods on a given day. */
    public AnimationCurve PodStepTimeCurve;

    /** Curve indicating the minimum pod slot count on a given day. */
    public AnimationCurve PodSlotCountMinCurve;

    /** Curve indicating the maximum pod slot count on a given day. */
    public AnimationCurve PodSlotCountMaxCurve;

    /** Prefab for creating new pods. */
    public Pod PodPrefab;

    /** Spawn point for new pods. */
    public Transform PodSpawnPoint;


    // Pod configuration properties
    // -----------------------------------------------------

    /** Returns the current pod spawn timing. */
    public float PodInterval
    { get { return PodIntervalCurve.Evaluate(Day); } }

    /** Returns the current pod time-step. */
    public float PodStepTime
    { get { return PodStepTimeCurve.Evaluate(Day); } }

    /** Returns the current pod time-step. */
    public int PodSlotCountMin
    { get { return Mathf.RoundToInt(PodSlotCountMinCurve.Evaluate(Day)); } }

    /** Returns the current pod time-step. */
    public int PodSlotCountMax
    { get { return Mathf.RoundToInt(PodSlotCountMaxCurve.Evaluate(Day)); } }


    // Members
    // -----------------------------------------------------

    /** Nutrient configuration lookup. */
    private Dictionary<Nutrient, NutrientConfig> _nutrientLookup;


    // Public Methods
    // -----------------------------------------------------

    /** Login to the game. */
    public void Login(string name)
    {
        UserName = name;
    }

    /** Adds some score to the game. */
    public void Deliver(Pod pod)
    {
        // Can only deliver pods during working shift.
        if (State != GameState.Working)
            return;

        // Update score values.
        var score = pod.Score;
        Score += score;
        TotalScore += score;

        // Pod has been delivered.
        PodsToDeliver--;

        // Update good / bad counts.
        if (pod.IsGood)
            PodGoodCount++;
        else
            PodBadCount++;
    }

    /** Nutrient configuration. */
    public NutrientConfig GetNutrientConfig(Nutrient type)
    {
        // Populate lookup on demand.
        if (_nutrientLookup == null)
        {
            var configs = GetComponentsInChildren<NutrientConfig>();
            _nutrientLookup = new Dictionary<Nutrient, NutrientConfig>();
            foreach (var config in configs)
                _nutrientLookup[config.Type] = config;
        }

        // Perform nutrient lookup.
        return _nutrientLookup[type];
    }


    // Unity Implementation
    // -----------------------------------------------------

    /** Pre-initialization. */
    private void Awake()
    {
        // Reset the current day.
        Day = 0;

    }

    /** Initialization. */
    private void Start()
    {
        // Fire up the game control routine.
        StartCoroutine(GameRoutine());
    }

    
    // Coroutines
    // -----------------------------------------------------

    /** Update the game. */
    private IEnumerator GameRoutine()
    {
        // First, play intro.
        yield return StartCoroutine(IntroRoutine());

        // Then, enter the daily grind.
        while (State != GameState.GameOver)
        {
            yield return StartCoroutine(MorningRoutine());
            yield return StartCoroutine(WorkRoutine());
            yield return StartCoroutine(EveningRoutine());
        }

        // Eventually the game ends.
        yield return StartCoroutine(GameOverRoutine());
    }

    /** Play the game intro. */
    private IEnumerator IntroRoutine()
    {
        SetState(GameState.Intro);

        // Look at monitor.
        CameraController.Instance.LookAtMonitor();
        MenuController.Instance.ShowLoginScreen();

        // Wait for player to log in.
        while (!LoggedIn)
            yield return 0;

        // Delay a bit after login.
        yield return new WaitForSeconds(1);
    }

    /** Handle the morning before a day's shift. */
    private IEnumerator MorningRoutine()
    {
        SetState(GameState.Morning);

        // Look at monitor.
        CameraController.Instance.LookAtMonitor();

        // Set up today's shift.
        Day = Day + 1;
        PodCount = 0;
        PodGoodCount = 0;
        PodBadCount = 0;
        PodTotalCount = Mathf.RoundToInt(PodTotalCurve.Evaluate(Day));
        PodsToDeliver = PodTotalCount;
        PodQuota = Mathf.RoundToInt(PodQuotaCurve.Evaluate(Day));

        yield return new WaitForSeconds(1);
    }

    /** Handle a day's shift. */
    private IEnumerator WorkRoutine()
    {
        SetState(GameState.Working);

        // Look at working area.
        CameraController.Instance.LookAtWorkArea();
        MenuController.Instance.ShowWorkingScreen();

        // Start coroutine for spawning pods.
        StartCoroutine(SpawnPodRoutine());

        // Wait until all pods have been delivered.
        while (PodsToDeliver > 0)
            yield return 0;

        // Wait a bit for final pod delivery
        yield return new WaitForSeconds(PodInterval * 2);
    }

    /** Handle the end of a day's shift. */
    private IEnumerator SpawnPodRoutine()
    {
        while (PodCount < PodTotalCount)
        {
            Instantiate(PodPrefab, PodSpawnPoint.position, Quaternion.identity);
            PodCount++;
            yield return new WaitForSeconds(PodInterval);
        }
    }

    /** Handle the end of a day's shift. */
    private IEnumerator EveningRoutine()
    {
        SetState(GameState.Evening);

        // Look at monitor.
        CameraController.Instance.LookAtMonitor();

        // Check if player has failed to meet today's pod quota.
        if (PodGoodCount < PodQuota)
            SetState(GameState.GameOver);
        else
            yield return new WaitForSeconds(5);

        yield return 0;
    }

    /** Handle the end of the game. */
    private IEnumerator GameOverRoutine()
    {
        SetState(GameState.GameOver);

        // Look at monitor.
        CameraController.Instance.LookAtMonitor();

        yield return new WaitForSeconds(5);
        yield return 0;
    }


    // Implementation
    // -----------------------------------------------------

    /** Change the current game state. */
    private void SetState(GameState value)
    {
        // Check if state has changed.
        if (State == value)
            return;

        // Assign the new state.
        State = value;
    }
    
}
