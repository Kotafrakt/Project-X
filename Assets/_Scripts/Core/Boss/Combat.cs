using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class CombatResult
{
    public TurnBase target;
    public float damage;
}

public static class Combat
{
    static TurnBase attacker;
    static TurnBase attacked;
    static SkillName skillName;
    static SkillNameB skillNameB;
    static DamageType damageType;
    static DamageType damageTypeB;
    static List<TurnBase> enemyes = new List<TurnBase>();
    static List<TurnBase> units = new List<TurnBase>();
    static List<TurnBase> targets = new List<TurnBase>();
    static bool targetIsPlayer = false; //фолс - атака по врагу.
    static List<int> selectedCells = new List<int>();
    static List<CombatResult> combatResults = new List<CombatResult>();
    static int skill_lvl;
    static int a, b, c, d, e, f;


    public static List<CombatResult> Attack(TurnBase attackerIn, TurnBase attackedIn, SkillName skillNameIn, SkillNameB skillNameBIn, List<TurnBase> enemyesIn, List<TurnBase> unitsIn, int skill_lvlIn)
    {
        InfoSkill infoSkill = SkillInfo.GetSkill(skillNameIn, skill_lvlIn);
        damageType = infoSkill.damageType;
        InfoSkill infoSkillB = SkillInfo.GetSkillB(skillNameBIn, skill_lvlIn);
        damageTypeB = infoSkillB.damageTypeB;
        targets.Clear();
        selectedCells.Clear();
        combatResults.Clear();
        attacker = attackerIn;
        attacked = attackedIn;
        skillName = skillNameIn;
        skillNameB = skillNameBIn;
        enemyes.Clear();
        units.Clear();
        enemyes = enemyesIn;
        units = unitsIn;
        skill_lvl = skill_lvlIn;
        CheckList();

        float damage = 0;
        if (skillName == SkillName.none && skillNameB == SkillNameB.none)
        {
            damage = attacker.general.PARAMS[GENERAL_DAMAGE_CURRENT] - attacked.general.PARAMS[GENERAL_DEFENSE_CURRENT];
            if (damage <= 0)
            {
                damage = 0;
            }
            else if (Random.Range(0, 100) < attacker.general.PARAMS[GENERAL_CRIT_CHANGE_CURRENT])
            {
                damage += attacker.general.PARAMS[GENERAL_CRIT_DAMAGE_CURRENT];
            }
            CombatResult cr = new CombatResult();
            cr.target = attacked;
            cr.damage = damage;
            combatResults.Add(cr);
        }
        else
        {
            if (damageType != DamageType.Default && skillName != SkillName.none)
            {
                switch (damageType)
                {
                    case DamageType.Link:
                        Link();
                        break;
                    case DamageType.Rebound:
                        Rebound();
                        break;
                    case DamageType.Row:
                        Row();
                        break;
                    case DamageType.Boom:
                        Boom();
                        break;
                    case DamageType.Dot:
                        
                        break;
                    case DamageType.Buff:
                        
                        break;
                    case DamageType.Debuff:
                        
                        break;
                }
            }
            else if (damageTypeB != DamageType.Default && skillNameB != SkillNameB.none)
            {
                switch (damageTypeB)
                {
                    case DamageType.Link:
                        Link();
                        break;
                    case DamageType.Rebound:
                        Rebound();
                        break;
                    case DamageType.Row:
                        Row();
                        break;
                    case DamageType.Boom:
                        Boom();
                        break;
                    case DamageType.Dot:

                        break;
                    case DamageType.Buff:

                        break;
                    case DamageType.Debuff:

                        break;
                }
            }
            else
            {

            }
            CombatResult crs = new CombatResult();
            crs.target = attacked;
            if (DamageType.Boom == damageType)
            {
               //crs.damage = DamageBoom();
            }
            crs.damage = 20;
            combatResults.Add(crs);
            foreach (TurnBase target in targets)
            {
                CombatResult crss = new CombatResult();
                crss.target = target;
                crss.damage = 15;
                combatResults.Add(crss);
            }

        }
        return combatResults; 
    }

    static void CheckList()
    {
        if (!attacked.isPlayer)
        {
            targetIsPlayer = true;
        }
        else
        {
            targetIsPlayer = false;
        }
    }
    static void WriteToList()
    {

        if (!targetIsPlayer)
        {
            for (int i = 0; i < selectedCells.Count; i++)
            {
                for (int j = 0; j < units.Count; j++)
                {
                    if (units[j].cellNum == selectedCells[i])
                    {
                        targets.Add(units[j] as TurnUnit);
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
                        targets.Add(enemyes[j] as TurnEnemy);
                    }
                }
            }
        }
    }

 
    ///////////////////////////////////////////////////////// ///////////////////////////////////////////////////////// ////////////////////////////////////////////////
    //////////////////////////////////////////////////////// ///////////цепной удар, что-то типа удара молнии/// ///////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////// ///////////////////////////////////////////////////////// ////////////////////////////////////////////////

    static void Link()
    {
        switch (skill_lvl)
        {
            case 1:
                LinkChoiceCellLvl_0();
                break;
            case 2:
                LinkChoiceCellLvl_1();
                break;
            case 3:
                LinkChoiceCellLvl_2();
                break;
        }
    }
    static void LinkChoiceCellLvl_0()
    {
        switch (attacked.cellNum)
        {
            case 0:
                b = 1; d = 3;
                a = -1; c = -1; e = -1; f = -1;
                SelectedCellsLink();
                WriteToList();
                break;
            case 1:
                a = 0; c = 2; e = 4;
                b = -1; d = -1; f = -1;
                SelectedCellsLink();
                WriteToList();
                break;
            case 2:
                b = 1; f = 5;
                a = -1; c = -1; d = -1; e = -1;
                SelectedCellsLink();
                WriteToList();
                break;
            case 3:
                a = 0; e = 4;
                b = -1; c = -1; d = -1; f = -1;
                SelectedCellsLink();
                WriteToList();
                break;
            case 4:
                b = 1; d = 3; f = 5;
                a = -1; c = -1; e = -1;
                SelectedCellsLink();
                WriteToList();
                break;
            case 5:
                c = 2; e = 4;
                a = -1; b = -1; d = -1; f = -1;
                SelectedCellsLink();
                WriteToList();
                break;           
        }
    }

    static void LinkChoiceCellLvl_1()
    {
        switch (attacked.cellNum)
        {
            case 0:
                b = 1; c = 2; d = 3; e = 4;
                a = -1; f = -1;
                SelectedCellsLink();
                WriteToList();
                break;
            case 1:
                a = 0; c = 2; d = 3; e = 4; f = 5;
                b = -1;
                SelectedCellsLink();
                WriteToList();
                break;
            case 2:
                a = 0; b = 1; e = 4; f = 5;
                c = -1; d = -1;
                SelectedCellsLink();
                WriteToList();
                break;
            case 3:
                a = 0; b = 1; e = 4; f = 5;
                c = -1; d = -1;
                SelectedCellsLink();
                WriteToList();
                break;
            case 4:
                a = 0; b = 1; c = 2; d = 3; f = 5;
                e = -1;
                SelectedCellsLink();
                WriteToList();
                break;
            case 5:
                b = 1; c = 2; d = 3; e = 4;
                a = -1; f = -1;
                SelectedCellsLink();
                WriteToList();
                break;
        }
    }

    static void LinkChoiceCellLvl_2()
    {
        switch (attacked.cellNum)
        {
            case 0:
                b = 1; c = 2; d = 3; e = 4; f = 5;
                a = -1;
                SelectedCellsLink();
                WriteToList();
                break;
            case 1:
                a = 0; c = 2; d = 3; e = 4; f = 5;
                b = -1;
                SelectedCellsLink();
                WriteToList();
                break;
            case 2:
                a = 0; b = 1; d = 3; e = 4; f = 5;
                c = -1;
                SelectedCellsLink();
                WriteToList();
                break;
            case 3:
                a = 0; b = 1; c = 2; e = 4; f = 5;
                d = -1;
                SelectedCellsLink();
                WriteToList();
                break;
            case 4:
                a = 0; b = 1; c = 2; d = 3; f = 5;
                e = -1;
                SelectedCellsLink();
                WriteToList();
                break;
            case 5:
                a = 0; b = 1; c = 2; d = 3; e = 4;
                f = -1;
                SelectedCellsLink();
                WriteToList();
                break;
        }
    }

    static void SelectedCellsLink()
    {
        if (a == 0)
        {
            selectedCells.Add(a);
        }
        if (b == 1)
        {
            selectedCells.Add(b);
        }
        if (c == 2)
        {
            selectedCells.Add(c);
        }
        if (d == 3)
        {
            selectedCells.Add(d);
        }
        if (e == 4)
        {
            selectedCells.Add(e);
        }
        if (f == 5)
        {
            selectedCells.Add(f);
        }
    }



    static void LinkDamage()
    {
        if (targets.Count > 0)
        {

        }
    }

    ///////////////////////////////////////////////////////// ///////////////////////////////////////////////////////// ////////////////////////////////////////////////
    //////////////////////////////////////////////////////Массовый удар по площади, что-то типа взрыва гранаты/// //////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////// ///////////////////////////////////////////////////////// ////////////////////////////////////////////////
    
    static void Boom()
    {
        switch (skill_lvl)
        {
            case 1:
                BoomChoiceCellLvl_0();
                break;
            case 2:
                BoomChoiceCellLvl_1();
                break;
            case 3:
                BoomChoiceCellLvl_2();
                break;
        }
    }

    static void BoomChoiceCellLvl_0()
    {
        switch (attacked.cellNum)
        {
            case 0:
                b = 1; d = 3;
                a = -1; c = -1; e = -1; f = -1;
                SelectedCellsBoom();
                WriteToList();
                break;
            case 1:
                a = 0; c = 2; e = 4;
                b = -1; d = -1; f = -1;
                SelectedCellsBoom();
                WriteToList();
                break;
            case 2:
                b = 1; f = 5;
                a = -1; c = -1; d = -1; e = -1;
                SelectedCellsBoom();
                WriteToList();
                break;
            case 3:
                a = 0; e = 4;
                b = -1; c = -1; d = -1; f = -1;
                SelectedCellsBoom();
                WriteToList();
                break;
            case 4:
                b = 1; d = 3; f = 5;
                a = -1; c = -1; e = -1;
                SelectedCellsBoom();
                WriteToList();
                break;
            case 5:
                c = 2; e = 4;
                a = -1; b = -1; d = -1; f = -1;
                SelectedCellsBoom();
                WriteToList();
                break;
        }
    }

    static void BoomChoiceCellLvl_1()
    {
        switch (attacked.cellNum)
        {
            case 0:
                b = 1; c = 2; d = 3; e = 4;
                a = -1; f = -1;
                SelectedCellsBoom();
                WriteToList();
                break;
            case 1:
                a = 0; c = 2; d = 3; e = 4; f = 5;
                b = -1;
                SelectedCellsBoom();
                WriteToList();
                break;
            case 2:
                a = 0; b = 1; e = 4; f = 5;
                c = -1; d = -1;
                SelectedCellsBoom();
                WriteToList();
                break;
            case 3:
                a = 0; b = 1; e = 4; f = 5;
                c = -1; d = -1;
                SelectedCellsBoom();
                WriteToList();
                break;
            case 4:
                a = 0; b = 1; c = 2; d = 3; f = 5;
                e = -1;
                SelectedCellsBoom();
                WriteToList();
                break;
            case 5:
                b = 1; c = 2; d = 3; e = 4;
                a = -1; f = -1;
                SelectedCellsBoom();
                WriteToList();
                break;
        }
    }

    static void BoomChoiceCellLvl_2()
    {
        switch (attacked.cellNum)
        {
            case 0:
                b = 1; c = 2; d = 3; e = 4; f = 5;
                a = -1;
                SelectedCellsBoom();
                WriteToList();
                break;
            case 1:
                a = 0; c = 2; d = 3; e = 4; f = 5;
                b = -1;
                SelectedCellsBoom();
                WriteToList();
                break;
            case 2:
                a = 0; b = 1; d = 3; e = 4; f = 5;
                c = -1;
                SelectedCellsBoom();
                WriteToList();
                break;
            case 3:
                a = 0; b = 1; c = 2; e = 4; f = 5;
                d = -1;
                SelectedCellsBoom();
                WriteToList();
                break;
            case 4:
                a = 0; b = 1; c = 2; d = 3; f = 5;
                e = -1;
                SelectedCellsBoom();
                WriteToList();
                break;
            case 5:
                a = 0; b = 1; c = 2; d = 3; e = 4;
                f = -1;
                SelectedCellsBoom();
                WriteToList();
                break;
        }
    }
    static void SelectedCellsBoom()
    {
        if (a == 0)
        {
            selectedCells.Add(a);
        }
        if (b == 1)
        {
            selectedCells.Add(b);
        }
        if (c == 2)
        {
            selectedCells.Add(c);
        }
        if (d == 3)
        {
            selectedCells.Add(d);
        }
        if (e == 4)
        {
            selectedCells.Add(e);
        }
        if (f == 5)
        {
            selectedCells.Add(f);
        }
    }

    static void DamageBoom()
    {
        switch (skill_lvl)
        {
            case 1:
                
                break;
            case 2:
                
                break;
            case 3:
                
                break;
        }
    }

    ///////////////////////////////////////////////////////// ///////////////////////////////////////////////////////// ////////////////////////////////////////////////
    //////////////////////////////////////////////////////Урон наносящийся по линиям. Бьёт либо первый ряд, либо задний/// /////////////////////////////////////////////
    ///////////////////////////////////////////////////////// ///////////////////////////////////////////////////////// ////////////////////////////////////////////////


    static void Row()
    {
        switch (skill_lvl)
        {
            case 1:
                RowChoiceCellLvl_0();
                break;
            case 2:
                RowChoiceCellLvl_1();
                break;
            case 3:
                RowChoiceCellLvl_2();
                break;
        }
    }

    static void RowChoiceCellLvl_0()
    {
        switch (attacked.cellNum)
        {
            case 0:
                b = 1;
                a = -1; c = -1; d =-1; e = -1; f = -1;
                SelectedCellsRow();
                WriteToList();
                break;
            case 1:
                a = 0; c = 2;
                b = -1; d = -1; e = -1; f = -1;
                SelectedCellsRow();
                WriteToList();
                break;
            case 2:
                b = 1;
                a = -1; c = -1; d = -1; e = -1; f = -1;
                SelectedCellsRow();
                WriteToList();
                break;
            case 3:
                e = 4;
                a = -1; b = -1; c = -1; d = -1; f = -1;
                SelectedCellsRow();
                WriteToList();
                break;
            case 4:
                d = 3; f = 5;
                a = -1; b = -1; c = -1; e = -1;
                SelectedCellsRow();
                WriteToList();
                break;
            case 5:
                e = 4;
                a = -1; b = -1; c = -1; d = -1; f = -1;
                SelectedCellsRow();
                WriteToList();
                break;
        }
    }

    static void RowChoiceCellLvl_1()
    {
        switch (attacked.cellNum)
        {
            case 0:
                b = 1; c = 2;
                a = -1; d = -1; e = -1; f = -1;
                SelectedCellsRow();
                WriteToList();
                break;
            case 1:
                a = 0; c = 2;
                b = -1; d = -1; e = -1; f = -1;
                SelectedCellsRow();
                WriteToList();
                break;
            case 2:
                a = 0; b = 1;
                c = -1; d = -1; e = -1; f = -1;
                SelectedCellsRow();
                WriteToList();
                break;
            case 3:
                e = 4; f = 5;
                a = -1; b = -1; c = -1; d = -1; 
                SelectedCellsRow();
                WriteToList();
                break;
            case 4:
                d = 3; f = 5;
                a = -1; b = -1; c = -1; e = -1;
                SelectedCellsRow();
                WriteToList();
                break;
            case 5:
                d = 3; e = 4;
                a = -1; b = -1; c = -1; f = -1;
                SelectedCellsRow();
                WriteToList();
                break;
        }
    }

    static void RowChoiceCellLvl_2()
    {
        switch (attacked.cellNum)
        {
            case 0:
                b = 1; c = 2;
                a = -1; d = -1; e = -1; f = -1;
                SelectedCellsRow();
                WriteToList();
                break;
            case 1:
                a = 0; c = 2;
                b = -1; d = -1; e = -1; f = -1;
                SelectedCellsRow();
                WriteToList();
                break;
            case 2:
                a = 0; b = 1;               
                c = -1; d = -1; e = -1; f = -1;
                SelectedCellsRow();
                WriteToList();
                break;
            case 3:
                e = 4; f = 5;
                a = -1; b = -1; c = -1; d = -1;
                SelectedCellsRow();
                WriteToList();
                break;
            case 4:
                d = 3; f = 5;
                a = -1; b = -1; c = -1; e = -1;
                SelectedCellsRow();
                WriteToList();
                break;
            case 5:
                d = 3; e = 4;
                a = -1; b = -1; c = -1; f = -1;
                SelectedCellsRow();
                WriteToList();
                break;
        }
    }
    static void SelectedCellsRow()
    {
        if (a == 0)
        {
            selectedCells.Add(a);
        }
        if (b == 1)
        {
            selectedCells.Add(b);
        }
        if (c == 2)
        {
            selectedCells.Add(c);
        }
        if (d == 3)
        {
            selectedCells.Add(d);
        }
        if (e == 4)
        {
            selectedCells.Add(e);
        }
        if (f == 5)
        {
            selectedCells.Add(f);
        }
    }



    ///////////////////////////////////////////////////////// ///////////////////////////////////////////////////////// ////////////////////////////////////////////////
    //////////////////////////////////////////////////////Урон отскок, по типу бумеранга. отскакивает от одного к дркгому/ /////////////////////////////////////////////
    ///////////////////////////////////////////////////////// ///////////////////////////////////////////////////////// ////////////////////////////////////////////////





    static void Rebound()
    {
        switch (skill_lvl)
        {
            case 1:
                ReboundChoiceCellLvl_0();
                break;
            case 2:
                ReboundChoiceCellLvl_1();
                break;
            case 3:
                ReboundChoiceCellLvl_2();
                break;
        }
    }
    static void ReboundChoiceCellLvl_0()
    {
        switch (attacked.cellNum)
        {
            case 0:
                b = 1; d = 3;
                a = -1; c = -1; e = -1; f = -1;
                SelectedCellsRebound();
                WriteToList();
                break;
            case 1:
                a = 0; c = 2; e = 4;
                b = -1; d = -1; f = -1;
                SelectedCellsRebound();
                WriteToList();
                break;
            case 2:
                b = 1; f = 5;
                a = -1; c = -1; d = -1; e = -1;
                SelectedCellsRebound();
                WriteToList();
                break;
            case 3:
                a = 0; e = 4;
                b = -1; c = -1; d = -1; f = -1;
                SelectedCellsRebound();
                WriteToList();
                break;
            case 4:
                b = 1; d = 3; f = 5;
                a = -1; c = -1; e = -1;
                SelectedCellsRebound();
                WriteToList();
                break;
            case 5:
                c = 2; e = 4;
                a = -1; b = -1; d = -1; f = -1;
                SelectedCellsRebound();
                WriteToList();
                break;
        }
    }

    static void ReboundChoiceCellLvl_1()
    {
        switch (attacked.cellNum)
        {
            case 0:
                b = 1; c = 2; d = 3; e = 4;
                a = -1; f = -1;
                SelectedCellsRebound();
                WriteToList();
                break;
            case 1:
                a = 0; c = 2; d = 3; e = 4; f = 5;
                b = -1;
                SelectedCellsRebound();
                WriteToList();
                break;
            case 2:
                a = 0; b = 1; e = 4; f = 5;
                c = -1; d = -1;
                SelectedCellsRebound();
                WriteToList();
                break;
            case 3:
                a = 0; b = 1; e = 4; f = 5;
                c = -1; d = -1;
                SelectedCellsRebound();
                WriteToList();
                break;
            case 4:
                a = 0; b = 1; c = 2; d = 3; f = 5;
                e = -1;
                SelectedCellsRebound();
                WriteToList();
                break;
            case 5:
                b = 1; c = 2; d = 3; e = 4;
                a = -1; f = -1;
                SelectedCellsRebound();
                WriteToList();
                break;
        }
    }

    static void ReboundChoiceCellLvl_2()
    {
        switch (attacked.cellNum)
        {
            case 0:
                b = 1; c = 2; d = 3; e = 4; f = 5;
                a = -1;
                SelectedCellsRebound();
                WriteToList();
                break;
            case 1:
                a = 0; c = 2; d = 3; e = 4; f = 5;
                b = -1;
                SelectedCellsRebound();
                WriteToList();
                break;
            case 2:
                a = 0; b = 1; d = 3; e = 4; f = 5;
                c = -1;
                SelectedCellsRebound();
                WriteToList();
                break;
            case 3:
                a = 0; b = 1; c = 2; e = 4; f = 5;
                d = -1;
                SelectedCellsRebound();
                WriteToList();
                break;
            case 4:
                a = 0; b = 1; c = 2; d = 3; f = 5;
                e = -1;
                SelectedCellsRebound();
                WriteToList();
                break;
            case 5:
                a = 0; b = 1; c = 2; d = 3; e = 4;
                f = -1;
                SelectedCellsRebound();
                WriteToList();
                break;
        }
    }

    static void SelectedCellsRebound()
    {
        if (a == 0)
        {
            selectedCells.Add(a);
        }
        if (b == 1)
        {
            selectedCells.Add(b);
        }
        if (c == 2)
        {
            selectedCells.Add(c);
        }
        if (d == 3)
        {
            selectedCells.Add(d);
        }
        if (e == 4)
        {
            selectedCells.Add(e);
        }
        if (f == 5)
        {
            selectedCells.Add(f);
        }
    }



    static void ReboundDamage()
    {
        if (targets.Count > 0)
        {

        }
    }
}