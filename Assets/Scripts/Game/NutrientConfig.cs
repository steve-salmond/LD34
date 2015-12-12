using UnityEngine;
using System.Collections;

public class NutrientConfig : MonoBehaviour
{

    /** The type of nutrient. */
    public Nutrient Type;

    /** Nutrient's color. */
    public Color Color;

    /** Prefab for dispensed nutrient blobs. */
    public NutrientBlob BlobPrefab;

    /** Key that controls dispensing. */
    public KeyCode Key;


}
