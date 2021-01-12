using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TownUnits : Button
{
    BarracksTown barracksTown;
    public Unit unit;
    public Text text;
    public Image img;
    public bool isCreate = false;
    bool isDown = false;
    bool isShowInfo = false;
    float time = 0;
    


    protected override void Start()
    {
        barracksTown = Camera.main.transform.GetComponent<BarracksTown>();
        img = transform.GetChild(0).GetComponent<Image>();
        text = transform.GetChild(1).GetComponent<Text>();        
    }
    private void FixedUpdate()
    {
        if (isDown && Time.timeScale != 0)
            time += 1 * Time.deltaTime / Time.timeScale;
        if (time > 0.5f && !isShowInfo)
        {
            isShowInfo = true;
            Debug.Log("Удержание");
        }
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (this.interactable)
            isDown = true;

    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (this.interactable)
        {
            if (isShowInfo)
            {
                time = 0;
                isShowInfo = false;
            }
            else
            {
                time = 0;
                isShowInfo = false;
                if (isCreate)
                {
                    barracksTown.ВыборЮнитаДляСоздания(this);
                }
                else
                {
                    barracksTown.ИнфоЮнитаНалиие(this);
                }
            }
            isDown = false;
        }
    }
}