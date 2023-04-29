using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PanelGameManager : MonoBehaviour
{
    private int randomTowerSpawnGold = 1;
    [SerializeField]
    private Player player;
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private EnemySpawner enemySpawner;

    [SerializeField]
    private GameObject randomTowerSpawnerImage;

    private bool isTowerCellMode = false;
    private bool isTowerSpawnMode = false;
    private bool isTowerCombineMode = false;
    private bool activeSomeThingButton = false;
    private RaycastHit2D hit;

    //for MouseFollowObject Image color change
    private Renderer panelGameSystemMouseImageRenderer;
    private Material panelMaterial;
    // Start is called before the first frame update
    void Start()
    {
        panelGameSystemMouseImageRenderer = randomTowerSpawnerImage.gameObject.GetComponent<Renderer>();
        panelMaterial = panelGameSystemMouseImageRenderer.material;
        randomTowerSpawnerImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            CancelSomethingButton();
        }

        if (activeSomeThingButton)
        {
            randomTowerSpawnerImage.transform.position = MousePosition;
        }

        if (Input.GetMouseButtonDown(0) && activeSomeThingButton)
        {
            hit = Physics2D.Raycast(MousePosition, Vector2.zero);

            if (isTowerSpawnMode)
            {
                if (player.gold > randomTowerSpawnGold)
                {
                    if (hit.transform == null || !hit.transform.CompareTag("WallMap"))
                    {
                        return;
                    }

                    towerSpawner.SpawnTower(MousePosition);
                    player.gold -= randomTowerSpawnGold;

                }
            }
            else if (isTowerCombineMode)
            {
                if (hit.transform == null || !hit.transform.CompareTag("Tower"))
                {
                    return;
                }

                towerSpawner.CombineTower(hit.transform.gameObject);
            }
            else if (isTowerCellMode)
            {
                if (hit.transform == null || !hit.transform.CompareTag("Tower"))
                {
                    return;
                }

                towerSpawner.CellTower(hit.transform.gameObject);
            }
        }

    }

    private void CancelSomethingButton()
    {
        isTowerSpawnMode = false;
        isTowerCombineMode = false;
        activeSomeThingButton = false;
        randomTowerSpawnerImage.SetActive(false);
    }
    public void RandomTowerSpawnerButton()
    {
        CancelSomethingButton();

        //Debug.Log("enable image");
        isTowerSpawnMode = true;
        activeSomeThingButton = true;
        randomTowerSpawnerImage.gameObject.SetActive(true);
        panelMaterial.color = Color.green;
    }
    public void TowerCombinationButton()
    {
        CancelSomethingButton();

        isTowerCombineMode = true;
        activeSomeThingButton = true;
        randomTowerSpawnerImage.gameObject.SetActive(true);
        panelMaterial.color = Color.blue;

    }
    public void TowerCellButton()
    {
        CancelSomethingButton();

        isTowerCellMode = true;
        activeSomeThingButton = true;
        randomTowerSpawnerImage.gameObject.SetActive(true);
        panelMaterial.color = Color.red;
    }
}
