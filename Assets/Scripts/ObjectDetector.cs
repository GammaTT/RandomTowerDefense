using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private TowerDataViewer towerDataViewer;
    [SerializeField]
    private EnemySpawner enemySpawner;

    private Camera mainCamera;
    private Ray ray;
    private Ray2D ray2D;
    private RaycastHit2D hit;
    private Transform hitTransform = null; // 임시 저장
    private Transform preiviousHittransfrom = null; //직전 마우스 타일

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //UI 클릭하고 있을때는 굳이 표시해줄 데이터는 없다.
        if (EventSystem.current.IsPointerOverGameObject() == true)
        {
            return;
        }


        if (Input.GetMouseButtonDown(1))
        {
            Vector2 raycasyPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            hit = Physics2D.Raycast(raycasyPoint, Vector2.zero);

            if (hit.transform == null)
            {
                return;
            }

            hitTransform = hit.transform;

            if (hit.transform.CompareTag("Tower"))
            {
                //Debug.Log("Tower");
                towerDataViewer.OnPanel(hit.transform);
            }
            else if (hit.transform.CompareTag("Enemy"))
            {
                Debug.Log("Enemy");

            }

        }
    }


}
