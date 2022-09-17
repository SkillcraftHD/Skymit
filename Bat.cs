using UnityEngine;

public class Bat : MonoBehaviour
{
    float speed;

    Rigidbody2D rb;
    Transform player;
    GameManager gameManager;

    public GameObject dieEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>().transform;
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        speed = Random.Range(-3.5f, 3.5f);
    }

    private void Update()
    {
        if (transform.position.y < player.position.y - 10f)
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(speed, 0f);
    }
}