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
    Button hit;
    Button skill0;
    Button skill1;
    Button skill2;
    Button skillSuper;
    Button attack;
    Button отмена;

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
        hit = playBossPanelDown.transform.GetChild(0).GetComponent<Button>();
        hit.onClick.AddListener(delegate { Hit(); });
        skill0 = playBossPanelDown.transform.GetChild(1).GetComponent<Button>();
        skill0.onClick.AddListener(delegate { Skill0(); });
        skill1 = playBossPanelDown.transform.GetChild(2).GetComponent<Button>();
        skill1.onClick.AddListener(delegate { Skill1(); });
        skill2 = playBossPanelDown.transform.GetChild(3).GetComponent<Button>();
        skill2.onClick.AddListener(delegate { Skill2(); });
        skillSuper = playBossPanelDown.transform.GetChild(4).GetComponent<Button>();
        skillSuper.onClick.AddListener(delegate { SkillSuper(); });
        attack = playBossPanelDown.transform.GetChild(5).GetComponent<Button>();
        attack.onClick.AddListener(delegate { Attack(); });
    }
    void Hit()
    {
        Debug.Log("Hit");
    }
    void Skill0()
    {
        Debug.Log("Skill0");
    }
    void Skill1()
    {
        Debug.Log("Skill1");
    }
    void Skill2()
    {
        Debug.Log("Skill2");
    }
    void SkillSuper()
    {
        Debug.Log("SkillSuper");
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

        Debug.Log(unitsCurrent.Count + " uC " + enemyesCurrent.Count + " eC");
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
            selectedEnemy.Select();
            selectedUnit.Select();
            attack.interactable = true;
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
        float damage = Combat.Attack(selectedUnit, selectedEnemy, SkillType.none, SkillTypeBonus.none, enemyes, units);
        selectedUnit.animator.Play("attack");
        selectedEnemy.general.PARAMS[GENERAL_HP_CURRENT] -= damage;
        GameObject gt = Instantiate(ResManager.instance.DamageText, selectedEnemy.transform);
        gt.transform.position = new Vector3(selectedEnemy.transform.parent.transform.position.x, selectedEnemy.transform.parent.transform.position.y, 5f);
        DamageText dt = gt.GetComponent<DamageText>();
        dt.startMitonEnemy("-" + damage, Colors.RedColor);
        if (selectedEnemy.general.PARAMS[GENERAL_HP_CURRENT] > 0)
        {
            selectedEnemy.animator.Play("hart");
            if (selectedEnemy.general.PARAMS[GENERAL_HP_CURRENT] > selectedEnemy.general.PARAMS[GENERAL_HP] / 4 * 3) //75+%
            {
                selectedEnemy.text.color = Colors.GreenColor;
            }
            else if (selectedEnemy.general.PARAMS[GENERAL_HP_CURRENT] < selectedEnemy.general.PARAMS[GENERAL_HP] / 4 * 3 && selectedEnemy.general.PARAMS[GENERAL_HP_CURRENT] > selectedEnemy.general.PARAMS[GENERAL_HP] / 2) //50% + /75%-
            {
                selectedEnemy.text.color = Colors.YellowColor;
            }
            else if (selectedEnemy.general.PARAMS[GENERAL_HP_CURRENT] < selectedEnemy.general.PARAMS[GENERAL_HP] / 2 && selectedEnemy.general.PARAMS[GENERAL_HP_CURRENT] > selectedEnemy.general.PARAMS[GENERAL_HP] / 4) //25%+ / 50%-
            {
                selectedEnemy.text.color = Colors.Orange;
            }
            else if (selectedEnemy.general.PARAMS[GENERAL_HP_CURRENT] < selectedEnemy.general.PARAMS[GENERAL_HP] / 4 && selectedEnemy.general.PARAMS[GENERAL_HP_CURRENT] > 0) //0+ / 25-
            {
                selectedEnemy.text.color = Colors.RedColor;
            }
            selectedEnemy.text.text = selectedEnemy.general.PARAMS[GENERAL_HP_CURRENT].ToString();
            selectedUnit.isEndTurn = true;
            return false;
        }
        else
        {
            selectedUnit.isEndTurn = true;
            return true;
        }
    }
    void EnemyAttack()
    {
        enemyesCurrent[0].animator.Play("attack");
        rHit = Random.Range(0, units.Count - 1);
        float damage = Combat.Attack(enemyesCurrent[0], units[rHit], SkillType.none, SkillTypeBonus.none, enemyes, units);
        units[rHit].general.PARAMS[GENERAL_HP_CURRENT] -= damage;
        GameObject gt = Instantiate(ResManager.instance.DamageText, units[rHit].transform);
        gt.transform.position = units[rHit].transform.parent.transform.localPosition;
        DamageText dt = gt.GetComponent<DamageText>();
        dt.startMitonUnit("-" + damage, Colors.RedColor);
        if (units[rHit].general.PARAMS[GENERAL_HP_CURRENT] > 0)
        {
            units[rHit].animator.Play("hart");
            if (units[rHit].general.PARAMS[GENERAL_HP_CURRENT] > units[rHit].general.PARAMS[GENERAL_HP] / 4 * 3) //75+%
            {
                units[rHit].text.color = Colors.GreenColor;
            }
            else if (units[rHit].general.PARAMS[GENERAL_HP_CURRENT] < units[rHit].general.PARAMS[GENERAL_HP] / 4 * 3 && units[rHit].general.PARAMS[GENERAL_HP_CURRENT] > units[rHit].general.PARAMS[GENERAL_HP] / 2) //50% + /75%-
            {
                units[rHit].text.color = Colors.YellowColor;
            }
            else if (units[rHit].general.PARAMS[GENERAL_HP_CURRENT] < units[rHit].general.PARAMS[GENERAL_HP] / 2 && units[rHit].general.PARAMS[GENERAL_HP_CURRENT] > units[rHit].general.PARAMS[GENERAL_HP] / 4) //25%+ / 50%-
            {
                units[rHit].text.color = Colors.Orange;
            }
            else if (units[rHit].general.PARAMS[GENERAL_HP_CURRENT] < units[rHit].general.PARAMS[GENERAL_HP] / 4 && units[rHit].general.PARAMS[GENERAL_HP_CURRENT] > 0) //0+ / 25-
            {
                units[rHit].text.color = Colors.RedColor;
            }
            units[rHit].text.text = units[rHit].general.PARAMS[GENERAL_HP_CURRENT].ToString();
            Invoke("EnemyAttackDelay", 1f);
        }
        else
        {
            Invoke("EnemyAttackDelay", 3f);
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
            Debug.Log("tResCount: " + tRes.Count);
            List<TrophyRes> tResSort = new List<TrophyRes>();
            for (int i = 0; i < tRes.Count; i++)
            {
                if(tRes[i].count > 0)
                {
                    tResSort.Add(tRes[i]);
                }
            }
            Debug.Log("tResSortCount: " + tResSort.Count);
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
