using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Defines;

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
    private Scrollbar countUnit;
    [SerializeField]
    private Image картинкаЮнита;
    [SerializeField]
    private ToggleGroup тоглы;
    [SerializeField]
    Text имя;
    [SerializeField]
    Text описание;

    [Header("Наличие")]
    [SerializeField]
    private GameObject наличие;
    [SerializeField]
    private GameObject слотыНаличие;
       
    [Header("Генералы")]
    [SerializeField]
    private GameObject генералы;
    [SerializeField]
    private GameObject генералыНаличие;


    

    private List<TownUnits> слотыСозданияЮнитов = new List<TownUnits>();
    private List<TownUnits> слотыНаличиеЮнитов = new List<TownUnits>();
    private List<TownGenerals> слотыГенералов = new List<TownGenerals>();

    public void ОткрытьСоздание()
    {
        ЗаполненияСоздание();
        наличие.SetActive(false);
        генералы.SetActive(false);
        создать.SetActive(false);
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
        картинкаЮнита.sprite = townUnit.unit.img2;
        картинкаЮнита.gameObject.SetActive(true);
        имя.text = GameText.GetUnitName(townUnit.unit.type);
        описание.text = GameText.GetUnitDescription(townUnit.unit.type);
    }

    public void СоздатьЮнита()
    {
        countUnit.value = 0;
        создать.SetActive(true);
    }
}
