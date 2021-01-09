using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarTown : MonoBehaviour
{
    [Header("Юниты")]
    [SerializeField]
    private GameObject юниты;
    [Header("Экипировка")]
    [SerializeField]
    private GameObject экипировка;
    [Header("Генералы")]
    [SerializeField]
    private GameObject генералы;
    [Header("Преподношение")]
    [SerializeField]
    private GameObject преподношение;
    [Header("Артефакты")]
    [SerializeField]
    private GameObject артефакты;

    public void ОткрытьЮнитов()
    {
        экипировка.SetActive(false);
        генералы.SetActive(false);
        преподношение.SetActive(false);
        артефакты.SetActive(false);
        юниты.SetActive(true);
    }
    public void ОткрытьЭкипировку()
    {
        юниты.SetActive(false);
        генералы.SetActive(false);
        преподношение.SetActive(false);
        артефакты.SetActive(false);
        экипировка.SetActive(true);
    }
    public void ОткрытьГенералов()
    {
        юниты.SetActive(false);
        экипировка.SetActive(false);        
        преподношение.SetActive(false);
        артефакты.SetActive(false);
        генералы.SetActive(true);
    }
    public void ОткрытьПреподношение()
    {
        юниты.SetActive(false);
        экипировка.SetActive(false);
        генералы.SetActive(false);        
        артефакты.SetActive(false);
        преподношение.SetActive(true);
    }
    public void ОткрытьАртефакты()
    {
        юниты.SetActive(false);
        экипировка.SetActive(false);
        генералы.SetActive(false);
        преподношение.SetActive(false);
        артефакты.SetActive(true);
    }
}
