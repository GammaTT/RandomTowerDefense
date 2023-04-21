using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerDataViewer : MonoBehaviour
{
    [SerializeField]
    private Image towerImage;
    [SerializeField]
    private TextMeshProUGUI textDamage;
    [SerializeField]
    private TextMeshProUGUI textRate;
    [SerializeField]
    private TextMeshProUGUI textRange;

    private TowerWeapon currentTowerWeapon;
    private void Awake()
    {
        OffPanel();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }

    public void OnPanel(Transform tower)
    {
        currentTowerWeapon = tower.GetComponent<TowerWeapon>();
        this.gameObject.SetActive(true);
        UpdateTowerData();

    }
    public void OffPanel()
    {
        this.gameObject.SetActive(false);
        //TowerAttackRange.OffAttackRange();
    }

    public void UpdateTowerData()
    {
        towerImage.sprite = currentTowerWeapon.towerSprite;
        textDamage.text = "Damage : " + currentTowerWeapon.damage;
        textRate.text = "Rate : " + currentTowerWeapon.rate;
        textRange.text = "Range : " + currentTowerWeapon.range;
        //textLevel.text = "Level : " + currentTower.Level;
        //textUpgradeCost.text = currentTower.UpgradeCost.ToString();
        //textSellCost.text = currentTower.SellCost.ToString();
    }

    public void UpGradeTowerButton()
    {
        currentTowerWeapon.UPGrade();
        UpdateTowerData();
    }
}
