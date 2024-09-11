using UnityEngine;

public class FollowPlayerUIView : MonoBehaviour
{
    public Transform player;  // Assign your player transform in the Inspector
    public float YOffset = -40;
    public float XOffset = -70;

    void Update()
    {
        this.transform.position = player.position + new Vector3(XOffset, YOffset);

    }
}
