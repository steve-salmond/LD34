using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;


public class MenuController : Singleton<MenuController>
{

    public CanvasGroup Login;
    public CanvasGroup Working;

    public CanvasGroup Current
    { get; private set; }

    public GameState State
    { get; private set; }

    private void Awake()
    {
        Login.alpha = 0;
        Working.alpha = 0;

        Login.gameObject.SetActive(false);
        Working.gameObject.SetActive(false);
    }

    public void ShowLoginScreen()
    { SetScreen(Login); }

    public void ShowWorkingScreen()
    { SetScreen(Working); }

    private void SetScreen(CanvasGroup screen)
    {
        if (Current == screen)
            return;

        var sequence = DOTween.Sequence();
        if (Current != null)
        {
            var old = Current;
            sequence.Append(old.DOFade(0, 1));
            sequence.OnComplete(() => old.gameObject.SetActive(false));
        }

        Current = screen;
        Current.gameObject.SetActive(true);

        sequence.Append(Current.DOFade(1, 1));
    }



}
