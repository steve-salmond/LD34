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

    /** Number of pods delivered. */
    public int PodDeliveredCount
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

    /** Whether player is working at the moment. */
    public bool IsPlaying
    { get { return State != GameState.GameOver; } }


    // Configuration
    // -----------------------------------------------------

    /** Maximum number of days in a game. */
    public int MaxDays;

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

    /** Score for delivering a good pod. */
    public int PodGoodScore = 100;

    /** Penalty for a bad pod. */
    public int PodBadScore = -25;

    /** Prefab for creating new pods. */
    public Pod PodPrefab;

    /** Spawn point for new pods. */
    public Transform PodSpawnPoint;


    // Pod configuration properties
    // -----------------------------------------------------

    /** Current game day progress fraction. */
    public float DayFraction
    { get { return (float) (Day - 1) / (MaxDays - 1); } }

    /** Returns the current pod spawn timing. */
    public float PodInterval
    { get { return PodIntervalCurve.Evaluate(DayFraction); } }

    /** Returns the current pod time-step. */
    public float PodStepTime
    { get { return PodStepTimeCurve.Evaluate(DayFraction); } }

    /** Returns the current pod time-step. */
    public int PodSlotCountMin
    { get { return Mathf.RoundToInt(PodSlotCountMinCurve.Evaluate(DayFraction)); } }

    /** Returns the current pod time-step. */
    public int PodSlotCountMax
    { get { return Mathf.RoundToInt(PodSlotCountMaxCurve.Evaluate(DayFraction)); } }


    // Members
    // -----------------------------------------------------

    /** Nutrient configuration lookup. */
    private Dictionary<Nutrient, NutrientConfig> _nutrientLookup;


    // Public Methods
    // -----------------------------------------------------

    /** Login to the game. */
    public void Login(string name)
    {
        // Check if already logged in.
        if (LoggedIn)
            return;

        // Set player's user name.
        UserName = name;
    }

    /** Welcome screen completed. */
    public void WelcomeCompleted()
    { SetState(GameState.Morning); }

    /** Start work for the day. */
    public void MorningCompleted()
    { SetState(GameState.Working); }

    /** Complete work for the day. */
    public void EveningCompleted()
    { SetState(GameState.Morning); }

    /** Game over screen completed. */
    public void GameOverCompleted()
    { SetState(GameState.None); }

    /** Victory screen completed. */
    public void VictoryCompleted()
    { SetState(GameState.None); }


    /** Deliver a pod. */
    public void Deliver(Pod pod)
    {
        // Can only deliver pods during working shift.
        if (State != GameState.Working)
            return;

        // Update score values.
        AddScore(pod.IsGood ? PodGoodScore : PodBadScore);

        // Pod has been delivered.
        PodDeliveredCount++;
        PodsToDeliver--;

        // Update good / bad counts.
        if (pod.IsGood)
            PodGoodCount++;
        else
            PodBadCount++;

        // Tell results screen about pod.
        ResultsScreen.Instance.Deliver(pod);
    }

    /** Add to player's score. */
    public void AddScore(int score)
    {
        Score += score;
        TotalScore += score;
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
        // Reset game state.
        SetState(GameState.None);
        Day = 0;
        UserName = "";
        Score = 0;
        TotalScore = 0;

        // First, play intro.
        yield return StartCoroutine(IntroRoutine());

        // Then, show welcome screen.
        yield return StartCoroutine(WelcomeRoutine());

        // Then, enter the daily grind.
        while (IsPlaying)
        {
            yield return StartCoroutine(MorningRoutine());
            yield return StartCoroutine(WorkRoutine());
            yield return StartCoroutine(EveningRoutine());
        }

        // Eventually the game completes.
        if (Day >= MaxDays)
            yield return StartCoroutine(VictoryRoutine());
        else
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
        yield return new WaitForSeconds(0.5f);
    }

    /** Show the welcome screen. */
    private IEnumerator WelcomeRoutine()
    {
        // Show welcome screen.
        MenuController.Instance.ShowWelcomeScreen();

        // Wait till player completes welcome screen.
        while (State == GameState.Intro)
            yield return 0;
    }

    /** Handle the morning before a day's shift. */
    private IEnumerator MorningRoutine()
    {
        SetState(GameState.Morning);

        // Set up today's shift.
        Day = Day + 1;
        PodCount = 0;
        PodDeliveredCount = 0;
        PodGoodCount = 0;
        PodBadCount = 0;
        PodTotalCount = Mathf.RoundToInt(PodTotalCurve.Evaluate(DayFraction));
        PodsToDeliver = PodTotalCount;
        PodQuota = Mathf.RoundToInt(PodQuotaCurve.Evaluate(DayFraction));

        // Look at monitor.
        CameraController.Instance.LookAtMonitor();
        MenuController.Instance.ShowMorningScreen();

        // Wait till player completes briefing.
        while (State == GameState.Morning)
            yield return 0;
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

        // Wait a bit for final pod delivery.
        yield return new WaitForSeconds(PodInterval);
        yield return new WaitForSeconds(1);
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
        MenuController.Instance.ShowEveningScreen();

        // Check if player has failed to meet today's pod quota.
        if (PodGoodCount < PodQuota)
            SetState(GameState.GameOver);
        else if (Day == MaxDays)
            SetState(GameState.GameOver);
        else
        {
            // Wait till player completes evening briefing.
            while (State == GameState.Evening)
                yield return 0;

            // Indicate that one night has passed.
            UIScreenFlash.Instance.Flash(Color.black, 1, 1);
            yield return new WaitForSeconds(1);
        }
    }

    /** Handle the game over condition. */
    private IEnumerator GameOverRoutine()
    {
        // Look at monitor.
        CameraController.Instance.LookAtMonitor();
        MenuController.Instance.ShowGameOverScreen();

        // Wait till player completes game over.
        while (State == GameState.GameOver)
            yield return 0;

        // Start a new game.
        StartCoroutine(GameRoutine());
    }

    /** Handle victory condition. */
    private IEnumerator VictoryRoutine()
    {
        // Look at monitor.
        CameraController.Instance.LookAtMonitor();
        MenuController.Instance.ShowVictoryScreen();

        // Wait till player completes game over.
        while (State == GameState.GameOver)
            yield return 0;

        // Start a new game.
        StartCoroutine(GameRoutine());
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
