using UnityEngine;
using Unity.Cinemachine;

public class AimStateManager : MonoBehaviour
{
    [SerializeField] float mouseSense = 1;
    float xAxis, yAxis;
    [SerializeField] Transform camFollowPos;
    [SerializeField] CinemachineCamera virtualCamera;
    [SerializeField] float cameraUpRestriction = 60f;
    [SerializeField] float cameraDownRestriction = -30f;

    private const float xForWalk = 1.2f;
    private const float yForWalk = 0.5f;

    private MovementStateManager movementStateManager;
    private float currentTurn = 0f; // Store the current turn value for smoothing
    [SerializeField] private float smoothTime = 0.1f; // The speed at which the turn value changes

    [SerializeField] Transform aimPos;
    [SerializeField] float aimSmoothSpeed = 60;
    [SerializeField] LayerMask aimMask;

    void Start()
    {
        if (virtualCamera == null)
        {
            Debug.LogError("Cinemachine Virtual Camera is not assigned!");
        }

        // Get the instance
        movementStateManager = GetComponent<MovementStateManager>();
        // Set the turn to neutral
        movementStateManager.anim.SetFloat("Turn", 0.5f);
    }

    void Update()
    {
        xAxis += Input.GetAxisRaw("Mouse X") * mouseSense;
        yAxis -= Input.GetAxisRaw("Mouse Y") * mouseSense;
        yAxis = Mathf.Clamp(yAxis, cameraDownRestriction, cameraUpRestriction);

        CalculateCharacterIdleTurn();

        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
        {
            aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSmoothSpeed * Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis, transform.eulerAngles.z);
    }

    private void CalculateCharacterIdleTurn()
    {
        // Get the horizontal mouse movement (X-axis)
        float mouseDelta = Input.GetAxisRaw("Mouse X") * mouseSense;

        // The target turn value is based on mouse input
        float targetTurn = Mathf.Clamp(mouseDelta, -1f, 1f);

        // Smoothly interpolate between the current turn and the target turn
        currentTurn = Mathf.Lerp(currentTurn, targetTurn, Time.deltaTime / smoothTime);

        // Update the animator's "Turn" parameter
        if (movementStateManager != null && movementStateManager.anim != null)
        {
            movementStateManager.anim.SetFloat("Turn", currentTurn);
        }
    }

}