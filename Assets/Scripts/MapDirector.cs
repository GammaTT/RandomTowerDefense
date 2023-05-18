using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
public class MapDirector : MonoBehaviour
{
    static public MapDirector Instance;

    public AStarGrid aStarGrid;
    public Tilemap WalkableMap;
    public Tilemap WallMap;
    public Tilemap NonWallMap;
    public Tile WallTile;
    public Tile WalkableTile;

    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject goal;
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private Pathfinder showPath;

    private Tile GoalTile;
    private AStarNode GoalNode;

    public List<AStarNode> StartToEndPath;

    public GameObject Boo;

    private void Awake()
    {
        Instance = this;
        //AStarGird_ = GetComponent<AStarGrid>();
        aStarGrid = new AStarGrid();
        aStarGrid.SetUp(WalkableMap, WallMap);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (enemySpawner.enemyList.Count > 0)
            {
                Debug.Log("적이 있을 때는 벽 설치 불가");
                return;
            }
            else if (player.gold < Constants.spawnWallGold)
            {
                Debug.Log("벽을 지을 골드가 부족합니다.");
            }

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 raycasyPoint = worldPos;
            //RaycastHit2D hit = Physics2D.Raycast(raycasyPoint, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(raycasyPoint, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Tile"));

            if (hit.transform == null || !hit.transform.CompareTag("WalkableMap"))
            {
                Debug.Log("필드 안에 설치해야됩니다.");
                return;
            }

            AStarNode Wall = aStarGrid.GetNodeFromWorld(worldPos);
            Vector3Int cellPosition = WalkableMap.WorldToCell(worldPos);

            if (CheckPath(Wall) == false)
            {
                Debug.Log("길을 막습니다.");
                Wall.isWalkable = true;
                return;
            }

            //null 체크와 WalkableMap에서 있는지 체크를 했기 때문에 필요가 없다.
/*            if (WallMap.HasTile(cellPosition))
            {
                return;
            }
            else if (NonWallMap.HasTile(cellPosition))
            {
                return;
            }
            else if (!WalkableMap.HasTile(cellPosition))
            {
                return;
            }*/

            WallMap.SetTile(cellPosition, WallTile);
            WalkableMap.SetTile(cellPosition, null);

            aStarGrid.ResetNode();
            aStarGrid.CreateGrid();
            enemySpawner.CheckPathForAllEnemy();
            showPath.ShowPath();
            player.gold -= Constants.spawnWallGold;
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public bool CheckPath(AStarNode WallNode)
    {
        WallNode.isWalkable = false;
        AStarNode StartNode = aStarGrid.GetNodeFromWorld(enemySpawner.gameObject.transform.position);
        AStarNode EndNode = aStarGrid.GetNodeFromWorld(goal.transform.position);

        List<AStarNode> Path = new List<AStarNode>();
        Path = aStarGrid.pathfinder.CreatePath(StartNode, EndNode);

        if (Path == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool CheckPath(Vector3Int cellPosition)
    {
        WallMap.SetTile(cellPosition, WallTile);
        WalkableMap.SetTile(cellPosition, null);

        aStarGrid.ResetNode();
        aStarGrid.CreateGrid();

        AStarNode StartNode = aStarGrid.GetNodeFromWorld(enemySpawner.gameObject.transform.position);
        AStarNode EndNode = aStarGrid.GetNodeFromWorld(goal.transform.position);

        List <AStarNode> Path = new List <AStarNode>();
        Path = aStarGrid.pathfinder.CreatePath(StartNode, EndNode);

        if (Path == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public List<AStarNode> SetPathFromPosition(Transform StartPosition)
    {

        AStarNode StartNode = aStarGrid.GetNodeFromWorld(StartPosition.position);
        AStarNode EndNode = aStarGrid.GetNodeFromWorld(goal.transform.position);

        StartToEndPath = new List<AStarNode>();
        //StartToEndPath = new List<AStarNode>(AStarGrid_.pathfinder.CreatePath(StartNode, EndNode));

        StartToEndPath = aStarGrid.pathfinder.CreatePath(StartNode, EndNode);

        for (int i = 0; i < StartToEndPath.Count; i++)
        {
            Vector3Int NodePosition = WalkableMap.WorldToCell(new Vector3(StartToEndPath[i].xPos, StartToEndPath[i].yPos));
            Vector3 CenterPosition = WalkableMap.GetCellCenterWorld(NodePosition);
            CenterPosition -= WalkableMap.cellGap / 2;

            StartToEndPath[i].xPos = CenterPosition.x;
            StartToEndPath[i].yPos = CenterPosition.y;
        }
 
        return StartToEndPath;
    }

    public Vector3 GetEnemySpanwerPosition()
    {
        return enemySpawner.gameObject.transform.position;
    }
}
