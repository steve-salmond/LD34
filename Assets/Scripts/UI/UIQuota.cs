using UnityEngine;

public class UIQuota : UILabel
{
	void Update()
    {
        Label.text = "Quota: " + GameController.Instance.PodQuota;
	}
}
