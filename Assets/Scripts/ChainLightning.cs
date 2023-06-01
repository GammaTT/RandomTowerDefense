using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : MonoBehaviour
{
    private int segments = 5;
    private int chainCount = 3;
    private float currentTime;
    private float damage = 1;
    private float searchRadius = 3.0f;
    private float lightningMoveTime = 0.2f;

    GameObject startObject;
    GameObject targetObject;

    Vector2 startPosition;
    Vector2 targetPosition;

    List<GameObject> hitList;

    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private LineRenderer lineRenderer;

    //bool isLightning = false;
    public void SetUp(GameObject Enemy, float damage)
    {
        this.damage = damage;
        startObject = this.gameObject;
        targetObject = Enemy;
    }
    // Start is called before the first frame update
    void Start()
    {
        hitList = new List<GameObject>();
    }

    public void ChainLightningStart()
    {
        lineRenderer.enabled = false;

        hitList.Clear();
        startObject = this.gameObject;
        //startObject = null;

        lineRenderer.enabled = true;
        StartCoroutine("ChainLightningSystem");
    }

    public IEnumerator ChainLightningSystem()
    {
        for (int i = 0; i < chainCount; i++)
        {
            //Ÿ�� ������Ʈ�� �ǰ� ����Ʈ�� ����
            if (targetObject != null && !hitList.Contains(targetObject))
            {
                hitList.Add(targetObject);
            }

            StartCoroutine("LightningEffect");

            startObject = targetObject; // ���� Ÿ���� ���ο� ���� ������Ʈ(����)�� ����
            targetObject = SearchEnemy(targetObject); // ���ο� Ÿ���� �� Ÿ�� �������� ã��

            //���ο� Ÿ���� ��ã���� ���� ����
            if (targetObject == null)
            {
                break;
            }
            yield return new WaitForSeconds(lightningMoveTime);
        }

        //�ǰݵ� �� ����Ʈ�� �ִ� ���� �ǰ� ó��
        foreach (GameObject enemy in hitList)
        {
            if (enemy != null)
            {
                enemy.GetComponent<EnemyHp>().TakeDamage(damage);
            }
        }

        lineRenderer.enabled = false;
    }

    public GameObject SearchEnemy(GameObject standardObject)
    {
        GameObject TargetEnemy = null;
        Collider2D[] colliders = null;

        if (standardObject != null)
        {
             colliders = Physics2D.OverlapCircleAll(standardObject.transform.position, searchRadius, layerMask);
        }
        else //��ġ ���� ���� ������Ʈ�� �ı��� => null ����
        {
            return TargetEnemy;
        }

        float closestDistance = Mathf.Infinity;

        //���� ����� �浹ü ã��
        foreach (Collider2D collider in colliders)
        {
            //����� �浹ü�� �ı��Ǿ null �̰ų� ��ġ�� �浹ü�� �ڱ�ۿ� ���ų� �̹� ���� ���̸� ���� �浹ü �˻�
            if (collider == null || standardObject == collider.gameObject
                || hitList.Contains(collider.gameObject))
            {
                continue;
            }
            
            float distance = Vector2.Distance(standardObject.transform.position, 
                collider.gameObject.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                TargetEnemy = collider.gameObject;
            }
        }

        return TargetEnemy;
    }

    //���۰� Ÿ�ٿ� ���� ����Ʈ�� ������ �׷��ִ� �ڷ�ƾ
    public IEnumerator LightningEffect()
    {
        if (startObject == null || targetObject == null) 
        {
            yield break;
        }

        lineRenderer.enabled = true;

        startPosition = startObject.transform.position;
        targetPosition = targetObject.transform.position;

        currentTime = Time.time;
        while (true)
        {
            if (Time.time - currentTime > lightningMoveTime)
            {
                break;
            }
            DrawLightning(startPosition, targetPosition, segments);
            yield return new WaitForEndOfFrame();
        }
    }

    //���� ����Ʈ , ���� �������� ������� ���¸� ���.
    public void DrawLightning(Vector2 source, Vector2 target, int segments)
    {
        lineRenderer.positionCount = segments;
        lineRenderer.SetPosition(0, source);

        for (int i = 1; i < segments - 1; i++)
        {
            Vector2 pos = Vector2.Lerp(source, target, (float)i / (float)segments);
            pos += Random.insideUnitCircle * 0.1f;
            lineRenderer.SetPosition(i, pos);
        }
        lineRenderer.SetPosition(segments - 1, target);
    }
}
