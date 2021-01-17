using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Default, Link, Rebound, Row,  Boom, Dot, Buff, Debuff
}

public class InfoSkill
{
    public Sprite img;
    public string name;
    public string disc;
    public int delay;
    public DamageType damageType;
    public DamageType damageTypeB;

    public int baseDamage;      //Базовый урон скила
    public int bonusDamage;     //Бонусный урон скила который добавляется к параметрам(Силы, интелекта либо к другим от которых он зависит)
    public int stun;            //% оглушения, оцепенения и т.д.
    public int stunVal;         //на сколько ходов оглушает
    public int цели;            //Шанс отработки скилла по доп целям, для каждой цели одинаковый шанс
    public int цель_0;          //Шанс отработки скилла по дополнительной цели
    public int цель_1;          //Шанс отработки скилла по дополнительной цели
    public int цель_2;          //Шанс отработки скилла по дополнительной цели

    public int дотУрон;          //Колличество дот урона (Дот - демадж оф тайм) 
    public int дотШанс;          //Шанс навесить дот урон




}
public static class SkillInfo
{
    public static InfoSkill GetSkill(SkillName name, int level)
    {
        InfoSkill iS = new InfoSkill();
        switch (name)
        {
            case SkillName.none:
                iS.damageType = DamageType.Default;
                iS.delay = 0;
                switch (level)
                {
                    case 1:

                        break;
                    case 2:

                        break;
                    case 3:

                        break;
                }
                break;
            case SkillName.skill0:
                iS.damageType = DamageType.Link;
                iS.delay = 2;
                iS.img = ResManager.instance.imgSkill[0];
                switch (level)
                {
                    case 1:

                        break;
                    case 2:

                        break;
                    case 3:

                        break;
                }
                break;
            case SkillName.skill1:
                iS.damageType = DamageType.Rebound;
                iS.delay = 2;
                iS.img = ResManager.instance.imgSkill[1];
                switch (level)
                {
                    case 1:

                        break;
                    case 2:

                        break;
                    case 3:

                        break;
                }
                break;
            case SkillName.skill2:
                iS.damageType = DamageType.Row;
                iS.delay = 2;
                iS.img = ResManager.instance.imgSkill[2];
                switch (level)
                {
                    case 1:

                        break;
                    case 2:

                        break;
                    case 3:

                        break;
                }
                break;
        }
        return iS;
    }

    public static InfoSkill GetSkillB(SkillNameB name, int level)
    {
        InfoSkill iS = new InfoSkill();
        switch (name)
        {
            case SkillNameB.none:
                iS.damageTypeB = DamageType.Default;
                iS.delay = 0;
                switch (level)
                {
                    case 1:

                        break;
                    case 2:

                        break;
                }
                break;
            case SkillNameB.skillB0:
                iS.damageTypeB = DamageType.Boom;
                iS.delay = 2;
                iS.img = ResManager.instance.imgSkill[3];
                switch (level)
                {
                    case 1:

                        break;
                    case 2:

                        break;
                }
                break;
        }
        return iS;
    }

}
