using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Defines;

public interface IMessage
{
    void Send(string s);
}

///Пошаговый бой
public class BossLevel : MonoBehaviour, IMessage
{
    [SerializeField]
    string levelName;
    [SerializeField]
    Transform[] spawnPlayer;
    [SerializeField]
    Transform[] spawnEnemy;

    [HideInInspector]
    public TurnUnit selectedUnit;
    [HideInInspector]
    public TurnEnemy selectedEnemy;

    GameObject startBossLevel;
    GameObject playBossPanelUp;
    GameObject playBossPanelDown;
    GameObject levelBossMenu;
    GameObject levelBossResult;

    public Transform canvas;
    int rHit;
    Text текстРезультат;
    Button toGlobal;
    Button btnMenu;
    Button btnStart;
    Button btnBack;
    Button принять;
    Button продолжить;
    Button бежать;
    Button настройки;
    Button выход;
    Button attack;
    Button отмена;

    SkillBtn defaultSkill;
    SkillBtn skill_0;
    SkillBtn skill_1;
    SkillBtn skill_2;
    SkillBtn skill_B;
    SkillName skillName = SkillName.none;
    SkillNameB skillNameB = SkillNameB.none;

    int skill_lvl = 0;

    GameObject выбранные;
    GameObject доступные;
    List<GeneralBtn> выбрГенерал = new List<GeneralBtn>();
    List<GeneralBtn> достГенерал = new List<GeneralBtn>();
    GeneralBtn selectedGeneral;
    Text инфоОкно0;
    Text инфоОкно1;
    Text resultText;
    Transform trophysPanel;
    bool isWinner = false;

    GameObject messagePanel;
    Text messageText;
    Button messageButton;

    public List<TurnUnit> units = new List<TurnUnit>();
    public List<TurnEnemy> enemyes = new List<TurnEnemy>();
    public List<TurnUnit> unitsCurrent = new List<TurnUnit>();
    public List<TurnEnemy> enemyesCurrent = new List<TurnEnemy>();
    public List<Transform> trophys = new List<Transform>();

    void Start()
    {
        CreatePanelBossLevel();
        СоздатьЛевелМеню();        
    }
    void CreatePanelBossLevel()
    {
        playBossPanelUp = Instantiate(ResManager.instance.playBossPanelUp, canvas);
        startBossLevel = Instantiate(ResManager.instance.startBossLevel, canvas);
        btnMenu = playBossPanelUp.transform.GetChild(0).GetComponent<Button>();
        btnMenu.onClick.AddListener(delegate { OpenMenu(); });
        toGlobal = playBossPanelUp.transform.GetChild(1).GetComponent<Button>();
        toGlobal.onClick.AddListener(delegate { ToGlobal(); });
        btnStart = startBossLevel.transform.GetChild(3).GetComponent<Button>();
        btnStart.onClick.AddListener(delegate { PlayLevel(); });

        выбранные = startBossLevel.transform.GetChild(0).gameObject;
        for (int i = 0; i < выбранные.transform.childCount; i++)
        {
            GeneralBtn gB = выбранные.transform.GetChild(i).GetComponent<GeneralBtn>();
            gB.isSlot = true;
            выбрГенерал.Add(gB);
        }
        доступные = startBossLevel.transform.GetChild(1).gameObject;
        for (int i = 0; i < доступные.transform.childCount; i++)
        {
            достГенерал.Add(доступные.transform.GetChild(i).GetComponent<GeneralBtn>());
        }
        инфоОкно0 = startBossLevel.transform.GetChild(4).GetComponent<Text>();
        отмена = startBossLevel.transform.GetChild(5).GetComponent<Button>();
        отмена.onClick.AddListener(delegate { Отмена(); });
        инфоОкно1 = startBossLevel.transform.GetChild(6).GetComponent<Text>();
        for (int i = 0; i < достГенерал.Count; i++) //Очистка
        {
            достГенерал[i].general = null;
            достГенерал[i].img.sprite = null;
            достГенерал[i].img.gameObject.SetActive(false);
            достГенерал[i].text.text = "";
            достГенерал[i].text.gameObject.SetActive(false);
            достГенерал[i].interactable = false;
        }
        for (int i = 0; i < GameManager.instance.generals.Count; i++) //Заполнение
        {
            if (!GameManager.instance.generals[i].isDead)
            {
                достГенерал[i].general = GameManager.instance.generals[i];
                достГенерал[i].img.sprite = достГенерал[i].general.img;
                достГенерал[i].img.gameObject.SetActive(true);
                достГенерал[i].text.text = достГенерал[i].general.PARAMS[GENERAL_LEVEL].ToString();
                достГенерал[i].text.gameObject.SetActive(true);
                достГенерал[i].interactable = true;
            }
        }
    }

    public void AddGeneral(GeneralBtn gB)
    {
        selectedGeneral = gB;
        if (gB.gBtn != null)
        {
            gB.gBtn.interactable = true;
            gB.general = null;
            gB.img.sprite = null;
            gB.img.gameObject.SetActive(false);
            gB.text.gameObject.SetActive(false);
        }
        выбранные.SetActive(false);
        btnStart.gameObject.SetActive(false);
        инфоОкно0.gameObject.SetActive(false);
        отмена.gameObject.SetActive(true);
        доступные.SetActive(true);
        инфоОкно1.gameObject.SetActive(true);
    }
    public void SelectGeneral(GeneralBtn gB)
    {
        gB.interactable = false;
        selectedGeneral.gBtn = gB;
        доступные.SetActive(false);
        инфоОкно1.gameObject.SetActive(false);
        отмена.gameObject.SetActive(false);
        selectedGeneral.general = gB.general;
        selectedGeneral.img.sprite = gB.general.img2;
        selectedGeneral.img.gameObject.SetActive(true);
        selectedGeneral.text.text = gB.general.PARAMS[GENERAL_LEVEL].ToString();
        selectedGeneral.text.gameObject.SetActive(true);
        выбранные.SetActive(true);
        btnStart.gameObject.SetActive(true);
        инфоОкно0.gameObject.SetActive(true);
    }
    void PlayLevel()
    {
        startBossLevel.SetActive(false);
        CreatePanelBossDawn();
        SpawnUnits();

        unitsCurrent = units;
        enemyesCurrent = enemyes;

        Sort();

        UcVsEc();
    }
    void ToGlobal()
    {
        SceneManager.LoadScene("GlobalMap");
    }

    void Отмена()
    {
        доступные.SetActive(false);
        инфоОкно1.gameObject.SetActive(false);
        отмена.gameObject.SetActive(false);
        выбранные.SetActive(true);
        инфоОкно0.gameObject.SetActive(true);
        btnStart.gameObject.SetActive(true);
    }
    

    void СоздатьЛевелМеню()
    {
        levelBossMenu = Instantiate(ResManager.instance.levelBossMenu, canvas);
        продолжить = levelBossMenu.transform.GetChild(0).GetComponent<Button>();
        продолжить.onClick.AddListener(delegate { Продолжить(); });
        бежать = levelBossMenu.transform.GetChild(1).GetComponent<Button>();
        бежать.onClick.AddListener(delegate { Бежать(); });
        настройки = levelBossMenu.transform.GetChild(2).GetComponent<Button>();
        настройки.onClick.AddListener(delegate { Настройки(); });
        выход = levelBossMenu.transform.GetChild(3).GetComponent<Button>();
        выход.onClick.AddListener(delegate { Выход(); });
        btnBack = startBossLevel.transform.GetChild(7).GetComponent<Button>();
        btnBack.onClick.AddListener(delegate { OpenMenu(); });
    }
    void OpenMenu()
    {
        if (levelBossMenu.activeSelf)
        {
            Time.timeScale = 1;
            levelBossMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            levelBossMenu.SetActive(true);
        }
    }
    void Продолжить()
    {
        Time.timeScale = 1;
        levelBossMenu.SetActive(false);
    }
    void Бежать()
    {
        Time.timeScale = 1;
        LevelManager.instance.UnitList.Clear();
        SceneManager.LoadScene("GlobalMap");
    }
    void Настройки()
    {

    }
    void Выход()
    {
        LevelManager.instance.UnitList.Clear();
        Application.Quit();
    }
    void CreatePanelBossDawn()
    {
        playBossPanelDown = Instantiate(ResManager.instance.playBossPanelDown, canvas);
        defaultSkill = playBossPanelDown.transform.GetChild(0).GetComponent<SkillBtn>();
        defaultSkill.onClick.AddListener(delegate { DefaultSkill(); });
        skill_0 = playBossPanelDown.transform.GetChild(1).GetComponent<SkillBtn>();
        skill_0.onClick.AddListener(delegate { Skill_0(); });
        skill_1 = playBossPanelDown.transform.GetChild(2).GetComponent<SkillBtn>();
        skill_1.onClick.AddListener(delegate { Skill_1(); });
        skill_2 = playBossPanelDown.transform.GetChild(3).GetComponent<SkillBtn>();
        skill_2.onClick.AddListener(delegate { Skill_2(); });
        skill_B = playBossPanelDown.transform.GetChild(4).GetComponent<SkillBtn>();
        skill_B.onClick.AddListener(delegate { Skill_B(); });
        attack = playBossPanelDown.transform.GetChild(5).GetComponent<Button>();
        attack.onClick.AddListener(delegate { Attack(); });
    }
    void DefaultSkill()
    {
        skillName = SkillName.none;
        skill_0.GetComponent<Image>().color = Colors.WhiteColor;
        skill_1.GetComponent<Image>().color = Colors.WhiteColor;
        skill_2.GetComponent<Image>().color = Colors.WhiteColor;
        skill_B.GetComponent<Image>().color = Colors.WhiteColor;
        defaultSkill.GetComponent<Image>().color = Colors.GreenColor;
        skill_lvl = 0;
    }
    void Skill_0()
    {
        skillName = skill_0.skillName;
        defaultSkill.GetComponent<Image>().color = Colors.WhiteColor;
        skill_1.GetComponent<Image>().color = Colors.WhiteColor;
        skill_2.GetComponent<Image>().color = Colors.WhiteColor;
        skill_B.GetComponent<Image>().color = Colors.WhiteColor;
        skill_0.GetComponent<Image>().color = Colors.GreenColor;
        skill_lvl = skill_0.skill_lvl;
        selectedGeneral.general.skill0Delay_Curent = selectedGeneral.general.skill0Delay;
    }
    void Skill_1()
    {
        skillName = skill_1.skillName;
        defaultSkill.GetComponent<Image>().color = Colors.WhiteColor;
        skill_0.GetComponent<Image>().color = Colors.WhiteColor;
        skill_2.GetComponent<Image>().color = Colors.WhiteColor;
        skill_B.GetComponent<Image>().color = Colors.WhiteColor;
        skill_1.GetComponent<Image>().color = Colors.GreenColor;
        skill_lvl = skill_1.skill_lvl;
    }
    void Skill_2()
    {
        skillName = skill_2.skillName;
        defaultSkill.GetComponent<Image>().color = Colors.WhiteColor;
        skill_0.GetComponent<Image>().color = Colors.WhiteColor;
        skill_1.GetComponent<Image>().color = Colors.WhiteColor;
        skill_B.GetComponent<Image>().color = Colors.WhiteColor;
        skill_2.GetComponent<Image>().color = Colors.GreenColor;
        skill_lvl = skill_2.skill_lvl;
    }
    void Skill_B()
    {
        skillNameB = skill_B.skillNameB;
        defaultSkill.GetComponent<Image>().color = Colors.WhiteColor;
        skill_0.GetComponent<Image>().color = Colors.WhiteColor;
        skill_1.GetComponent<Image>().color = Colors.WhiteColor;
        skill_2.GetComponent<Image>().color = Colors.WhiteColor;
        skill_B.GetComponent<Image>().color = Colors.GreenColor;
        skill_lvl = skill_B.skill_lvl;
    }
    void Attack()
    {
        if (selectedEnemy != null)
        {
            selectedEnemy.UnSelected();
        }
        selectedUnit.UnSelected();
        attack.interactable = false;
        if (!TurnPlayer())
        {
            Invoke("AttackDelay", 1f);
        }
        else
        {
            Invoke("AttackDelay", 3f);
        }
    }

    void AttackDelay()
    {
        if (unitsCurrent.Count == 0 && enemyesCurrent.Count == 0)
        {
            for (int i = 0; i < units.Count; i++)
            {
                units[i].isEndTurn = false;
            }
            for (int i = 0; i < enemyes.Count; i++)
            {
                enemyes[i].isEndTurn = false;
            }
            unitsCurrent = units;
            enemyesCurrent = enemyes;
        }
        Sort();
        UcVsEc();
    }
    void Sort()
    {
        unitsCurrent = SortPlayerReady();
        enemyesCurrent = SortEnemyReady();
        unitsCurrent = SortPlayerInitiative();
        enemyesCurrent = SortEnemyInitiative();
    }

    void SpawnUnits()
    {
        List<General> mobs = CreateGeneralEnemy.CreateEnemy(levelName, 0);
        for (int i = 0; i < mobs.Count; i++)
        {
            GameObject go = Instantiate(mobs[i].prefab, spawnEnemy[i]);
            TurnEnemy tEnemy = go.GetComponent<TurnEnemy>();
            if (tEnemy != null)
            {
                tEnemy.cellNum = i;
                tEnemy.general = mobs[i];
                tEnemy.text.color = Colors.GreenColor;
                enemyes.Add(tEnemy);
            }

        }
        List<General> mobsU = new List<General>();
        for (int i = 0; i < выбрГенерал.Count; i++)
        {
            if (выбрГенерал[i].general != null)
            {
                GameObject go = Instantiate(выбрГенерал[i].general.prefab, spawnPlayer[i]);
                TurnUnit tUnit = go.GetComponent<TurnUnit>();
                tUnit.general = выбрГенерал[i].general;
                tUnit.cellNum = i;
                tUnit.text.color = Colors.GreenColor;
                tUnit.text.text = выбрГенерал[i].general.PARAMS[GENERAL_HP_CURRENT].ToString();
                units.Add(tUnit);
            }
        }
    }

    List<TurnUnit> SortPlayerReady()
    {
        List<TurnUnit> lst = new List<TurnUnit>();
        for (int i = 0; i < unitsCurrent.Count; i++)
        {
            if (!unitsCurrent[i].isEndTurn)
            {
                lst.Add(unitsCurrent[i]);
            }
        }
        return lst;
    }
    List<TurnUnit> SortPlayerInitiative()
    {
        TurnUnit temp;
        for (int i = 0; i < unitsCurrent.Count; i++)
        {
            for (int j = i + 1; j < unitsCurrent.Count; j++)
            {
                if (unitsCurrent[i].general.PARAMS[GENERAL_INITIATIVE_CURRENT] < unitsCurrent[j].general.PARAMS[GENERAL_INITIATIVE_CURRENT])
                {
                    temp = unitsCurrent[i];
                    unitsCurrent[i] = unitsCurrent[j];
                    unitsCurrent[j] = temp;
                }
            }
        }
        return unitsCurrent;
    }


    List<TurnEnemy> SortEnemyReady()
    {
        List<TurnEnemy> lst = new List<TurnEnemy>();
        for (int i = 0; i < enemyesCurrent.Count; i++)
        {
            if (!enemyesCurrent[i].isEndTurn)
            {
                lst.Add(enemyesCurrent[i]);
            }
        }
        return lst;
    }
    List<TurnEnemy> SortEnemyInitiative()
    {
        TurnEnemy temp;
        for (int i = 0; i < enemyesCurrent.Count; i++)
        {
            for (int j = i + 1; j < enemyesCurrent.Count; j++)
            {
                if (enemyesCurrent[i].general.PARAMS[GENERAL_INITIATIVE_CURRENT] < enemyesCurrent[j].general.PARAMS[GENERAL_INITIATIVE_CURRENT])
                {
                    temp = enemyesCurrent[i];
                    enemyesCurrent[i] = enemyesCurrent[j];
                    enemyesCurrent[j] = temp;
                }
            }
        }
        return enemyesCurrent;
    }

    void UcVsEc()
    {
        float u;
        float e;
        if (unitsCurrent.Count > 0)
        {
            u = unitsCurrent[0].general.PARAMS[GENERAL_INITIATIVE_CURRENT];
        }

        else u = 0;

        if (enemyesCurrent.Count > 0)
        {
            e = enemyesCurrent[0].general.PARAMS[GENERAL_INITIATIVE_CURRENT];
        }
        else e = 0;

        if (u >= e && unitsCurrent.Count > 0 && enemyesCurrent.Count > 0)
        {
            selectedUnit = unitsCurrent[0];
            if (selectedEnemy == null)
            {
                selectedEnemy = enemyesCurrent[0];
            }
            selectedEnemy.Select();
            selectedUnit.Select();
            attack.interactable = true;
            WriteSkills();
        }
        else if (u < e && unitsCurrent.Count > 0 && enemyesCurrent.Count > 0)
        {
            if (selectedEnemy != null)
            {
                selectedEnemy.UnSelected();
            }
            if (selectedUnit != null)
            {
                selectedUnit.UnSelected();
            }
            TurnEnemy();
        }
        else if (unitsCurrent.Count > 0 && enemyesCurrent.Count == 0)
        {
            selectedUnit = unitsCurrent[0];
            if (selectedEnemy == null)
            {
                selectedEnemy = enemyes[0];
            }
            selectedEnemy.Select();
            selectedUnit.Select();
            attack.interactable = true;
            WriteSkills();
        }
        else if (unitsCurrent.Count == 0 && enemyesCurrent.Count > 0)
        {
            selectedEnemy.UnSelected();
            if (selectedUnit != null)
            {
                selectedUnit.UnSelected();
            }
            TurnEnemy();
        }
        else if (unitsCurrent.Count == 0 && enemyesCurrent.Count == 0)
        {
            for (int i = 0; i < units.Count; i++)
            {
                units[i].isEndTurn = false;
            }
            for (int i = 0; i < enemyes.Count; i++)
            {
                enemyes[i].isEndTurn = false;
            }
            unitsCurrent = units;
            enemyesCurrent = enemyes;
            Sort();

            UcVsEc();
        }
    }

    void WriteSkills()
    {
        if (selectedUnit.general.skill0 != SkillName.none)
        {
            if(selectedGeneral.general.skill0Delay_Curent < 1)
            {
                skill_0.interactable = true;
            }
            else skill_0.interactable = false;
            skill_0.transform.GetChild(0).GetComponent<Image>().sprite = SkillInfo.GetSkill(selectedUnit.general.skill0, selectedUnit.general.skill_lvl_0).img;
            skill_0.transform.GetChild(0).gameObject.SetActive(true);
            skill_0.skillName = selectedUnit.general.skill0;
            skill_0.skill_lvl = selectedUnit.general.skill_lvl_0;
        }
        if (selectedUnit.general.skill1 != SkillName.none)
        {
            skill_1.interactable = true;
            skill_1.transform.GetChild(0).GetComponent<Image>().sprite = SkillInfo.GetSkill(selectedUnit.general.skill1, selectedUnit.general.skill_lvl_1).img;
            skill_1.transform.GetChild(0).gameObject.SetActive(true);
            skill_1.skillName = selectedUnit.general.skill1;
            skill_1.skill_lvl = selectedUnit.general.skill_lvl_1;
        }
        if (selectedUnit.general.skill2 != SkillName.none)
        {
            skill_2.interactable = true;
            skill_2.transform.GetChild(0).GetComponent<Image>().sprite = SkillInfo.GetSkill(selectedUnit.general.skill2, selectedUnit.general.skill_lvl_2).img;
            skill_2.transform.GetChild(0).gameObject.SetActive(true);
            skill_2.skillName = selectedUnit.general.skill2;
            skill_2.skill_lvl = selectedUnit.general.skill_lvl_2;
        }
        if (selectedUnit.general.skillB != SkillNameB.none)
        {
            skill_B.interactable = true;
            skill_B.transform.GetChild(0).GetComponent<Image>().sprite = SkillInfo.GetSkillB(selectedUnit.general.skillB, selectedUnit.general.skill_lvl_B).img;
            skill_B.transform.GetChild(0).gameObject.SetActive(true);
            skill_B.skillNameB = selectedUnit.general.skillB;
            skill_B.skill_lvl = selectedUnit.general.skill_lvl_B;
        }
    }

    void CheckAdnRegenSkill()
    {
        if(selectedGeneral.general.skill0 != skillName)
        {
            if (selectedGeneral.general.skill0Delay_Curent > 0)
                selectedGeneral.general.skill0Delay_Curent--;
        }
        if (selectedGeneral.general.skill1 != skillName)
        {
            if (selectedGeneral.general.skill1Delay_Curent > 0)
                selectedGeneral.general.skill1Delay_Curent--;
        }
        if (selectedGeneral.general.skill2 != skillName)
        {
            if (selectedGeneral.general.skill2Delay_Curent > 0)
                selectedGeneral.general.skill2Delay_Curent--;
        }
        if (selectedGeneral.general.skillB != skillNameB)
        {
            if (selectedGeneral.general.skillBDelay_Curent > 0)
                selectedGeneral.general.skillBDelay_Curent--;
        }
    }

    void CleanSkills()
    {
        if (selectedUnit.general.skill0 != SkillName.none)
        {
            skill_0.interactable = false;
            skill_0.transform.GetChild(0).GetComponent<Image>().sprite = null;
            skill_0.transform.GetChild(0).gameObject.SetActive(false);
            skill_0.skillName = SkillName.none;
        }
        if (selectedUnit.general.skill1 != SkillName.none)
        {
            skill_1.interactable = false;
            skill_1.transform.GetChild(0).GetComponent<Image>().sprite = null;
            skill_1.transform.GetChild(0).gameObject.SetActive(false);
            skill_1.skillName = SkillName.none;
        }
        if (selectedUnit.general.skill2 != SkillName.none)
        {
            skill_2.interactable = false;
            skill_2.transform.GetChild(0).GetComponent<Image>().sprite = null;
            skill_2.transform.GetChild(0).gameObject.SetActive(false);
            skill_2.skillName = SkillName.none;
        }
        if (selectedUnit.general.skillB != SkillNameB.none)
        {
            skill_B.interactable = false;
            skill_B.transform.GetChild(0).GetComponent<Image>().sprite = null;
            skill_B.transform.GetChild(0).gameObject.SetActive(false);
            skill_B.skillNameB = SkillNameB.none;
        }
    }

    bool TurnPlayer()
    {
        if (!PlayerAttack())
        {
            unitsCurrent.Remove(unitsCurrent[0]);
            return false;
        }
        else
        {
            unitsCurrent.Remove(unitsCurrent[0]);
            return true;
        }

    }
    void TurnEnemy()
    {
        Invoke("EnemyAttack", 1f);
    }

    bool PlayerAttack()
    {
        bool targetIsDead = false;
        List<TurnBase> enemyesOut = new List<TurnBase>();
        foreach(TurnBase enemy in enemyes)
        {
            enemyesOut.Add(enemy);
        }
        List<TurnBase> unitsOut = new List<TurnBase>();
        foreach (TurnBase unit in units)
        {
            unitsOut.Add(unit);
        }

        List<CombatResult> combatResults = Combat.Attack(selectedUnit, selectedEnemy, skillName, skillNameB, enemyesOut, unitsOut, skill_lvl);
        for(int i = 0; i < combatResults.Count; i++)
        {
            TurnBase target = combatResults[i].target;
            float damage = combatResults[i].damage;
            target.general.PARAMS[GENERAL_HP_CURRENT] -= damage;
            GameObject gt = Instantiate(ResManager.instance.DamageText, target.transform);
            gt.transform.position = new Vector3(target.transform.parent.transform.position.x, target.transform.parent.transform.position.y, 5f);
            DamageText dt = gt.GetComponent<DamageText>();
            dt.startMitonEnemy("-" + damage, Colors.RedColor);

            if (target.general.PARAMS[GENERAL_HP_CURRENT] > 0)
            {
                target.animator.Play("hart");
                if (target.general.PARAMS[GENERAL_HP_CURRENT] > target.general.PARAMS[GENERAL_HP] / 4 * 3) //75+%
                {
                    target.text.color = Colors.GreenColor;
                }
                else if (target.general.PARAMS[GENERAL_HP_CURRENT] < target.general.PARAMS[GENERAL_HP] / 4 * 3 && target.general.PARAMS[GENERAL_HP_CURRENT] > target.general.PARAMS[GENERAL_HP] / 2) //50% + /75%-
                {
                    target.text.color = Colors.YellowColor;
                }
                else if (target.general.PARAMS[GENERAL_HP_CURRENT] < target.general.PARAMS[GENERAL_HP] / 2 && target.general.PARAMS[GENERAL_HP_CURRENT] > target.general.PARAMS[GENERAL_HP] / 4) //25%+ / 50%-
                {
                    target.text.color = Colors.Orange;
                }
                else if (target.general.PARAMS[GENERAL_HP_CURRENT] < target.general.PARAMS[GENERAL_HP] / 4 && target.general.PARAMS[GENERAL_HP_CURRENT] > 0) //0+ / 25-
                {
                    target.text.color = Colors.RedColor;
                }
                target.text.text = target.general.PARAMS[GENERAL_HP_CURRENT].ToString();
            }
            else
                targetIsDead = true;
        }
        skill_0.GetComponent<Image>().color = Colors.WhiteColor;
        skill_1.GetComponent<Image>().color = Colors.WhiteColor;
        skill_2.GetComponent<Image>().color = Colors.WhiteColor;
        skill_B.GetComponent<Image>().color = Colors.WhiteColor;
        defaultSkill.GetComponent<Image>().color = Colors.GreenColor;
        selectedUnit.animator.Play("attack");        
        selectedUnit.isEndTurn = true;
        skillName = SkillName.none;
        skillNameB = SkillNameB.none;
        skill_lvl = 0;
        CleanSkills();
        CheckAdnRegenSkill();
        return targetIsDead;
    }
    void EnemyAttack()
    {
        enemyesCurrent[0].animator.Play("attack");
        rHit = Random.Range(0, units.Count - 1);
        bool targetIsDead = false;
        List<TurnBase> enemyesOut = new List<TurnBase>();
        foreach (TurnBase enemy in enemyes)
        {
            enemyesOut.Add(enemy);
        }
        List<TurnBase> unitsOut = new List<TurnBase>();
        foreach (TurnBase unit in units)
        {
            unitsOut.Add(unit);
        }

        List<CombatResult> combatResults = Combat.Attack(enemyesCurrent[0], units[rHit], skillName, skillNameB, enemyesOut, unitsOut, skill_lvl);
        for (int i = 0; i < combatResults.Count; i++)
        {
            TurnBase target = combatResults[i].target;
            float damage = combatResults[i].damage;
            target.general.PARAMS[GENERAL_HP_CURRENT] -= damage;
            GameObject gt = Instantiate(ResManager.instance.DamageText, target.transform);
            gt.transform.position = target.transform.parent.transform.localPosition;
            DamageText dt = gt.GetComponent<DamageText>();
            dt.startMitonUnit("-" + damage, Colors.RedColor);
            if (target.general.PARAMS[GENERAL_HP_CURRENT] > 0)
            {
                target.animator.Play("hart");
                if (target.general.PARAMS[GENERAL_HP_CURRENT] > target.general.PARAMS[GENERAL_HP] / 4 * 3) //75+%
                {
                    target.text.color = Colors.GreenColor;
                }
                else if (target.general.PARAMS[GENERAL_HP_CURRENT] < target.general.PARAMS[GENERAL_HP] / 4 * 3 && target.general.PARAMS[GENERAL_HP_CURRENT] > target.general.PARAMS[GENERAL_HP] / 2) //50% + /75%-
                {
                    target.text.color = Colors.YellowColor;
                }
                else if (target.general.PARAMS[GENERAL_HP_CURRENT] < target.general.PARAMS[GENERAL_HP] / 2 && target.general.PARAMS[GENERAL_HP_CURRENT] > target.general.PARAMS[GENERAL_HP] / 4) //25%+ / 50%-
                {
                    target.text.color = Colors.Orange;
                }
                else if (target.general.PARAMS[GENERAL_HP_CURRENT] < target.general.PARAMS[GENERAL_HP] / 4 && target.general.PARAMS[GENERAL_HP_CURRENT] > 0) //0+ / 25-
                {
                    target.text.color = Colors.RedColor;
                }
                target.text.text = target.general.PARAMS[GENERAL_HP_CURRENT].ToString();
            }
            else
                targetIsDead = true;
        }
        skillName = SkillName.none;
        skillNameB = SkillNameB.none;
        skill_lvl = 0;
        if (targetIsDead)
        {
            Invoke("EnemyAttackDelay", 3f);
        }
        else
        {
            Invoke("EnemyAttackDelay", 1f);
        }
    }

    void EnemyAttackDelay()
    {
        enemyesCurrent[0].isEndTurn = true;
        enemyesCurrent.Remove(enemyesCurrent[0]);
        Sort();
        UcVsEc();
    }

    public void Winner()
    {
        Time.timeScale = 0f;
        isWinner = true;
        LevelBossResult();
    }
    public void Lose()
    {
        Time.timeScale = 0f;
        isWinner = false;
        LevelBossResult();
    }

    void LevelBossResult()
    {
        levelBossResult = Instantiate(ResManager.instance.levelBossResult, canvas);
        принять = levelBossResult.transform.GetChild(0).GetComponent<Button>();
        принять.onClick.AddListener(delegate { Принять(); });
        resultText = levelBossResult.transform.GetChild(1).GetComponent<Text>();
        trophysPanel = levelBossResult.transform.GetChild(2);
        for(int i = 0; i < trophysPanel.childCount; i++)
        {
            trophys.Add(trophysPanel.GetChild(i));
            trophys[i].gameObject.SetActive(false);
        }
        if(isWinner)
        {
            resultText.text = GameText.VictoryText();
            List<TrophyRes> tRes = Trophy.GetRes(levelName);
            List<TrophyRes> tResSort = new List<TrophyRes>();
            for (int i = 0; i < tRes.Count; i++)
            {
                if(tRes[i].count > 0)
                {
                    tResSort.Add(tRes[i]);
                }
            }
            for (int i = 0; i < tResSort.Count; i++)
            {
                InfoResources res = GetInfoResources.GetInfo(tResSort[i].num);
                trophys[i].GetChild(0).GetComponent<Image>().sprite = res.img;
                trophys[i].GetChild(1).GetComponent<Text>().text = tResSort[i].count.ToString();
                GameManager.resource[tResSort[i].num] += tResSort[i].count;
                trophys[i].transform.GetComponent<ResBtn>().Name = res.name;
                trophys[i].transform.GetComponent<ResBtn>().message = this;
                trophys[i].gameObject.SetActive(true);
            }
        }
        else
            resultText.text = GameText.LoseText();
    }

    void Принять()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GlobalMap");
    }
    
    void CreateMessagePanel()
    {
        messagePanel = Instantiate(ResManager.instance.message, canvas);
        messageText = messagePanel.transform.GetChild(0).transform.GetComponent<Text>();
        messageButton = messagePanel.transform.GetChild(1).transform.GetComponent<Button>();
        messageButton.onClick.AddListener(delegate { CloseMessage(); });
        messagePanel.SetActive(false);
    }

    void CloseMessage()
    {
        messagePanel.SetActive(false);
    }

    public void OpenMessage(string text)
    {
        messageText.text = text;
        messagePanel.SetActive(true);
    }
    

    public void Send(string s)
    {
        if(messagePanel == null)
        {
            CreateMessagePanel();
        }        
        OpenMessage(s);
    }
}
