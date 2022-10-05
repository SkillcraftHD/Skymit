using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject bird, bat, star;
    public Transform cam;

    GameManager gameManager;
    Transform player;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        player = FindObjectOfType<Player>().transform;
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitUntil(() => gameManager.isPlaying);
        yield return new WaitUntil(() => !gameManager.spawnLock);
        yield return new WaitUntil(() => !gameManager.isInShop);

        yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));

        if (gameManager.score < WorldGen.areaDistance[0])
        {
            int side = Random.Range(0, 2);
            Instantiate(bird, new Vector2(15f * (1f - 2f * side), player.position.y + Random.Range(12.5f, 20f)), Quaternion.identity);
        }
        else if (gameManager.score < WorldGen.areaDistance[1])
            Instantiate(bat, new Vector2(Random.Range(-7.5f, 7.5f), player.position.y + 12.5f), Quaternion.identity);
        else if (gameManager.score < WorldGen.areaDistance[2])
            Instantiate(star, new Vector2(Random.Range(-7.5f, 7.5f), player.position.y + 12.5f), Quaternion.identity);

        StartCoroutine(SpawnEnemies());
    }
}