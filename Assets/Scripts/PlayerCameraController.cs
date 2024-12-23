using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class PlayerCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera mainCamera;
    public CinemachineVirtualCamera aimCamera;
    public CinemachineVirtualCamera walkAimCamera;

    public Image crosshair;

    public int mouseSense = 1;
    public Transform aimPos;
    public float aimSmoothSpeed = 20f;

    private bool isAiming;
    private bool isRunning;

    void Update()
    {
        isAiming = Input.GetMouseButton(1);

        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        isRunning = inputDirection.magnitude > 0.1f;

        if (isAiming && isRunning)
        {
            aimCamera.Priority = 0;
            mainCamera.Priority = 0;
            walkAimCamera.Priority = 10;
            crosshair.enabled = true;
        }
        else if (isAiming)
        {
            aimCamera.Priority = 10;
            mainCamera.Priority = 0;
            walkAimCamera.Priority = 0;
            crosshair.enabled = true;
        }
        else
        {
            aimCamera.Priority = 0;
            mainCamera.Priority = 10;
            walkAimCamera.Priority = 0;
            crosshair.enabled = false;
        }
    }
}
