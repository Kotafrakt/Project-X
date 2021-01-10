using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaTown : MonoBehaviour
{
    [SerializeField]
    private GameObject ВыборЛиги;
    [SerializeField]
    private GameObject ЛигаНовичка;
    public void ОткрытьАрену()
    {
        ЛигаНовичка.SetActive(false);
        ВыборЛиги.SetActive(true);
    }
    public void ToArena()
    {
        SceneManager.LoadScene("Arena");
    }
}
