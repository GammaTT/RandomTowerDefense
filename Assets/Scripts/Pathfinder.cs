using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private List<AStarNode> Path;
    private float nodeArriveDistance = 0.1f;
    private float pathFindDelay = 5.0f;
    private float lastPathFindTime;
    private float nextNodeMoveTime = 0.1f;
    private float currentTime;

    [SerializeField]
    private GameObject boo;
    // Start is called before the first frame update
    void Start()
    {
        Path = new List<AStarNode>();
        currentTime = Time.time;
        SetPath();
        StartCoroutine("MoveToPath");
        lastPathFindTime = currentTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time -  lastPathFindTime > pathFindDelay)
        {
            ShowPath();
        }
    }

    public void ShowPath()
    {
        lastPathFindTime = Time.time;
        transform.position = MapDirector.Instance.GetEnemySpanwerPosition();
        StopCoroutine("MoveToPath");
        SetPath();
        StartCoroutine("MoveToPath");
    }
    public void SetPath()
    {
        Path = MapDirector.Instance.SetPathFromPosition(transform);
    }

    public IEnumerator MoveToPath()
    {
        foreach (var node in Path)
        {
            Vector2 targetPositon = new Vector2(node.xPos, node.yPos);
            Vector2 currentPosition = transform.position;
            currentTime = Time.time;
            while(true)
            {
                Vector3 move = (targetPositon - currentPosition).normalized * Time.deltaTime;
                //transform.Translate(move);
                transform.position += move;

                float u = (Time.time - currentTime) / nextNodeMoveTime;

                transform.position = Vector3.Lerp(currentPosition, targetPositon, u);

                if (Vector2.Distance(transform.position, targetPositon) < nodeArriveDistance)
                {
                    break;
                }

                yield return new WaitForEndOfFrame();
            }

        }
    }
}
