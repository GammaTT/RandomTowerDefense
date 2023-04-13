using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyDestroyType { Kill = 0, Arrive }
public class Enemy : MonoBehaviour
{
    public List<AStarNode> EnemyPath;

    public float PathUpdateDelay = 0.5f;
    private float LastPathUpdate;
    private EnemySpawner enemySpawner;

    [SerializeField]
    private int gold = 10;

    public void SetUp(EnemySpawner enemySpawner)
    {
        this.enemySpawner = enemySpawner;
    }
    // Start is called before the first frame update
    void Start()
    {
        LastPathUpdate = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - LastPathUpdate > PathUpdateDelay) 
        {
            LastPathUpdate = Time.time;
            SetPath();
        }
    }

    void SetPath()
    {
        EnemyPath = new List<AStarNode>(MapDirector.Instance.SetPathFromPosition(transform));

        StopCoroutine("Move");
        StartCoroutine("Move");

        //Debug.Log(EnemyPath.Count);
    }

    public IEnumerator Move()
    {
        for (int i = 1; i < EnemyPath.Count; i++)
        {
            var node = EnemyPath[i];
            Vector2 targetPositon = new Vector2(node.xPos, node.yPos);
            Vector2 currentPosition = transform.position;

            float xDiff = targetPositon.x - currentPosition.x;
            float yDiff = targetPositon.y - currentPosition.y;

            Vector2 moveDirection = Vector2.zero;

            if (Mathf.Abs(xDiff) > Mathf.Abs(yDiff))
            {
                moveDirection = new Vector2(Mathf.Sign(xDiff), 0);
            }
            else
            {
                moveDirection = new Vector2(0, Mathf.Sign(yDiff));
            }

            Vector2 targetCenter = targetPositon + moveDirection * 0.5f;

            while (Vector2.Distance(transform.position, targetCenter) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetCenter, 1f * Time.deltaTime);
                yield return null;
            }

            transform.position = targetCenter;
        }
    }


    public void OnDie(EnemyDestroyType type)
    {
        // EnemySpawner에서 리스트로 적 정보를 관리하기 때문에 Destroy()를 직접하지 않고
        // EnemySpawner에게 본인이 삭제될 때 필요한 처리를 하도록 DestroyEnemy() 함수 호출
        enemySpawner.DestroyEnemy(type, this, gold);
    }
}
