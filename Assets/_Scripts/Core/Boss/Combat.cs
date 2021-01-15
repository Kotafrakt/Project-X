using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public static class Combat
{
    static TurnBase attacker;
    static TurnBase attacked;
    static SkillType skillType;
    static SkillTypeBonus skillTypeBonus;
    static List<TurnEnemy> enemyes = new List<TurnEnemy>();
    static List<TurnUnit> units = new List<TurnUnit>();
    static List<TurnBase> targets = new List<TurnBase>();
    static bool targetIsPlayer = false; //фолс - атака по врагу.
    static List<int> selectedCells = new List<int>();
    static int a;
    static int b;
    static int c;
    static int d;
    static int e;
    static int f;

    public static float Attack(TurnBase attackerIn, TurnBase attackedIn, SkillType skillTypeIn, SkillTypeBonus skillTypeBonusIn, List<TurnEnemy> enemyesIn, List<TurnUnit> unitsIn)
    {
        targets.Clear();
        attacker = attackerIn;
        attacked = attackedIn;
        skillType = skillTypeIn;
        skillTypeBonus = skillTypeBonusIn;
        enemyes = enemyesIn;
        units = unitsIn;
        CheckList();

        float damage = 0;
        if (skillType == SkillType.none && skillTypeBonus == SkillTypeBonus.none)
        {
            damage = attacker.general.PARAMS[GENERAL_DAMAGE_CURRENT] - attacked.general.PARAMS[GENERAL_DEFENSE_CURRENT];
            Debug.Log(attacker.general.PARAMS[GENERAL_DAMAGE_CURRENT] + " - " + attacked.general.PARAMS[GENERAL_DEFENSE_CURRENT]);
            if (damage <= 0)
            {
                damage = 0;
            }
            else if (Random.Range(0, 100) < attacker.general.PARAMS[GENERAL_CRIT_CHANGE_CURRENT])
            {
                damage += attacker.general.PARAMS[GENERAL_CRIT_DAMAGE_CURRENT];
            }
        }
        else
        {
            if (skillType != SkillType.none)
            {
                switch (skillType)
                {
                    case SkillType.skill0:

                        break;
                    case SkillType.aoeLink:
                        AOELink();
                        break;
                    case SkillType.aoeBoom:
                        AOELink();
                        break;
                }
            }
            else if (skillTypeBonus != SkillTypeBonus.none)
            {
                switch (skillTypeBonus)
                {
                    case SkillTypeBonus.skillB0 :

                        break;
                }
            }
        }
        return damage; 
    }

    static void CheckList()
    {
        for (int i = 0; i < enemyes.Count; i++)
        {
            if (enemyes[i] == attacked)
            {
                targetIsPlayer = false;
            }
            else
            {
                targetIsPlayer = true;
            }
        }
    }
    static void WriteToList()
    {

        if (targetIsPlayer)
        {
            for (int i = 0; i < selectedCells.Count; i++)
            {
                for (int j = 0; j < units.Count; j++)
                {
                    if (units[j].cellNum == selectedCells[i])
                    {
                        targets.Add(units[j]);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < selectedCells.Count; i++)
            {
                for (int j = 0; j < enemyes.Count; j++)
                {
                    if (enemyes[j].cellNum == selectedCells[i])
                    {
                        targets.Add(enemyes[j]);
                    }
                }
            }
        }
    }

    ///////////////////////////////////////////////////////// ///////////////////////////////////////////////////////// ////////////////////////////////////////////////
    //////////////////////////////////////////////////////// ///////////цепной удар, что-то типа удара молнии/// ///////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////// ///////////////////////////////////////////////////////// ////////////////////////////////////////////////

    static void AOELink()
    {
        switch (attacker.general.skillLvl_0)
        {
            case 1:
                AOELinkChoiceCellLvl_0();
                break;
            case 2:
                AOELinkChoiceCellLvl_1();
                break;
            case 3:
                AOELinkChoiceCellLvl_2();
                break;
        }
        switch (attacker.general.skillLvl_1)
        {
            case 1:
                AOELinkChoiceCellLvl_0();
                break;
            case 2:
                AOELinkChoiceCellLvl_1();
                break;
            case 3:
                AOELinkChoiceCellLvl_2();
                break;
        }
        switch (attacker.general.skillLvl_2)
        {
            case 1:
                AOELinkChoiceCellLvl_0();
                break;
            case 2:
                AOELinkChoiceCellLvl_1();
                break;
            case 3:
                AOELinkChoiceCellLvl_2();
                break;
        }
        switch (attacker.general.skillLvlBoss)
        {
            case 1:

                break;
            case 2:

                break;
        }

    }
    static void AOELinkChoiceCellLvl_0()
    {
        switch (attacked.cellNum)
        {
            case 0:
                a = 1;
                b = 3;
                selectedCells.Add(a);
                selectedCells.Add(b);
                WriteToList();
                break;
            case 1:
                a = 0;
                b = 2;
                c = 4;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                WriteToList();
                break;
            case 2:
                a = 1;
                b = 5;
                selectedCells.Add(a);
                selectedCells.Add(b);
                WriteToList();
                break;
            case 3:
                a = 0;
                b = 4;
                selectedCells.Add(a);
                selectedCells.Add(b);
                WriteToList();
                break;
            case 4:
                a = 1;
                b = 3;
                c = 5;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                WriteToList();
                break;
            case 5:
                a = 2;
                b = 4;
                selectedCells.Add(a);
                selectedCells.Add(b);
                WriteToList();
                break;
        }
    }

    static void AOELinkChoiceCellLvl_1()
    {
         switch (attacked.cellNum)
        {
            case 0:
                a = 1;
                b = 2;
                c = 3;
                d = 4;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                WriteToList();
                break;
            case 1:
                a = 0;
                b = 2;
                c = 3;
                d = 4;
                e = 5;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                selectedCells.Add(e);
                WriteToList();
                break;
            case 2:
                a = 0;
                b = 1;
                c = 4;
                d = 5;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                WriteToList();
                break;
            case 3:
                a = 0;
                b = 1;
                c = 4;
                d = 5;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                WriteToList();
                break;
            case 4:
                a = 0;
                b = 1;
                c = 2;
                d = 3;
                e = 5;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                selectedCells.Add(e);
                WriteToList();
                break;
            case 5:
                a = 1;
                b = 2;
                c = 3;
                d = 4;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                WriteToList();
                break;
        }
    }

    static void AOELinkChoiceCellLvl_2()
    {
        switch (attacked.cellNum)
        {
            case 0:
                a = 1;
                b = 2;
                c = 3;
                d = 4;
                e = 5;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                selectedCells.Add(e);
                WriteToList();
                break;
            case 1:
                a = 0;
                b = 2;
                c = 3;
                d = 4;
                e = 5;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                selectedCells.Add(e);
                WriteToList();
                break;
            case 2:
                a = 0;
                b = 1;
                c = 3;
                d = 4;
                e = 5;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                selectedCells.Add(e);
                WriteToList();
                break;
            case 3:
                a = 0;
                b = 1;
                c = 2;
                d = 4;
                e = 5;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                selectedCells.Add(e);
                WriteToList();
                break;
            case 4:
                a = 0;
                b = 1;
                c = 2;
                d = 3;
                e = 5;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                selectedCells.Add(e);
                WriteToList();
                break;
            case 5:
                a = 0;
                b = 1;
                c = 2;
                d = 3;
                e = 4;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                selectedCells.Add(e);
                WriteToList();
                break;
        }
    }
    
    static void AOELinkDamage()
    {
        if (targets.Count > 0)
        {

        }
    }

    ///////////////////////////////////////////////////////// ///////////////////////////////////////////////////////// ////////////////////////////////////////////////
    //////////////////////////////////////////////////////Массовый удар по площади, что-то типа взрыва гранаты/// //////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////// ///////////////////////////////////////////////////////// ////////////////////////////////////////////////
    
    static void AOEBoom()
    {
        switch (attacker.general.skillLvl_0)
        {
            case 1:
                AOEBoomChoiceCellLvl_0();
                break;
            case 2:
                AOEBoomChoiceCellLvl_1();
                break;
            case 3:
                AOEBoomChoiceCellLvl_2();
                break;
        }
        switch (attacker.general.skillLvl_1)
        {
            case 1:
                AOEBoomChoiceCellLvl_0();
                break;
            case 2:
                AOEBoomChoiceCellLvl_1();
                break;
            case 3:
                AOEBoomChoiceCellLvl_2();
                break;
        }
        switch (attacker.general.skillLvl_2)
        {
            case 1:
                AOEBoomChoiceCellLvl_0();
                break;
            case 2:
                AOEBoomChoiceCellLvl_1();
                break;
            case 3:
                AOEBoomChoiceCellLvl_2();
                break;
        }
        switch (attacker.general.skillLvlBoss)
        {
            case 1:

                break;
            case 2:

                break;
        }

    }

    static void AOEBoomChoiceCellLvl_0()
    {
        switch (attacked.cellNum)
        {
            case 0:
                a = 1;
                b = 3;
                selectedCells.Add(a);
                selectedCells.Add(b);
                WriteToList();
                break;
            case 1:
                a = 0;
                b = 2;
                c = 4;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                WriteToList();
                break;
            case 2:
                a = 1;
                b = 5;
                selectedCells.Add(a);
                selectedCells.Add(b);
                WriteToList();
                break;
            case 3:
                a = 0;
                b = 4;
                selectedCells.Add(a);
                selectedCells.Add(b);
                WriteToList();
                break;
            case 4:
                a = 1;
                b = 3;
                c = 5;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                WriteToList();
                break;
            case 5:
                a = 2;
                b = 4;
                selectedCells.Add(a);
                selectedCells.Add(b);
                WriteToList();
                break;
        }
    }

    static void AOEBoomChoiceCellLvl_1()
    {
        switch (attacked.cellNum)
        {
            case 0:
                a = 1;
                b = 2;
                c = 3;
                d = 4;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                WriteToList();
                break;
            case 1:
                a = 0;
                b = 2;
                c = 3;
                d = 4;
                e = 5;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                selectedCells.Add(e);
                WriteToList();
                break;
            case 2:
                a = 0;
                b = 1;
                c = 4;
                d = 5;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                WriteToList();
                break;
            case 3:
                a = 0;
                b = 1;
                c = 4;
                d = 5;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                WriteToList();
                break;
            case 4:
                a = 0;
                b = 1;
                c = 2;
                d = 3;
                e = 5;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                selectedCells.Add(e);
                WriteToList();
                break;
            case 5:
                a = 1;
                b = 2;
                c = 3;
                d = 4;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                WriteToList();
                break;
        }
    }

    static void AOEBoomChoiceCellLvl_2()
    {
        switch (attacked.cellNum)
        {
            case 0:
                a = 1;
                b = 2;
                c = 3;
                d = 4;
                e = 5;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                selectedCells.Add(e);
                WriteToList();
                break;
            case 1:
                a = 0;
                b = 2;
                c = 3;
                d = 4;
                e = 5;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                selectedCells.Add(e);
                WriteToList();
                break;
            case 2:
                a = 0;
                b = 1;
                c = 3;
                d = 4;
                e = 5;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                selectedCells.Add(e);
                WriteToList();
                break;
            case 3:
                a = 0;
                b = 1;
                c = 2;
                d = 4;
                e = 5;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                selectedCells.Add(e);
                WriteToList();
                break;
            case 4:
                a = 0;
                b = 1;
                c = 2;
                d = 3;
                e = 5;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                selectedCells.Add(e);
                WriteToList();
                break;
            case 5:
                a = 0;
                b = 1;
                c = 2;
                d = 3;
                e = 4;
                selectedCells.Add(a);
                selectedCells.Add(b);
                selectedCells.Add(c);
                selectedCells.Add(d);
                selectedCells.Add(e);
                WriteToList();
                break;
        }
    }
}
