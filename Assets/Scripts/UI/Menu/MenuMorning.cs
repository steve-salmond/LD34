using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DG.Tweening;


public class MenuMorning : MonoBehaviour
{

    public Text Title;

	void OnEnable()
    {
        var name = GameController.Instance.UserName;
        Title.text = string.Format("Good morning, {0}", name);
    }

    public void OnOK()
    {
        GameController.Instance.MorningCompleted();
    }

}
