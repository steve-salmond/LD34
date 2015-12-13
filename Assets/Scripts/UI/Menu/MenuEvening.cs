using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DG.Tweening;


public class MenuEvening : MonoBehaviour
{

    public void OnOK()
    {
        GameController.Instance.EveningCompleted();
    }

}
