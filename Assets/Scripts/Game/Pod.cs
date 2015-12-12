using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

public class Pod : MonoBehaviour
{

    // Properties
    // -----------------------------------------------------

    /** The list of slots on this pod. */
    public List<PodSlot> Slots;

    /** Delay between pod advancements. */
    public float StepTime;

    /** Amount to move when advancing. */
    public Vector3 StepOffset;

    /** Number of steps before delivery. */
    public int StepsToDeliver;

    /** Number of steps between delivery and death. */
    public int StepsToDeath;


    // Unity Implementation
    // -----------------------------------------------------

    /** Initialization. */
    private void Start()
    {
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

        // TODO: Deliver the pod.

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
        // Add nutrients.
        foreach (var slot in Slots)
            if (slot.Consume(blob))
            {

            }

        // Kill the blob.
        Destroy(blob.gameObject);
    }


}
