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
}
public static class SkillInfo
{
    public static InfoSkill GetSkill(SkillName name)
    {
        InfoSkill iS = new InfoSkill();
        switch (name)
        {
            case SkillName.none:
                iS.damageType = DamageType.Default;
                iS.delay = 0;
                break;
            case SkillName.skill0:
                iS.damageType = DamageType.Link;
                iS.delay = 2;
                iS.img = ResManager.instance.imgSkill[0];
                break;
            case SkillName.skill1:
                iS.damageType = DamageType.Rebound;
                iS.delay = 2;
                iS.img = ResManager.instance.imgSkill[1];
                break;
            case SkillName.skill2:
                iS.damageType = DamageType.Row;
                iS.delay = 2;
                iS.img = ResManager.instance.imgSkill[2];
                break;
        }
        return iS;
    }

    public static InfoSkill GetSkillB(SkillNameB name)
    {
        InfoSkill iS = new InfoSkill();
        switch (name)
        {
            case SkillNameB.none:
                iS.damageType = DamageType.Default;
                iS.delay = 0;
                break;
            case SkillNameB.skillB0:
                iS.damageType = DamageType.Boom;
                iS.delay = 2;
                iS.img = ResManager.instance.imgSkill[3];
                break;
        }
        return iS;
    }

}
