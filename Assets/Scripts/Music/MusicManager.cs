using UnityEngine;
using System.Collections;

using DG.Tweening;

public class MusicManager : Singleton<MusicManager>
{

    public float FilterCutoffMonitor = 5000;
    public float FilterCutoffWorkArea = 22000;

    private AudioLowPassFilter Filter;
    private AudioSource AudioSource;

    public bool Playing
    { get; private set; }

    private void Start()
    {
        Playing = true;
        AudioSource = GetComponent<AudioSource>();
        Filter = GetComponent<AudioLowPassFilter>();
    }

    public void Toggle()
    {
        Playing = !Playing;
        AudioSource.DOFade(Playing ? 1 : 0, 1);
    }

    public void LookingAtMonitor()
    { SetCutoff(FilterCutoffMonitor); }

    public void LookingAtWorkArea()
    { SetCutoff(FilterCutoffWorkArea); }

    private void SetCutoff(float value)
    {
        DOTween.To(x => Filter.cutoffFrequency = x, Filter.cutoffFrequency, value, 1);
    }

}
