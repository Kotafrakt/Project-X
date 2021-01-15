﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Defines;

public class RNCU
{
    public int num;
    public int count;
}

public class BarracksTown : MonoBehaviour
{
    [Header("Создание")]
    [SerializeField]
    private GameObject создание;
    [SerializeField]
    private GameObject слотыСоздания;
    [SerializeField]
    private GameObject создать;
    [SerializeField]
    private Image картинкаЮнита;
    [SerializeField]
    Button создатьЮнита;
    [SerializeField]
    private ToggleGroup тоглы;
    [SerializeField]
    Text имя;
    [SerializeField]
    Text имяСоздающегося;
    [SerializeField]
    Text описание;
    [SerializeField]
    private Image картинкаСоздаваемого;
    private Unit создающийся;
    [SerializeField]
    private GameObject ресурсы;
    private bool canBuy = false;
    private int maxCountBuy = 1;
    private int currentCountUnit = 1;
    private int oldVale = 1;
    [SerializeField]
    private Slider countUnit;
    [SerializeField]
    Text countUnitT;
    [SerializeField]
    private GameObject оповещение;
    [SerializeField]
    Text оповещениеТекст;

    [Header("Наличие")]
    [SerializeField]
    private GameObject наличие;
    [SerializeField]
    private GameObject слотыНаличие;
    [SerializeField]
    private Image картинкаЮнитаНаличие;
    [SerializeField]
    Button upgradeBtn;
    [SerializeField]
    private GameObject апгрейд;

    [Header("Улучшение")]
    UnitType apgradeType;

    [Header("Генералы")]
    [SerializeField]
    private GameObject генералы;
    [SerializeField]
    private GameObject генералыНаличие;




    private List<TownUnits> слотыСозданияЮнитов = new List<TownUnits>();
    private List<TownUnits> слотыНаличиеЮнитов = new List<TownUnits>();
    private List<TownGenerals> слотыГенералов = new List<TownGenerals>();
    private List<NeedResources> кнопкиРесурсов = new List<NeedResources>();
    private List<RNCU> rNCU = new List<RNCU>();

    public void ОткрытьСоздание()
    {
        ЗаполненияСоздание();
        наличие.SetActive(false);
        генералы.SetActive(false);
        создать.SetActive(false);
        if (картинкаЮнита == null) создатьЮнита.interactable = false;
        создание.SetActive(true);
    }
    public void ОткрытьНаличие()
    {
        ЗаполненияНаличие();
        создание.SetActive(false);
        генералы.SetActive(false);
        создать.SetActive(false);
        наличие.SetActive(true);
    }
    public void ОткрытьГенералы()
    {
        ЗаполнениеГенералы();
        создание.SetActive(false);
        наличие.SetActive(false);
        создать.SetActive(false);
        генералы.SetActive(true);
    }

    public void ЗаполненияНаличие()
    {
        for (int i = 0; i < слотыНаличие.transform.childCount; i++)
        {
            TownUnits tU = слотыНаличие.transform.GetChild(i).GetComponent<TownUnits>();
            tU.unit = null;
            tU.img.sprite = null;
            tU.img.gameObject.SetActive(false);
            tU.text.text = "";
            tU.text.gameObject.SetActive(false);
            tU.interactable = false;
            слотыНаличиеЮнитов.Add(tU);
        }
        for (int i = 0; i < GameManager.instance.units.Count; i++) //Заполнение
        {
            if (GameManager.instance.units[i].PARAMS[UNIT_COUNT] > 0)
            {
                слотыНаличиеЮнитов[i].unit = GameManager.instance.units[i];
                слотыНаличиеЮнитов[i].img.sprite = слотыНаличиеЮнитов[i].unit.img;
                слотыНаличиеЮнитов[i].img.gameObject.SetActive(true);
                слотыНаличиеЮнитов[i].text.text = слотыНаличиеЮнитов[i].unit.PARAMS[GENERAL_LEVEL].ToString();
                слотыНаличиеЮнитов[i].text.gameObject.SetActive(true);
                слотыНаличиеЮнитов[i].interactable = true;
            }
        }
    }

    public void ИнфоЮнитаНалиие(TownUnits tUnit)
    {
        картинкаЮнитаНаличие.sprite = tUnit.unit.img2;
        UnitType type = CheckUpgrade.CheckUnit(tUnit.unit.type);
        if (type == UnitType.None)
        {
            upgradeBtn.interactable = false;
        }
        else
        {
            upgradeBtn.interactable = true;
            apgradeType = type;
        }

    }

    public void ЗаполненияСоздание()
    {
        for (int i = 0; i < слотыСоздания.transform.childCount; i++)
        {
            TownUnits tU = слотыСоздания.transform.GetChild(i).GetComponent<TownUnits>();
            tU.unit = null;
            tU.img.sprite = null;
            tU.img.gameObject.SetActive(false);
            tU.text.text = "";
            tU.text.gameObject.SetActive(false);
            tU.interactable = false;
            слотыСозданияЮнитов.Add(tU);
        }
        List<Unit> tempUnit = new List<Unit>();
        for (int i = 0; i < GameManager.instance.units.Count; i++) //Заполнение
        {
            if (AvailableList.availableUnit[GameManager.instance.units[i].type])
            {
                tempUnit.Add(GameManager.instance.units[i]);
            }
        }
        for (int i = 0; i < tempUnit.Count; i++) //Заполнение
        {
            слотыСозданияЮнитов[i].unit = tempUnit[i];
            слотыСозданияЮнитов[i].img.sprite = tempUnit[i].img;
            слотыСозданияЮнитов[i].img.gameObject.SetActive(true);
            слотыСозданияЮнитов[i].text.text = GameText.GetUnitName(tempUnit[i].type);
            слотыСозданияЮнитов[i].text.gameObject.SetActive(true);
            слотыСозданияЮнитов[i].interactable = true;
            слотыСозданияЮнитов[i].isCreate = true;
        }
    }
    public void ЗаполнениеГенералы()
    {
        for (int i = 0; i < генералыНаличие.transform.childCount; i++)
        {
            TownGenerals tG = генералыНаличие.transform.GetChild(i).GetComponent<TownGenerals>();
            tG.general = null;
            tG.img.sprite = null;
            tG.img.gameObject.SetActive(false);
            tG.text.text = "";
            tG.text.gameObject.SetActive(false);
            tG.interactable = false;
            слотыГенералов.Add(tG);
        }
        for (int i = 0; i < GameManager.instance.generals.Count; i++) //Заполнение
        {
            if (!GameManager.instance.generals[i].isDead)
            {
                слотыГенералов[i].general = GameManager.instance.generals[i];
                слотыГенералов[i].img.sprite = слотыГенералов[i].general.img;
                слотыГенералов[i].img.gameObject.SetActive(true);
                слотыГенералов[i].text.text = слотыГенералов[i].general.PARAMS[GENERAL_LEVEL].ToString();
                слотыГенералов[i].text.gameObject.SetActive(true);
                слотыГенералов[i].interactable = true;
            }
        }
    }

    public void ВыборЮнитаДляСоздания(TownUnits townUnit)
    {
        создающийся = townUnit.unit;
        картинкаЮнита.sprite = townUnit.unit.img2;
        картинкаЮнита.gameObject.SetActive(true);
        имя.text = GameText.GetUnitName(townUnit.unit.type);
        описание.text = GameText.GetUnitDescription(townUnit.unit.type);
        создатьЮнита.interactable = true;
    }

    public void СоздатьЮнита()
    {
        //countUnit.minValue = 1;
        //countUnitT.text = countUnit.value.ToString();
        картинкаСоздаваемого.sprite = создающийся.img2;
        картинкаСоздаваемого.gameObject.SetActive(true);
        canBuy = false;
        ЗаполнениеРесурсов();
        имяСоздающегося.text = GameText.GetUnitName(создающийся.type);
        создать.SetActive(true);
    }

    public void ЗакрытьОповещение()
    {
        оповещение.SetActive(false);
    }

    public void ЗаполнениеРесурсов()
    {
        rNCU.Clear();
        for (int i = 0; i < ресурсы.transform.childCount; i++)
        {
            GameObject gO = ресурсы.transform.GetChild(i).gameObject;
            gO.SetActive(false);
            кнопкиРесурсов.Add(gO.GetComponent<NeedResources>());
        }
        Resource res = NeedResToUnit.GetRes(создающийся.type);
        List<InfoResources> ir = new List<InfoResources>();
        for (int i = 0; i < res.resource.Length; i++)
        {
            if (res.resource[i] != 0)
            {
                InfoResources ires = GetInfoResources.GetInfo(i);
                ir.Add(ires);
                RNCU temp = new RNCU();
                temp.num = i;
                temp.count = res.resource[i] * currentCountUnit;
                rNCU.Add(temp);
            }                
        }
        CheckAvaibelUnitCreate(ir, res);
        for (int i = 0; i < ir.Count; i++)
        {
            кнопкиРесурсов[i].img.sprite = ir[i].img;
            кнопкиРесурсов[i].text.text = (res.resource[ir[i].num] * currentCountUnit).ToString();
            if(GameManager.resource[ir[i].num] >= res.resource[ir[i].num])
            {
                canBuy = true;
                кнопкиРесурсов[i].text.color = Colors.GreenColor;
            }
            else
            {
                кнопкиРесурсов[i].text.color = Colors.RedColor;
                canBuy = false;
            }
            кнопкиРесурсов[i].resource = ir[i].num;
            кнопкиРесурсов[i].count = res.resource[ir[i].num];
            кнопкиРесурсов[i].gameObject.SetActive(true);
        }
    }

    public void ResInfo(int recource, int count)
    {

    }

    void CheckAvaibelUnitCreate(List<InfoResources> infoRes, Resource res)
    {
        List<int> avaibelC = new List<int>();
        int count = 0;
        for(int i = 0; i < infoRes.Count; i++)
        {
            int c = 0;
            if (GameManager.resource[infoRes[i].num] > 0)
            {
                c = Mathf.FloorToInt(GameManager.resource[infoRes[i].num] / res.resource[infoRes[i].num]);
            }
            avaibelC.Add(c);
        }
        count = SortCount(avaibelC)[0];
        if(count == 0)
        {
            maxCountBuy = 1;
            countUnit.value = 1;
            countUnit.maxValue = 1;
        }
        else
        {
            maxCountBuy = count;
            countUnit.maxValue = maxCountBuy;
            countUnit.value = 1;
        }
    }

    List<int> SortCount(List<int> avaibelC)
    {
        int temp;
        for (int i = 0; i < avaibelC.Count; i++)
        {
            for (int j = i + 1; j > avaibelC.Count; j++)
            {
                if (avaibelC[i] < avaibelC[j])
                {
                    temp = avaibelC[i];
                    avaibelC[i] = avaibelC[j];
                    avaibelC[j] = temp;
                }
            }
        }
        return avaibelC;
    }

    public void CreateUnits()
    {
        if(canBuy)
        {
            for(int i = 0; i < rNCU.Count; i++)
            {
                GameManager.resource[rNCU[i].num] -= rNCU[i].count;
            }
            создающийся.PARAMS[UNIT_COUNT] += currentCountUnit;
            rNCU.Clear();
            currentCountUnit = 1;
            ЗаполнениеРесурсов();
            оповещениеТекст.text = "Создан кто-то";
            оповещение.SetActive(true);
        }
        else
        {
            оповещениеТекст.text = "Иди крабь нуб";
            оповещение.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        CheckSlider();
    }

    void CheckSlider()
    {
        float count = countUnit.value;
        int countInt = Mathf.RoundToInt(count);
        countUnit.value = countInt;
        if (countInt > 0)
        {
            currentCountUnit = countInt;
        }
        else
            currentCountUnit = 1;
        if(oldVale != currentCountUnit)
        {
            oldVale = currentCountUnit;
            ЗаполнениеРесурсов();
        }
        countUnitT.text = countInt.ToString();
    }
    public void ОткрытьАпгрейд()
    {
        апгрейд.SetActive(true);
    }
    public void ЗакрытьАпгрейд()
    {
        апгрейд.SetActive(false);
    }
}
