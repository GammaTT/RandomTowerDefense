using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum EnemyDestroyType { Kill = 0, Arrive }
public class Enemy : MonoBehaviour
{
    public List<AStarNode> enemyPath;

    //private float PathUpdateDelay = 1f;
    private float LastPathUpdate;
    private EnemySpawner enemySpawner;

    private int gold;
    private int scorePoint;
    private float moveSpeed;
    private float rotateSpeed;

    public EnemyData enemyData;


    private float currentTime;
    private float nextNodeMoveTime = 1.0f;
    private float nodeArriveDistance = 0.1f;
    public void SetUp(EnemySpawner enemySpawner)
    {
        this.enemySpawner = enemySpawner;

    }
    // Start is called before the first frame update
    void Start()
    {
        enemyPath = new List<AStarNode>();

        this.gold = enemyData.gold;
        this.moveSpeed = enemyData.moveSpeed;
        this.rotateSpeed = enemyData.rotateSpeed;

        LastPathUpdate = Time.time;
        SetPath();

        //nextNodeMoveTime 기본은 1 MoveSpeed가 빨라 질때마다 점점 줄어듬
        nextNodeMoveTime *= (1 / moveSpeed);

    }

    // Update is called once per frame
    void Update()
    {
        /*if (Time.time - LastPathUpdate > PathUpdateDelay) 
        {
            LastPathUpdate = Time.time;
            SetPath();
        }*/ //벽을 설치할 때만 새로 경로 설정으로 바꿈

        transform.Rotate(Vector3.forward * -rotateSpeed);
    }

    public void SetPath()
    {
        StopCoroutine("Move");

        enemyPath = (MapDirector.Instance.SetPathFromPosition(transform));

        StartCoroutine("Move");

        //Debug.Log(EnemyPath.Count);
    }

    public IEnumerator Move()
    {
        foreach (var node in enemyPath)
        {
            //벽을 설치할때 첫번째 노드(타일)에서 버벅거리기 때문에 다음 반복으로 넘어가기
            if (node == enemyPath[0])
            {
                continue;
            }

            Vector2 targetPositon = new Vector2(node.xPos, node.yPos);
            Vector2 currentPosition = transform.position;
            currentTime = Time.time;

            while (true)
            {
                Vector3 move = (targetPositon - currentPosition).normalized * Time.deltaTime;
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

        //원래 이동했던 방법. 패스 파인더와 같게 하기 위해 바꿈
        /*
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
                transform.position = Vector2.MoveTowards(transform.position, targetCenter, 1f * Time.deltaTime * MoveSpeed);
                yield return null;
            }

            transform.position = targetCenter;
        }*/
    }

    

    public void OnDie(EnemyDestroyType type)
    {
        // EnemySpawner에서 리스트로 적 정보를 관리하기 때문에 Destroy()를 직접하지 않고
        // EnemySpawner에게 본인이 삭제될 때 필요한 처리를 하도록 DestroyEnemy() 함수 호출
        enemySpawner.DestroyEnemy(type, this, gold, scorePoint);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Goal")
        {
            enemySpawner.DestroyEnemy(EnemyDestroyType.Arrive, this, gold, scorePoint);
        }
    }
}
