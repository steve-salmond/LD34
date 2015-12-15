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

    public void Flash(Color c, float duration = 0.1f, float alpha = 0.1f)
    {
        Graphic.color = new Color(c.r, c.g, c.b, 0);

        DOTween.Sequence()
            .Append(Graphic.DOFade(alpha, duration))
            .Append(Graphic.DOFade(0, duration));
    }

    public void FadeIn(Color c, float duration = 1, float delay = 0, float alpha = 1)
    {
        Graphic.color = new Color(c.r, c.g, c.b, alpha);
        DOTween.Sequence()
            .AppendInterval(delay)
            .Append(Graphic.DOFade(0, duration));
    }

    public void FadeOut(Color c, float duration = 1, float delay = 0, float alpha = 1)
    {
        Graphic.color = new Color(c.r, c.g, c.b, 0);
        DOTween.Sequence()
            .AppendInterval(delay)
            .Append(Graphic.DOFade(alpha, duration));
    }

}
