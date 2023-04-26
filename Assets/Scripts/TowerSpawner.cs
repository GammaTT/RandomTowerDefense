using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerSpawner : MonoBehaviour
{
    public bool testTowerOn;


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

        GameObject Tower;
        if (testTowerOn)
        {
            Tower = Instantiate(TestTower, transform.position, Quaternion.identity);
        }
        else
        {
            Tower = Instantiate(TowerPrefab[Random.Range(0, TowerPrefab.Length)], transform.position, Quaternion.identity);
        }

        towerList.Add(Tower);
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
        List <GameObject> SameTower = new List<GameObject>();
        int [] findSameTower = new int[3];

        Debug.Log(towerList.Count);

        for (int i = 0; i < towerList.Count; i++)
        {
            TowerWeapon towerWeaponInList = towerList[i].GetComponent<TowerWeapon>();
            if (towerWeaponInList.weaponType == towerWeapon.weaponType)
            {
                SameTower.Add(towerList[i]);
                findSameTower[SearchCount] = i;
                SearchCount++;

                if (SearchCount == 3)
                {
                    //List 에서 하나씩 인덱스로 삭제 했더니 
                    //삭제 후 리스트 가 알아서 재정비해서 인덱스가 안맞아서 한번에 삭제하는식으로 함
                    foreach (GameObject materialTower in SameTower)
                    {
                        Tower towerScript = materialTower.GetComponent<Tower>();
                        towerScript.DestoryThisTower();
                    }
                    //SameTower.RemoveAt();
                    break;
                }
            }
        }

        //Debug.Log(towerWeaponType.ToString());
    }

    public void CombineTower(List <GameObject> combineList)
    {

    }
    public void DestoryTower(GameObject Tower)
    {
        towerList.Remove(Tower);
        Destroy(Tower.gameObject);
    }
}
