using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform player;

    private void Awake()
    {
        player = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, player.position.y + 4f, transform.position.z);
    }
}