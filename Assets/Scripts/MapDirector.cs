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
    private GameObject Goal;
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
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            AStarNode Wall = aStarGrid.GetNodeFromWorld(worldPos);
            Vector3Int cellPosition = WalkableMap.WorldToCell(worldPos);

            if (CheckPath(Wall) == false)
            {
                Debug.Log("Can not build wall tile there");
                Wall.isWalkable = true;
                return;
            }

            if (WallMap.HasTile(cellPosition))
            {
                return;
            }
            else if (NonWallMap.HasTile(cellPosition))
            {
                Debug.Log("nonwallmap");
                return;
            }

            WallMap.SetTile(cellPosition, WallTile);
            WalkableMap.SetTile(cellPosition, null);

            aStarGrid.ResetNode();
            aStarGrid.CreateGrid();
            enemySpawner.CheckPathForAllEnemy();
            showPath.ShowPath();
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
        AStarNode EndNode = aStarGrid.GetNodeFromWorld(Goal.transform.position);

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
        AStarNode EndNode = aStarGrid.GetNodeFromWorld(Goal.transform.position);

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
        AStarNode EndNode = aStarGrid.GetNodeFromWorld(Goal.transform.position);

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
