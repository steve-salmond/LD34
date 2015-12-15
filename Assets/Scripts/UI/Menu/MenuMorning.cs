using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using DG.Tweening;


public class MenuMorning : MonoBehaviour
{

    public Text Title;
    public Text Message;

    private bool okClicked;

    public List<string> DailyMessages;

    void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(MenuRoutine());
    }

    IEnumerator MenuRoutine()
    {
        var name = GameController.Instance.UserName;
        var day = GameController.Instance.Day;
        var days = GameController.Instance.MaxDays;
        var quota = GameController.Instance.PodQuota;

        Title.text = string.Format("SHIFT {0} of {1}", day, days);

        if (day == 1)
        {
            SetMessage(string.Format("Press buttons on your desk to feed specimens as they arrive - simple, right?", name));
            yield return StartCoroutine(WaitForOK());
        }
        else if (day == 2)
        {
            SetMessage(string.Format("Clones are picky - feed each specimen nutrients in the proper order! (LEFT to RIGHT)"));
            yield return StartCoroutine(WaitForOK());
        }
        else
        {
            if (DailyMessages.Count > 0)
            {
                var index = Random.Range(0, DailyMessages.Count);
                var message = DailyMessages[index];
                SetMessage(string.Format(message, name, day, days, quota));
            }

            yield return StartCoroutine(WaitForOK());
        }

        GameController.Instance.MorningCompleted();
    }

    private void SetMessage(string value)
    {
        DOTween.Sequence()
            .Append(Message.transform.DOScale(0, 0.25f))
            .AppendCallback(() => Message.text = value)
            .Append(Message.transform.DOScale(1, 0.25f));
    }

    private IEnumerator WaitForOK(float delay = 10)
    {
        var timeout = Time.time + delay;
        okClicked = false;

        while (!okClicked && (Time.time < timeout))
            yield return 0;
    }

    public void OnOK()
    {
        okClicked = true;
    }
}
