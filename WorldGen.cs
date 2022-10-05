using System.Collections;
using UnityEngine;
using TMPro;

public class WorldGen : MonoBehaviour
{
    public GameObject cloud, cloud_night, background_star;
    public GameObject screw;

    public GameObject speedEffect, blackHole, godEffect;
    public GameObject boss, bossHP;

    public Camera cam;
    GameManager gameManager;
    AudioManager audioManager;
    Rigidbody2D player;

    public TextMeshProUGUI[] itemDescription = new TextMeshProUGUI[3];

    public static readonly int[] areaDistance = { 10, 20, 100 };

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();
        player = FindObjectOfType<Player>().GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(WaitForPlay());
        StartCoroutine(WaitForNewArea());
    }

    IEnumerator WaitForPlay()
    {
        yield return new WaitUntil(() => gameManager.isPlaying);

        StartCoroutine(SpawnClouds());
        StartCoroutine(SpawnScrews());

        yield return new WaitUntil(() => gameManager.score > areaDistance[1]);

        speedEffect.SetActive(true);
        blackHole.SetActive(true);

        yield return new WaitUntil(() => gameManager.score > areaDistance[2]);

        speedEffect.SetActive(false);
        blackHole.SetActive(false);

        godEffect.SetActive(true);
        bossHP.SetActive(true);

        GameObject _boss = Instantiate(boss);
        _boss.transform.parent = cam.transform;
        _boss.transform.localPosition = new Vector3(0f, 5f, 10f);

        audioManager.PlayMusic(0);
        gameManager.fightingBoss = true;
    }

    IEnumerator SpawnClouds()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        int fromTop = Random.Range(0, 2);
        float distance = Random.Range(0f, 20f);

        GameObject cloudtoSpawn = cloud;

        if (gameManager.score > areaDistance[0])
            cloudtoSpawn = cloud_night;
        else if (gameManager.score > areaDistance[1])
            yield break;

        if (fromTop == 0)
        {
            float height = player.position.y + Random.Range(2f, 10f) + 0.6f * distance;
            Instantiate(cloudtoSpawn, new Vector3(15f + 0.6f * distance, height, distance), Quaternion.identity);
        }
        else
        {
            float height = player.position.y + 12.5f + 0.6f * distance;
            Instantiate(cloudtoSpawn, new Vector3(Random.Range(-8.5f, 8.5f), height, distance), Quaternion.identity);
        }

        StartCoroutine(SpawnClouds());
    }
    IEnumerator SpawnScrews()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 5f));

        yield return new WaitUntil(() => !gameManager.spawnLock);
        yield return new WaitUntil(() => !gameManager.isInShop);
        Instantiate(screw, player.position + Vector2.up * 15f, Quaternion.identity);

        StartCoroutine(SpawnScrews());
    }

    IEnumerator WaitForNewArea()
    {
        yield return new WaitUntil(() => gameManager.score > areaDistance[0]);

        cam.backgroundColor = new Color32(8, 17, 31, 255);

        foreach (TextMeshProUGUI _text in itemDescription)
        {
            _text.color = Color.grey;
        }
    }
}