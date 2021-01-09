using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Герой")]
    [SerializeField]
    private GameObject герой;
    [SerializeField]
    private GameObject склад;
    [SerializeField]
    private GameObject достижения;
    [SerializeField]
    private GameObject арена;
    [SerializeField]
    private GameObject компания;
    [SerializeField]
    private GameObject эквип;
    [SerializeField]
    private GameObject закрытьЭквип;
    [SerializeField]
    private GameObject инфоПредмет;
    [SerializeField]
    private GameObject параметры;
    [SerializeField]
    private Image imgItem;
    [SerializeField]
    private Text название;
    [SerializeField]
    private Text описание;

    private GameObject SelectedSlot;


    private bool isArena = false;



    private void Start()
    {
        //   Заполнения();
        арена.SetActive(false);
        склад.SetActive(false);
        достижения.SetActive(false);
        герой.SetActive(true);
        компания.SetActive(true);
    }

    public void ОткрытьГерой()
    {
        склад.SetActive(false);
        достижения.SetActive(false);
        герой.SetActive(true);

    }
    public void ОткрытьСклад()
    {
        герой.SetActive(false);
        достижения.SetActive(false);
        склад.SetActive(true);

    }
    public void ОткрытьДостижения()
    {
        герой.SetActive(false);
        склад.SetActive(false);
        достижения.SetActive(true);
    }

    public void АренаКомпания()
    {
        if (арена.activeSelf)
        {
            арена.SetActive(false);
            isArena = false;
            ЗакратьЭквип();
            компания.SetActive(true);
        }
        else
        {
            компания.SetActive(false);
            ЗакратьЭквип();
            isArena = true;
            арена.SetActive(true);
        }
    }

    public void ЗакратьЭквип()
    {
        эквип.SetActive(false);
        закрытьЭквип.SetActive(false);
    }

    public void ВыбратьСлот(BtnInvent btn)
    {
        switch (btn.type)
        {
            case ItemSlotsType.head:
                эквип.SetActive(true);
                ЗаполнениеЭквип(btn.type);
                SelectedSlot = btn.gameObject;
                break;
            case ItemSlotsType.tors:
                эквип.SetActive(true);
                ЗаполнениеЭквип(btn.type);
                SelectedSlot = btn.gameObject;
                break;
            case ItemSlotsType.pants:
                эквип.SetActive(true);
                ЗаполнениеЭквип(btn.type);
                SelectedSlot = btn.gameObject;
                break;
            case ItemSlotsType.bots:
                эквип.SetActive(true);
                ЗаполнениеЭквип(btn.type);
                SelectedSlot = btn.gameObject;
                break;
            case ItemSlotsType.weapon0:
                эквип.SetActive(true);
                ЗаполнениеЭквип(btn.type);
                SelectedSlot = btn.gameObject;
                break;
            case ItemSlotsType.weapon1:
                эквип.SetActive(true);
                ЗаполнениеЭквип(btn.type);
                SelectedSlot = btn.gameObject;
                break;
            case ItemSlotsType.amulet:
                эквип.SetActive(true);
                ЗаполнениеЭквип(btn.type);
                SelectedSlot = btn.gameObject;
                break;
            case ItemSlotsType.ring:
                эквип.SetActive(true);
                ЗаполнениеЭквип(btn.type);
                SelectedSlot = btn.gameObject;
                break;
            case ItemSlotsType.art:
                эквип.SetActive(true);
                ЗаполнениеЭквип(btn.type);
                SelectedSlot = btn.gameObject;
                break;
        }
    }
    void ЗаполнениеЭквип(ItemSlotsType type)
    {
        List<Item> items = new List<Item>();
        закрытьЭквип.SetActive(true);
        for (int i = 0; i < эквип.transform.childCount; i++) //Тут было какое-то говно с ячейкой одной!
        {
            Transform trans = эквип.transform.GetChild(i);
            Button btn = trans.GetComponent<Button>();
            GameObject image = trans.GetChild(1).gameObject;
            Image img = image.GetComponent<Image>();
            image.SetActive(false);
            btn.interactable = false;
            img.sprite = null;
        }
        if (isArena)
        {
            if (GameManager.instance.itemsArena.Count > 0)
            {
                for (int i = 0; i < GameManager.instance.itemsArena.Count; i++)
                {
                    if (GameManager.instance.itemsArena[i].slot == type)
                    {
                        items.Add(GameManager.instance.itemsArena[i]);
                    }
                }
                if (items.Count > 0)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        Transform trans = эквип.transform.GetChild(i);
                        BtnEcuip btnEcuip = trans.GetComponent<BtnEcuip>();
                        btnEcuip.item = items[i];
                        Button btn = trans.GetComponent<Button>();
                        GameObject image = trans.GetChild(1).gameObject;
                        Image img = image.GetComponent<Image>();
                        image.SetActive(true);
                        btn.interactable = true;
                        img.sprite = items[i].img;
                        if (items[i].isUse)
                        {
                            btn.interactable = false;
                        }
                    }
                }
            }
        }
        else
        {
            if (GameManager.instance.itemsCompany.Count > 0)
            {
                for (int i = 0; i < GameManager.instance.itemsCompany.Count; i++)
                {
                    if (GameManager.instance.itemsCompany[i].slot == type)
                    {
                        items.Add(GameManager.instance.itemsCompany[i]);
                    }
                }

                for (int i = 0; i < items.Count; i++)
                {
                    Transform trans = эквип.transform.GetChild(i);
                    BtnEcuip btnEcuip = trans.GetComponent<BtnEcuip>();
                    btnEcuip.item = items[i];
                    Button btn = trans.GetComponent<Button>();
                    GameObject image = trans.GetChild(1).gameObject;
                    Image img = image.GetComponent<Image>();
                    image.SetActive(true);
                    btn.interactable = true;
                    img.sprite = items[i].img;
                    if (items[i].isUse)
                    {
                        btn.interactable = false;
                    }
                }
            }
        }
    }

    public void PasteItem(Transform cell)
    {
        BtnEcuip btnEcuip = cell.GetComponent<BtnEcuip>();
        Item item = btnEcuip.item;
        BtnInvent btnInvent = SelectedSlot.GetComponent<BtnInvent>();
        Image slotImage = SelectedSlot.GetComponent<Image>();
        ItemSlotsType type = btnInvent.type;
        int slotNumber = btnInvent.slotNumber;
        slotImage.sprite = item.img;
        if (btnInvent.item != null)
        {
            btnInvent.item.isUse = false;
        }
        btnInvent.item = item;
        item.isUse = true;
        ЗаполнениеЭквип(type);
        Debug.Log(ItemText.GetName(item.nameId) + "\n" + ItemText.GetDescription(item.nameId));
        switch (type)
        {
            case ItemSlotsType.head:
                if (isArena)
                {
                    Hero.arenaHead = item;
                }
                else
                {
                    Hero.head = item;
                }
                break;
            case ItemSlotsType.tors:
                if (isArena)
                {
                    Hero.arenaTors = item;
                }
                else
                {
                    Hero.tors = item;
                }
                break;
            case ItemSlotsType.pants:
                if (isArena)
                {
                    Hero.arenaPants = item;
                }
                else
                {
                    Hero.pants = item;
                }
                break;
            case ItemSlotsType.bots:
                if (isArena)
                {
                    Hero.arenaBots = item;
                }
                else
                {
                    Hero.bots = item;
                }
                break;
            case ItemSlotsType.weapon0:
                if (isArena)
                {
                    Hero.arenaWeapon0 = item;
                }
                else
                {
                    Hero.weapon0 = item;
                }
                break;
            case ItemSlotsType.weapon1:
                if (isArena)
                {
                    Hero.arenaWeapon1 = item;
                }
                else
                {
                    Hero.weapon1 = item;
                }
                break;
            case ItemSlotsType.amulet:
                if (isArena)
                {
                    Hero.arenaAmulet = item;
                }
                else
                {
                    Hero.amulet = item;
                }
                break;
            case ItemSlotsType.ring:
                if (isArena)
                {
                    if (slotNumber == 0)
                    {
                        Hero.arenaRing0 = item;
                    }
                    else
                    {
                        Hero.arenaRing1 = item;
                    }
                }
                else
                {
                    if (slotNumber == 0)
                    {
                        Hero.ring0 = item;
                    }
                    else
                    {
                        Hero.ring1 = item;
                    }
                }
                break;
            case ItemSlotsType.art:
                if (isArena)
                {
                    Hero.arenaArt = item;
                }
                else
                {
                    switch (slotNumber)
                    {
                        case 0:
                            Hero.art0 = item;
                            break;
                        case 1:
                            Hero.art1 = item;
                            break;
                        case 2:
                            Hero.art2 = item;
                            break;
                    }
                }
                break;
        }
    }

    public void ОткрытьИнфоПредмет(Item item)
    {
        int counter = 0;
        int макс;
        инфоПредмет.SetActive(true);
        название.text = ItemText.GetName(item.nameId);
        imgItem.sprite = item.img;
        описание.text = ItemText.GetDescription(item.nameId);
        макс = параметры.transform.childCount;

        for (int i = 0; i < макс; i++)
        {
            параметры.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < item.PARAMS.Length; i++)
        {
            if (item.PARAMS[i] != 0)
            {
                GameObject go = параметры.transform.GetChild(counter).gameObject;
                go.SetActive(true);
                Text name = go.transform.GetChild(0).GetComponent<Text>();
                Text value = go.transform.GetChild(1).GetComponent<Text>();
                name.text = ItemText.GetParamName(i);
                value.text = item.PARAMS[i].ToString();
                counter++;
                //if (counter > макс - 1) counter = макс - 1;
            }
        }
    }

}
