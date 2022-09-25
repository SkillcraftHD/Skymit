using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TriggerObjects : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    GameManager gameManager;

    Color normal;
    public Color hover;

    public GameObject settings;
    public Image slider;

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
        else
            slider.fillAmount = 0f;
    }

    IEnumerator WaitTillAction()
    {
        isTouching = true;

        float time = 1.5f;

        while (time > 0f)
        {
            time -= Time.deltaTime;
            slider.fillAmount = 1f - (2f / 3f * time);
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