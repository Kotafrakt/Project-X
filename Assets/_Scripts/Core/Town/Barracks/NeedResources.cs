using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Defines;

public class NeedResources : Button
{
    BarracksTown barracksTown;
    int resource;
    int count;
    public Text text;
    public Image img;



    protected override void Start()
    {
        barracksTown = Camera.main.transform.GetComponent<BarracksTown>();
        text = transform.GetChild(1).GetComponent<Text>();
        img = transform.GetChild(0).GetComponent<Image>();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        barracksTown.ResInfo(resource, count);
    }
}
