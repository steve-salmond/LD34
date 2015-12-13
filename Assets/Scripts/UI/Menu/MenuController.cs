using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;


public class MenuController : Singleton<MenuController>
{

    public CanvasGroup Login;
    public CanvasGroup Morning;
    public CanvasGroup Working;
    public CanvasGroup Evening;
    public CanvasGroup GameOver;

    public CanvasGroup Current
    { get; private set; }

    public GameState State
    { get; private set; }

    private void Awake()
    {
        Login.gameObject.SetActive(false);
        Working.gameObject.SetActive(false);
    }

    public void ShowLoginScreen()
    { SetScreen(Login); }

    public void ShowMorningScreen()
    { SetScreen(Morning); }

    public void ShowWorkingScreen()
    { SetScreen(Working); }

    public void ShowEveningScreen()
    { SetScreen(Evening); }

    public void ShowGameOverScreen()
    { SetScreen(GameOver); }

    private void SetScreen(CanvasGroup screen)
    {
        if (Current == screen)
            return;

        if (Current != null)
            Current.gameObject.SetActive(false);

        Current = screen;
        Current.gameObject.SetActive(true);
        Current.DOFade(0, 1.0f).From();
        Current.transform.DOScale(Vector3.one * 0.95f, 0.5f)
            .From()
            .SetEase(Ease.OutBounce);
    }



}
