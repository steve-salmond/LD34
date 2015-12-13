using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DG.Tweening;


public class MenuWorking : MonoBehaviour
{
    public Text User;

    void OnEnable()
    {
        var name = GameController.Instance.UserName;
        User.text = string.Format("{0}", name);
    }
}
