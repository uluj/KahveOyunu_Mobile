using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool isMobile; // Toggle this to switch between PC and Mobile
    private float previousDistance; // Distance between two fingers in the previous frame
    private bool isZooming = false;

    void Update()
    {
        if (isMobile)
        {
            HandleTouchZoom(); // Handle mobile-specific 2-finger zoom
        }
        else
        {
            HandlePCInput(); // Handle PC-specific input
        }
    }

    // Handle PC Input
    void HandlePCInput()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel"); // Get scroll wheel input

        if (scrollInput > 0)
        {
            CustomEventManager.TriggerZoomIn(); // Zoom in on scroll up
        }
        else if (scrollInput < 0)
        {
            CustomEventManager.TriggerZoomOut(); // Zoom out on scroll down
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            CustomEventManager.TriggerCameraInterpolation();
        }
        
        if (Input.GetKeyDown(KeyCode.Y))
        {
            CustomEventManager.ReverseTriggerCameraInterpolation();
        }
    }

    // Handle 2-Finger Zoom Gesture for Mobile
    void HandleTouchZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            float currentDistance = Vector2.Distance(touch1.position, touch2.position);

            if (!isZooming)
            {
                previousDistance = currentDistance;
                isZooming = true;
                return;
            }

            float distanceDelta = currentDistance - previousDistance;

            if (distanceDelta > 0)
            {
                CustomEventManager.TriggerZoomIn();
            }
            else if (distanceDelta < 0)
            {
                CustomEventManager.TriggerZoomOut();
            }

            previousDistance = currentDistance;
        }
        else
        {
            isZooming = false; // Reset zooming state if fewer than 2 touches
        }
    }

    // UI Button Triggers
    public void ZoomIn()
    {
        CustomEventManager.TriggerZoomIn();
    }

    public void ZoomOut()
    {
        CustomEventManager.TriggerZoomOut();
    }

    public void InterpolateCamera()
    {
        CustomEventManager.TriggerCameraInterpolation();
    }
    public void ReverseInterpolateCamera()
    {
        CustomEventManager.ReverseTriggerCameraInterpolation();
    }
}
