using UnityEngine;

public class CameraFollowView : MonoBehaviour
{
    private Transform player;  // Reference to the player's transform
    public Vector3 offset;  // Offset between the player and the camera
    public float followSpeed = 2f;  // Speed at which the camera follows the player
    public float threshold = 2f;  // Distance from the center before the camera starts following

    private bool isFollowing = false;

    public void StartFollowing(GameObject Player)
    {
        player = Player.transform;
        isFollowing = true;
    }

    private void Update()
    {

        if (!isFollowing)
        {
            return;
        }

        // Calculate the player's position in the viewport
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(player.position);
        Vector3 desiredPosition = transform.position;

        // Check if the player has moved too far from the center
        if (viewportPosition.x < 0.5f - threshold || viewportPosition.x > 0.5f + threshold ||
            viewportPosition.y < 0.5f - threshold || viewportPosition.y > 0.5f + threshold)
        {
            // Calculate the new camera position with offset
            desiredPosition = player.position + offset;
            desiredPosition.z = transform.position.z;  // Keep the same z position
        }

        // Smoothly move the camera towards the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
}
