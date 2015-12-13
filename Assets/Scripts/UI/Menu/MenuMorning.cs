using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DG.Tweening;


public class MenuMorning : MonoBehaviour
{

    public Text Title;
    public Text Message;

    void OnEnable()
    {
        var name = GameController.Instance.UserName;
        var day = GameController.Instance.Day;
        var days = GameController.Instance.MaxDays;
        var quota = GameController.Instance.PodQuota;

        Title.text = string.Format("SHIFT {0} of {1}", day, days);
        Message.text = string.Format("Good morning {0}! Wake up and smell the nutrients!\n" 
            + "Today your delivery quota will be {1} viable specimens.", name, quota);
    }

    public void OnOK()
    {
        GameController.Instance.MorningCompleted();
    }

}
