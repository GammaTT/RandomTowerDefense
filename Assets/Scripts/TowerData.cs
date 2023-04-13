using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TowerData : ScriptableObject
{
    public GameObject towerPrefab;
    public GameObject followTowerPrefab;    // �ӽ� Ÿ�� ������
    public Weapon[] weapon;             // ������ Ÿ��(����) ����

    [System.Serializable]
    public struct Weapon
    {
        public Sprite sprite;   // �������� Ÿ�� �̹��� (UI)
        public float damage;    // ���ݷ�
        public float slow;  // ���� �ۼ�Ʈ (0.2 = 20%)
        public float buff;  // ���ݷ� ������ (1.2 = 120%)
        public float rate;  // ���� �ӵ�
        public float range; // ���� ����
        public int cost;    // �ʿ� ��� (0���� : �Ǽ�, 1~���� : ���׷��̵�)
        public int sell;    // Ÿ�� �Ǹ� �� ȹ�� ���
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
