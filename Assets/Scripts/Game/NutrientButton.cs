using UnityEngine;
using System.Collections;

using DG.Tweening;


public class NutrientButton : MonoBehaviour
{

    // Properties
    // -----------------------------------------------------

    /** The type of nutrient dispensed by this button. */
    public Nutrient Nutrient
    { get; private set; }

    /** The nutrient dispenser for this button. */
    public NutrientDispenser Dispenser
    { get; private set; }

    /** Blob's light mesh. */
    public MeshRenderer Mesh;


    // Methods
    // -----------------------------------------------------

    /** Configures this button. */
    public void SetDispenser(NutrientDispenser dispenser)
    {
        Dispenser = dispenser;
        Nutrient = Dispenser.NutrientConfig.Type;

        // Set dispenser color.
        Mesh.material = new Material(Mesh.material);
        Mesh.material.EnableKeyword("_EMISSION");
        Mesh.material.DOColor(Dispenser.NutrientConfig.Color, "_EmissionColor", 1);
    }

    /** Presses this button. */
    public void Pressed()
    {
        if (Dispenser)
            transform.DOPunchScale(Vector3.one * 0.1f, Dispenser.Cooldown);
    }


    // Unity Implementation
    // -----------------------------------------------------

    /** Dispense when player clicks on dispenser. */
    void OnMouseDown()
    {
        if (Dispenser)
            Dispenser.Use();
    }

}
