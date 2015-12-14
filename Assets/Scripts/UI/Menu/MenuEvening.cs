using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DG.Tweening;


public class MenuEvening : MonoBehaviour
{
    public Text Title;
    public Text Message;

    public Graphic Rating;
    public Text Grade;

    public Graphic Stats;
    public Text StatsLabel;

    void OnEnable()
    {
        var day = GameController.Instance.Day;
        var score = GameController.Instance.Score;
        var good = GameController.Instance.PodGoodCount;
        var bad = GameController.Instance.PodBadCount;
        var total = GameController.Instance.PodTotalCount;

        Title.text = string.Format("SHIFT {0} COMPLETE", day);

        Rating.transform.DOScale(Vector3.zero, 0.5f).SetDelay(1).From().SetEase(Ease.OutBounce);

        var rating = Mathf.Clamp01((float) good / total);
        if (rating >= 0.8f)
        {
            Grade.text = "A";
            Message.text = "EXCEEDS EXPECTATIONS!";
        }
        else if (rating >= 0.6f)
        {
            Grade.text = "B";
            Message.text = "MEETS EXPECTATIONS";
        }
        else if (rating >= 0.4f)
        {
            Grade.text = "C";
            Message.text = "SATISFACTORY";
        }
        else if (rating >= 0.2f)
        {
            Grade.text = "D";
            Message.text = "ROOM FOR IMPROVEMENT";
        }
        else
        {
            Grade.text = "E";
            Message.text = "TERRIBLE";
        }

        Grade.color = Color.Lerp(Color.red, Color.green, rating);

        Stats.transform.DOScale(Vector3.zero, 0.5f).SetDelay(1).From().SetEase(Ease.OutBounce);
        StatsLabel.text = string.Format("VIABLE: {0}\nREJECTS: {1}\nWAGES: ${2}", good, bad, score);
    }

    public void OnOK()
    {
        GameController.Instance.EveningCompleted();
    }

}
