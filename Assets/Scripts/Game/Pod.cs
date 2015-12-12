using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using DG.Tweening;

public class Pod : MonoBehaviour
{

    // Properties
    // -----------------------------------------------------

    /** The list of slots on this pod. */
    public List<PodSlot> Slots
    { get; private set; }

    /** Delay between pod advancements. */
    public float StepTime;

    /** Amount to move when advancing. */
    public Vector3 StepOffset;

    /** Number of steps before delivery. */
    public int StepsToDeliver;

    /** Number of steps between delivery and death. */
    public int StepsToDeath;

    /** Pod capsule. */
    public Transform Capsule;

    /** Return the current score for this pod. */
    public int Score
    { get { return Slots.Sum(slot => slot.Score); } }



    // Unity Implementation
    // -----------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        // Get game controller.
        var controller = GameController.Instance;

        // Configure the pod.
        StepTime = controller.PodStepTime;

        // Activate slots.
        int min = controller.PodSlotCountMin;
        int max = controller.PodSlotCountMax;
        int count = Random.Range(min, max + 1);
        var slots = GetComponentsInChildren<PodSlot>();
        foreach (var slot in slots)
        {
            slot.gameObject.SetActive(count > 0);
            count--;
        }

        // Get list of active slots.
        Slots = slots.Where(slot => slot.gameObject.activeSelf).ToList();

        // Fire up the pod control routine.
        StartCoroutine(UpdateRoutine());
    }

    /** Handle objects hitting the pod. */
    private void OnTriggerEnter(Collider other)
    {
        var blob = other.GetComponent<NutrientBlob>();
        if (blob == null)
            return;

        Consume(blob);
    }


    // Coroutines
    // -----------------------------------------------------

    /** Update the pod. */
    private IEnumerator UpdateRoutine()
    {
        // Advance the pod until it's time to deliver.
        for (int i = 0; i < StepsToDeliver; i++)
            yield return StartCoroutine(Advance());

        // Deliver the pod.
        Deliver();

        // Advance until it's time to die.
        for (int i = 0; i < StepsToDeath; i++)
            yield return StartCoroutine(Advance());

        // Kill the pod.
        Destroy(gameObject);
    }

    /** Advances the pod by one step. */
    private IEnumerator Advance()
    {
        transform.DOMove(StepOffset, StepTime).SetRelative();
        yield return new WaitForSeconds(StepTime);
    }


    // Private Methods
    // -----------------------------------------------------

    /** Consume a nutrient blob. */
    private void Consume(NutrientBlob blob)
    {
        // Ask each slot if it wants the nutrient.
        foreach (var slot in Slots)
            if (slot.Consume(blob))
            {
                transform.DOPunchScale(Vector3.one * 0.05f, 0.5f);
                break;
            }

        // Kill the blob.
        Destroy(blob.gameObject);
    }

    /** Deliver the pod. */
    private void Deliver()
    {
        // Determine if pod meets the grade.
        var passes = (Slots.Count(slot => slot.IsGood) / Slots.Count) >= 0.5f;

        // Update game score.
        GameController.Instance.AddScore(Score);

        // Shake the pod a bit.
        transform.DOPunchScale(Vector3.one * 0.05f, 0.5f);

        // Move pod up or down depending on whether it passes.
        if (passes)
            Capsule.DOMoveY(10, 0.5f).SetRelative();
        else
            Capsule.DOMoveY(-10, 0.5f).SetRelative();

    }


}
