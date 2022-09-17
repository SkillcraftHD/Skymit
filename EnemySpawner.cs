using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject bat;

    GameManager gameManager;
    Transform player;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        player = FindObjectOfType<Player>().transform;
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(0.5f);

        yield return new WaitUntil(() => !gameManager.spawnLock);
        yield return new WaitUntil(() => !gameManager.isInShop);

        if (gameManager.score < WorldGen.areaDistance[0])
            Instantiate(bat, new Vector2(0f, player.position.y + 12.5f), Quaternion.identity);

        Debug.Log("s");

        StartCoroutine(SpawnEnemies());
    }
}