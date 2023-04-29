using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : MonoBehaviour
{
    private int segments = 5;
    private int chainCount = 3;
    private float lightningDuration = 0.2f;
    private float currentTime;
    private float damage = 1;
    private float searchRadius = 3.0f;
    private float lightningMoveTime = 0.1f;

    GameObject startObject;
    GameObject targetObject;

    Vector2 startPosition;
    Vector2 targetPosition;

    List<GameObject> alreadySearchObject;


    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private LineRenderer lineRenderer;

    //bool isLightning = false;
    public void SetUp(GameObject Enemy, float damage)
    {
        this.damage = damage;
        targetObject = Enemy;
    }
    // Start is called before the first frame update
    void Start()
    {
        alreadySearchObject = new List<GameObject>();
        startObject = this.gameObject;
        //targetObject = new GameObject();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChainLightningStart()
    {
        lineRenderer.enabled = false;

        alreadySearchObject.Clear();
        startObject = this.gameObject;
        //startObject = null;

        StartCoroutine("ChainLightningSystem");
    }

    public IEnumerator ChainLightningSystem()
    {
        lineRenderer.enabled = true;
        for (int i = 0; i < chainCount; i++)
        {
            //Debug.Log("Start : " + startObject.transform.position);
            //Debug.Log("Target : " + targetObject.transform.position);

            if (startObject.CompareTag("Enemy") && !alreadySearchObject.Contains(startObject))
            {
                alreadySearchObject.Add(startObject);
            }

            alreadySearchObject.Add(targetObject);

            StartCoroutine("LightningEffect");

            startObject = targetObject; // 이전 타겟을 새로운 시작 오브젝트로 설정
            targetObject = SearchEnemy(targetObject); // find new target object

            if (targetObject == startObject)
            {
                //Debug.Log("No enemy");
                break;
            }
            yield return new WaitForSeconds(lightningMoveTime);
        }

        foreach (GameObject enemy in alreadySearchObject)
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(standardObject.transform.position, searchRadius, layerMask);
        float closestDistance = Mathf.Infinity;
        bool activeSearch = false;

        foreach (Collider2D collider in colliders)
        {
            if (standardObject == collider.gameObject || startObject == collider.gameObject 
                || alreadySearchObject.Contains(collider.gameObject))
            {
                continue;
            }
            
            float distance = Vector2.Distance(standardObject.transform.position, collider.gameObject.transform.position);
            if (distance < closestDistance)
            {
                activeSearch = true;
                closestDistance = distance;
                TargetEnemy = collider.gameObject;
            }
        }

        if (activeSearch)
            return TargetEnemy;
        else
            return standardObject;
    }

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
