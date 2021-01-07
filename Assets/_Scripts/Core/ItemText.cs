﻿

public static class ItemText
{
    public static string GetName(int id)
    {
        string name = "";
        switch (id)
        {
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////Head////Head////Head////Head////Head////Head////Head////Head////Head////Head////Head////Head////Head//////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 0:
                if (GameManager.instance.isRussian)
                {
                    name = "Ржавый шлем";
                }
                else
                {
                    name = "Rusty helmet";
                }
                break;
            case 1:
                if (GameManager.instance.isRussian)
                {
                    name = "Новый картонный шлем";
                }
                else
                {
                    name = "New cardboard helmet";
                }
                break;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////tors////tors////tors////tors////tors////tors////tors////tors////tors////tors////tors////tors////tors//////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 1000:
                if (GameManager.instance.isRussian)
                {
                    name = "Ржавый грудак";
                }
                else
                {
                    name = "Ржавый грудак";
                }
                break;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////pants////pants////pants////pants////pants////pants////pants////pants////pants////pants////pants////pants////pants/////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 2000:
                if (GameManager.instance.isRussian)
                {
                    name = "Трусы бомжа";
                }
                else
                {
                    name = "Трусы бомжа";
                }
                break;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////bots////bots////bots////bots////bots////bots////bots////bots////bots////bots////bots////bots////bots//////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 3000:
                if (GameManager.instance.isRussian)
                {
                    name = "Белые тапочки";
                }
                else
                {
                    name = "Белые тапочки";
                }
                break;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////weapon0////weapon0////weapon0////weapon0////weapon0////weapon0////weapon0////weapon0////weapon0////weapon0////weapon0////weapon0////weapon0///////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 4000:
                if (GameManager.instance.isRussian)
                {
                    name = "Охуенная палка-нагибалка";
                }
                else
                {
                    name = "Охуенная палка-нагибалка";
                }
                break;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////weapon1////weapon1////weapon1////weapon1////weapon1////weapon1////weapon1////weapon1////weapon1////weapon1////weapon1////weapon1////weapon1///////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 5000:
                if (GameManager.instance.isRussian)
                {
                    name = "Крышка от кастрюли";
                }
                else
                {
                    name = "Крышка от кастрюли";
                }
                break;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////amulet////amulet////amulet////amulet////amulet////amulet////amulet////amulet////amulet////amulet////amulet////amulet////amulet////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 6000:
                if (GameManager.instance.isRussian)
                {
                    name = "Брелок";
                }
                else
                {
                    name = "Брелок";
                }
                break;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////ring////ring////ring////ring////ring////ring////ring////ring////ring////ring////ring////ring////ring//////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 7000:
                if (GameManager.instance.isRussian)
                {
                    name = "леска на палец";
                }
                else
                {
                    name = "леска на палец";
                }
                break;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /////////////////////////////art////art////art////art////art////art////art////art////art////art////art////art////art//////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 8000:
                if (GameManager.instance.isRussian)
                {
                    name = "Непонятная херня";
                }
                else
                {
                    name = "Непонятная херня";
                }
                break;
        }
        return name;
    }
    public static string GetDescription(int id)
    {
        string desc = "";
        switch (id)
        {
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////Head////Head////Head////Head////Head////Head////Head////Head////Head////Head////Head////Head////Head//////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 0:
                if (GameManager.instance.isRussian)
                {
                    desc = "Ржавый шлем";
                }
                else
                {
                    desc = "Ржавый шлем";
                }
                break;
            case 1:
                if (GameManager.instance.isRussian)
                {
                    desc = "Пусть из кртона, зато новый!";
                }
                else
                {
                    desc = "Let from krton, but new!";
                }
                break;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////tors////tors////tors////tors////tors////tors////tors////tors////tors////tors////tors////tors////tors//////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 1000:
                if (GameManager.instance.isRussian)
                {
                    desc = "Ржавый грудак";
                }
                else
                {
                    desc = "Ржавый грудак";
                }
                break;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////pants////pants////pants////pants////pants////pants////pants////pants////pants////pants////pants////pants////pants/////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 2000:
                if (GameManager.instance.isRussian)
                {
                    desc = "Трусы бомжа";
                }
                else
                {
                    desc = "Трусы бомжа";
                }
                break;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////bots////bots////bots////bots////bots////bots////bots////bots////bots////bots////bots////bots////bots//////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 3000:
                if (GameManager.instance.isRussian)
                {
                    desc = "Белые тапочки";
                }
                else
                {
                    desc = "Белые тапочки";
                }
                break;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////weapon0////weapon0////weapon0////weapon0////weapon0////weapon0////weapon0////weapon0////weapon0////weapon0////weapon0////weapon0////weapon0///////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 4000:
                if (GameManager.instance.isRussian)
                {
                    desc = "Охуенная палка-нагибалка";
                }
                else
                {
                    desc = "Охуенная палка-нагибалка";
                }
                break;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////weapon1////weapon1////weapon1////weapon1////weapon1////weapon1////weapon1////weapon1////weapon1////weapon1////weapon1////weapon1////weapon1///////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 5000:
                if (GameManager.instance.isRussian)
                {
                    desc = "Крышка от кастрюли";
                }
                else
                {
                    desc = "Крышка от кастрюли";
                }
                break;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////amulet////amulet////amulet////amulet////amulet////amulet////amulet////amulet////amulet////amulet////amulet////amulet////amulet////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 6000:
                if (GameManager.instance.isRussian)
                {
                    desc = "Брелок";
                }
                else
                {
                    desc = "Брелок";
                }
                break;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////ring////ring////ring////ring////ring////ring////ring////ring////ring////ring////ring////ring////ring//////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 7000:
                if (GameManager.instance.isRussian)
                {
                    desc = "леска на палец";
                }
                else
                {
                    desc = "леска на палец";
                }
                break;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /////////////////////////////art////art////art////art////art////art////art////art////art////art////art////art////art//////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 8000:
                if (GameManager.instance.isRussian)
                {
                    desc = "Непонятная херня";
                }
                else
                {
                    desc = "Непонятная херня";
                }
                break;
        }
        return desc;
    }

    public static string GetParamName(int id)
    {
        string name = "";
        switch (id)
        {
            case 0:
                if (GameManager.instance.isRussian)
                {
                    name = "Дополнительное здоровье для монстра";
                }
                else
                {
                    name = "hp";
                }
                break;
            case 1:
                if (GameManager.instance.isRussian)
                {
                    name = "Востановление здоровья монстров";
                }
                else
                {
                    name = "HP_REGEN";
                }
                break;
            case 2:
                if (GameManager.instance.isRussian)
                {
                    name = "Бонус урона";
                }
                else
                {
                    name = "DAMAGE";
                }
                break;
            case 3:
                if (GameManager.instance.isRussian)
                {
                    name = "Бонус критического урона";
                }
                else
                {
                    name = "CRITS_DAMAGE";
                }
                break;
            case 4:
                if (GameManager.instance.isRussian)
                {
                    name = "Бонус шанса критической атаки";
                }
                else
                {
                    name = "CRITS_CHANCE";
                }
                break;
            case 5:
                if (GameManager.instance.isRussian)
                {
                    name = "Задержка между атаками";
                }
                else
                {
                    name = "ATTACK_DELAY";
                }
                break;
            case 6:
                if (GameManager.instance.isRussian)
                {
                    name = "Бонус дистанции для атаки";
                }
                else
                {
                    name = "DISTANCE";
                }
                break;
            case 7:
                if (GameManager.instance.isRussian)
                {
                    name = "Бонус брони";
                }
                else
                {
                    name = "ARMOR";
                }
                break;
            case 8:
                if (GameManager.instance.isRussian)
                {
                    name = "Бонус резиста от огня";
                }
                else
                {
                    name = "FIRE_REZIST";
                }
                break;
            case 9:
                if (GameManager.instance.isRussian)
                {
                    name = "Бонус резиста от холода";
                }
                else
                {
                    name = "ICE_RESIST";
                }
                break;
            case 10:
                if (GameManager.instance.isRussian)
                {
                    name = "Бонус резиста от молнии";
                }
                else
                {
                    name = "ELECTRIC_RESIST";
                }
                break;
            case 11:
                if (GameManager.instance.isRussian)
                {
                    name = "Бонус скорости передвижения";
                }
                else
                {
                    name = "SPEED";
                }
                break;
            case 12:
                if (GameManager.instance.isRussian)
                {
                    name = "Бонусный юнит бесплатно";
                }
                else
                {
                    name = "UNIT";
                }
                break;
            case 13:
                if (GameManager.instance.isRussian)
                {
                    name = "Халявное грейд юнита во время призыва";
                }
                else
                {
                    name = "UNIT_UPGRADE";
                }
                break;
            case 14:
                if (GameManager.instance.isRussian)
                {
                    name = "Дополнительное здоровье для монстра    %";
                }
                else
                {
                    name = "HP_P";
                }
                break;
            case 15:
                if (GameManager.instance.isRussian)
                {
                    name = "Востановление здоровья монстров    %";
                }
                else
                {
                    name = "HP_REGEN_P";
                }
                break;
            case 16:
                if (GameManager.instance.isRussian)
                {
                    name = "Бонус урона    %";
                }
                else
                {
                    name = "DAMAGE_P";
                }
                break;
            case 17:
                if (GameManager.instance.isRussian)
                {
                    name = "Бонус критического урона    %";
                }
                else
                {
                    name = "CRITS_DAMAGE_P";
                }
                break;
            case 18:
                if (GameManager.instance.isRussian)
                {
                    name = "Бонус шанса критической атаки    %";
                }
                else
                {
                    name = "CRITS_CHANCE_P";
                }
                break;
            case 19:
                if (GameManager.instance.isRussian)
                {
                    name = "Задержка между атаками    %";
                }
                else
                {
                    name = "ATTACK_DELAY_P";
                }
                break;
            case 20:
                if (GameManager.instance.isRussian)
                {
                    name = "Бонус дистанции для атаки    %";
                }
                else
                {
                    name = "DISTANCE_P";
                }
                break;
            case 21:
                if (GameManager.instance.isRussian)
                {
                    name = "Бонус брони    %";
                }
                else
                {
                    name = "ARMOR_P";
                }
                break;
            case 22:
                if (GameManager.instance.isRussian)
                {
                    name = "Бонус резиста от огня    %";
                }
                else
                {
                    name = "FIRE_REZIST_P";
                }
                break;
            case 23:
                if (GameManager.instance.isRussian)
                {
                    name = "Бонус резиста от холода    %";
                }
                else
                {
                    name = "ICE_RESIST_P";
                }
                break;
            case 24:
                if (GameManager.instance.isRussian)
                {
                    name = "Бонус резиста от молнии    %";
                }
                else
                {
                    name = "ELECTRIC_RESIST_P";
                }
                break;
            case 25:
                if (GameManager.instance.isRussian)
                {
                    name = "Бонус скорости передвижения    %";
                }
                else
                {
                    name = "SPEED_P";
                }
                break;
        }
        return name;
    }
}