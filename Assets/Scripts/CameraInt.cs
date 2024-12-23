using UnityEngine;

public class CameraInt : MonoBehaviour
{
    public Transform cameraPawnStart;   // Starting point for interpolation
    public Transform cameraPawnEnd;     // Ending point for interpolation
    public float transitionSpeed = 1.0f; // Speed of interpolation
    [SerializeField] private Camera mainCamera; // Reference to the main camera
    private float transitionProgress = 0f; // Progress of the transition (0 to 1)
    private bool isTransitioning = false; // Whether the camera is transitioning

    // Camera zoom settings
    // We need to change this to native resolution
    public float zoomSpeed = 5f; // Speed of zoom
    public float zoomSpeedTouch = 10f;
    public float minZoom = 1f;   // Minimum orthographic size
    public float maxZoom = 10f;  // Maximum orthographic size

    private bool reverseInterpolation = false;

    void OnEnable()
    {
        CustomEventManager.OnCameraInterpolation += TriggerCameraInterpolation;
        CustomEventManager.OnReverseCameraInterpolation += ReverseTriggerCameraInterpolation;
        // Zoom for Mouse
        CustomEventManager.OnZoomIn += ZoomIn;
        CustomEventManager.OnZoomOut += ZoomOut;
        // Zoom for Touch
        CustomEventManager.OnZoomInTouch += ZoomInTouch;
        CustomEventManager.OnZoomOutTouch += ZoomOutTouch;
        // Drag
        CustomEventManager.OnDrag += HandleDrag;
    }

    void OnDisable()
    {
        CustomEventManager.OnCameraInterpolation -= TriggerCameraInterpolation;
        CustomEventManager.OnReverseCameraInterpolation -= ReverseTriggerCameraInterpolation;
        // Zoom for mouse
        CustomEventManager.OnZoomIn -= ZoomIn;
        CustomEventManager.OnZoomOut -= ZoomOut;
        // Zoom for Touch
        CustomEventManager.OnZoomInTouch -= ZoomInTouch;
        CustomEventManager.OnZoomOutTouch -= ZoomOutTouch;
        // Drag
        CustomEventManager.OnDrag -= HandleDrag;
    }

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Automatically assign main camera if not set
        }
    }

    void Update()
    {
        if (isTransitioning)
        {
            HandleCameraInterpolation();
        }
    }
    private void HandleTouchZoom()
    {

    }
    private void ZoomInTouch(float distance)
    {
        float adjustedDistance = distance * 100f;
        mainCamera.orthographicSize -= adjustedDistance * zoomSpeed * Time.deltaTime;
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minZoom, maxZoom);
    }
    private void ZoomOutTouch(float distance)
    {
        float adjustedDistance = distance * 100f;
        mainCamera.orthographicSize += adjustedDistance * zoomSpeed * Time.deltaTime;
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minZoom, maxZoom);
    }
    private void HandleDrag(Vector2 dragDelta)
    {
        // Implement your drag handling logic here
        Debug.Log($"Dragged by {dragDelta}");
        // For example, move the camera:
        Vector2 adjustedDelta = Camera.main.ScreenToViewportPoint(dragDelta);
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x - adjustedDelta.x, mainCamera.transform.position.y, mainCamera.transform.position.z - adjustedDelta.y);
    }

    // Trigger camera interpolation
    public void TriggerCameraInterpolation()
    {
        isTransitioning = true;
        transitionProgress = 0f; // Reset the progress
        reverseInterpolation = false; // Set direction explicitly to forward
    }
    public void ReverseTriggerCameraInterpolation()
    {
        isTransitioning = true;
        transitionProgress = 0f; // Reset the progress
        reverseInterpolation = true; // Set direction explicitly to reverse
    }

    // Perform the interpolation
    private void HandleCameraInterpolation()
    {
        // Update progress
        transitionProgress += Time.deltaTime * transitionSpeed;
        transitionProgress = Mathf.Clamp01(transitionProgress);

        // Determine the start and end points based on the direction
        Transform from = reverseInterpolation ? cameraPawnEnd : cameraPawnStart;
        Transform to = reverseInterpolation ? cameraPawnStart : cameraPawnEnd;

        // Interpolate position and rotation
        mainCamera.transform.position = Vector3.Lerp(from.position, to.position, transitionProgress);
        mainCamera.transform.rotation = Quaternion.Lerp(from.rotation, to.rotation, transitionProgress);

        // Stop transitioning once progress reaches 1
        if (transitionProgress >= 1f)
        {
            isTransitioning = false;
        }
    }


    // Zoom in the camera
    private void ZoomIn()
    {
        AdjustZoom(-1); // Negative scroll for zoom in
    }

    // Zoom out the camera
    private void ZoomOut()
    {
        AdjustZoom(1); // Positive scroll for zoom out
    }

    // Adjust the zoom level
    private void AdjustZoom(float zoomDelta)
    {
        mainCamera.orthographicSize += zoomDelta * zoomSpeed * Time.deltaTime;
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minZoom, maxZoom);
    }
}
