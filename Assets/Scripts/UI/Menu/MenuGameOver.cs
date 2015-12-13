using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DG.Tweening;


public class MenuGameOver : MonoBehaviour
{

    public void OnOK()
    {
        GameController.Instance.EveningCompleted();
    }

}
