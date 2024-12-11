using UnityEngine;
using System;

public class CustomEventManager : MonoBehaviour
{
    // Events for Zoom In, Zoom Out, and Camera Interpolation
    public static event Action OnZoomIn;
    public static event Action OnZoomOut;
    public static event Action OnCameraInterpolation;
    public static event Action OnReverseCameraInterpolation;

    // Public methods to trigger the events
    public static void TriggerZoomIn()
    {
        OnZoomIn?.Invoke(); // Trigger all methods subscribed to OnZoomIn
    }

    public static void TriggerZoomOut()
    {
        OnZoomOut?.Invoke(); // Trigger all methods subscribed to OnZoomOut
    }

    public static void TriggerCameraInterpolation()
    {
        OnCameraInterpolation?.Invoke(); // Trigger all methods subscribed to OnCameraInterpolation
    }
    public static void ReverseTriggerCameraInterpolation()
    {
        OnReverseCameraInterpolation?.Invoke(); // Trigger all methods subscribed to OnCameraInterpolation
    }
}
