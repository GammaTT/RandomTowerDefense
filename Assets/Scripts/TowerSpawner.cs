using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TowerGrade
{
    Grade1 = 1,
    Grade2,
    Grade3,
};
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
    private GameObject[] towerGrade1Prefabs;
    [SerializeField]
    private GameObject[] towerGrade2Prefabs;
    [SerializeField]
    private GameObject[] towerGrade3Prefabs;

    [SerializeField]
    private GameObject TestTower;

    [SerializeField]
    private Tilemap WallMap;

    public List<GameObject[]> towerTypeList;
    //private float towerCellGoldPercent = 0.8f;
    private List<GameObject> towerList;
    private int towerCombineCount = 3;

    // Start is called before the first frame update
    void Start()
    {
        towerList = new List<GameObject>();
        towerTypeList = new List<GameObject[]>
        {
            towerGrade1Prefabs,
            towerGrade2Prefabs,
            towerGrade3Prefabs
        };

        //Debug.Log(towerTypeList[2].Length);
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
        AStarNode WallNode = MapDirector.Instance.aStarGrid.GetNodeFromWorld(towerSpawnPosition);

        if (WallNode.isBuildTower)
        {
            return;
        }

        SpawnTower(towerSpawnPosition, TowerGrade.Grade1);
        return;

        /*Vector3Int tilePosition = MapDirector.Instance.WallMap.WorldToCell(towerSpawnPosition);

        //�� �Լ��� �θ��� PanelGameManager�� �Լ��� CompareTag�� WallMap ���� Ȯ���� �߱� ������ hastile �� üũ�ϴ��� ����

        GameObject Tower;
        if (testTowerOn)
        {
            Tower = Instantiate(TestTower, transform.position, Quaternion.identity);
        }
        else
        {
            Tower = Instantiate(towerGrade1Prefabs[Random.Range(0, towerGrade1Prefabs.Length)], transform.position, Quaternion.identity);
        }

        towerList.Add(Tower);
        TowerWeapon towerWeapon = Tower.GetComponent<TowerWeapon>();
        Tower tower = Tower.GetComponent<Tower>();
        towerWeapon.SetUp(this, enemySpawner);
        tower.SetUp(this, WallNode);

        Vector3 CenterPosition = WallMap.GetCellCenterWorld(tilePosition);
        CenterPosition -= WallMap.cellGap / 2;

        Tower.transform.position = CenterPosition;*/
    }
    public void SpawnTower(Vector2 towerLocation, TowerGrade grade)
    {
        AStarNode WallNode = MapDirector.Instance.aStarGrid.GetNodeFromWorld(towerLocation);

        if (WallNode.isBuildTower)
        {
            return;
        }

        //Debug.Log(grade);

        int gradeIndex = (int)(grade - 1);
        Vector3Int tilePosition = MapDirector.Instance.WallMap.WorldToCell(towerLocation);

        Vector3 tileCenterPosition = MapDirector.Instance.WallMap.GetCellCenterWorld(tilePosition);
        tileCenterPosition -= WallMap.cellGap / 2;

        GameObject spawnTower;
        if (testTowerOn)
        {
            //�׽�Ʈ �� Ÿ�� ����
            spawnTower = Instantiate(TestTower, tileCenterPosition, Quaternion.identity);
        }
        else
        {
            //������ ����� Ÿ���߿��� �������� ��ȯ, ��ġ�� Ÿ���� �߾�
            spawnTower = Instantiate(towerTypeList[gradeIndex][Random.Range(0, towerTypeList[gradeIndex].Length)],
                tileCenterPosition, Quaternion.identity);
        }

        towerList.Add(spawnTower);
        TowerWeapon spawntowerWeapon = spawnTower.GetComponent<TowerWeapon>();
        Tower spawnTowerScript = spawnTower.GetComponent<Tower>();
        spawntowerWeapon.SetUp(this, enemySpawner);
        spawnTowerScript.SetUp(this, MapDirector.Instance.aStarGrid.GetNodeFromWorld(tileCenterPosition));
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
        TowerWeapon selectTowerWeapon = Tower.GetComponent<TowerWeapon>();
        Tower selectTowerScript = Tower.GetComponent<Tower>();
        TowerGrade materialTowerGrade = selectTowerWeapon.towerGrade;
        WeaponType towerWeaponType = selectTowerWeapon.weaponType;

        Debug.Log("before combine tower Count : " + towerList.Count);

        int[] findSameTower = new int[towerCombineCount];
        List<GameObject> SameTower = new List<GameObject>();
        int SearchCount = 0;

        for (int i = 0; i < towerList.Count; i++)
        {
            TowerWeapon towerWeaponInList = towerList[i].GetComponent<TowerWeapon>();
            if (towerWeaponInList.weaponType == selectTowerWeapon.weaponType)
            {
                SameTower.Add(towerList[i]);
                findSameTower[SearchCount] = i;
                SearchCount++;

                if (SearchCount == Constants.towerCombineCount)
                {
                    //upGradeTowerNode = SameTower[Random.Range(0, towerCombineCount)].GetComponent<Tower>().towerNode;

                    //List ���� �ϳ��� �ε����� ���� �ߴ��� 
                    //���� �� ����Ʈ �� �˾Ƽ� �������ؼ� �ε����� �ȸ¾Ƽ� �ѹ��� �����ϴ½����� ��
                    foreach (GameObject materialTower in SameTower)
                    {
                        //3���� ���� ����� ��� Ÿ�� ����
                        Tower towerScript = materialTower.GetComponent<Tower>();
                        towerScript.DestoryThisTower();
                    }

                    //���� ����� Ÿ�� ����
                    SpawnTower(SameTower[Random.Range(0, towerCombineCount)].transform.position, materialTowerGrade + 1);
                    break;
                }
            }
        }

    }

    public void CellTower(GameObject cellTower)
    {
        Tower towerScript = cellTower.GetComponent<Tower>();
        TowerWeapon cellTowerWeapon = cellTower.GetComponent<TowerWeapon>();

        int cellGold = Constants.spawnRandomTowerGold;

        //��޿� ���� �⺻���� Ÿ���� �Ҹ�� ��� ���
        for (int i = 1; i < (int)cellTowerWeapon.towerGrade; i++)
        {
            cellGold *= Constants.towerCombineCount;
        }

        //���׷��̵忡 ����� ��� ���
        cellGold += cellTowerWeapon.useGoldToUpGrade;

        //������ ���� �Ǹ� ��� ���
        float cellGoldCal = cellGold * Constants.cellTowerReturnGoldMulti;
        cellGold = (int)cellGoldCal;

        player.gold += cellGold;
        towerScript.DestoryThisTower();
    }
    public void DestoryTower(GameObject Tower)
    {
        towerList.Remove(Tower);
        Destroy(Tower.gameObject);
    }
}
