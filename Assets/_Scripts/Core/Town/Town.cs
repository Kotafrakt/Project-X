using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Defines;


public class Town : MonoBehaviour
{
    [Header("Казармы")]
    [SerializeField]
    private GameObject казармы;
    [SerializeField]
    private GameObject создание;
    [SerializeField]
    private GameObject наличие;
    [SerializeField]
    private GameObject геры;
    [SerializeField]
    private GameObject слотыНаличие;
    [SerializeField]
    private GameObject слотыСоздания;
    [SerializeField]
    private Image картинкаЮнита;

    [Header("Арена")]
    [SerializeField]
    private GameObject арена;

    [Header("Герой")]
    [SerializeField]
    private GameObject герой;
    [SerializeField]
    private GameObject генералыНаличие;
    private List<TownGenerals> слотыГенералов = new List<TownGenerals>();


    private List<GameObject> слотыСозданияЮнитов = new List<GameObject>();
    private List<GameObject> слотыНаличиеЮнитов = new List<GameObject>();



    public void ОткрытьКазармы()
    {
        ЗаполненияСоздание();
        казармы.SetActive(true);
        создание.SetActive(true);
        наличие.SetActive(false);
        геры.SetActive(false);
    }
    public void ОткрытьСоздание()
    {
        ЗаполненияСоздание();
        наличие.SetActive(false);
        геры.SetActive(false);
        создание.SetActive(true);
    }
    public void ОткрытьНаличие()
    {
        ЗаполненияНаличие();
        создание.SetActive(false);
        геры.SetActive(false);
        наличие.SetActive(true);
    }
    public void ОткрытьГеры()
    {
        ЗаполнениеГенералы();
        создание.SetActive(false);
        наличие.SetActive(false);
        геры.SetActive(true);
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
    public void ЗаполненияНаличие()
    {
        слотыНаличиеЮнитов.Clear();
        for (int i = 0; i < слотыНаличие.transform.childCount; i++)
        {
            слотыНаличиеЮнитов.Add(слотыНаличие.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < GameManager.instance.units.Count; i++)
        {
            слотыНаличиеЮнитов[i].GetComponent<Image>().sprite = GameManager.instance.units[i].img;
            слотыНаличиеЮнитов[i].GetComponent<UnitBtn>().unit = GameManager.instance.units[i];
            Button Кнопка = слотыНаличиеЮнитов[i].GetComponent<Button>();
            Кнопка.interactable = true;
            слотыНаличиеЮнитов[i].transform.GetChild(0).GetComponent<Text>().text = GameManager.instance.units[i].PARAMS[UNIT_COUNT].ToString();
        }
    }

    public void ЗаполненияСоздание()
    {
        слотыСозданияЮнитов.Clear();
        for (int i = 0; i < слотыСоздания.transform.childCount; i++)
        {
            слотыСозданияЮнитов.Add(слотыСоздания.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < GameManager.instance.units.Count; i++)
        {
            слотыСозданияЮнитов[i].GetComponent<Image>().sprite = GameManager.instance.units[i].img;
            слотыСозданияЮнитов[i].GetComponent<UnitBtn>().unit = GameManager.instance.units[i];
            Button Кнопка = слотыСозданияЮнитов[i].GetComponent<Button>();
            Кнопка.interactable = true;
        }
    }
    public void СоздатьЮнита()
    {
        Debug.Log("СоздатьЮнита");
    }
    public void ОткрытьЮнита(GameObject button)
    {
        картинкаЮнита.sprite = button.GetComponent<UnitBtn>().unit.img2;
    }



    //АРЕНА\\//АРЕНА\\//АРЕНА\\//АРЕНА\\//АРЕНА\\//АРЕНА\\//АРЕНА\\//АРЕНА\\//АРЕНА\\//АРЕНА\\//АРЕНА\\//АРЕНА\\//АРЕНА\\//АРЕНА\\//АРЕНА\\//АРЕНА\\//АРЕНА\\//АРЕНА\\//АРЕНА\\

    public void ОткрытьАрену()
    {
        арена.SetActive(true);

    }

    public void ОткрытьГерой()
    {
        герой.SetActive(true);
    }
    public void Закрытие()
    {
        казармы.SetActive(false);
        герой.SetActive(false);
    }
    public void ToArena()
    {
        SceneManager.LoadScene("Arena");
    }

    public void ToGlobal()
    {
        SceneManager.LoadScene("GlobalMap");
    }

    public void ExitMenu()
    {
        if (казармы.activeSelf || герой.activeSelf || арена.activeSelf)
        {
            казармы.SetActive(false);
            герой.SetActive(false);
            арена.SetActive(false);
        }
        else
            SceneManager.LoadScene("Main");
    }
}
