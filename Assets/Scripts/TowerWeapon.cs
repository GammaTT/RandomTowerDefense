using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType 
{ 
    Cannon = 0, 
    Laser, Slow, Buff, 
    ChainLightning, Bomb, 
    MultiWayShooting,
    MultiBomb,
    MultiLaser
}

public enum WeaponState 
{ 
    SearchTarget = 0, TryAttackCannon,
    TryAttackLaser, TryAttackChainLightning,
    TryAttackMultiShooting,
    TryAttackMultiBomb
}
public class TowerWeapon : MonoBehaviour
{
    [SerializeField]
    private TowerData towerData;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    public WeaponType weaponType;

    [Header("Projectile")]
    [SerializeField]
    private GameObject targetProjectilePrefab;

    [Header("BombProjectile")]
    [SerializeField]
    private GameObject bombProjectilePrefab;

    [Header("MultiShoot")]
    [SerializeField]
    private GameObject projectilePrefab;

    [Header("MultiBomb")]
    [SerializeField]
    private GameObject bombPrefab;

    [Header("Laser")]
    [SerializeField]
    private LineRenderer lineRenderer;

    [Header("MultiLaserPlus")]
    [SerializeField]
    private LineRenderer lineRenderer2;
    [SerializeField]
    private LineRenderer lineRenderer3;

    private Slow slow;

    public WeaponState weaponState = WeaponState.SearchTarget;
    private Transform attackTarget = null;
    private Vector3 attackTargetVector;
    private SpriteRenderer spriteRenderer; //타워 현재 정보용 이미지

    private TowerSpawner towerSpawner;
    private EnemySpawner enemySpawner;

    public TowerGrade towerGrade;
    public Sprite towerSprite => towerData.sprite;
    public int level = 1; // 이것도 나중에 돼는지 확인 해야됌
    public int upGradeGold;
    public int useGoldToUpGrade = 0;

    [HideInInspector]
    public float damage;
    [HideInInspector]
    public float range;
    [HideInInspector]
    public float rate;
    bool doubleShot;

    //슬로우 타워일때
    [HideInInspector]
    public float slowValue;

    // Start is called before the first frame update
    void Start()
    {
        this.damage = towerData.weapon.damage;
        this.range = towerData.weapon.range;
        this.rate = towerData.weapon.rate;
        this.doubleShot = towerData.weapon.doubleShot;

        if (weaponType == WeaponType.Slow)
        {
            this.slowValue = towerData.weapon.slowValue;
            slow.SetUp(slowValue, range);
        }

        upGradeGold = (int)towerGrade * Constants.upGradeGoldMulti;
    }

    public void SetUp(TowerSpawner towerSpawner, EnemySpawner enemySpawner)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.towerSpawner = towerSpawner;
        this.enemySpawner = enemySpawner;

        //Invoke("ChangeState(WeaponState.SearchTarget)", 1f);
        if (!(weaponType == WeaponType.Slow || weaponType == WeaponType.Buff))
        {
            ChangeState(WeaponState.SearchTarget);
        }
        else if (weaponType == WeaponType.Slow)
        {
            slow = GetComponentInChildren<Slow>();
        }
    }
    // Update is called once per frame

    public bool UPGrade()
    {
        useGoldToUpGrade += upGradeGold;
        upGradeGold += (int)towerGrade;
        damage += towerData.weaponUpGradeValue.damage;
        range += towerData.weaponUpGradeValue.range;
        rate += towerData.weaponUpGradeValue.rate;


        if (weaponType == WeaponType.Slow)
        {
            slowValue += towerData.weaponUpGradeValue.slowValue;
            slow.SetUp(slowValue, range);
        }

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
                switch(weaponType)
                {
                    case WeaponType.Cannon:
                    case WeaponType.Bomb:
                        ChangeState(WeaponState.TryAttackCannon);
                        break;

                    case WeaponType.ChainLightning:
                        ChangeState(WeaponState.TryAttackChainLightning);
                        break;

                    case WeaponType.Laser:
                        ChangeState(WeaponState.TryAttackLaser);
                        break;

                    case WeaponType.MultiWayShooting:
                        ChangeState(WeaponState.TryAttackMultiShooting);
                        break;

                    case WeaponType.MultiBomb:
                        ChangeState(WeaponState.TryAttackMultiBomb);
                        break;
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
            if (distance <= range && distance <= closestDistSqr)
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
        if (distance > range)
        {
            attackTarget = null;
            return false;
        }

        return true;
    }

    private IEnumerator TryAttackMultiBomb()
    {
        while (true)
        {
            if (IsPossibleToAttackTarget() == false)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            SpawnMultiBomb();

            yield return new WaitForSeconds(rate);

        }
    }

    private IEnumerator TryAttackMultiShooting()
    {
        while (true)
        {
            if (IsPossibleToAttackTarget() == false)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            attackTargetVector = attackTarget.transform.position;

            SpawnMultiProjectile(attackTargetVector);

            //두번 쏘는 타입 일때
            if (doubleShot)
            {
                yield return new WaitForSeconds(0.2f);
                SpawnMultiProjectile(attackTargetVector);
            }

            yield return new WaitForSeconds(rate);
        }
    }
    private IEnumerator TryAttackChainLightning()
    {
        if (IsPossibleToAttackTarget() == false)
        {
            ChangeState(WeaponState.SearchTarget);
            yield return null;
        }
        else
        {
            ChainLightning lightning = GetComponent<ChainLightning>();
            lightning.SetUp(attackTarget.gameObject, damage);

            //try-catch use?
            lightning.ChainLightningStart();
 
            yield return new WaitForSeconds(rate);
            ChangeState(WeaponState.SearchTarget);
        }
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

            if (weaponType == WeaponType.Cannon)
            {
                SpawnTargetProjectile();
            }
            else if (weaponType == WeaponType.Bomb)
            {
                SpawnBombProjectile();
            }

            yield return new WaitForSeconds(rate);
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

                //enable and Research cooltime
                yield return new WaitForSeconds(rate);

                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // 레이저 공격
            SpawnLaser();

            yield return new WaitForEndOfFrame();
        }
    }
    private void SpawnMultiBomb()
    {
        Vector2 bombDirectoin = (attackTarget.transform.position - transform.position).normalized;
        Vector2 towerPositon = transform.position;
        float bombRange = 5.0f;
        int bombCount = 5;
        Vector2 bombVector = bombDirectoin * bombRange;

        Vector2[] bombPosition = new Vector2[bombCount];

        for(int i = 0; i < bombCount; i++)
        {
            bombPosition[i] = towerPositon + bombVector * ((i + 1) * (1.0f / bombCount));
            GameObject bombs = Instantiate(bombPrefab, bombPosition[i], Quaternion.identity);
            bombs.GetComponent<Bomb>().SetUp(damage);
        }

    }
    private void SpawnMultiProjectile(Vector3 attackTargetPosition)
    {
        Vector3 []targetMove = new Vector3[3] ;
        targetMove[0] = (attackTargetPosition - transform.position);
        targetMove[1] = Quaternion.AngleAxis(45f, Vector3.forward) * targetMove[0];
        targetMove[2] = Quaternion.AngleAxis(-45f, Vector3.forward) * targetMove[0];

        GameObject [] projectileClones = new GameObject[3];
        float damage = this.damage; //+add damage

        for (int i = 0; i < projectileClones.Length; i++)
        {
            projectileClones[i] = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
            projectileClones[i].GetComponent<Projectile>().Setup(targetMove[i], damage);
        }
    }
    private void SpawnBombProjectile()
    {
        GameObject clone = Instantiate(bombProjectilePrefab, spawnPoint.position, Quaternion.identity);
        // 생성된 발사체에게 공격대상(attackTarget) 정보 제공
        // 공격력 = 타워 기본 공격력 + 버프에 의해 추가된 공격력
        float damage = this.damage; // +Adddamage
        clone.GetComponent<BombProjectile>().Setup(attackTarget, damage);
    }
    private void SpawnTargetProjectile()
    {
        GameObject clone = Instantiate(targetProjectilePrefab, spawnPoint.position, Quaternion.identity);
        // 생성된 발사체에게 공격대상(attackTarget) 정보 제공
        // 공격력 = 타워 기본 공격력 + 버프에 의해 추가된 공격력
        float damage = this.damage; // +Adddamage
        clone.GetComponent<TargetProjectile>().Setup(attackTarget, damage);
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
        RaycastHit2D[] hit = Physics2D.RaycastAll(spawnPoint.position, direction, this.range);

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
                float damage = this.damage; // + AddedDamage;
                attackTarget.GetComponent<EnemyHp>().TakeDamage(damage * Time.deltaTime);
            }
        }
    }


}
