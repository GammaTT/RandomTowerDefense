using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType 
{ 
    Cannon = 0, 
    Laser, Slow, Buff, 
    ChainLightning, Bomb, 
    MultiWayShooting
}

public enum WeaponState 
{ 
    SearchTarget = 0, TryAttackCannon,
    TryAttackLaser, TryAttackChainLightning,
    TryAttackMultiShooting
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

    [Header("Laser")]
    [SerializeField]
    private LineRenderer lineRenderer;

    public WeaponState weaponState = WeaponState.SearchTarget;
    private Transform attackTarget = null;
    private SpriteRenderer spriteRenderer; //Ÿ�� ���� ������ �̹���

    private TowerSpawner towerSpawner;
    private EnemySpawner enemySpawner;

    public Sprite towerSprite => towerData.weapon[level].sprite; // ���� ������ Ÿ�� �̹������� �����Լ�
    public int level = 1; // �̰͵� ���߿� �Ŵ��� Ȯ�� �ؾ߉�
    public int grade;
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
    //�ӽ� ��ũ���ͺ��ε� �Ǵ��� ������
    //��� �ұ�..

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
        // ������ ������̴� ���� ����
        StopCoroutine(weaponState.ToString());
        // ���� ����
        weaponState = newState;
        // ���ο� ���� ���
        StartCoroutine(weaponState.ToString());
    }
    public IEnumerator SearchTarget()
    {
        while (true)
        {
            // ���� Ÿ���� ���� ������ �ִ� ���� ���(��) Ž��
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
                }
/*                if (weaponType == WeaponType.Cannon || weaponType == WeaponType.Bomb)
                {
                    ChangeState(WeaponState.TryAttackCannon);
                }
                else if (weaponType == WeaponType.Laser)
                {
                    ChangeState(WeaponState.TryAttackLaser);
                }
                else if (weaponType == WeaponType.ChainLightning)
                {
                    ChangeState(WeaponState.TryAttackChainLightning);
                }
                else if (weaponType == WeaponType.MultiWayShooting)
                {
                    ChangeState(WeaponState.TryAttackMultiShooting);
                }*/
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private Transform FindClosestAttackTarget()
    {
        // ���� ������ �ִ� ���� ã�� ���� ���� �Ÿ��� �ִ��� ũ�� ����
        float closestDistSqr = Mathf.Infinity;
        // EnemySpawner�� EnemyList�� �ִ� ���� �ʿ� �����ϴ� ��� �� �˻�
        for (int i = 0; i < enemySpawner.enemyList.Count; ++i)
        {
            float distance = Vector3.Distance(enemySpawner.enemyList[i].transform.position, transform.position);
            // ���� �˻����� ������ �Ÿ��� ���ݹ��� ���� �ְ�, ������� �˻��� ������ �Ÿ��� ������
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
        // target�� �ִ��� �˻� (�ٸ� �߻�ü�� ���� ����, Goal �������� �̵��� ���� ��)
        if (attackTarget == null)
        {
            return false;
        }

        // target�� ���� ���� �ȿ� �ִ��� �˻� (���� ������ ����� ���ο� �� Ž��)
        float distance = Vector3.Distance(attackTarget.position, transform.position);
        if (distance > towerData.weapon[level].range)
        {
            attackTarget = null;
            return false;
        }

        return true;
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

            SpawnMultiProjectile();

            yield return new WaitForSeconds(towerData.weapon[level].rate);

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
            lightning.SetUp(attackTarget.gameObject, towerData.weapon[level].damage);

            //try-catch use?
            lightning.ChainLightningStart();
 
            yield return new WaitForSeconds(towerData.weapon[level].rate);
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
            yield return new WaitForSeconds(towerData.weapon[level].rate);

            if (weaponType == WeaponType.Cannon)
            {
                SpawnTargetProjectile();
            }
            else if (weaponType == WeaponType.Bomb)
            {
                SpawnBombProjectile();
            }
        }
    }

    private IEnumerator TryAttackLaser()
    {
        // ������, ������ Ÿ�� ȿ�� Ȱ��ȭ
        EnableLaser();

        while (true)
        {
            // target�� �����ϴ°� �������� �˻�
            if (IsPossibleToAttackTarget() == false)
            {
                // ������, ������ Ÿ�� ȿ�� ��Ȱ��ȭ
                DisableLaser();

                //enable and Research cooltime
                yield return new WaitForSeconds(towerData.weapon[level].rate);

                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // ������ ����
            SpawnLaser();

            yield return new WaitForEndOfFrame();
        }
    }

    private void SpawnMultiProjectile()
    {
        Vector3 []targetMove = new Vector3[3] ;
        targetMove[0] = (attackTarget.transform.position - transform.position);
        targetMove[1] = Quaternion.AngleAxis(45f, Vector3.forward) * targetMove[0];
        targetMove[2] = Quaternion.AngleAxis(-45f, Vector3.forward) * targetMove[0];

        GameObject [] projectileClones = new GameObject[3];
        float damage = towerData.weapon[level].damage; //+add damage

        for (int i = 0; i < projectileClones.Length; i++)
        {
            projectileClones[i] = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
            projectileClones[i].GetComponent<Projectile>().Setup(targetMove[i], damage);
        }
    }
    private void SpawnBombProjectile()
    {
        GameObject clone = Instantiate(bombProjectilePrefab, spawnPoint.position, Quaternion.identity);
        // ������ �߻�ü���� ���ݴ��(attackTarget) ���� ����
        // ���ݷ� = Ÿ�� �⺻ ���ݷ� + ������ ���� �߰��� ���ݷ�
        float damage = towerData.weapon[level].damage; // +Adddamage
        clone.GetComponent<BombProjectile>().Setup(attackTarget, damage);
    }
    private void SpawnTargetProjectile()
    {
        GameObject clone = Instantiate(targetProjectilePrefab, spawnPoint.position, Quaternion.identity);
        // ������ �߻�ü���� ���ݴ��(attackTarget) ���� ����
        // ���ݷ� = Ÿ�� �⺻ ���ݷ� + ������ ���� �߰��� ���ݷ�
        float damage = towerData.weapon[level].damage; // +Adddamage
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
        RaycastHit2D[] hit = Physics2D.RaycastAll(spawnPoint.position, direction, towerData.weapon[level].range);

        // ���� �������� ���� ���� ������ ���� �� �� ���� attackTarget�� ������ ������Ʈ�� ����
        for (int i = 0; i < hit.Length; ++i)
        {
            if (hit[i].transform == attackTarget)
            {
                // ���� ��������
                lineRenderer.SetPosition(0, spawnPoint.position);
                // ���� ��ǥ����
                lineRenderer.SetPosition(1, new Vector3(hit[i].point.x, hit[i].point.y, 0) + Vector3.back);
                // Ÿ�� ȿ�� ��ġ ����
                //hitEffect.position = hit[i].point;
                // �� ü�� ���� (1�ʿ� damage��ŭ ����)
                // ���ݷ� = Ÿ�� �⺻ ���ݷ� + ������ ���� �߰��� ���ݷ�
                float damage = towerData.weapon[level].damage; // + AddedDamage;
                attackTarget.GetComponent<EnemyHp>().TakeDamage(damage * Time.deltaTime);
            }
        }
    }


}
