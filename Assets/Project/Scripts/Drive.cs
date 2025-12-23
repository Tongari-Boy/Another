using UnityEngine;
using UnityEngine.InputSystem;

public class Drive : MonoBehaviour
{
    public float acceleration = 20f;
    public float brakePower = 30f;

    public float natutalSpeed = 1.0f;
    public float maxSpeed = 100.0f;

    public float naturalDrag = 1.0f; // CS0103: 'naturalDrag' が未定義 → 追加

    private Rigidbody rb;
    public float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        HandleInput();
        ApplyForces();
        ApplyDrag();
        ClampSpeed();
    }

    //入力
    void HandleInput()
    {
        float accel = GetAccelerateInput();
        float brake = GetBrakeInput();
        moveInput = CalculateMoveInput(accel, brake);
    }

    float GetAccelerateInput()
    {
        return Mouse.current.rightButton.isPressed ? 1f : 0f;
    }

    float GetBrakeInput()
    {
        return Mouse.current.leftButton.isPressed ? 1f : 0f;
    }

    float CalculateMoveInput(float accel, float brake)
    {
        return accel - brake;
    }

    //力の適応
    void ApplyForces()
    {
        if (moveInput > 0)
        {
            ApplyAcceleration();
        }
        else if (moveInput > 0)
        {
            ApplyBrake();
        }
    }

    void ApplyAcceleration()
    {
        rb.AddForce(GetForward() * acceleration, ForceMode.Acceleration);
    }

    void ApplyBrake()
    {
        rb.AddForce(-GetForward() * brakePower, ForceMode.Acceleration);
    }

    //補助処理
    void ApplyDrag()
    {
        rb.linearDamping = moveInput == 0 ? naturalDrag : 0f; // CS0618: drag → linearDamping に修正
    }

    void ClampSpeed()
    {
        float speed = GetForwardSpeed();

        if (speed > maxSpeed)
        {
            rb.angularVelocity = GetForward() * maxSpeed;
        }
    }

    //共通ユーティリティ
    Vector3 GetForward()
    {
        return transform.forward;
    }

    public float GetForwardSpeed()
    {
        return Vector3.Dot(rb.angularVelocity, GetForward());
    }
}