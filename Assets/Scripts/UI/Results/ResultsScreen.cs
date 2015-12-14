using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;


public class ResultsScreen : Singleton<ResultsScreen>
{
    public Text Pod;
    public Text Viability;

    void Start()
    {
    }

    void Update()
    {
        if (!GameController.Instance.IsWorking)
        {
            Pod.text = "";
            Viability.text = "";
        }
    }

    public void Deliver(Pod pod)
    {
        var delivered = GameController.Instance.PodDeliveredCount;
        var total = GameController.Instance.PodTotalCount;

        Pod.text = string.Format("POD {0} of {1}", delivered, total);

        if (pod.IsFullyGrown)
        {
            Viability.text = "VIABLE";
            Viability.DOColor(Color.green, 0.5f);
        }
        else if (pod.IsGood)
        {
            Viability.text = "MARGINAL";
            Viability.DOColor(Color.yellow, 0.5f);
        }
        else
        {
            Viability.text = "REJECTED";
            Viability.DOColor(Color.red, 0.5f);
        }

        DOTween.Sequence()
            .Append(Viability.transform.DOPunchScale(Vector3.one * 0.25f, 0.5f, 3))
            .Append(Viability.transform.DOScale(Vector3.one, 0.2f));

    }
}
