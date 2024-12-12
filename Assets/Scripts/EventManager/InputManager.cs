using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputActions inputActions;
    public bool isMobile; // Toggle this to switch between PC and Mobile
    private float previousDistance; // Distance between two fingers in the previous frame
    //private bool isZooming = false;
    private Vector2 lastMousePosition;
    private bool isDragging = false;  // To track whether the drag is ongoing
    private Vector2 lastTouchPosition;
    private Coroutine ZoomCoroutine;

    private void Awake()
    {
        inputActions = new InputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        // Touch zoom (Primary and Secondary Finger Position)
        inputActions.Touch.SecondaryTouchContact.started += _ => TouchZoomStart();
        inputActions.Touch.SecondaryTouchContact.started += _ => TouchZoomEnd();

        // Touch Drag (Mobile)
        inputActions.Touch.PrimaryFingerPressed.performed += HandleTouchPressStart;
        inputActions.Touch.PrimaryFingerReleased.performed += HandleTouchRelease;
        inputActions.Touch.PrimaryFingerDragged.performed += HandleTouchDrag;

        // Mouse Drag (PC)
        inputActions.Mouse.MouseL1Pressed.performed += HandleMousePressStart;
        inputActions.Mouse.MouseL1Released.performed += HandleMouseRelease;
        inputActions.Mouse.MouseL1Dragged.performed += HandleMouseDrag;
        //scroll wheel pc
        inputActions.Mouse.MouseScrollWheel.performed += HandlePCInputScroll;
    }

    private void OnDisable()
    {
        inputActions.Disable();

        // Touch zoom (Primary and Secondary Finger Position)
        inputActions.Touch.SecondaryTouchContact.started -= _ => TouchZoomStart();
        inputActions.Touch.SecondaryTouchContact.started -= _ => TouchZoomEnd();
        // Touch Drag (Mobile)
        inputActions.Touch.PrimaryFingerPressed.performed -= HandleTouchPressStart;
        inputActions.Touch.PrimaryFingerReleased.performed -= HandleTouchRelease;
        inputActions.Touch.PrimaryFingerDragged.performed -= HandleTouchDrag;
        // Mouse Drag (PC)
        inputActions.Mouse.MouseL1Pressed.performed -= HandleMousePressStart;
        inputActions.Mouse.MouseL1Released.performed -= HandleMouseRelease;
        inputActions.Mouse.MouseL1Dragged.performed -= HandleMouseDrag;
        //scroll wheel pc
        inputActions.Mouse.MouseScrollWheel.performed -= HandlePCInputScroll;
    }
    private void HandlePCInputScroll(InputAction.CallbackContext context)
    {
        Vector2 scrollValue = context.ReadValue<Vector2>();

        if (scrollValue.y > 0)
        {
            Debug.Log("Scroll Up Detected!");
            CustomEventManager.TriggerZoomIn();
        }
        else if (scrollValue.y < 0)
        {
            Debug.Log("Scroll Down Detected!");
            CustomEventManager.TriggerZoomOut();
        }
    }

    private void TouchZoomStart()
    {
       ZoomCoroutine = StartCoroutine(ZoomDetection());
    }
    private void TouchZoomEnd()
    {
        StopCoroutine(ZoomCoroutine);
    }
    IEnumerator ZoomDetection()
    {
        float previousDistance = 0f, distance = 0f;
        while (true)
        {
            distance = Vector2.Distance(inputActions.Touch.PrimaryFingerPosition.ReadValue<Vector2>(), inputActions.Touch.SecondaryFingerPosition.ReadValue<Vector2>());
            // Detection
            
            // Zoom Out
            if (distance > previousDistance)
            {

                CustomEventManager.TriggerZoomOutTouch(distance);
            }
            // Zoom In
            else if (distance < previousDistance)
            {
                CustomEventManager.TriggerZoomInTouch(distance);
            }
            // Keep Track of Previous Distance for next loop
            previousDistance = distance;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void HandleTouchPressStart(InputAction.CallbackContext context)
    {
        Debug.Log("Primary Finger Pressed!");
        lastTouchPosition = inputActions.Touch.PrimaryFingerPosition.ReadValue<Vector2>();  // Capture initial touch position
        isDragging = true;  // Start dragging
    }

    private void HandleTouchRelease(InputAction.CallbackContext context)
    {
        Debug.Log("Primary Finger Released!");
        isDragging = false;  // Stop dragging
    }

    private void HandleTouchDrag(InputAction.CallbackContext context)
    {
        if (isDragging && Input.touchCount == 1) // Only drag if one finger is on the screen
        {
            Vector2 currentTouchPosition = inputActions.Touch.PrimaryFingerPosition.ReadValue<Vector2>();
            Vector2 dragDelta = currentTouchPosition - lastTouchPosition;  // Calculate the drag delta

            // Trigger the drag event in the EventManager
            CustomEventManager.TriggerDrag(dragDelta);

            lastTouchPosition = currentTouchPosition;  // Update touch position for next calculation
        }
    }

    private void HandleMousePressStart(InputAction.CallbackContext context)
    {
        Debug.Log("Mouse L1 Pressed!");
        lastMousePosition = Mouse.current.position.ReadValue();  // Capture the initial position of the mouse when pressed
        isDragging = true;  // Start dragging when the mouse is pressed
    }

    private void HandleMouseRelease(InputAction.CallbackContext context)
    {
        Debug.Log("Mouse L1 Released!");
        isDragging = false;  // Stop dragging when the mouse button is released
    }

    private void HandleMouseDrag(InputAction.CallbackContext context)
    {
        if (isDragging)
        {
            Vector2 currentMousePosition = Mouse.current.position.ReadValue(); // Get current mouse position
            Vector2 dragDelta = currentMousePosition - lastMousePosition;  // Calculate the drag delta

            // Trigger the drag event in the EventManager
            CustomEventManager.TriggerDrag(dragDelta);

            lastMousePosition = currentMousePosition;  // Update last mouse position for next calculation
        }
    }

    private void Update()
    {
        if (isMobile)
        {
            //HandleTouchZoom(); // Handle mobile-specific 2-finger zoom
        }
        else
        {
            HandlePCInput(); // Handle PC-specific input
        }
    }

    // Handle PC Input
    private void HandlePCInput()
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

    // Handle Touch Zoom

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
