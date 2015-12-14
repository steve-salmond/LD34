using UnityEngine;
using UnityEngine.UI;
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

    /** Buttons' label. */
    public Text Label
    { get; private set; }


    // Methods
    // -----------------------------------------------------

    /** Configures this button. */
    public void SetDispenser(NutrientDispenser dispenser)
    {
        Dispenser = dispenser;
        Nutrient = Dispenser.NutrientConfig.Type;

        Label = GetComponentInChildren<Text>();
        Label.text = Dispenser.Key;

        // Set dispenser color.
        Mesh.material = new Material(Mesh.material);
        Mesh.material.EnableKeyword("_EMISSION");
        Mesh.material.DOColor(Dispenser.NutrientConfig.OffColor, "_EmissionColor", 1);
    }

    /** Presses this button. */
    public void Pressed()
    {
        if (!Dispenser)
            return;

        // Make a keypress sound.
        UISounds.Instance.KeyPress();

        var cooldown = Dispenser.Cooldown;
        transform.DOPunchScale(Vector3.one * 0.1f, cooldown);
        
        DOTween.Sequence()
            .Append(Mesh.material.DOColor(Dispenser.NutrientConfig.OnColor, "_EmissionColor", cooldown * 0.25f))
            .Append(Mesh.material.DOColor(Dispenser.NutrientConfig.OffColor, "_EmissionColor", cooldown * 0.25f));
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
