using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Defines;


public class Town : MonoBehaviour
{
    [Header("ПриПервомВходе")]
    [SerializeField]
    private GameObject ппвАлтарь;
    [SerializeField]
    private GameObject ппвАрена;
    [SerializeField]
    private GameObject ппвГерой;
    [SerializeField]
    private GameObject ппвКазармы;
    [SerializeField]
    private GameObject ппвЛабаратория;
    [SerializeField]
    private GameObject ппвРынок;
    [SerializeField]
    private GameObject ппвКвесты;
    [SerializeField]
    private GameObject ппвИспытания;
    [SerializeField]
    private GameObject ппвДостижения;
    [SerializeField]
    private GameObject ппвДозор;

    [Header("Шапка")]
    [SerializeField]
    Text золото;
    [SerializeField]
    Text дерево;
    [SerializeField]
    Text камень;
    [SerializeField]
    Text железо;
    [SerializeField]
    Text кости;
    [SerializeField]
    Text плоть;
    [SerializeField]
    Text души;
    [SerializeField]
    Text реал;
    //Разделы
    [Header("Разделы")]
    [SerializeField]
    private GameObject алтарь;
    AltarTown altarTown;

    [SerializeField]
    private GameObject арена;
    ArenaTown arenaTown;

    [SerializeField]
    private GameObject герой;
    HeroTown heroTown;

    [SerializeField]
    private GameObject казармы;
    BarracksTown barracksTown;

    [SerializeField]
    private GameObject лабаратория;
    LabTown labTown;

    [SerializeField]
    private GameObject рынок;
    MarketTown marketTown;
    
    [SerializeField]
    private GameObject квесты;
    QuestsBoardTown questsBoardTown;

    [SerializeField]
    private GameObject испытания;
    TrialsTown trialsTown;

    [SerializeField]
    private GameObject достижения;
    AchievementsTown achievementsTown;

    [SerializeField]
    private GameObject дозор;
    PatrolTown patrolTown;

    private void FixedUpdate()
    {
        золото.text = GameManager.resource[GOLD].ToString();
        дерево.text = GameManager.resource[WOOD].ToString();
        камень.text = GameManager.resource[ROCK].ToString();
        железо.text = GameManager.resource[IRON].ToString();
        кости.text = GameManager.resource[BONES].ToString();
        плоть.text = GameManager.resource[BODY].ToString();
        души.text = GameManager.resource[SOULS].ToString();
        реал.text = GameManager.resource[REAL].ToString();
    }

    private void Start()
    {
        altarTown = Camera.main.transform.GetComponent<AltarTown>();
        arenaTown = Camera.main.transform.GetComponent<ArenaTown>();
        barracksTown = Camera.main.transform.GetComponent<BarracksTown>();
        heroTown = Camera.main.transform.GetComponent<HeroTown>();
        labTown = Camera.main.transform.GetComponent<LabTown>();
        marketTown = Camera.main.transform.GetComponent<MarketTown>();
        questsBoardTown = Camera.main.transform.GetComponent<QuestsBoardTown>();
        trialsTown = Camera.main.transform.GetComponent<TrialsTown>();
    }

    public void ЗакрытьППВ()
    {
        if (алтарь.activeSelf)
        {
            ппвАлтарь.SetActive(false);
        }
        if (арена.activeSelf)
        {
            ппвАрена.SetActive(false);
        }
        if (герой.activeSelf)
        {
            ппвГерой.SetActive(false);
        }
        if (казармы.activeSelf)
        {
            ппвКазармы.SetActive(false);
        }
        if (лабаратория.activeSelf)
        {
            ппвЛабаратория.SetActive(false);
        }
        if (рынок.activeSelf)
        {
            ппвРынок.SetActive(false);
        }
        if (квесты.activeSelf)
        {
            ппвКвесты.SetActive(false);
        }
        if (испытания.activeSelf)
        {
            ппвИспытания.SetActive(false);
        }
        if (достижения.activeSelf)
        {
            ппвДостижения.SetActive(false);
        }
        if (дозор.activeSelf)
        {
            ппвДозор.SetActive(false);
        }
    }



    public void ОткрытьАлтарь()
    {
        алтарь.SetActive(true);
        altarTown.ОткрытьЮнитов();
    }
    public void ОткрытьАрену()
    {
        арена.SetActive(true);
        arenaTown.ОткрытьАрену();
    }
    public void ОткрытьГероя()
    {
        герой.SetActive(true);
        heroTown.ОткрытьГероя();
    }
    public void ОткрытьКазармы()
    {
        казармы.SetActive(true);
        barracksTown.ОткрытьНаличие();
    }
    public void ОткрытьЛабараторию()
    {
        лабаратория.SetActive(true);
        //labTown.ОткрытьНаличие();
    }
    public void ОткрытьРынок()
    {
        рынок.SetActive(true);
        //marketTown.ОткрытьНаличие();
    }
    public void ОткрытьКвесты()
    {
        квесты.SetActive(true);
        questsBoardTown.ОткрытьЕжедневные();
    }
    public void ОткрытьИспытания()
    {
        испытания.SetActive(true);
        trialsTown.ОткрытьИспытания();
    }
    public void ОткрытьДостижения()
    {
        достижения.SetActive(true);
        achievementsTown.ОткрытьДостижения();
    }
    public void ОткрытьДозор()
    {
        дозор.SetActive(true);
        patrolTown.ОткрытьДозор();
    }


    public void Меню()
    {
        if (алтарь.activeSelf || арена.activeSelf || казармы.activeSelf || герой.activeSelf || лабаратория.activeSelf || рынок.activeSelf || квесты.activeSelf || испытания.activeSelf)
        {
            алтарь.SetActive(false);
            арена.SetActive(false);
            казармы.SetActive(false);
            герой.SetActive(false);
            лабаратория.SetActive(false);
            рынок.SetActive(false);
            квесты.SetActive(false);
            испытания.SetActive(false);
        }
        else
        {
            SceneManager.LoadScene("Main");
        }
    }

    public void ToGlobal()
    {
        SceneManager.LoadScene("GlobalMap");
    }
}
