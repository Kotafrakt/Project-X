using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public  class InfoResources
{
    public Sprite img;
    public string name;
    public int num;
}
public static class GetInfoResources
{
    public static InfoResources GetInfo(int resNum)
    {
        InfoResources res = new InfoResources();
        switch (resNum)
        {
            case GOLD:
                res.img = ResManager.instance.imgRes[0];
                if (GameManager.instance.isRussian)
                {
                    res.name = "Золото";
                }
                else
                {
                    res.name = "Gold";
                }
                break;
            case BONES:
                res.img = ResManager.instance.imgRes[1];
                if (GameManager.instance.isRussian)
                {
                    res.name = "Кости";
                }
                else
                {
                    res.name = "Bones";
                }
                break;
            case BODY:
                res.img = ResManager.instance.imgRes[2];
                if (GameManager.instance.isRussian)
                {
                    res.name = "Плоть";
                }
                else
                {
                    res.name = "Body";
                }
                break;
            case WOOD:
                res.img = ResManager.instance.imgRes[3];
                if (GameManager.instance.isRussian)
                {
                    res.name = "Дерево";
                }
                else
                {
                    res.name = "Wood";
                }
                break;
            case IRON:
                res.img = ResManager.instance.imgRes[6];
                if (GameManager.instance.isRussian)
                {
                    res.name = "Железо";
                }
                else
                {
                    res.name = "Iron";
                }
                break;
            case SOULS:
                res.img = ResManager.instance.imgRes[5];
                if (GameManager.instance.isRussian)
                {
                    res.name = "Души";
                }
                else
                {
                    res.name = "Souls";
                }
                break;
            case REAL:
                res.img = ResManager.instance.imgRes[4];
                if (GameManager.instance.isRussian)
                {
                    res.name = "Кристаллы";
                }
                else
                {
                    res.name = "Real";
                }
                break;
            default:
                res.img = ResManager.instance.imgRes[7];
                if (GameManager.instance.isRussian)
                {
                    res.name = "Хрень";
                }
                else
                {
                    res.name = "Gadget";
                }
                break;
        }
        res.num = resNum;
        return res;
    }
}
