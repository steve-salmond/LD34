using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DG.Tweening;


public class MenuWorking : MonoBehaviour
{
    public Text Title;
    public Text Delivered;
    public Text Quota;

    private int LastViable;

    void OnEnable()
    {
        var day = GameController.Instance.Day;
        // var days = GameController.Instance.MaxDays;
        var quota = GameController.Instance.PodQuota;
        var total = GameController.Instance.PodTotalCount;

        Title.text = string.Format("SHIFT {0}", day);
        Quota.text = string.Format("TODAY'S QUOTA: {0}", quota, total);
        Delivered.color = Color.white;
    }

    void Update()
    {
        var viable = GameController.Instance.PodGoodCount;
        Delivered.text = string.Format("VIABLE SPECIMENS: {0}", viable);

        if (viable != LastViable)
            OnViableChanged();

        LastViable = viable;
    }

    private void OnViableChanged()
    {
        var viable = GameController.Instance.PodGoodCount;
        var quota = GameController.Instance.PodQuota;

        if (viable == quota)
        {
            Delivered.DOColor(Color.green, 0.5f);
            DOTween.Sequence()
                .Append(Delivered.transform.DOPunchScale(Vector3.one, 0.5f, 1))
                .Append(Delivered.transform.DOScale(Vector3.one, 0.2f));
        }
    }

}
