using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroTown : MonoBehaviour
{
    [Header("Герой")]
    [SerializeField]
    private GameObject герой;
    [SerializeField]
    private GameObject арена;
    [SerializeField]
    private GameObject компания;
    [HideInInspector]
    public bool isArena = false;
    public GameObject эквип;
    public GameObject эквипЗакрыть;
    public GameObject инфоПредмет;

    [Header("Склад")]
    [SerializeField]
    private GameObject склад;

    [Header("Достижения")]
    [SerializeField]
    private GameObject достижения;
    




    public void ОткрытьГероя()
    {
        склад.SetActive(false);
        достижения.SetActive(false);
        инфоПредмет.SetActive(false);
        герой.SetActive(true);
    }
    public void ОткрытьСклад()
    {
        герой.SetActive(false);
        достижения.SetActive(false);
        склад.SetActive(true);
    }
    public void ОткрытьДостижения()
    {
        герой.SetActive(false);
        склад.SetActive(false);
        достижения.SetActive(true);
    }

    public void АренаКомпания()
    {
        if (арена.activeSelf)
        {
            арена.SetActive(false);
            isArena = false;
            //ЗакратьЭквип();
            компания.SetActive(true);
        }
        else
        {
            компания.SetActive(false);
            //ЗакратьЭквип();
            isArena = true;
            арена.SetActive(true);
        }
    }
    public void ЗакрытьЭквип()
    {
        эквип.SetActive(false);
        эквипЗакрыть.SetActive(false);
    }
    public void ОткрытьЭквип()
    {
        эквип.SetActive(true);
    }
    public void ЗакрытьИнфоПредмет()
    {
        инфоПредмет.SetActive(false);
    }
}
