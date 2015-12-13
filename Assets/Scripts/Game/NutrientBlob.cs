using UnityEngine;
using System.Collections;

using DG.Tweening;


public class NutrientBlob : MonoBehaviour
{
    /** The type of nutrient. */
    public Nutrient Nutrient;

    /** Blob's material. */
    public MeshRenderer Mesh;

    /** Blob's light. */
    public Light Light;

    /** Blob's trail. */
    public ParticleSystem Trail;


    // Unity Implementation
    // -----------------------------------------------------

    /** Initialization. */
    private void Start()
    {
        // Animate blob.
        transform.DOScale(0, 0.25f).From()
            .SetEase(Ease.InOutBounce);
        transform.DOMoveY(-5, 1.5f)
            .SetEase(Ease.InQuad)
            .OnComplete(Cleanup);

        var config = GetNutrientConfig(Nutrient);

        Mesh.material = new Material(Mesh.material);
        Mesh.material.EnableKeyword("_EMISSION");
        Mesh.material.DOColor(config.OnColor, "_EmissionColor", 1);

        Trail.startColor = config.OnColor;
        Light.DOColor(config.OnColor, 0.5f);
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
