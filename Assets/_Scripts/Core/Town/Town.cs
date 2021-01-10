using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Defines;


public class Town : MonoBehaviour
{
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
