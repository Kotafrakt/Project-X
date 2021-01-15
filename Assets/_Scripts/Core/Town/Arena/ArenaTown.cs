using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaTown : MonoBehaviour
{
    [SerializeField]
    private GameObject выборЛиги;
    [SerializeField]
    private GameObject лигаНовичка;
    public void ОткрытьАрену()
    {
        лигаНовичка.SetActive(false);
        выборЛиги.SetActive(true);
    }
    public void ОткрытьЛигаНовичка()
    {
        лигаНовичка.SetActive(true  );
    }
    public void ToArena()
    {
        SceneManager.LoadScene("Arena");
    }
}
