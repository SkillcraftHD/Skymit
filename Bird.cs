using UnityEngine;

public class Bird : MonoBehaviour
{
    float speed;
    int leftSide;

    Rigidbody2D rb;
    Transform player;

    public GameObject dieEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>().transform;
    }

    private void Start()
    {
        if (transform.position.x < 0f)
        {
            leftSide = 1;
            transform.eulerAngles = new Vector2(0f, 180f);
        }
        else
            leftSide = -1;

        speed = Random.Range(5f, 7.5f);
    }

    private void Update()
    {
        if (transform.position.y < player.position.y - 10f)
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(leftSide * speed, -0.1f);
    }
}