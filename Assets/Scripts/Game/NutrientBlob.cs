using UnityEngine;
using System.Collections;

using DG.Tweening;


public class NutrientBlob : MonoBehaviour
{
    /** The type of nutrient. */
    public Nutrient Nutrient;

    /** Blob's material. */
    public MeshRenderer Mesh;


    // Unity Implementation
    // -----------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        // Animate blob.
        transform.DOScale(0, 0.25f).From()
            .SetEase(Ease.InOutBounce);
        transform.DOMoveY(0, 1)
            .SetEase(Ease.InQuad)
            .OnComplete(Cleanup);

        var config = GetNutrientConfig(Nutrient);

        Mesh.material = new Material(Mesh.material);
        Mesh.material.DOColor(config.OnColor, 1);
    }


    // Private Methods
    // -----------------------------------------------------

    private void Cleanup()
    {
        Destroy(gameObject);
    }

    /** Nutrient configuration. */
    private NutrientConfig GetNutrientConfig(Nutrient nutrient)
    { return GameController.Instance.GetNutrientConfig(nutrient); }

}
