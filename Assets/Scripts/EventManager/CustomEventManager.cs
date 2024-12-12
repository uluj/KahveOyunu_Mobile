using UnityEngine;
using System;

public class CustomEventManager : MonoBehaviour
{
    // Events for Zoom In, Zoom Out, and Camera Interpolation
    public static event Action OnZoomIn;
    public static event Action OnZoomOut;
    public static event Action<float> OnZoomInTouch;
    public static event Action<float> OnZoomOutTouch;
    public static event Action OnCameraInterpolation;
    public static event Action OnReverseCameraInterpolation;
    public static event Action<Vector2> OnDrag;
    // log console messeage
    public static event Action<string> OnLogMessage;
    public static void TriggerLogMessage(string message)
    {
        OnLogMessage?.Invoke(message);
    }
    public static void TriggerDrag(Vector2 dragDelta)
    {
        OnDrag?.Invoke(dragDelta); // Trigger all methods subscribed to OnDrag
    }

    // Public methods to trigger the events
    public static void TriggerZoomIn()
    {
        OnZoomIn?.Invoke(); // Trigger all methods subscribed to OnZoomIn
    }

    public static void TriggerZoomOut()
    {
        OnZoomOut?.Invoke(); // Trigger all methods subscribed to OnZoomOut
    }

    public static void TriggerZoomInTouch(float distance)
    {
        OnZoomInTouch?.Invoke(distance); // Trigger all methods subscribed to OnZoomInTouch
    }
    public static void TriggerZoomOutTouch(float distance)
    {
        OnZoomOutTouch?.Invoke(distance); // Trigger all methods subscribed to OnZoomOutTouch
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
