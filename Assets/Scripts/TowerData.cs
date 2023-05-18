using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TowerData : ScriptableObject
{
    public GameObject towerPrefab;
    public GameObject followTowerPrefab;    // �ӽ� Ÿ�� ������
    public Weapon weapon;             // ������ Ÿ��(����) ����
    public WeaponUpGradeValue weaponUpGradeValue;
    public TowerGrade grade;
    public Sprite sprite;   // �������� Ÿ�� �̹��� (UI)


    [System.Serializable]
    public struct Weapon
    {
        public float damage;    // ���ݷ�
        public float rate;  // ���� �ӵ�
        public float range; // ���� ����
        public int sell;    // Ÿ�� �Ǹ� �� ȹ�� ���

        [Header("About SlowTower (0.0 ~ 1.0)")]
        public float slowValue;
    }

    [System.Serializable]
    public struct WeaponUpGradeValue
    {
        public float damage;    // ���ݷ�
        public float rate;  // ���� �ӵ�
        public float range; // ���� ����

        [Header("About SlowTower")]
        public float slowValue;
    }


}
