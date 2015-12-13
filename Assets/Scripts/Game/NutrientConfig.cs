using UnityEngine;
using System.Collections;

public class NutrientConfig : MonoBehaviour
{

    /** The type of nutrient. */
    public Nutrient Type;

    /** Nutrient's off color. */
    public Color OffColor;

    /** Nutrient's on color. */
    public Color OnColor;

    /** Prefab for dispensed nutrient blobs. */
    public NutrientBlob BlobPrefab;

}
