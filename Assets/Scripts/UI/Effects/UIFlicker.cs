using UnityEngine;
using UnityEngine.UI;

public class UIFlicker : MonoBehaviour
{
    public Vector2 AlphaRange;

    public Graphic Graphic
    { get; private set; }

    private void Awake()
    {
        Graphic = GetComponent<Graphic>();
    }

    private void Update()
    {
        var c = Graphic.color;
        var alpha = Random.Range(AlphaRange.x, AlphaRange.y);
        Graphic.color = new Color(c.r, c.g, c.b, alpha);
    }

}
