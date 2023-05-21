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
    private Transform canvasTransform;

    private int gold;
    private int scorePoint;
    private float moveSpeed;
    private float rotateSpeed;

    private float baseNextNodeMoveTime;
    public EnemyData enemyData;

    //[HideInInspector]
    public float nextNodeMoveTime = 1.0f;
    private float currentTime;
    private float nodeArriveDistance = 0.1f;

    public bool obstructed = false;

    public void SetUp(EnemySpawner enemySpawner)
    {
        this.enemySpawner = enemySpawner;
        //this.canvasTransform = canvasTransform;
    }
    // Start is called before the first frame update
    void Start()
    {
        enemyPath = new List<AStarNode>();

        this.gold = enemyData.gold;
        scorePoint = enemyData.scorePoint;
        this.moveSpeed = enemyData.moveSpeed;
        this.rotateSpeed = enemyData.rotateSpeed;

        LastPathUpdate = Time.time;
        SetPath();

        //nextNodeMoveTime �⺻�� 1 MoveSpeed�� ���� �������� ���� �پ��
        nextNodeMoveTime *= (1 / moveSpeed);
        baseNextNodeMoveTime = nextNodeMoveTime;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Time.time - LastPathUpdate > PathUpdateDelay) 
        {
            LastPathUpdate = Time.time;
            SetPath();
        }*/ //���� ��ġ�� ���� ���� ��� �������� �ٲ�

        transform.Rotate(Vector3.forward * -rotateSpeed * (1 - nextNodeMoveTime));
    }

    public int GetGold()
    {
        return gold;
    }

    public int GetScorePoint()
    {
        return scorePoint;
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
            //���� ��ġ�Ҷ� ù��° ���(Ÿ��)���� �����Ÿ��� ������ ���� �ݺ����� �Ѿ��
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

        //���� �̵��ߴ� ���. �н� ���δ��� ���� �ϱ� ���� �ٲ�
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

    //movespeed �� ����� ���� ���(Ÿ��)�� �̵��ϴ� �ð��� �ֱ⿡
    //move
    public void ReSetSpeed()
    {
        nextNodeMoveTime = baseNextNodeMoveTime;
    }


    public void OnDie(EnemyDestroyType type)
    {
        // EnemySpawner���� ����Ʈ�� �� ������ �����ϱ� ������ Destroy()�� �������� �ʰ�
        // EnemySpawner���� ������ ������ �� �ʿ��� ó���� �ϵ��� DestroyEnemy() �Լ� ȣ��
        enemySpawner.DestroyEnemy(type, this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Goal")
        {
            enemySpawner.DestroyEnemy(EnemyDestroyType.Arrive, this);
        }
    }
}
