﻿using UnityEngine;
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
   
}
