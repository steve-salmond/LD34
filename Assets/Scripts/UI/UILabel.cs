using UnityEngine;
using UnityEngine.UI;

public class UILabel : MonoBehaviour
{
    public Text Label
    { get; private set; }

    private void Awake()
    {
        Label = GetComponent<Text>();
    }

}
