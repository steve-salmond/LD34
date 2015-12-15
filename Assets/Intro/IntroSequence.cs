using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections;
using DG.Tweening;

public class IntroSequence : MonoBehaviour
{
    public Image Logo;

    public Text BroughtBy;
    public Text Jam;
    public AudioSource Sting;

    void Start()
    {
        StartCoroutine(IntroRoutine());
	}
	
	IEnumerator IntroRoutine()
    {
        Sting.PlayDelayed(0.25f);

        DOTween.Sequence()
            .AppendInterval(0.5f)
            .Append(Logo.transform.DOScale(0, 1).From().SetEase(Ease.OutBounce))
            .Append(Logo.transform.DOScale(0.5f, 10f).SetEase(Ease.InBounce));
        DOTween.Sequence()
            .AppendInterval(2.0f)
            .Append(Jam.DOFade(0, 2).From());

        yield return new WaitForSeconds(1);

        var timeout = Time.realtimeSinceStartup + 5;
        while (Time.realtimeSinceStartup < timeout && !Input.anyKeyDown)
            yield return 0;

        Sting.DOFade(0, 0.5f);
        Logo.DOFade(0, 1.0f);
        Jam.DOFade(0, 1.0f);

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("Game");
    }
}
