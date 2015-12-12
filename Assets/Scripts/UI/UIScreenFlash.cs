using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class UIScreenFlash : Singleton<UIScreenFlash>
{

    public Graphic Graphic
    { get; private set; }

    private void Awake()
    {
        Graphic = GetComponent<Graphic>();
    }

    public void Flash(Color c, float duration = 0.25f)
    {
        Graphic.DOBlendableColor(c, duration);

        // TODO: Revert to transparent.
    }
    
	
}
