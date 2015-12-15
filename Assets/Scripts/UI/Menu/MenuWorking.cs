using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using DG.Tweening;


public class MenuWorking : MonoBehaviour
{
    public Text Title;
    public Text Delivered;
    public Text Quota;
    public Text Message;

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

        StopAllCoroutines();
        StartCoroutine(UpdateRoutine());
    }

    void Update()
    {
        var viable = GameController.Instance.PodGoodCount;
        Delivered.text = string.Format("VIABLE SPECIMENS: {0}", viable);

        if (viable != LastViable)
            OnViableChanged();

        LastViable = viable;
    }

    IEnumerator UpdateRoutine()
    {
        var day = GameController.Instance.Day;

        if (day == 1)
        {
            yield return StartCoroutine(MessageRoutine("REMEMBER - Feed specimens the correct <color=#5EF8FD>COLOR</color>!"));

            while (GameController.Instance.Score == 0)
                yield return 0;

            var score = GameController.Instance.Score;
            if (score < 0)
                yield return StartCoroutine(MessageRoutine("Oops! You fed that one the WRONG COLOR.."));

            while (GameController.Instance.Score < 0)
                yield return 0;

            yield return StartCoroutine(MessageRoutine("Great work!"));

            while (GameController.Instance.PodDeliveredCount == 0)
                yield return 0;

            yield return new WaitForSeconds(1);
            yield return StartCoroutine(MessageRoutine("Make perfect specimens for bonus wages!"));
        }
        else if (day == 2)
        {
            yield return StartCoroutine(MessageRoutine("Remember, feed from LEFT to RIGHT!", 5));
        }
        else if (day == 3)
            yield return StartCoroutine(MessageRoutine("Things might start to get a bit tricky now.."));
        else if (day == 4)
            yield return StartCoroutine(MessageRoutine("Still here? Great!"));
        else if (day == 5)
            yield return StartCoroutine(MessageRoutine("Wow, you're amazing :)"));
        else if (day == 6)
            yield return StartCoroutine(MessageRoutine("You've almost cracked it!"));
        else if (day == 7)
            yield return StartCoroutine(MessageRoutine("Last day - so exciting!"));
    }

    private IEnumerator MessageRoutine(string value, float interval = 3)
    {
        Message.text = value; 

        Quota.transform.DOScale(0, 0.25f);
        Delivered.transform.DOScale(0, 0.25f);

        DOTween.Sequence()
            .Append(Message.transform.DOScale(1, 0.25f))
            .AppendInterval(3)
            .Append(Message.transform.DOScale(0, 0.25f))
            .Append(Quota.transform.DOScale(1, 0.25f))
            .Append(Delivered.transform.DOScale(1, 0.25f));

        yield return new WaitForSeconds(interval + 1);
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
