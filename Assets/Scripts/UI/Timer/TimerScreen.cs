using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DG.Tweening;


public class TimerScreen : MonoBehaviour
{
    public Text User;
    public Text Score;

    private int LastScore;

    void Start()
    {
        LastScore = GameController.Instance.TotalScore;
    }

    void Update()
    {
        var user = GameController.Instance.UserName;
        var score = GameController.Instance.TotalScore;
        User.text = string.Format("{0}", user);

        if (score >= 0)
            Score.text = string.Format("${0}", score);
        else
            Score.text = string.Format("<color=#ff0000>-${0}</color>", -score);

        if (score != LastScore)
            OnScoreChanged(score - LastScore);

        LastScore = score;
    }

    private void OnScoreChanged(int delta)
    {
        DOTween.Sequence()
            .Append(Score.DOColor(delta > 0 ? Color.green : Color.red, 0.5f))
            .Append(Score.DOColor(Color.white, 0.5f));

        DOTween.Sequence()
            .Append(Score.transform.DOPunchScale(Vector3.one * 0.25f, 0.5f, 3))
            .Append(Score.transform.DOScale(Vector3.one, 0.2f));
    }
}
