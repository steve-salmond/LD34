using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DG.Tweening;


public class MenuLogin : MonoBehaviour
{

    public InputField UserName;

    public Text Message;


    void Start()
    {
    }

	void OnEnable()
    {
        UserName.text = "";
        Message.text = "";
    }

    public void OnLogin()
    {
        var name = UserName.text;

        if (!IsValidUserName(name))
            Message.text = "<color=#ff0000>Invalid username!</color>";
        else
        {
            Message.text = string.Format("Welcome, {0}!", name);
            GameController.Instance.Login(name);
        }
    }

    private bool IsValidUserName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return false;

        return true;
    }
}
