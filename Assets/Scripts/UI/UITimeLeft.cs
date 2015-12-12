using UnityEngine;

public class UITimeLeft : UILabel
{
	void Update()
    {
        var time = GameController.Instance.TimeLeft;
        Label.text = string.Format("Time left: {0:0.0}", time);
	}
}
