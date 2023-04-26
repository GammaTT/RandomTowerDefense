using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    private int segments = 5;
    private int chainCount = 3;
    private float lightningDuration = 0.2f;
    private float currentTime;
    private float damage = 1;
    private float searchRadius = 10.0f;
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
    public void SetUp(GameObject Enemy)
    {
        targetObject = Enemy;
    }
    // Start is called before the first frame update
    void Start()
    {
        alreadySearchObject = new List<GameObject>();
        startObject = new GameObject();
        targetObject = new GameObject();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChainLightningStart()
    {
        lineRenderer.enabled = true;
        alreadySearchObject.Clear();
        startObject = this.gameObject;
        //startObject = null;

        StartCoroutine("ChainLightning");
    }
/*    public IEnumerator ChainLightning()
    {
        StartCoroutine("LightningEffect");
        yield return new WaitForSeconds(lightningMoveTime);

        for (int i = 0; i < chainCount - 1; i++)
        {
            startPosition = targetPosition;
            targetPosition = SearchEnemy(targetPosition).transform.position;
            yield return new WaitForSeconds(lightningMoveTime);
            StartCoroutine("LightningEffect");

        }
    }*/

/*    public IEnumerator ChainLightning()
    {
        for (int i = 0; i < chainCount; i++)
        {
            lineRenderer.enabled = false;

            Debug.Log("Start : " + startObject.transform.position);
            Debug.Log("Target : " + targetObject.transform.position);

            StartCoroutine("LightningEffect");

            startObject = targetObject;
            
            //startPosition = targetPosition;
            targetObject = SearchEnemy(startObject);
            yield return new WaitForSeconds(lightningMoveTime);

        }
        lineRenderer.enabled = false;
    }*/

    public IEnumerator ChainLightning()
    {
        lineRenderer.enabled = false;

        for (int i = 0; i < chainCount; i++)
        {
            Debug.Log("Start : " + startObject.transform.position);
            Debug.Log("Target : " + targetObject.transform.position);

            alreadySearchObject.Add(startObject);
            alreadySearchObject.Add(targetObject);
            StartCoroutine("LightningEffect");

            GameObject prevTarget = targetObject; // 이전 타겟 저장
            startObject = prevTarget; // 이전 타겟을 새로운 시작 오브젝트로 설정
            targetObject = SearchEnemy(startObject); // 시작 오브젝트를 기준으로 새로운 타겟 찾기
            while (targetObject == prevTarget) // 새로운 타겟이 이전 타겟과 같다면 다시 타겟을 찾음
            {
                targetObject = SearchEnemy(startObject);
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(lightningMoveTime);
        }
        lineRenderer.enabled = false;
    }

    /*    public GameObject SearchEnemy(Vector2 standardPosition)
        {
            GameObject TargetEnemy = new GameObject();
            Collider2D[] colliders = Physics2D.OverlapCircleAll(standardPosition, searchRadius, layerMask, 0.2f);
            float closestDistance = Mathf.Infinity;

            foreach (Collider2D collider in colliders)
            {
                float distance = Vector2.Distance(standardPosition, collider.gameObject.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    TargetEnemy = collider.gameObject;
                }
            }

            return TargetEnemy;
        }*/

    public GameObject SearchEnemy(GameObject standardObject)
    {
        GameObject TargetEnemy = new GameObject();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(standardObject.transform.position, searchRadius, layerMask);
        float closestDistance = Mathf.Infinity;

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
                closestDistance = distance;
                TargetEnemy = collider.gameObject;
            }
        }

        return TargetEnemy;
    }

    public IEnumerator LightningEffect()
    {
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
