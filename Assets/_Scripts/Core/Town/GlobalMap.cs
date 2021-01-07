using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalMap : MonoBehaviour
{
    GameObject[] points;
    public GameObject Hero;
    public GameObject InfoLoc;
    private string nameLoc;


    void Start()
    {
        AddPoints();
        CheckMap();
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].name == GameManager.instance.HeroPos)
                Hero.transform.position = points[i].transform.position;
        }
    }

    public void ShowInfoPanel(string name)
    {
        nameLoc = name;
        InfoLoc.SetActive(true);
    }

    public void EnterLoc()
    {
        InfoLoc.SetActive(false);
        SceneManager.LoadScene(nameLoc);
        //MoveToLoc();
    }

    void MoveToLoc()
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].name == nameLoc)
            {
                Hero.transform.position = points[i].transform.position;
                GameManager.instance.HeroPos = points[i].name;
                GameManager.instance.locations[nameLoc] = 2;
            }
        }

    }

    public void CloseInfoLoc()
    {
        InfoLoc.SetActive(false);
    }

    void AddPoints()
    {
        points = GameObject.FindGameObjectsWithTag("MapPoint");
    }

    void CheckMap()
    {
        for (int i = 0; i < points.Length; i++)
        {

        }
    }
}
