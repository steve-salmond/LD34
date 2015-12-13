using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DG.Tweening;


public class MenuWelcome : MonoBehaviour
{

    public Text Title;

	void OnEnable()
    {
        var name = GameController.Instance.UserName;
        Title.text = string.Format("Welcome, {0}!", name);
    }

    public void OnOK()
    {
        GameController.Instance.WelcomeCompleted();
    }

}
