using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int gold;
    public int maxHp;
    public int currentHp
    {
        get { return gold; }
        set { gold = Mathf.Max(0,value); }
    }
    private int wallCount;
    private int towerCount;

    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private Image hitRedImage;

    private void Awake()
    {
        currentHp = maxHp;

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        StopCoroutine("HitAnimation");
        StartCoroutine("HitAnimation");

        if (currentHp < 0)
        {
            //holy moly wtf game over damn it
        }
    }


    private IEnumerator HitAnimation()
    {
        //�� �̹��� UI�� ȭ�� ��ü�� ���� ������ RayCastTarget�� ������
        // ��üȭ�� ũ��� ��ġ�� imageScreen�� ������ color ������ ����
        // imageScreen�� ������ 40%�� ����
        Color color = hitRedImage.color;
        color.a = 0.4f;
        hitRedImage.color = color;

        // ������ 0%�� �ɶ����� ����
        while (color.a >= 0.0f)
        {
            color.a -= Time.deltaTime;
            hitRedImage.color = color;

            yield return null;
        }
    }
}
