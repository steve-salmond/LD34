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

    /** The current work day. */
    public int Day
    { get; private set; }

    /** Duration for the current day's shift (seconds). */
    public float Duration
    { get; private set; }

    /** Score quota for the current day. */
    public int Quota
    { get; private set; }

    /** Player's score for today. */
    public int Score
    { get; private set; }

    /** Time left in the current day's shift (seconds). */
    public float TimeLeft
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
    public AnimationCurve DurationCurve;

    /** Curve indicating the initial time allocated on a given day. */
    public AnimationCurve QuotaCurve;

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

    /** Adds some score to the game. */
    public void AddScore(int value)
    {
        // Can only add score during working shift.
        if (State != GameState.Working)
            return;

        // Update score values.
        Score += value;
        TotalScore += value;
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
        yield return new WaitForSeconds(1);
    }

    /** Handle the morning before a day's shift. */
    private IEnumerator MorningRoutine()
    {
        SetState(GameState.Morning);

        // Set up today's shift.
        Day = Day + 1;
        Quota = Mathf.RoundToInt(QuotaCurve.Evaluate(Day));
        Duration = Mathf.RoundToInt(DurationCurve.Evaluate(Day));

        yield return new WaitForSeconds(1);
    }

    /** Handle a day's shift. */
    private IEnumerator WorkRoutine()
    {
        SetState(GameState.Working);

        // Determine when today's shift will end.
        var endOfDay = Time.time + Duration;

        // Start coroutine for spawning pods.
        StartCoroutine(SpawnPodRoutine());

        // Wait until the day is over.
        while (Time.time < endOfDay)
        {
            // Update the current time left.
            var t = Time.time;
            TimeLeft = Mathf.Max(0, endOfDay - t);

            // Wait until the next frame.
            yield return 0;
        }

        // Day is over.
        TimeLeft = 0;

        yield return 0;
    }

    /** Handle the end of a day's shift. */
    private IEnumerator SpawnPodRoutine()
    {
        while (State == GameState.Working)
        {
            Instantiate(PodPrefab);
            yield return new WaitForSeconds(PodInterval);
        }
    }

    /** Handle the end of a day's shift. */
    private IEnumerator EveningRoutine()
    {
        SetState(GameState.Evening);
        yield return new WaitForSeconds(5);

        // Check if player has failed to meet today's quota.
        if (Score < Quota)
            SetState(GameState.GameOver);

        yield return 0;
    }

    /** Handle the end of the game. */
    private IEnumerator GameOverRoutine()
    {
        SetState(GameState.GameOver);
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
