using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    Transform player;

    GameManager gameManager;
    CameraManager cameraManager;
    WorldGen worldGen;

    public GameObject bulletDestroyEffect;

    public const float speed = 25f;

    public static float currentSpeed;
    public static int strength;

    [HideInInspector]
    public int strengthLeft;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>().transform;

        gameManager = FindObjectOfType<GameManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        worldGen = FindObjectOfType<WorldGen>();
    }
    private void Start()
    {
        rb.velocity = transform.up * currentSpeed;
        strengthLeft = strength;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            GameObject enemy = collision.gameObject;

            if (enemy.TryGetComponent<Bird>(out var bird))
                Instantiate(bird.dieEffect, transform.position, bird.dieEffect.transform.rotation);
            else if (enemy.TryGetComponent<Bat>(out var bat))
                Instantiate(bat.dieEffect, transform.position, bat.dieEffect.transform.rotation);

            cameraManager.CameraShake(5f, 0.2f);

            Destroy(enemy.gameObject);
        }

        CheckUpgrades();

        strengthLeft--;

        if (strengthLeft <= 0)
            DestroyBullet();
    }

    void DestroyBullet()
    {
        Instantiate(bulletDestroyEffect, transform.position, bulletDestroyEffect.transform.rotation);
        Destroy(gameObject);
    }
    void CheckUpgrades()
    {
        if(gameManager.bounty && --gameManager.enemiesLeftForScrew <= 0)
        {
            gameManager.enemiesLeftForScrew = gameManager.enemiesForScrew;
            Instantiate(worldGen.screw, transform.position, Quaternion.identity);
        }
        if(gameManager.vampire && --gameManager.enemiesLeftForHP <= 0)
        {
            gameManager.enemiesLeftForHP = gameManager.enemiesForHP;
            gameManager.HP++;
        }
    }

    private void Update()
    {
        if (transform.position.y < player.position.y - 10f || transform.position.y > player.position.y + 10f)
            Destroy(gameObject);
    }
}