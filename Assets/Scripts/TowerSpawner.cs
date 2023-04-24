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
    private GameObject[] TowerPrefab;

    [SerializeField]
    private GameObject TestTower;

    [SerializeField]
    private Tilemap WallMap;

    private List<GameObject> towerList;

    // Start is called before the first frame update
    void Start()
    {
        towerList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
/*        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 raycasyPoint = worldpos;

            RaycastHit2D hit = Physics2D.Raycast(raycasyPoint, Vector2.zero);

            if (hit.transform == null || !hit.transform.CompareTag("TileMap"))
            {
                Debug.Log("Not TileMap");
                return;
            }


            AStarNode WallNode = MapDirector.Instance.AStarGrid_.GetNodeFromWorld(worldpos);

            if (WallNode.isBuildTower)
            {
                return;
            }

            Vector3Int tilePosition = MapDirector.Instance.WallMap.WorldToCell(worldpos);

            if (MapDirector.Instance.WallMap.HasTile(tilePosition))
            {

                GameObject Tower = Instantiate(TowerPrefab[Random.Range(0, TowerPrefab.Length)], transform.position, Quaternion.identity);
                //GameObject Tower = Instantiate(TowerPrefab[1], transform.position, Quaternion.identity);
                TowerWeapon towerWeapon = Tower.GetComponent<TowerWeapon>();
                towerWeapon.SetUp(this, enemySpawner);

                Vector3 CenterPosition = WallMap.GetCellCenterWorld(tilePosition);
                CenterPosition -= WallMap.cellGap / 2;

                Tower.transform.position = CenterPosition;

                WallNode.isBuildTower = true;
            }
            //MapDirector.Instance.WallMap.HasTile(new Vector3Int(worldpos.x, worldpos.y, 0));
        }*/


    }

    public void SpawnTower(Vector2 towerSpawnPosition)
    {
        AStarNode WallNode = MapDirector.Instance.AStarGrid_.GetNodeFromWorld(towerSpawnPosition);

        if (WallNode.isBuildTower)
        {
            return;
        }

        Vector3Int tilePosition = MapDirector.Instance.WallMap.WorldToCell(towerSpawnPosition);

        //이 함수를 부르는 PanelGameManager의 함수는 CompareTag로 WallMap 인지 확인을 했기 때문에 hastile 로 체크하던거 없앰
        GameObject Tower = Instantiate(TowerPrefab[Random.Range(0, TowerPrefab.Length)], transform.position, Quaternion.identity);

        towerList.Add(Tower);
        //GameObject Tower = Instantiate(TestTower, transform.position, Quaternion.identity);
        TowerWeapon towerWeapon = Tower.GetComponent<TowerWeapon>();
        Tower tower = Tower.GetComponent<Tower>();
        towerWeapon.SetUp(this, enemySpawner);
        tower.SetUp(this, WallNode);

        Vector3 CenterPosition = WallMap.GetCellCenterWorld(tilePosition);
        CenterPosition -= WallMap.cellGap / 2;

        Tower.transform.position = CenterPosition;
        WallNode.isBuildTower = true;
        //MapDirector.Instance.WallMap.HasTile(new Vector3Int(worldpos.x, worldpos.y, 0));
    }

    public void CombineTower(GameObject Tower)
    {
        TowerWeapon towerWeapon = Tower.GetComponent<TowerWeapon>();
        WeaponType towerWeaponType = towerWeapon.weaponType;
        int SearchCount = 0;
        //GameObject [] SameTower = new GameObject[3];
        int [] findSameTower = new int[3];

        Debug.Log("CombineTower");

        for (int i = 0; i < towerList.Count; i++)
        {
            TowerWeapon towerWeaponInList = towerList[i].GetComponent<TowerWeapon>();
            if (towerWeaponInList.weaponType == towerWeapon.weaponType)
            {
                findSameTower[SearchCount] = i;
                SearchCount++;

                if (SearchCount == 3)
                {
                    break;
                }
            }
        }

        Debug.Log("CombineTower2");

        for (int i = 0; i < 3; i++)
        {
            Tower towerScript = towerList[findSameTower[i]].GetComponent<Tower>();
            towerScript.DestoryThisTower();
            //towerScript.towerNode.isBuildTower = false;
            //towerList.Remove(towerList[findSameTower[i]]);
            //Destroy(towerList[findSameTower[i]].gameObject);
        }
        //Debug.Log(towerWeaponType.ToString());
    }
    public void DestoryTower(GameObject Tower)
    {
        towerList.Remove(Tower);
        Destroy(Tower.gameObject);
    }
}
