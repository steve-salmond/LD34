using UnityEngine;

public class UITotalScore : UILabel
{
	void Update()
    {
        Label.text = "Total: " + GameController.Instance.TotalScore;
	}
}
