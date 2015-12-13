using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DG.Tweening;

public class UIButton : MonoBehaviour
{

    public void OnClicked()
    {
        var scale = transform.localScale.x;
        transform.DOPunchScale(Vector3.one * scale * 1.01f, 0.25f, 1).SetEase(Ease.OutBounce);
    }
}
