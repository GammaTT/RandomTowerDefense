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
    private Transform hitTransform = null; // �ӽ� ����
    private Transform preiviousHittransfrom = null; //���� ���콺 Ÿ��

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
        //UI Ŭ���ϰ� �������� ���� ǥ������ �����ʹ� ����.
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
