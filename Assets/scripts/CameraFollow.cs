using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;      // Reference to the player's transform
    public Vector3 offset;        // Offset from the player
    public float followSpeed = 10f; // Speed of the camera following

    private void Start()
    {
        // Set the initial offset based on current camera position
        offset = transform.position - player.position;
    }

    private void LateUpdate()
    {
        // Calculate the desired position
        Vector3 desiredPosition = player.position + offset;

        // Smoothly move the camera towards the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
}