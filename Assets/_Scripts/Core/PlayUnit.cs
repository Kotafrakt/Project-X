using UnityEngine;

public class PlayUnit : MonoBehaviour
{
    public UnitType type;
    public float hp;                                    //Здоровья
    public float hpRegen;                               //Востановление здоровья монстров
    public float hpCurrent;                             //Текущее здоровье 
    public float attackDelay;                           //Задержка атаки
    public float distance;                              //Дистанция атаки
    public float speed;                                 //Скорость движения
    public float damage;                                //Урон
    public float critsDamage;                           //Критический урон
    public float critsChance;                           //Шанса критической атаки
    public float armor;                                 //Защита
    public float fireResist;                            //Защита от урона огня
    public float iceResist;                             //Защита от урона холода 
    public float electricResist;                        //Защита от урона молнии 
    public bool IsDead = false;                         //Живой/мертвый
    float saveSpeed;                                    //Сохранение скорости юнита
    bool isBlock;                                       //Столкновение с преградой
    BlockPoint block;                                   //Ссылка на конечный объект
    float attackCouter;                                 //Время последней атаки

    int target = 0;
    float navigationTime = 0;
    Collider2D unitCollider;
    Animator anim;
    Transform unit;
    public Unit baseUnit;
    //    [HideInInspector]
    public Transform[] wayPoints;
    [HideInInspector]
    public Transform finish;
    [SerializeField]
    float navigation;

    private void Start()
    {
        unit = transform;
        unitCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        navigationTime = 0.02f;
        hpCurrent = hp;
        attackCouter = attackDelay;
    }

    void Update()
    {
        if (wayPoints != null && !IsDead)
        {
            navigationTime += Time.deltaTime * speed;
            if (navigationTime > navigation)
            {
                if (target < wayPoints.Length)
                {
                    unit.position = Vector2.MoveTowards(unit.position, wayPoints[target].position, navigationTime);
                }
                else if (!IsDead)
                {
                    unit.position = Vector2.MoveTowards(unit.position, finish.position, navigationTime);
                }
                navigationTime = 0;
            }
        }
        if (block != null)
        {
            if (isBlock && !block.isDead)
            {

                if (attackCouter <= 0)
                {
                    attackCouter = attackDelay;
                    block.currentHp -= damage;
                    hpCurrent -= block.damage;
                    Debug.Log("Хрышь " + hpCurrent + " хп, Забор " + block.currentHp + " хп");
                }
                else
                {
                    attackCouter -= Time.deltaTime;
                }
            }
            else
            {
                speed = saveSpeed;
                isBlock = false;
                block = null;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MoviengPoint")
        {
            target++;
        }
        else if (collision.tag == "Block")
        {
            BlockPoint target = collision.transform.GetComponent<BlockPoint>();
            if (!target.isDead)
            {
                saveSpeed = speed;
                speed = 0;
                //Тут должна быть смена анимации на атаку барьера
                isBlock = true;
                block = target;
            }
            else
            {
                speed = saveSpeed;
                isBlock = false;
                block = null;
            }
        }
        else if (collision.tag == "Projectile")
        {
            Projectile newP = collision.gameObject.GetComponent<Projectile>();
            if (newP != null)
                UnitHit(newP);
            Destroy(collision.gameObject);

        }
        else if (collision.tag == "Finish")
        {
            LevelManager.instance.finishedUnits.Add(baseUnit);
            LevelManager.instance.UnitList.Remove(this);
            LevelManager.instance.Finished(baseUnit);
            Destroy(gameObject);
        }
    }

    public void UnitHit(Projectile newP)
    {
        int damage = newP.AttackDamage;
        int endDamage = damage;


        if (endDamage < 0)
            endDamage = 0;

        GameObject gt = Instantiate(ResManager.instance.DamageText, transform);
        gt.transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y + 1, transform.localPosition.z);
        DamageText dt = gt.GetComponent<DamageText>();
        dt.startMitonPlay("-" + endDamage, Colors.RedColor);

        if (hpCurrent - endDamage > 0)
        {
            hpCurrent -= endDamage;
            //Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Hit, 0.6f);
            //anim.Play("Hurt");
        }
        else
        {
            //anim.SetTrigger("didDie");
            Die();
        }
        if(newP.PType == ProjectileType.fireball)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, newP.radius);
            foreach(Collider2D hit in colliders)
            {
                PlayUnit playUnit = hit.transform.GetComponent<PlayUnit>();
                if(playUnit != null && playUnit !=this)
                {
                    GameObject go = Instantiate(ResManager.instance.Explosion, transform);
                    go.transform.position = transform.localPosition;
                    playUnit.UnitHit2(newP.AttackDamage);
                }
            }
        }
    }

    public void UnitHit2(int damage)
    {
        int endDamage = damage/2;

        if (endDamage < 0)
            endDamage = 0;
        GameObject gt = Instantiate(ResManager.instance.DamageText, transform);
        gt.transform.position = new Vector3(transform.localPosition.x, transform.localPosition.y + 1, transform.localPosition.z);
        DamageText dt = gt.GetComponent<DamageText>();
        dt.startMitonPlay("-" + endDamage, Colors.RedColor);
        if (hpCurrent - endDamage > 0)
        {
            hpCurrent -= endDamage;
            //Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Hit, 0.6f);
            //anim.Play("Hurt");
        }
        else
        {
            //anim.SetTrigger("didDie");
            Die();
        }
    }

    public void Die()
    {
        IsDead = true;
        unitCollider.enabled = false;
        LevelManager.instance.UnitList.Remove(this);
        Destroy(gameObject);
        //Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Death);
    }
}
