using System.Collections;
using UnityEngine;

public class TriggerObjects : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    GameManager gameManager;

    Color normal;
    public Color hover;

    public GameObject settings;

    string task;

    bool isTouching;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        normal = spriteRenderer.color;
        task = transform.parent.name;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        spriteRenderer.color = hover;

        if (task == "Settings")
        {
            settings.SetActive(true);
            return;
        }

        if (!isTouching)
            StartCoroutine(WaitTillAction());
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        spriteRenderer.color = normal;
        isTouching = false;

        if (task == "Settings")
            settings.SetActive(false);
    }

    IEnumerator WaitTillAction()
    {
        isTouching = true;

        float time = 3f;

        while (time > 0f)
        {
            time -= Time.deltaTime;
            yield return null;

            if (!isTouching)
                yield break;
        }

        if (task == "Quit")
            Application.Quit();
        else if (task == "Play")
            gameManager.isPlaying = true;
    }
}