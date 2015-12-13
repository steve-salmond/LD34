using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DG.Tweening;


public class MenuWorking : MonoBehaviour
{
    public Text Title;
    public Text Delivered;
    public Text Quota;

    void OnEnable()
    {
        var day = GameController.Instance.Day;
        var days = GameController.Instance.MaxDays;
        var quota = GameController.Instance.PodQuota;
        var total = GameController.Instance.PodTotalCount;

        Title.text = string.Format("SHIFT {0} of {1}", day, days);
        Quota.text = string.Format("TODAY'S QUOTA - {0}", quota, total);
    }

    void Update()
    {
        var viable = GameController.Instance.PodGoodCount;
        Delivered.text = string.Format("VIABLE SPECIMENS - {0}", viable);
    }
}
