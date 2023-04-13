using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private TowerData[] towerData;

    [SerializeField]
    private EnemySpawner enemySpawner;

    [SerializeField]
    private GameObject TowerPrefab;

    [SerializeField]
    private Tilemap WallMap;

    public void SpawnTower()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int tilePosition = MapDirector.Instance.WallMap.WorldToCell(worldpos);

            if (MapDirector.Instance.WallMap.HasTile(tilePosition))
            {
                GameObject Tower = Instantiate(TowerPrefab, transform.position, Quaternion.identity);
                TowerWeapon towerWeapon = Tower.GetComponent<TowerWeapon>();
                towerWeapon.SetUp(this, enemySpawner);

                Vector3 CenterPosition = WallMap.GetCellCenterWorld(tilePosition);
                CenterPosition -= WallMap.cellGap / 2;

                Tower.transform.position = CenterPosition;

            }
            //MapDirector.Instance.WallMap.HasTile(new Vector3Int(worldpos.x, worldpos.y, 0));
        }


    }
}
