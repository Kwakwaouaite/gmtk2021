using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float m_TranslationSpeed = 10;
    [SerializeField]
    float m_RotationSpeed = 10;
    Vector2 moveInput;
    float altitudeMoveInput;
    Vector2 lookInput;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float xTranslation = moveInput.x * m_TranslationSpeed;
        float yTranslation = altitudeMoveInput * m_TranslationSpeed;
        float zTranslation = moveInput.y * m_TranslationSpeed;
        Vector3 vTranslation = new Vector3(xTranslation, yTranslation, zTranslation);
        Quaternion horizontalRotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, new Vector3(0, 1, 0));
        rb.velocity = horizontalRotation * vTranslation;

        float xRotation = -lookInput.y * Time.deltaTime * m_RotationSpeed; // Up/down
        float yRotation = lookInput.x * Time.deltaTime * m_RotationSpeed; //right/left
        transform.Rotate(new Vector3(1, 0, 0), xRotation, Space.Self);
        transform.Rotate(new Vector3(0, 1, 0), yRotation, Space.World);
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnAltitudeMove(InputValue value)
    {
        altitudeMoveInput = value.Get<float>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnPause()
    {
        GameManager.Instance.Pause();
    }
}
