using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DG.Tweening;


public class MenuVictory : MonoBehaviour
{

    public Text Message;

    void OnEnable()
    {
        var name = GameController.Instance.UserName;
        Message.text = string.Format("WELCOME ABOARD, {0}", name);
    }

    public void OnOK()
    {
        GameController.Instance.VictoryCompleted();
    }

}
