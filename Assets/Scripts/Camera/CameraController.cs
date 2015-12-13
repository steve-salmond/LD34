using UnityEngine;
using DG.Tweening;

public class CameraController : Singleton<CameraController>
{

    public Transform CameraRig;

    public Transform MonitorWaypoint;
    public Transform WorkingWaypoint;

    public void Start()
    {
        CameraRig.position = MonitorWaypoint.position;
        CameraRig.rotation = MonitorWaypoint.rotation;
    }

    public void LookAtMonitor()
    { GoToWaypoint(MonitorWaypoint); }

    public void LookAtWorkArea()
    { GoToWaypoint(WorkingWaypoint); }

    public void GoToWaypoint(Transform waypoint)
    {
        CameraRig.transform.DOMove(waypoint.position, 1);
        CameraRig.transform.DORotateQuaternion(waypoint.rotation, 1).SetEase(Ease.OutBack);
    }
   
}
