using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Transform player;

    bool isShaking;

    private void Awake()
    {
        player = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        if (!isShaking)
            transform.position = new Vector3(transform.position.x, player.position.y + 4f, transform.position.z);
    }

    public void CameraShake(float _strength, float _length)
    {
        StartCoroutine(DoShake(_strength, _length));
    }

    IEnumerator DoShake(float _strength, float _length)
    {
        isShaking = true;
        float origPosX = transform.position.x;

        while(_length > 0f)
        {
            Vector2 offset = _strength * 0.01f * new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            transform.position = new Vector3(transform.position.x + offset.x, player.position.y + 4f + offset.y, transform.position.z);

            _length -= Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(origPosX, transform.position.y, transform.position.z);
        isShaking = false;
    }
}