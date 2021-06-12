using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float translationSpeed = 10;
    public float rotationSpeed = 10;
    private Vector2 moveInput;
    private float altitudeMoveInput;
    private Vector2 lookInput;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.visible = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float xTranslation = moveInput.x * translationSpeed;
        float yTranslation = altitudeMoveInput * translationSpeed;
        float zTranslation = moveInput.y * translationSpeed;
        Vector3 vTranslation = new Vector3(xTranslation, yTranslation, zTranslation);
        Quaternion horizontalRotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, new Vector3(0, 1, 0));
        rb.velocity = horizontalRotation * vTranslation;

        float xRotation = -lookInput.y * Time.deltaTime * rotationSpeed; // Up/down
        float yRotation = lookInput.x * Time.deltaTime * rotationSpeed; //right/left
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
}
