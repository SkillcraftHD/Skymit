using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bullet;
    public GameObject spikeBullet;

    [HideInInspector]
    public bool shootSpikeBullet;

    [HideInInspector]
    public float movement;

    Rigidbody2D rb;
    GameManager gameManager;

    [HideInInspector]
    public float shootCooldown = 1f, speed = 5f, moveSpeed = 6.5f;
    float currentShootCooldown;

    [HideInInspector]
    public float bulletSpeed = 1f;

    AudioManager audioManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        currentShootCooldown = shootCooldown;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            GetHit();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Screw"))
        {
            speed += 0.5f;
            gameManager.screws++;
            audioManager.PlaySound(1);
            Destroy(collision.gameObject);
        }
    }
    void GetHit()
    {
        gameManager.HP--;

        if (gameManager.HP <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Move(int _direction)
    {
        if (Application.isMobilePlatform)
            movement = (-0.5f + _direction) * 2f;
    }
    public void StopMove()
    {
        if (Application.isMobilePlatform)
            movement = 0f;
    }

    private void Update()
    {
        if (currentShootCooldown < 0f && !gameManager.isInShop && gameManager.isPlaying)
        {
            currentShootCooldown = shootCooldown;

            if (!shootSpikeBullet)
                Instantiate(bullet, transform.position, Quaternion.identity);
            else
                Instantiate(spikeBullet, transform.position, Quaternion.identity);

            audioManager.PlaySound(0);
        }

        currentShootCooldown -= Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (gameManager.isPlaying)
            rb.velocity = new Vector2(movement * moveSpeed, speed);
        else
            rb.velocity = new Vector2(movement * moveSpeed, 0f);
    }
}