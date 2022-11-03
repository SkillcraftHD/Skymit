using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    CameraManager cameraManager;

    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    Vector3 startPos;

    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        startPos = transform.localPosition;

        float rot = Random.Range(-35f, 35f);
        transform.localEulerAngles = new Vector3(0f, 0f, rot);
        StartCoroutine(CreateLaser());
    }

    IEnumerator CreateLaser()
    {
        transform.localScale = new Vector3(transform.localScale.x, 20f, 0f);
        transform.localPosition -= 10f * transform.up;

        float t = 0f;

        while (t < 0.5f)
        {
            transform.localScale += new Vector3(0.5f * Time.deltaTime, 0f, 0f);
            t += Time.deltaTime;
            yield return null;
        }

        cameraManager.CameraShake(10f, 0.1f);

        spriteRenderer.color = new Color32(255, 66, 66, 255);
        transform.localScale = new Vector3(0.2f, 0f, 0f);
        transform.localPosition = startPos;

        boxCollider.enabled = true;

        StartCoroutine(MoveLaser());
    }

    IEnumerator MoveLaser()
    {
        float t = 0f;
        float speed = 40f;

        while (t < 0.25f)
        {
            transform.localPosition -= speed * Time.deltaTime * transform.up;
            transform.localScale += new Vector3(0f, Time.deltaTime * 2f * speed, 0f);
            spriteRenderer.color += new Color(0f, 0f, 0f, Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}