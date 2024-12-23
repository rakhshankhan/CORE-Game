using UnityEngine;

public class PlayerAimingController : MonoBehaviour
{
    public Animator animator;
    public Transform weapon;

    public Vector3 aimingGunPosition;
    public Vector3 aimingGunRotation;

    public Vector3 normalGunPosition;
    public Vector3 normalGunRotation;

    public Vector3 runningAimingGunPosition;
    public Vector3 runningAimingGunRotation;

    private bool isAiming;
    private bool isRunning;

    void Update()
    {   
        isAiming = Input.GetMouseButton(1);

        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        isRunning = inputDirection.magnitude > 0.1f;    // Check if player is moving

        animator.SetBool("IsAiming", isAiming);
        animator.SetBool("IsRunning", isRunning);

        if (isAiming && isRunning)
        {
            weapon.localPosition = runningAimingGunPosition;
            weapon.localEulerAngles = runningAimingGunRotation;
        }
        else if (isAiming)
        {
            weapon.localPosition = aimingGunPosition;
            weapon.localEulerAngles = aimingGunRotation;
        }
        else
        {
            weapon.localPosition = normalGunPosition;
            weapon.localEulerAngles = normalGunRotation;
        }
    }
}
