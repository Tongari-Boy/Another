using UnityEngine;
using UnityEngine.InputSystem;

public class Rotate : MonoBehaviour
{
    public float maxSteerAngle = 30f;     // 最大ハンドル角
    public float steerSpeed = 5f;         // マウス追従速度
    public float returnSpeed = 3f;        // ハンドル戻り速度

    public float minSteerSpeed = 0.2f;    // 低速時の影響カット

    private Rigidbody rb;
    private float currentSteerAngle;      // 現在のハンドル角
    private float targetSteerAngle;       // 目標ハンドル角

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        UpdateTargetSteerAngle();
        UpdateSteerAngle();
        ApplySteering();   
    }


    //目標角度の更新
    void UpdateTargetSteerAngle()
    {
        if (IsMouseSteering())
        {
            targetSteerAngle = GetMouseSteerAngle();
        }
        else
        {
            targetSteerAngle = 0f;  //入力がなければ中央へ
        }
    }

    bool IsMouseSteering()
    {
        // 画面中央からある程度離れているか
        float mouseX = Mouse.current.position.ReadValue().x;
        float center = Screen.width * 0.5f;
        return Mathf.Abs(mouseX - center) > 5f;
    }

    float GetMouseSteerAngle()
    {
        float mouseX = Mouse.current.position.ReadValue().x;
        float center = Screen.width * 0.5f;
        float normalized = (mouseX - center) / center;

        return Mathf.Clamp(normalized, -1f, 1f) * maxSteerAngle;
    }

    //ハンドル角の更新
    void UpdateSteerAngle()
    {
        float speed = Mathf.Abs(rb.maxAngularVelocity);
        float speedFactor = Mathf.Clamp01(speed / rb.maxAngularVelocity);

        float lerpSpeed = targetSteerAngle == 0f ? returnSpeed:steerSpeed;

        currentSteerAngle = Mathf.Lerp(
            currentSteerAngle,
            targetSteerAngle,
            lerpSpeed * speedFactor * Time.fixedDeltaTime
        );
    }

    //回転適応
    void ApplySteering()
    {
        float rotation = currentSteerAngle * Time.fixedDeltaTime;
        Quaternion delta = Quaternion.Euler(0f, rotation, 0f);
        rb.MoveRotation(rb.rotation * delta);
    }
}
