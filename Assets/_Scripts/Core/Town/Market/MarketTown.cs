using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketTown : MonoBehaviour
{
    [Header("Купить")]
    [SerializeField]
    private GameObject купить;
    [Header("Продать")]
    [SerializeField]
    private GameObject продать;
    [Header("ВременныйТорговец")]
    [SerializeField]
    private GameObject временныйТорговец;

    public void ОткрытьКупить()
    {
        продать.SetActive(false);
        временныйТорговец.SetActive(false);
        купить.SetActive(true);
    }
    public void ОткрытьПродать()
    {
        купить.SetActive(false);        
        временныйТорговец.SetActive(false);
        продать.SetActive(true);
    }
    public void ОткрытьВрТорговец()
    {
        купить.SetActive(false);
        продать.SetActive(false);
        временныйТорговец.SetActive(true);
    }
}
