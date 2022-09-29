using UnityEngine;

public class Star : MonoBehaviour
{
    float fallSpeed;

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Transform player;

    public GameObject dieEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<Player>().transform;
    }

    private void Start()
    {
        fallSpeed = Random.Range(7.5f, 10f);

        transform.localScale *= Random.Range(0.5f, 2f);
        spriteRenderer.color = Random.ColorHSV();
    }

    private void Update()
    {
        transform.eulerAngles += new Vector3(0f, 0f, 10f * Time.deltaTime);

        if (transform.position.y < player.position.y - 10f)
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(0f, -fallSpeed);
    }
}