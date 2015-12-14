using UnityEngine;
using System.Collections;

using DG.Tweening;

public class RadioButton : MonoBehaviour
{

    public MeshRenderer ButtonFace;
    public Light Light;
    public Color OnColor;
    public Color OffColor;

    public Color LightOnColor;
    public Color LightOffColor;

    void OnMouseDown()
    { Toggle(); }

    public void Toggle()
    {
        MusicManager.Instance.Toggle();

        var playing = MusicManager.Instance.Playing;

        ButtonFace.material = new Material(ButtonFace.material);
        ButtonFace.material.EnableKeyword("_EMISSION");
        ButtonFace.material.DOColor(playing ? OnColor : OffColor, "_EmissionColor", 0.25f);
        Light.DOColor(playing ? LightOnColor : LightOffColor, 0.1f);

        UISounds.Instance.MouseClick();
    }

}
