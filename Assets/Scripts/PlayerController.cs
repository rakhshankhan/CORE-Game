using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float turnSpeed = 20f;
    public float moveSpeed = 1f;
    public float aimingMoveSpeed = 0.5f;

    public Transform cameraTransform;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Use the camera's forward and right directions for movement
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Ignore vertical components of the camera's direction
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Calculate movement based on camera's orientation
        m_Movement = forward * vertical + right * horizontal;
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isRunning = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsRunning", isRunning);

        // Check if the player is aiming
        bool isAiming = Input.GetMouseButton(1);
        m_Animator.SetBool("IsAiming", isAiming);

        // Adjust movement speed based on aiming state
        if (isAiming)
        {
            moveSpeed = aimingMoveSpeed;

            if (!isRunning)
            {
                // Align player's rotation with the camera's forward direction while stationary
                Vector3 cameraForward = cameraTransform.forward;
                cameraForward.y = 0f; // Ignore vertical component
                m_Rotation = Quaternion.LookRotation(cameraForward);
            }
        }
        else
        {
            moveSpeed = 1f; // Reset to default move speed
        }

        // Update the rotation to face the movement direction
        if (isRunning)
        {
            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
            m_Rotation = Quaternion.LookRotation(desiredForward);
        }
    }

    void OnAnimatorMove()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * moveSpeed * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}
