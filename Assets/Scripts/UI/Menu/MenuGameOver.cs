using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using DG.Tweening;


public class MenuGameOver : MonoBehaviour
{

    public Text Message;

    public List<string> Messages;

    private void OnEnable()
    {
        var name = GameController.Instance.UserName;

        if (Messages.Count > 0)
        {
            var index = Random.Range(0, Messages.Count);
            var message = Messages[index];
            Message.text = string.Format(message, name);
        }
    }

    public void OnOK()
    {
        GameController.Instance.GameOverCompleted();
    }

}
