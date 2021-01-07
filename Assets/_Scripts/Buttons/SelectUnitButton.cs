using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Defines;


public class SelectUnitButton : Button
{
    public Unit unit;

    float time = 0;
    bool isDown = false;
    bool isShowInfo = false;
    Text text;

    protected override void Start()
    {
        text = transform.GetChild(0).GetComponent<Text>();
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
        if (unit != null)
        {
            text.text = unit.PARAMS[UNIT_COUNT].ToString();
            if (unit.PARAMS[UNIT_COUNT] == 0)
                this.interactable = false;
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
                LevelManager.instance.runingUnit[unit.type] = !LevelManager.instance.runingUnit[unit.type];
                if (LevelManager.instance.runingUnit[unit.type])
                    LevelManager.instance.SpawnUnit(unit, 2f);
            }
            isDown = false;
        }
    }
}
