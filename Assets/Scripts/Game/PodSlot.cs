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

    /** Score for good slot state. */
    public int GoodScore;

    /** Score for bad slot state. */
    public int BadScore;

    /** Score for empty slot state. */
    public int EmptyScore;

    /** Whether slot is open. */
    public bool Open
    { get; private set; }

    /** Whether slot is empty. */
    public bool IsEmpty
    { get { return Current == Nutrient.None; } }

    /** Whether slot is full. */
    public bool IsFull
    { get { return Current != Nutrient.None; } }

    /** Whether slot is filled correctly. */
    public bool IsGood
    { get { return IsFull && Current == Requested; } }

    /** Whether slot is filled incorrectly. */
    public bool IsBad
    { get { return IsFull && Current != Requested; } }

    /** Return the current score for this pod. */
    public int Score
    {
        get
        {
            if (IsGood)
                return GoodScore;
            else if (IsBad)
                return BadScore;
            else
                return EmptyScore;
        }
    }

    /** Return the current slot color. */
    public Color Color
    { get { return GetNutrientConfig(Current).OnColor; } }


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

    /** Open the slot. */
    public void SetOpen(bool open)
    {
        Open = open;
        SetRequested(Requested);
    }

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

        // Update player's score.
        GameController.Instance.AddScore(Score);

        return true;
    }


    // Private Methods
    // -----------------------------------------------------

    /** Set the required nutrient. */
    private void SetRequested(Nutrient nutrient)
    {
        Requested = nutrient;

        var config = GetNutrientConfig(nutrient);
        var color = Open ? config.OnColor : Color.Lerp(config.OffColor, Color.black, 0.2f);
        SetEmissionColor(RequestMesh, color);
    }

    /** Set the required nutrient. */
    private void SetCurrent(Nutrient nutrient)
    {
        Current = nutrient;

        RequestMesh.enabled = false;
        CurrentMesh.enabled = false;

        // SetEmissionColor(RequestMesh, GetNutrientConfig(Requested).OffColor, 0.5f);
        // SetEmissionColor(CurrentMesh, GetNutrientConfig(Current).OffColor, 0.5f);

        var lightColor = IsGood ? Color.green : Color.red;
        SetEmissionColor(LightMesh, lightColor);
        
        // if (IsBad)
        //    UIScreenFlash.Instance.Flash(lightColor);
    }

    private void SetEmissionColor(MeshRenderer mesh, Color c, float a = 1)
    {
        mesh.material = new Material(mesh.material);
        mesh.material.EnableKeyword("_EMISSION");
        mesh.material.DOColor(new Color(c.r * a, c.g * a, c.b * a, a), "_EmissionColor", 0.5f);
    }

    /** Nutrient configuration. */
    private NutrientConfig GetNutrientConfig(Nutrient nutrient)
    { return GameController.Instance.GetNutrientConfig(nutrient); }

}
