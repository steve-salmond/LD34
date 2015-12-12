using UnityEngine;

public class UIDay : UILabel
{
	void Update()
    {
        Label.text = "Day " + GameController.Instance.Day;
	}
}
