using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Cannon = 0, Laser, Slow, Buff, Lightning}

public enum WeaponState { SearchTarget = 0, TryAttackCannon, TryAttackLaser }
public class TowerWeapon : MonoBehaviour
{
    [SerializeField]
    private TowerData towerData;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private WeaponType weaponType;

    [SerializeField]
    private GameObject projectilePrefab;

    private WeaponState weaponState = WeaponState.SearchTarget;
    private Transform attackTarget = null;

    private TowerSpawner towerSpawner;
    private EnemySpawner enemySpawner;

    public int level;
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetUp(TowerSpawner towerSpawner, EnemySpawner enemySpawner)
    {
        this.towerSpawner = towerSpawner;
        this.enemySpawner = enemySpawner;

        ChangeState(WeaponState.SearchTarget);
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeState(WeaponState newState)
    {
        // 이전에 재생중이던 상태 종료
        StopCoroutine(weaponState.ToString());
        // 상태 변경
        weaponState = newState;
        // 새로운 상태 재생
        StartCoroutine(weaponState.ToString());
    }
    public IEnumerator SearchTarget()
    {
        while (true)
        {
            // 현재 타워에 가장 가까이 있는 공격 대상(적) 탐색
            attackTarget = FindClosestAttackTarget();

            if (attackTarget != null)
            {
                //string weaponTypeString = 
                if (weaponType == WeaponType.Cannon)
                {
                    ChangeState(WeaponState.TryAttackCannon);
                }
                else if (weaponType == WeaponType.Laser)
                {
                    ChangeState(WeaponState.TryAttackLaser);
                }
            }

            yield return null;
        }
    }

    private Transform FindClosestAttackTarget()
    {
        // 제일 가까이 있는 적을 찾기 위해 최초 거리를 최대한 크게 설정
        float closestDistSqr = Mathf.Infinity;
        // EnemySpawner의 EnemyList에 있는 현재 맵에 존재하는 모든 적 검사
        for (int i = 0; i < enemySpawner.enemyList.Count; ++i)
        {
            float distance = Vector3.Distance(enemySpawner.enemyList[i].transform.position, transform.position);
            // 현재 검사중인 적과의 거리가 공격범위 내에 있고, 현재까지 검사한 적보다 거리가 가까우면
            if (distance <= towerData.weapon[level].range && distance <= closestDistSqr)
            {
                closestDistSqr = distance;
                attackTarget = enemySpawner.enemyList[i].transform;
            }
        }

        return attackTarget;
    }

    private bool IsPossibleToAttackTarget()
    {
        // target이 있는지 검사 (다른 발사체에 의해 제거, Goal 지점까지 이동해 삭제 등)
        if (attackTarget == null)
        {
            return false;
        }

        // target이 공격 범위 안에 있는지 검사 (공격 범위를 벗어나면 새로운 적 탐색)
        float distance = Vector3.Distance(attackTarget.position, transform.position);
        if (distance > towerData.weapon[level].range)
        {
            attackTarget = null;
            return false;
        }

        return true;
    }

    private IEnumerator TryAttackCannon()
    {
        while (true)
        {
            if (IsPossibleToAttackTarget() == false)
            {
                ChangeState(WeaponState.SearchTarget); 
                break;
            }
            yield return new WaitForSeconds(towerData.weapon[level].rate);

            SpawnProjectile();

        }
    }

    private void SpawnProjectile()
    {
        GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        // 생성된 발사체에게 공격대상(attackTarget) 정보 제공
        // 공격력 = 타워 기본 공격력 + 버프에 의해 추가된 공격력
        float damage = towerData.weapon[level].damage; // +Adddamage
        clone.GetComponent<Projectile>().Setup(attackTarget, damage);
    }
}
