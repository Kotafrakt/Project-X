using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsBoardTown : MonoBehaviour
{
    [Header("Ежедневные")]
    [SerializeField]
    private GameObject ежедневные;
    [Header("Еженедельные")]
    [SerializeField]
    private GameObject еженедельные;
    [Header("Компания")]
    [SerializeField]
    private GameObject компания;

    public void ОткрытьЕжедневные()
    {
        еженедельные.SetActive(false);
        компания.SetActive(false);
        ежедневные.SetActive(true);
    }
    public void ОткрытьЕженедельные()
    {
        ежедневные.SetActive(false);
        компания.SetActive(false);
        еженедельные.SetActive(true);
    }
    public void ОткрытьКомпания()
    {
        ежедневные.SetActive(false);
        еженедельные.SetActive(false);
        компания.SetActive(true);
    }
}
