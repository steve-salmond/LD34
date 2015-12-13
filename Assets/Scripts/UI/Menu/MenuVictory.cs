using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using DG.Tweening;


public class MenuVictory : MonoBehaviour
{

    public void OnOK()
    {
        GameController.Instance.VictoryCompleted();
    }

}
