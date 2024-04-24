using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] float yOffset;
    [SerializeField] float zOffset;

    void Update() =>
        transform.position = new Vector3(player.transform.position.x, 
            player.transform.position.y + yOffset, player.transform.position.z - zOffset);
}
