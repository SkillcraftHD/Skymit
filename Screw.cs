using UnityEngine;

public class Screw : MonoBehaviour
{
    Transform player;

    private void Awake()
    {
        player = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        if (transform.position.y < player.position.y - 10f)
            Destroy(gameObject);
    }
}