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

    /** Nutrient button. */
    public NutrientButton Button;

    /** Whether nutrient is being dispensed. */
    public bool Dispensing
    { get; private set; }

    /** Nutrient configuration. */
    public NutrientConfig NutrientConfig
    { get { return GameController.Instance.GetNutrientConfig(Nutrient); } }



    // Public Methods
    // -----------------------------------------------------

    /** Attempt to dispense. */
    public bool Use()
    {
        var working = GameController.Instance.IsWorking;
        var canUse = working && !Dispensing;
        if (canUse)
            StartCoroutine(Dispense());

        return canUse;
    }


    // Unity Implementation
    // -----------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        // Set dispenser color.
        LightMesh.material = new Material(LightMesh.material);
        LightMesh.material.EnableKeyword("_EMISSION");
        LightMesh.material.DOColor(NutrientConfig.OffColor, "_EmissionColor", 1);

        // Configure button.
        if (Button)
            Button.SetDispenser(this);

        // Fire up the game control routine.
        StartCoroutine(UpdateRoutine());
    }

    /** Dispense when player clicks on dispenser. */
    void OnMouseDown()
        { Use(); }


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

        // Press the button.
        if (Button)
            Button.Pressed();

        // Animate dispenser.
        transform.DOPunchScale(Vector3.one * 0.1f, Cooldown);

        // Animate color.
        DOTween.Sequence()
            .Append(LightMesh.material.DOColor(NutrientConfig.OnColor, "_EmissionColor", Cooldown * 0.25f))
            .Append(LightMesh.material.DOColor(NutrientConfig.OffColor, "_EmissionColor", Cooldown * 0.25f));

        // Emit a blob of nutrients.
        var blob = Instantiate<NutrientBlob>(NutrientConfig.BlobPrefab);
        blob.transform.position = Emitter.position;
        blob.Nutrient = Nutrient;

        // Wait until cooldown expires.
        yield return new WaitForSeconds(Cooldown);

        Dispensing = false;
    }

}
