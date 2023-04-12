using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDirector : MonoBehaviour
{
    static public MapDirector Instance;

    public AStarGrid AStarGrid_;
    public Tilemap WalkableMap;
    public Tilemap WallMap;
    public Tile WallTile;

    [SerializeField]
    private GameObject Goal;
    private Tile GoalTile;
    private AStarNode GoalNode;

    public List<AStarNode> StartToEndPath;

    private void Awake()
    {
        Instance = this;
        //AStarGird_ = GetComponent<AStarGrid>();
        AStarGrid_ = new AStarGrid();
        AStarGrid_.SetUp(WalkableMap, WallMap);
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
            AStarNode Wall = AStarGrid_.GetNodeFromWorld(worldPos);

            Vector3Int cellPosition = WalkableMap.WorldToCell(worldPos);

            WallMap.SetTile(cellPosition, WallTile);
            WalkableMap.SetTile(cellPosition, null);

            AStarGrid_.ResetNode();
            AStarGrid_.CreateGrid();
        }
    }
    public List<AStarNode> SetPathFromPosition(Transform StartPosition)
    {

        AStarNode StartNode = AStarGrid_.GetNodeFromWorld(StartPosition.position);
        AStarNode EndNode = AStarGrid_.GetNodeFromWorld(Goal.transform.position);

        StartToEndPath = new List<AStarNode>(AStarGrid_.pathfinder.CreatePath(StartNode, EndNode));

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

}
