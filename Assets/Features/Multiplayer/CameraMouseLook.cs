using UnityEngine;

public class CameraMouseLook : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float mouseSensitivity = 100f;

    [SerializeField] private float maxVerticalAngle = 80f;

    private float xRotation = 0f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        if (playerTransform == null)
        {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxVerticalAngle, maxVerticalAngle);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerTransform.Rotate(Vector3.up * mouseX);
    }
}
