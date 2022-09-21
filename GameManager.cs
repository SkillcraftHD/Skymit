using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;
    [HideInInspector]
    public bool isPaused;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI screwsText;
    public TextMeshProUGUI HPText;

    public Animator scoreAnim;
    bool didMilestoneAnim;

    Rigidbody2D playerRB;
    Player player;

    [HideInInspector]
    public float score;
    int shopScore = 0;

    public Shop shop;
    [HideInInspector]
    public bool isInShop, spawnLock;

    [HideInInspector]
    public int enemiesForScrew = 7, enemiesLeftForScrew, enemiesForHP = 10, enemiesLeftForHP;
    [HideInInspector]
    public bool bounty, vampire;

    [HideInInspector]
    public int HP = 1, screws;

    [HideInInspector]
    public bool isPlaying;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Bullet.strength = 1;
        Bullet.currentSpeed = Bullet.speed;

        StartCoroutine(WaitForShop());

        foreach (Upgrade _upgrade in shop.upgradeList)
        {
            _upgrade.upgradesDone = 0;
        }

        if (Application.isMobilePlatform)
            Application.targetFrameRate = 60;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
            Pause(true);
    }

    public void Pause(bool _pause)
    {
        if (isPaused == _pause)
            return;

        isPaused = _pause;
        pauseMenu.SetActive(isPaused);

        if (isPaused)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        if(!isPlaying)
        {
            scoreText.text = null;
            screwsText.text = null;
            speedText.text = null;
            HPText.text = null;
            return;
        }
        if (!isInShop)
            score += playerRB.velocity.y * 5f * Time.deltaTime;

        if (((int)score / 100f) % 1 == 0)
        {
            if (didMilestoneAnim)
                return;

            didMilestoneAnim = true;
            scoreAnim.Play("Milestone");
        }
        else
            didMilestoneAnim = false;

        scoreText.text = score.ToString("0");
        screwsText.text = screws.ToString();
        speedText.text = "Speed: " + "\n" + (playerRB.velocity.y * 5f).ToString("0") + "m/s";
        HPText.text = "HP Left: " + HP;
    }

    IEnumerator WaitForShop()
    {
        yield return new WaitUntil(() => isPlaying);

        shopScore += 1000;

        yield return new WaitUntil(() => (int)score >= shopScore - 100);
        spawnLock = true;
        yield return new WaitUntil(() => (int)score >= shopScore);
        isInShop = true;
        spawnLock = false;

        shop.gameObject.SetActive(true);
        shop.EnterShop();

        yield return new WaitUntil(() => !isInShop);

        StartCoroutine(WaitForShop());
    }
}