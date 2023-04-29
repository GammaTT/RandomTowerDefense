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
    private GameObject[] towerGrade1Prefab;
    [SerializeField]
    private GameObject[] towerGrade2Prefab;

    [SerializeField]
    private GameObject TestTower;

    [SerializeField]
    private Tilemap WallMap;

    private float towerCellGoldPercent = 0.8f;
    private List<GameObject> towerList;
    private int towerCombineCount = 3;
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
            Tower = Instantiate(towerGrade1Prefab[Random.Range(0, towerGrade1Prefab.Length)], transform.position, Quaternion.identity);
        }

        towerList.Add(Tower);
        TowerWeapon towerWeapon = Tower.GetComponent<TowerWeapon>();
        Tower tower = Tower.GetComponent<Tower>();
        towerWeapon.SetUp(this, enemySpawner);
        tower.SetUp(this, WallNode);

        Vector3 CenterPosition = WallMap.GetCellCenterWorld(tilePosition);
        CenterPosition -= WallMap.cellGap / 2;

        Tower.transform.position = CenterPosition;

        //change this in tower setup
        //WallNode.isBuildTower = true;
        //MapDirector.Instance.WallMap.HasTile(new Vector3Int(worldpos.x, worldpos.y, 0));
    }

/*    public void TowerSetup(int towerGrade, Vector3 towerPosition)
    {
        GameObject Tower = new GameObject();

        if (towerGrade == 1)
        {
            Tower = Instantiate(towerGrade1Prefab[Random.Range(0, towerGrade1Prefab.Length)], transform.position, Quaternion.identity);
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
    }*/
    public void CombineTower(GameObject Tower)
    {
        TowerWeapon towerWeapon = Tower.GetComponent<TowerWeapon>();
        int towerGrade = towerWeapon.grade;
        WeaponType towerWeaponType = towerWeapon.weaponType;
        int SearchCount = 0;
        List <GameObject> SameTower = new List<GameObject>();
        int [] findSameTower = new int[towerCombineCount];
        AStarNode upGradeTowerNode; 

        Debug.Log("tower Count : " + towerList.Count);

        for (int i = 0; i < towerList.Count; i++)
        {
            TowerWeapon towerWeaponInList = towerList[i].GetComponent<TowerWeapon>();
            if (towerWeaponInList.weaponType == towerWeapon.weaponType)
            {
                SameTower.Add(towerList[i]);
                findSameTower[SearchCount] = i;
                SearchCount++;

                if (SearchCount == towerCombineCount)
                {
                    upGradeTowerNode = SameTower[Random.Range(0, towerCombineCount)].GetComponent<Tower>().towerNode;


                    //new Up Grade tower Spawn

                    //current same towers position random
                    Vector3Int tilePosition = MapDirector.Instance.WallMap.WorldToCell
                        (SameTower[Random.Range(0, towerCombineCount)].transform.position);

                    Vector3 tileCenterPosition = MapDirector.Instance.WallMap.GetCellCenterWorld(tilePosition);
                    tileCenterPosition -= WallMap.cellGap / 2;

                    GameObject upTower = Instantiate(towerGrade2Prefab[Random.Range(0, towerGrade2Prefab.Length)],
                        transform.position, Quaternion.identity);

                    towerList.Add(upTower);
                    TowerWeapon uptowerWeapon = upTower.GetComponent<TowerWeapon>();
                    Tower upTowerScript = upTower.GetComponent<Tower>();
                    uptowerWeapon.SetUp(this, enemySpawner);
                    upTowerScript.SetUp(this, MapDirector.Instance.AStarGrid_.GetNodeFromWorld(tileCenterPosition));

                    upTower.transform.position = tileCenterPosition;

                    //List 에서 하나씩 인덱스로 삭제 했더니 
                    //삭제 후 리스트 가 알아서 재정비해서 인덱스가 안맞아서 한번에 삭제하는식으로 함
                    foreach (GameObject materialTower in SameTower)
                    {
                        //before tower delete
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

    public void CellTower(GameObject cellTower)
    {
        Tower towerScript = cellTower.GetComponent<Tower>();
        TowerWeapon cellTowerWeapon = cellTower.GetComponent<TowerWeapon>();

        switch (cellTowerWeapon.grade)
        {
            case 1:
                player.gold += 5;
                break;
            case 2:
                player.gold += 15;
                break;
            case 3:
                player.gold += 45;
                break;
        }

        towerScript.DestoryThisTower();
    }
    public void DestoryTower(GameObject Tower)
    {
        towerList.Remove(Tower);
        Destroy(Tower.gameObject);
    }
}
