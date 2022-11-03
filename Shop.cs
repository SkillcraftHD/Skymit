using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public ShopItem[] selection = new ShopItem[3];

    public RectTransform upgradesBoughtParent;
    public GameObject upgradeBought;
    List<UpgradeBoughtItem> upgradesBoughtList = new List<UpgradeBoughtItem>();

    GameManager gameManager;
    Player player;

    public List<Upgrade> upgradeList = new List<Upgrade>();

    int[] upgradeIndex = new int[3];

    public Sprite player_Vampire;
    public GameObject upgradeEffect;
    Transform cam;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        player = FindObjectOfType<Player>();
        cam = FindObjectOfType<Camera>().transform;
    }

    public void EnterShop()
    {
        for (int i = 0; i < selection.Length; i++)
        {
            upgradeIndex[i] = GetUpgrade(i);

            string upgradesLeft;

            if (upgradeIndex[i] == -1)  // If no upgrade assigned
            {
                selection[i].name.text = "Out of Stock";
                selection[i].description.text = "<color=black>" + "All other upgrades are out of stock." + "</color>";
                selection[i].improvement.text = null;
                selection[i].inStock.text = null;
                selection[i].cost.text = null;
                //selection[i].image.sprite = null;     // [Replace once actual sold out sprite exists]
                selection[i].button.interactable = false;
                return;
            }

            if (upgradeList[upgradeIndex[i]].maxUpgrades > 10000)
                upgradesLeft = "endless in stock";
            else
                upgradesLeft = upgradeList[upgradeIndex[i]].maxUpgrades - upgradeList[upgradeIndex[i]].upgradesDone + " in stock";

            bool enoughScrews = gameManager.screws >= CalculateCost(upgradeList[upgradeIndex[i]]);
            selection[i].button.interactable = enoughScrews;

            if (enoughScrews)
                selection[i].cost.color = Color.green;
            else
                selection[i].cost.color = Color.red;

            selection[i].name.text = upgradeList[upgradeIndex[i]].name;
            selection[i].description.text = upgradeList[upgradeIndex[i]].description;
            selection[i].improvement.text = GetImprovement(upgradeList[upgradeIndex[i]]);
            selection[i].inStock.text = upgradesLeft;
            selection[i].cost.text = CalculateCost(upgradeList[upgradeIndex[i]]).ToString();
            selection[i].image.sprite = upgradeList[upgradeIndex[i]].sprite;
        }
    }

    string GetImprovement(Upgrade _upgrade)
    {
        string improvement = "";

        if (_upgrade.name == "Bounty")
            improvement = gameManager.enemiesForScrew + " > " + (gameManager.enemiesForScrew - 1);
        else if (_upgrade.name == "Faster Bullets")
            improvement = player.bulletSpeed + "m/s > " + (player.bulletSpeed + 1) + "m/s";
        else if (_upgrade.name == "Move Speed")
            improvement = player.moveSpeed + "m/s > " + (player.moveSpeed + 1) + "m/s";
        else if (_upgrade.name == "Reload Speed")
            improvement = player.shootCooldown + "s > " + (player.shootCooldown * 0.8f) + "s";
        else if (_upgrade.name == "Stronger Bullets")
            improvement = Bullet.strength + " > " + (Bullet.strength + 1);
        else if (_upgrade.name == "Vampire")
            improvement = gameManager.enemiesForHP + " > " + (gameManager.enemiesForHP - 1);

        return improvement;
    }

    public void LeaveShop()
    {
        gameManager.isInShop = false;
        gameObject.SetActive(false);
    }

    int GetUpgrade(int _index)
    {
        int upgrade = Random.Range(0, upgradeList.Count);

        int tries = upgradeList.Count;

        while (IsSameUpgrade(upgrade, _index) && tries >= 0)
        {
            if (upgrade >= upgradeList.Count - 1)
                upgrade = 0;
            else
                upgrade++;

            tries--;
        }

        if (tries <= -1)    // If no upgrade assigned
            return -1;      // Then return explicit value
        else
            return upgrade;
    }

    bool IsSameUpgrade(int _upgrade, int _index)
    {
        if (_index == 0)
            return false;
        if (_index == 1)
            return upgradeList[_upgrade] == upgradeList[upgradeIndex[0]];
        else
            return (upgradeList[_upgrade] == upgradeList[upgradeIndex[0]]) || (upgradeList[_upgrade] == upgradeList[upgradeIndex[1]]);
    }

    public void SelectUpgrade(int _num)
    {
        gameManager.isInShop = false;

        gameManager.screws -= CalculateCost(upgradeList[upgradeIndex[_num]]);

        if (upgradeList[upgradeIndex[_num]].maxUpgrades < 10000)    // Makes sure to truly make it infinite upgrades
            upgradeList[upgradeIndex[_num]].upgradesDone++;

        ApplyUpgrade(upgradeList[upgradeIndex[_num]], _num);
    }

    int CalculateCost(Upgrade _upgrade)
    {
        if (_upgrade.upgradesDone == 0)
            return _upgrade.cost;
        else if (_upgrade.upgradesDone == 1)
            return _upgrade.secondCost;
        else if (_upgrade.upgradesDone == 2)
            return _upgrade.thirdCost;
        else
            return _upgrade.thirdCost + _upgrade.costGain * (_upgrade.upgradesDone - 3);
    }

    void ApplyUpgrade(Upgrade _upgrade, int _num)
    {
        switch (_upgrade.name)
        {
            case "Move Speed":
                player.moveSpeed += 1f;
                break;
            case "Bounty":
                gameManager.enemiesForScrew--;
                gameManager.bounty = true;
                break;
            case "Faster Bullets":
                Bullet.currentSpeed += 5f;
                break;
            case "Reload Speed":
                player.shootCooldown *= 0.8f;
                break;
            case "Spike Bullets":
                Bullet.strength++;
                player.shootSpikeBullet = true;
                break;
            case "Stronger Bullets":
                Bullet.strength++;
                break;
            case "Vampire":
                gameManager.enemiesForHP--;
                gameManager.vampire = true;
                player.GetComponent<SpriteRenderer>().sprite = player_Vampire;
                break;

            default:
                player.moveSpeed += 1f;
                break;
        }

        AddUpgradeBought(_upgrade, _num);

        GameObject _effect = Instantiate(upgradeEffect);
        _effect.transform.SetParent(cam.transform, false);
    }

    void AddUpgradeBought(Upgrade _upgrade, int _num)
    {
        if (upgradeList[upgradeIndex[_num]].maxUpgrades - upgradeList[upgradeIndex[_num]].upgradesDone <= 0)
            upgradeList.RemoveAt(upgradeIndex[_num]);

        if (_upgrade.upgradesDone > 1)
        {
            for (int i = 0; i < upgradesBoughtList.Count; i++)
            {
                if (upgradesBoughtList[i].upgrade == _upgrade)
                {
                    upgradesBoughtList[i].level.text = _upgrade.upgradesDone.ToString();
                    break;
                }
            }
        }
        else
        {
            UpgradeBoughtItem _upgradeBoughtItem = new UpgradeBoughtItem();

            RectTransform _upgradeBought = Instantiate(upgradeBought, upgradesBoughtParent).GetComponent<RectTransform>();
            _upgradeBought.GetChild(0).GetComponent<Image>().sprite = _upgrade.sprite;
            _upgradeBought.GetChild(1).GetComponent<TextMeshProUGUI>().text = _upgrade.upgradesDone.ToString();
            _upgradeBoughtItem.image = _upgradeBought.GetChild(0).GetComponent<Image>();
            _upgradeBoughtItem.level = _upgradeBought.GetChild(1).GetComponent<TextMeshProUGUI>();
            _upgradeBoughtItem.upgrade = _upgrade;

            upgradesBoughtList.Add(_upgradeBoughtItem);
        }

        gameObject.SetActive(false);
    }
}

[System.Serializable]
public struct ShopItem
{
    public TextMeshProUGUI name;
    public TextMeshProUGUI description;
    public TextMeshProUGUI improvement;
    public TextMeshProUGUI inStock;
    public TextMeshProUGUI cost;

    public Image image;
    public Button button;
}
struct UpgradeBoughtItem
{
    public Image image;
    public TextMeshProUGUI level;

    public Upgrade upgrade;
}