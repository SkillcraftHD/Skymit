using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    WorldGen worldGen;

    Slider HPSlider;
    public GameObject laser;

    public int HP = 100;
    int currentHP;

    private void Awake()
    {
        worldGen = FindObjectOfType<WorldGen>();
    }

    private void Start()
    {
        HPSlider = worldGen.bossHP.GetComponent<Slider>();
        HPSlider.maxValue = HP;
        HPSlider.value = HP;
        currentHP = HP;

        StartCoroutine(Attack());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
            return;

        Destroy(collision.gameObject);

        currentHP--;

        if (currentHP <= 0)
            Destroy(gameObject);

        HPSlider.value = currentHP;
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1f);

        Attack1();

        StartCoroutine(Attack());
    }

    void Attack1()
    {
        Instantiate(laser, transform.parent);
    }
}