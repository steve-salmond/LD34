using UnityEngine;
using DG.Tweening;

public class CameraController : Singleton<CameraController>
{

    public Transform CameraRig;

    public Transform MonitorWaypoint;
    public Transform WorkingWaypoint;

    public float Aspect = 16 / 9.0f;

    public Camera Camera;

    public void Awake()
    {
        CameraRig.position = MonitorWaypoint.position;
        CameraRig.rotation = MonitorWaypoint.rotation;
    }

    private void Update()
    {
        UpdateAspect();
    }

    private void UpdateAspect()
    {
        var screenAspect = (float) Screen.width / (float) Screen.height;

        if (screenAspect > Aspect)
        {
            Camera.rect = new Rect(0, 0, 1, 1);
            return;
        }

        Rect rect = new Rect();
        rect.width = 1;
        rect.height = screenAspect / Aspect;
        rect.x = 0;
        rect.y = (1 - rect.height) * 0.5f;
        Camera.rect = rect;
    }

    public void LookAtMonitor()
    {
        GoToWaypoint(MonitorWaypoint);
        MusicManager.Instance.LookingAtMonitor();
    }

    public void LookAtWorkArea()
    {
        GoToWaypoint(WorkingWaypoint);
        MusicManager.Instance.LookingAtWorkArea();
    }

    public void GoToWaypoint(Transform waypoint)
    {
        CameraRig.transform.DOMove(waypoint.position, 1.5f);
        CameraRig.transform.DORotateQuaternion(waypoint.rotation, 1.5f).SetEase(Ease.OutBack);
    }
   
    public void Shake()
    {
        Camera.DOShakePosition(1.0f, 0.025f);
        Camera.DOShakeRotation(1.0f, 0.075f);
    }
}
