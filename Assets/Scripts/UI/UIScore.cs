using UnityEngine;

public class UIScore : UILabel
{
	void Update()
    {
        Label.text = "Score: " + GameController.Instance.Score;
	}
}
