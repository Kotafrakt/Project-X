using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Defines;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;
    //    [HideInInspector]
    //    public List<PlayEnemy> EnemyList = new List<PlayEnemy>();
    public volatile Dictionary<UnitType, bool> runingUnit = new Dictionary<UnitType, bool>();
    Text mana;

    [HideInInspector]
    public List<PlayUnit> UnitList = new List<PlayUnit>();

    Transform canvas;
    private Transform[] wayPointsUnit0;
    private Transform[] wayPointsUnit1;
    private Transform[] wayPointsUnit2;
    private Transform finish;
    private GameObject spawnPoint;
    private int needFinished;
    private int countFinished;

    GameObject panel;

    [HideInInspector]
    public string levelName;
    [SerializeField]
    private int unitsPerSpawn = 10;
    [SerializeField]
    private int totalUnits = 10;

    private GameObject whichUnitsToSpawn;
    private float spawnDelay;

    private GameObject buttonHero;
    private List<GameObject> buttonsGeneral = new List<GameObject>();
    private List<GameObject> buttonsUnit = new List<GameObject>();
    public List<Unit> finishedUnits = new List<Unit>();

    private void Awake()
    {
        if (instance == null)
        { // Экземпляр менеджера не был найден
            instance = this; // Задаем ссылку на экземпляр объекта
        }
        else
        { // Экземпляр объекта уже существует на сцене
            Destroy(gameObject); // Удаляем объект
        }
        DontDestroyOnLoad(gameObject);
        unitsPerSpawn = 1;
        totalUnits = 10;
        runingUnit.Add(UnitType.SkeletonV0, false);
        runingUnit.Add(UnitType.SkeletonV1, false);
        runingUnit.Add(UnitType.SkeletonV2, false);
        runingUnit.Add(UnitType.ZombieV0, false);
        runingUnit.Add(UnitType.ZombieV1, false);
        runingUnit.Add(UnitType.ZombieV2, false);
    }
    private void FixedUpdate()
    {
        if (mana != null)
        {
            mana.text = (Mathf.RoundToInt(Hero.PARAMS[HERO_MANA_CURRENT]).ToString() + "/" + Hero.PARAMS[HERO_MANA_MAX].ToString());
        }
        if (Hero.PARAMS[HERO_MANA_CURRENT] < Hero.PARAMS[HERO_MANA_MAX])
        {
            Hero.PARAMS[HERO_MANA_CURRENT] += Hero.PARAMS[HERO_MANA_REGEN] * Time.deltaTime;
            if (Hero.PARAMS[HERO_MANA_CURRENT] >= Hero.PARAMS[HERO_MANA_MAX])
            {
                Hero.PARAMS[HERO_MANA_CURRENT] = Hero.PARAMS[HERO_MANA_MAX];
            }
        }
    }

    public void StartLevel(string name, Transform[] wayPU0, Transform[] wayPU1, Transform[] wayPU2, Transform finish, GameObject sp, int needFinished, Text mana)
    {
        Hero.PARAMS[HERO_MANA_CURRENT] = Hero.PARAMS[HERO_MANA_MAX];
        this.levelName = name;
        this.mana = mana;
        spawnPoint = sp;
        this.wayPointsUnit0 = new Transform[wayPU0.Length];
        for (int i = 0; i < wayPointsUnit0.Length; i++)
        {
            wayPointsUnit0[i] = wayPU0[i];
        }
        this.wayPointsUnit1 = new Transform[wayPU1.Length];
        for (int i = 0; i < wayPointsUnit1.Length; i++)
        {
            wayPointsUnit1[i] = wayPU1[i];
        }
        this.wayPointsUnit2 = new Transform[wayPU2.Length];
        for (int i = 0; i < wayPointsUnit2.Length; i++)
        {
            wayPointsUnit2[i] = wayPU2[i];
        }
        this.needFinished = needFinished;
        this.finish = finish;

        runingUnit[UnitType.SkeletonV0] = false;
        runingUnit[UnitType.SkeletonV1] = false;
        runingUnit[UnitType.SkeletonV2] = false;
        runingUnit[UnitType.ZombieV0] = false;
        runingUnit[UnitType.ZombieV1] = false;
        runingUnit[UnitType.ZombieV2] = false;
    }

    public void SpawnUnit(Unit unit, float spDelay)
    {
        whichUnitsToSpawn = unit.prefab;
        spawnDelay = spDelay;
        totalUnits = Mathf.Clamp(1000, 0, (int)unit.PARAMS[UNIT_COUNT]);
        StartCoroutine(Spawn(unit));
    }

    IEnumerator Spawn(Unit unit)
    {
        if (!runingUnit[unit.type])
        {
            yield break;
        }

        if (unitsPerSpawn > 0 && unit.PARAMS[UNIT_COUNT] > 0 && Hero.PARAMS[HERO_MANA_CURRENT] >= unit.PARAMS[UNIT_COST])
        {
            for (int i = 0; i < unitsPerSpawn; i++)
            {

                if (unit.PARAMS[UNIT_COUNT] > 0)
                {
                    GameObject portal = Instantiate(ResManager.instance.portal) as GameObject;
                    portal.transform.position = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y + 0.8f, spawnPoint.transform.position.z);
                    GameObject newUnit = Instantiate(whichUnitsToSpawn) as GameObject;
                    newUnit.transform.position = spawnPoint.transform.position;
                    PlayUnit pUnit = newUnit.GetComponent<PlayUnit>();
                    pUnit.type = unit.type;
                    pUnit.hp = unit.PARAMS[UNIT_HP_MAX];
                    pUnit.hpCurrent = unit.PARAMS[UNIT_HP_MAX];
                    pUnit.attackDelay = unit.PARAMS[UNIT_ATTACK_DELAY];
                    pUnit.speed = unit.PARAMS[UNIT_SPEED];
                    pUnit.damage = unit.PARAMS[UNIT_DAMAGE];
                    pUnit.distance = unit.PARAMS[UNIT_DISTANCE];
                    pUnit.armor = unit.PARAMS[UNIT_ARMOR];
                    pUnit.fireResist = unit.PARAMS[UNIT_FIRE_RESIST];
                    pUnit.iceResist = unit.PARAMS[UNIT_ICE_RESIST];
                    pUnit.electricResist = unit.PARAMS[UNIT_ELECTRIC_RESIST];
                    pUnit.IsDead = false;
                    pUnit.wayPoints = wayPointsUnit0;
                    pUnit.finish = finish;
                    pUnit.baseUnit = unit;
                    UnitList.Add(pUnit);
                    unit.PARAMS[UNIT_COUNT] -= 1;
                    Hero.PARAMS[HERO_MANA_CURRENT] -= unit.PARAMS[UNIT_COST];
                }
            }

            if (unit.PARAMS[UNIT_COUNT] == 0)
            {
                runingUnit[unit.type] = false;
                yield break;
            }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn(unit));
        }
    }

    public void RegisterUnit(PlayUnit enemy)
    {
        UnitList.Add(enemy);
    }

    public void UnRegisterUnit(PlayUnit enemy)
    {
        UnitList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void DestroyUnit()
    {
        foreach (PlayUnit enemy in UnitList)
        {
            Destroy(enemy.gameObject);
        }
        UnitList.Clear();
    }

    public void Finished(Unit unit)
    {
        countFinished++;
        if (countFinished >= needFinished)
        {
            Time.timeScale = 0;
            countFinished = 0;
            Returningunits();
            panel = Camera.main.transform.GetComponent<Level>().finished;
            panel.SetActive(true);
        }
    }

    public void Fin()
    {
        panel.SetActive(false);
        GameManager.instance.HeroPos = levelName;
        GameManager.instance.locations[levelName] = 2;
        Hero.PARAMS[HERO_MANA_CURRENT] = Hero.PARAMS[HERO_MANA_MAX];
        bool isRoad = true;
        Time.timeScale = 1;
        switch (levelName[0])
        {
            case 'V':
                isRoad = false;
                break;
            case 'C':
                isRoad = false;
                break;
        }
        if (isRoad)
            SceneManager.LoadScene("GlobalMap");
        else
            SceneManager.LoadScene(levelName + "B");
    }

    void Returningunits()
    {
        if (finishedUnits.Count > 0)
        {
            for (int i = 0; i < finishedUnits.Count; i++)
            {
                for (int j = 0; j < GameManager.instance.units.Count; j++)
                {
                    if (GameManager.instance.units[j].type == finishedUnits[i].type)
                        GameManager.instance.units[j].PARAMS[UNIT_COUNT]++;
                }
            }
        }
        finishedUnits.Clear();
        if (UnitList.Count > 0)
        {
            for (int i = 0; i < UnitList.Count; i++)
            {
                for (int j = 0; j < GameManager.instance.units.Count; j++)
                {
                    if (GameManager.instance.units[j].type == UnitList[i].type)
                        GameManager.instance.units[j].PARAMS[UNIT_COUNT]++;
                }
            }
        }
        UnitList.Clear();
    }
}
