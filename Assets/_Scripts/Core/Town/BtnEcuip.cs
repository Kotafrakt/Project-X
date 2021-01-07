using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnEcuip : Button
{
    float time = 0;
    bool isDown = false;
    bool isShowInfo = false;
    Inventory inventory;

    [HideInInspector]
    public Item item;

    protected override void Start()
    {
        inventory = Camera.main.transform.GetComponent<Inventory>();
    }

    private void FixedUpdate()
    {
        if (isDown)
            time += 1 * Time.deltaTime;
        if (time > 0.5f && !isShowInfo)
        {
            isShowInfo = true;
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
                inventory.ОткрытьИнфоПредмет(item);
                Debug.Log("Удержание: \n" + ItemText.GetName(item.nameId) + "\n" + ItemText.GetDescription(item.nameId));
            }
            else
            {
                time = 0;
                isShowInfo = false;
                inventory.PasteItem(transform);
            }
            isDown = false;
        }
    }
}
