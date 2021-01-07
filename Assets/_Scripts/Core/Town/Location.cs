using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Location : MonoBehaviour, IPointerDownHandler
{
    public string NameLoc;
    public GameObject[] nearPoint;
    [HideInInspector]
    public SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void SelectPoint(RaycastHit2D hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (hit.transform == transform)
            {
                if (NameLoc == "C0")
                {
                    SceneManager.LoadScene("Town");
                }
                else
                {
                    bool isnearPoint = false;
                    for (int i = 0; i < nearPoint.Length; i++)
                    {
                        if (GameManager.instance.HeroPos == nearPoint[i].name)
                            isnearPoint = true;
                    }
                    if (isnearPoint)
                    {
                        GlobalMap gm = Camera.main.GetComponent<GlobalMap>();
                        gm.ShowInfoPanel(NameLoc);
                    }
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GlobalMap gm = Camera.main.GetComponent<GlobalMap>();
        if (NameLoc == "C0")
        {
            SceneManager.LoadScene("Town");
        }
        /*
        else if (GameManager.instance.locations[NameLoc] == 2)
        {
            gm.ShowInfoPanel(NameLoc);
        }*/
        else
        {
            bool isnearPoint = true;
            for (int i = 0; i < nearPoint.Length; i++)
            {
                if (GameManager.instance.HeroPos == nearPoint[i].name)
                    isnearPoint = true;
            }
            if (isnearPoint)
            {
                gm.ShowInfoPanel(NameLoc);
            }
        }
    }
}
