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

    public void Flash(Color c, float alpha = 0.1f, float duration = 0.1f)
    {
        Graphic.color = new Color(c.r, c.g, c.b, 0);

        DOTween.Sequence()
            .Append(Graphic.DOFade(alpha, duration))
            .Append(Graphic.DOFade(0, duration));
    }
    
	
}
