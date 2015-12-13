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
        Mesh.material.DOColor(Dispenser.NutrientConfig.OffColor, 1);
    }

    /** Presses this button. */
    public void Pressed()
    {
        if (!Dispenser)
            return;

        var cooldown = Dispenser.Cooldown;
        transform.DOPunchScale(Vector3.one * 0.1f, cooldown);
        
        DOTween.Sequence()
            .Append(Mesh.material.DOColor(Dispenser.NutrientConfig.OnColor, cooldown))
            .Append(Mesh.material.DOColor(Dispenser.NutrientConfig.OffColor, cooldown));
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
