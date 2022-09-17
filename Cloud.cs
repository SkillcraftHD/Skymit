using UnityEngine;

public class Cloud : MonoBehaviour
{
    float speed;

    Transform player;

    private void Awake()
    {
        player = FindObjectOfType<Player>().transform;
    }

    private void Start()
    {
        if (transform.position.x > 0f)
            speed = -1f;
        else
            speed = 1f;
    }

    void Update()
    {
        transform.position += new Vector3(speed, 0f) * Time.deltaTime;

        if (transform.position.y < player.position.y - 10f - transform.position.z)
            Destroy(gameObject);
    }
}