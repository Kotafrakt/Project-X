using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    float timeBetWeenAttacks;
    [SerializeField]
    private float attackRadius;
    [SerializeField]
    Projectile projectile;
    PlayUnit targetEnemy = null;
    float attackCouter;
    bool isAttacking = false;
    Unit unit;

    void Start()
    {
        unit = null;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        attackCouter -= Time.deltaTime;
        PlayUnit nearestEnemy = null;
        if (GetNearestEnemy() != null)
        {
            nearestEnemy = GetNearestEnemy();
        }
        if (targetEnemy == null || targetEnemy.IsDead)
        {
            if (nearestEnemy != null)
            {
                targetEnemy = nearestEnemy;
            }
        }
        else
        {
            if (attackCouter <= 0)
            {
                isAttacking = true;
                attackCouter = timeBetWeenAttacks;
            }
            else
            {
                isAttacking = false;
            }
            if (Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > attackRadius)
            {
                targetEnemy = null;
            }
        }
        if (isAttacking)
        {
            Attack();
        }
    }

    public void Attack()
    {
        isAttacking = false;
        Projectile newProjectile = Instantiate(projectile) as Projectile;
        newProjectile.transform.position = transform.position;
        newProjectile.UnitAttacker = unit;

        if (newProjectile.PType == ProjectileType.arrow)
        {
            //Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Arrow, 0.2f);
        }
        else if (newProjectile.PType == ProjectileType.fireball)
        {
            //Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Fireball, 0.5f);
        }
        else if (newProjectile.PType == ProjectileType.rock)
        {
            //Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Rock);
        }

        if (targetEnemy == null)
        {
            Destroy(newProjectile);
        }
        else
        {
            StartCoroutine(MoveProjectile(newProjectile));
        }
    }

    IEnumerator MoveProjectile(Projectile projectile)
    {
        while (GetTargetDistance(targetEnemy) > 0.2f && projectile != null && targetEnemy != null)
        {
            if (projectile != null && targetEnemy == null)
            {
                Destroy(projectile);
                yield break;
            }
            if (targetEnemy != null && transform != null)
            {
                var dir = targetEnemy.transform.localPosition - projectile.transform.localPosition;
                var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                if (projectile != null)
                {
                    projectile.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
                    projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.localPosition, targetEnemy.transform.localPosition, 10f * Time.deltaTime);
                }
            }
            yield return null;
        }
    }

    float GetTargetDistance(PlayUnit thisEnemy)
    {
        if (thisEnemy == null)
        {
            thisEnemy = GetNearestEnemy();
            if (thisEnemy == null)
            {
                return 0f;
            }
        }
        return Mathf.Abs(Vector2.Distance(transform.localPosition, thisEnemy.transform.localPosition));
    }

    PlayUnit GetNearestEnemy()
    {
        PlayUnit nearestEnemy = null;
        float smallestDistance = float.PositiveInfinity;
        if (GetEnemiesInRange() != null)
        {
            foreach (PlayUnit enemy in GetEnemiesInRange())
            {
                if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= smallestDistance)
                {
                    smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
                    nearestEnemy = enemy;
                }
            }
        }
        return nearestEnemy;
    }

    List<PlayUnit> GetEnemiesInRange()
    {
        List<PlayUnit> enemiesInRange = new List<PlayUnit>();
        if (LevelManager.instance.UnitList != null && LevelManager.instance.UnitList.Count > 0)
        {
            foreach (PlayUnit unit in LevelManager.instance.UnitList)
            {
                if (Vector2.Distance(transform.localPosition, unit.transform.localPosition) <= attackRadius)
                {
                    enemiesInRange.Add(unit);
                }
            }
        }
        return enemiesInRange;
    }

}
