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

    [Header("Projectile")]
    [SerializeField]
    private GameObject projectilePrefab;

    [Header("Laser")]
    [SerializeField]
    private LineRenderer lineRenderer;

    private WeaponState weaponState = WeaponState.SearchTarget;
    private Transform attackTarget = null;
    private SpriteRenderer spriteRenderer; //타워 현재 정보용 이미지

    private TowerSpawner towerSpawner;
    private EnemySpawner enemySpawner;

    public Sprite towerSprite => towerData.weapon[level].sprite; // 현재 레벨의 타워 이미지로의 람다함수
    public int level = 1; // 이것도 나중에 돼는지 확인 해야됌
    public float damage => towerData.weapon[level].damage;
    public float range => towerData.weapon[level].range;
    public float rate => towerData.weapon[level].rate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetUp(TowerSpawner towerSpawner, EnemySpawner enemySpawner)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.towerSpawner = towerSpawner;
        this.enemySpawner = enemySpawner;

        //Invoke("ChangeState(WeaponState.SearchTarget)", 1f);
        if (!(weaponType == WeaponType.Slow || weaponType == WeaponType.Buff))
            ChangeState(WeaponState.SearchTarget);
    }
    // Update is called once per frame
    void Update()
    {

    }
    //임시 스크립터블인데 되는지 수정중
    //어떻게 할까..

    public bool UPGrade()
    {
/*        towerData.weapon[level].damage += 5;
        towerData.weapon[level].range += 5;
        towerData.weapon[level].rate += 5;*/

        level++;

        return true;
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

            yield return new WaitForEndOfFrame();
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

    private IEnumerator TryAttackLaser()
    {
        // 레이저, 레이저 타격 효과 활성화
        EnableLaser();

        while (true)
        {
            // target을 공격하는게 가능한지 검사
            if (IsPossibleToAttackTarget() == false)
            {
                // 레이저, 레이저 타격 효과 비활성화
                DisableLaser();
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // 레이저 공격
            SpawnLaser();

            yield return new WaitForEndOfFrame();
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

    private void EnableLaser()
    {
        lineRenderer.gameObject.SetActive(true);
        //hitEffect.gameObject.SetActive(true);
    }

    private void DisableLaser()
    {
        lineRenderer.gameObject.SetActive(false);
        //hitEffect.gameObject.SetActive(false);
    }

    private void SpawnLaser()
    {
        Vector3 direction = attackTarget.position - spawnPoint.position;
        //RaycastHit2D[] hit = Physics2D.RaycastAll(spawnPoint.position, direction, towerData.weapon[level].range, targetLayer);
        RaycastHit2D[] hit = Physics2D.RaycastAll(spawnPoint.position, direction, towerData.weapon[level].range);

        // 같은 방향으로 여러 개의 광선을 쏴서 그 중 현재 attackTarget과 동일한 오브젝트를 검출
        for (int i = 0; i < hit.Length; ++i)
        {
            if (hit[i].transform == attackTarget)
            {
                // 선의 시작지점
                lineRenderer.SetPosition(0, spawnPoint.position);
                // 선의 목표지점
                lineRenderer.SetPosition(1, new Vector3(hit[i].point.x, hit[i].point.y, 0) + Vector3.back);
                // 타격 효과 위치 설정
                //hitEffect.position = hit[i].point;
                // 적 체력 감소 (1초에 damage만큼 감소)
                // 공격력 = 타워 기본 공격력 + 버프에 의해 추가된 공격력
                float damage = towerData.weapon[level].damage; // + AddedDamage;
                attackTarget.GetComponent<EnemyHp>().TakeDamage(damage * Time.deltaTime);
            }
        }
    }


}
