using UnityEngine; 

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;  // reference to the player object
    private Vector3 offset = new Vector3(0, 3, -10);  // the offset distance of the camera from the player

    void Update()
    {
        // set the camera position to the player's position plus the offset
        transform.position = player.transform.position + offset;
    }
}
