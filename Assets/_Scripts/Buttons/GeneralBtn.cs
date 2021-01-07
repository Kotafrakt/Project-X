using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GeneralBtn : Button
{
    BossLevel bossLevel;
    public General general;
    public GeneralBtn gBtn;
    public Text text;
    public Image img;
    bool isDown = false;
    bool isShowInfo = false;
    float time = 0;
    public bool isSlot = false;


    protected override void Start()
    {
        bossLevel = Camera.main.transform.GetComponent<BossLevel>();
        text = transform.GetChild(0).GetComponent<Text>();
        img = transform.GetChild(1).GetComponent<Image>();
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
                if (isSlot)
                {
                    bossLevel.AddGeneral(this);
                }
                else
                {
                    bossLevel.SelectGeneral(this);
                }
            }
            isDown = false;
        }
    }
}
