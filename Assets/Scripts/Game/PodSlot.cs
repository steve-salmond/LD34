using UnityEngine;
using System.Collections;
using DG.Tweening;


public class PodSlot : MonoBehaviour
{

    // Properties
    // -----------------------------------------------------

    /** The nutrient currently in this slot. */
    public Nutrient NutrientCurrent;

    /** THe nutrient required by this slot. */
    public Nutrient NutrientRequested;

    /** Renderer that displays requested nutrient. */
    public MeshRenderer RequestMesh;

    /** Renderer that displays current nutrient. */
    public MeshRenderer CurrentMesh;


    // Unity Implementation
    // -----------------------------------------------------

    /** Initialization. */
    private void Start()
    {

        /*
        // Pick a random required nutrient.
        var first = (int) Nutrient.Oxygen;
        var last = (int) Nutrient.Sodium;
        var request = (Nutrient) Random.Range(first, last + 1);
        SetRequested(request);
        */

        SetRequested(Nutrient.Oxygen);
    }


    // Public Methods
    // -----------------------------------------------------

    /** Whether this slot can consume the given nutrient blob. */
    public bool CanConsume(NutrientBlob blob)
    {
        // Check if we already have a nutrient.
        if (NutrientCurrent != Nutrient.None)
            return false;

        // If not, check if blob is the right type.
        return blob.Nutrient == NutrientRequested;
    }

    /** Try to consume a nutrient blob. */
    public bool Consume(NutrientBlob blob)
    {
        // Check if we can consume this blob.
        if (!CanConsume(blob))
            return false;

        // Consume the blob.
        SetCurrent(blob.Nutrient);

        return true;
    }


    // Private Methods
    // -----------------------------------------------------

    /** Set the required nutrient. */
    private void SetRequested(Nutrient nutrient)
    {
        NutrientRequested = nutrient;

        var config = GetNutrientConfig(nutrient);
        RequestMesh.material = new Material(RequestMesh.material);
        RequestMesh.material.EnableKeyword("_EMISSION");
        RequestMesh.material.DOColor(config.Color, "_EmissionColor", 0.25f);
    }

    /** Set the required nutrient. */
    private void SetCurrent(Nutrient nutrient)
    {
        NutrientCurrent = nutrient;

        var config = GetNutrientConfig(nutrient);
        CurrentMesh.material = new Material(CurrentMesh.material);
        CurrentMesh.material.EnableKeyword("_EMISSION");
        CurrentMesh.material.DOColor(config.Color, "_EmissionColor", 0.25f);
    }

    /** Nutrient configuration. */
    private NutrientConfig GetNutrientConfig(Nutrient nutrient)
    { return GameController.Instance.GetNutrientConfig(nutrient); }

}
