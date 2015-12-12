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


    // Unity Implementation
    // -----------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        // Fire up the game control routine.
        StartCoroutine(UpdateRoutine());
    }


    // Coroutines
    // -----------------------------------------------------

    /** Update the pod. */
    private IEnumerator UpdateRoutine()
    {
        while (true)
        {
            transform.DOMove(StepOffset, StepTime).SetRelative();
            yield return new WaitForSeconds(StepTime);
        }
    }




}
