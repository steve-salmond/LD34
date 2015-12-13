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

    void OnEnable()
    {
        var day = GameController.Instance.Day;
        var good = GameController.Instance.PodGoodCount;
        var total = GameController.Instance.PodTotalCount;

        Title.text = string.Format("SHIFT {0} COMPLETE", day);
        // Message.text = string.Format("", name, quota);

        Rating.transform.DOScale(Vector3.zero, 0.5f).SetDelay(1).From().SetEase(Ease.OutBounce);

        var rating = Mathf.Clamp01((float) good / total);
        if (rating >= 0.8f)
            Grade.text = "A";
        else if (rating >= 0.6f)
            Grade.text = "B";
        else if (rating >= 0.4f)
            Grade.text = "C";
        else if (rating >= 0.2f)
            Grade.text = "D";
        else
            Grade.text = "E";

        Grade.color = Color.Lerp(Color.red, Color.green, rating);
    }

    public void OnOK()
    {
        GameController.Instance.EveningCompleted();
    }

}
