using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using DG.Tweening;


public class MenuWelcome : MonoBehaviour
{

    public Text Title;
    public Text Message;

    private bool okClicked;

    void OnEnable()
    {
        var name = GameController.Instance.UserName;
        Title.text = string.Format("Welcome, {0}!", name);

        StopAllCoroutines();
        StartCoroutine(WelcomeRoutine());
    }

    IEnumerator WelcomeRoutine()
    {
        SetMessage("Congratulations for being accepted to your 1 week employment trial!");
        yield return StartCoroutine(WaitForOK());

        SetMessage("We trust you will work diligently to become a valued member of our 'growing' team, haha!");
        yield return StartCoroutine(WaitForOK());

        GameController.Instance.WelcomeCompleted();
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
