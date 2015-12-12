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

    /** Button that controls dispensing. */
    public string Key;

    /** Blob's light mesh. */
    public MeshRenderer LightMesh;

    /** Whether nutrient is being dispensed. */
    public bool Dispensing
    { get; private set; }

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
        // Set dispenser color.
        LightMesh.material = new Material(LightMesh.material);
        LightMesh.material.EnableKeyword("_EMISSION");
        LightMesh.material.DOColor(NutrientConfig.Color, "_EmissionColor", 1);

        // Fire up the game control routine.
        StartCoroutine(UpdateRoutine());
    }

    /** Dispense when player clicks on dispenser. */
    void OnMouseDown()
    {
        var working = GameController.Instance.IsWorking;
        if (working && !Dispensing)
            StartCoroutine(Dispense());
    }


    // Coroutines
    // -----------------------------------------------------

    /** Update the pod. */
    private IEnumerator UpdateRoutine()
    {
        while (true)
        {
            var working = GameController.Instance.IsWorking;
            if (working && Input.GetKeyDown(Key) && !Dispensing)
                yield return StartCoroutine(Dispense());

            yield return 0;
        }
    }

    /** Dispense nutrients. */
    private IEnumerator Dispense()
    {
        Dispensing = true;

        transform.DOPunchScale(Vector3.one * 0.1f, Cooldown);

        // Emit a blob of nutrients.
        var blob = Instantiate<NutrientBlob>(NutrientConfig.BlobPrefab);
        blob.transform.position = Emitter.position;
        blob.Nutrient = Nutrient;

        // Wait until cooldown expires.
        yield return new WaitForSeconds(Cooldown);

        Dispensing = false;
    }

}
