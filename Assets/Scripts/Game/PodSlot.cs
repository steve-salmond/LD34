﻿using UnityEngine;
using System.Collections;
using DG.Tweening;


public class PodSlot : MonoBehaviour
{

    // Properties
    // -----------------------------------------------------

    /** The nutrient currently in this slot. */
    public Nutrient Current;

    /** THe nutrient required by this slot. */
    public Nutrient Requested;

    /** Renderer that displays requested nutrient. */
    public MeshRenderer RequestMesh;

    /** Renderer that displays current nutrient. */
    public MeshRenderer CurrentMesh;

    /** Renderer for the 'good/bad' light. */
    public MeshRenderer LightMesh;


    // Unity Implementation
    // -----------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        // Pick a random required nutrient.
        var first = (int) Nutrient.Oxygen;
        var last = (int) Nutrient.Hydrogen;
        var request = (Nutrient) Random.Range(first, last + 1);
        SetRequested(request);
    }


    // Public Methods
    // -----------------------------------------------------

    /** Whether this slot can consume the given nutrient blob. */
    public bool CanConsume(NutrientBlob blob)
    {
        // Check if we already have a nutrient.
        if (Current != Nutrient.None)
            return false;

        // Always accept a blob even if it's the wrong color.
        return true;

        // If not, check if blob is the right type.
        // return blob.Nutrient == NutrientRequested;
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
        Requested = nutrient;
        SetEmissionColor(RequestMesh, GetNutrientConfig(nutrient).Color);
    }

    /** Set the required nutrient. */
    private void SetCurrent(Nutrient nutrient)
    {
        Current = nutrient;
        SetEmissionColor(CurrentMesh, GetNutrientConfig(nutrient).Color);
        SetEmissionColor(LightMesh, Current == Requested ? Color.green : Color.red);
    }

    private void SetEmissionColor(MeshRenderer mesh, Color color)
    {
        mesh.material = new Material(mesh.material);
        mesh.material.EnableKeyword("_EMISSION");
        mesh.material.DOColor(color, "_EmissionColor", 0.25f);
    }

    /** Nutrient configuration. */
    private NutrientConfig GetNutrientConfig(Nutrient nutrient)
    { return GameController.Instance.GetNutrientConfig(nutrient); }

}
