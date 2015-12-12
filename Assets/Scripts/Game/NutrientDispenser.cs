using UnityEngine;
using System.Collections;

using DG.Tweening;

public class NutrientDispenser : MonoBehaviour
{

    // Properties
    // -----------------------------------------------------

    /** The type of nutrient dispensed. */
    public Nutrient Nutrient;

    /** Time before dispenser can be used again (seconds). */
    public float Cooldown;

    /** Location to emit from. */
    public Transform Emitter;


    // Private Properties
    // -----------------------------------------------------

    /** Nutrient configuration. */
    private NutrientConfig NutrientConfig
    { get { return GameController.Instance.GetNutrientConfig(Nutrient); } }


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
            var working = GameController.Instance.IsWorking;
            if (working && Input.GetKeyDown(NutrientConfig.Key))
                yield return StartCoroutine(Dispense());

            yield return 0;
        }
    }

    /** Dispense nutrients. */
    private IEnumerator Dispense()
    {
        transform.DOPunchScale(Vector3.one * 0.1f, Cooldown);

        // Emit a blob of nutrients.
        var blob = Instantiate<NutrientBlob>(NutrientConfig.BlobPrefab);
        blob.transform.position = Emitter.position;
        blob.Nutrient = Nutrient;

        // Wait until cooldown expires.
        yield return new WaitForSeconds(Cooldown);
    }

}
