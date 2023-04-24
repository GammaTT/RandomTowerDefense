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

    private bool isTowerSpawnMode = false;
    private bool isTowerCombineMode = false;
    private bool activeSomeThingButton = false;
    private RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {
        randomTowerSpawnerImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            isTowerSpawnMode = false;
            isTowerCombineMode = false;
            activeSomeThingButton = false;
            randomTowerSpawnerImage.SetActive(false);
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
        }


    }

    public void RandomTowerSpawnerButton()
    {
        //Debug.Log("enable image");
        isTowerSpawnMode = true;
        activeSomeThingButton = true;
        randomTowerSpawnerImage.gameObject.SetActive(true);
    }
    public void TowerCombinationButton()
    {
        isTowerCombineMode = true;
        activeSomeThingButton = true;
        randomTowerSpawnerImage.gameObject.SetActive(true);
    }
}
