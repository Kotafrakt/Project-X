using static Defines;

public static class CreateUnit
{

    public static Unit UnitCreate(UnitType type)
    {
        Unit unit = new Unit();
        switch (type)                            //Обезательно у всех мобов должны быть подпись V0,V1 либо V2
        {
            case UnitType.SkeletonV0:
                unit.type = type;
                unit.prefab = ResManager.instance.units[0];
                unit.img = ResManager.instance.unitImg[0];
                unit.img2 = ResManager.instance.unitImg2[0];
                unit.PARAMS[UNIT_COST] = 5;
                unit.PARAMS[UNIT_HP_MAX] = 5;
                unit.PARAMS[UNIT_HP_REGEN] = 1;
                unit.PARAMS[UNIT_ATTACK_DELAY] = 15;
                unit.PARAMS[UNIT_DISTANCE] = 5;
                unit.PARAMS[UNIT_SPEED] = 1f;
                unit.PARAMS[UNIT_DAMAGE] = 5;
                unit.PARAMS[UNIT_CRIT_DAMAGE] = 1.2f;
                unit.PARAMS[UNIT_CRIT_CHANCE] = 5;
                unit.PARAMS[UNIT_ARMOR] = 5;
                unit.PARAMS[UNIT_FIRE_RESIST] = 5;
                unit.PARAMS[UNIT_ICE_RESIST] = 5;
                unit.PARAMS[UNIT_ELECTRIC_RESIST] = 5;
                break;
            case UnitType.SkeletonV1:
                unit.type = type;
                unit.prefab = ResManager.instance.units[1];
                unit.img = ResManager.instance.unitImg[1];
                unit.img2 = ResManager.instance.unitImg2[1];
                unit.PARAMS[UNIT_COST] = 10;
                unit.PARAMS[UNIT_HP_MAX] = 10;
                unit.PARAMS[UNIT_HP_REGEN] = 2;
                unit.PARAMS[UNIT_ATTACK_DELAY] = 10;
                unit.PARAMS[UNIT_SPEED] = 0.4f;
                unit.PARAMS[UNIT_DAMAGE] = 10;
                unit.PARAMS[UNIT_CRIT_DAMAGE] = 1.5f;
                unit.PARAMS[UNIT_CRIT_CHANCE] = 10;
                unit.PARAMS[UNIT_DISTANCE] = 10;
                unit.PARAMS[UNIT_ARMOR] = 10;
                unit.PARAMS[UNIT_FIRE_RESIST] = 10;
                unit.PARAMS[UNIT_ICE_RESIST] = 10;
                unit.PARAMS[UNIT_ELECTRIC_RESIST] = 10;
                break;
            case UnitType.SkeletonV2:
                unit.type = type;
                unit.prefab = ResManager.instance.units[2];
                unit.img = ResManager.instance.unitImg[2];
                unit.img2 = ResManager.instance.unitImg2[2];
                unit.PARAMS[UNIT_COST] = 15;
                unit.PARAMS[UNIT_HP_MAX] = 15;
                unit.PARAMS[UNIT_HP_REGEN] = 3;
                unit.PARAMS[UNIT_ATTACK_DELAY] = 5;
                unit.PARAMS[UNIT_SPEED] = 0.6f;
                unit.PARAMS[UNIT_DAMAGE] = 15;
                unit.PARAMS[UNIT_CRIT_DAMAGE] = 1.8f;
                unit.PARAMS[UNIT_CRIT_CHANCE] = 15;
                unit.PARAMS[UNIT_DISTANCE] = 15;
                unit.PARAMS[UNIT_ARMOR] = 15;
                unit.PARAMS[UNIT_FIRE_RESIST] = 15;
                unit.PARAMS[UNIT_ICE_RESIST] = 15;
                unit.PARAMS[UNIT_ELECTRIC_RESIST] = 15;
                break;
            case UnitType.ZombieV0:
                unit.type = type;
                unit.prefab = ResManager.instance.units[3];
                unit.img = ResManager.instance.unitImg[3];
                unit.img2 = ResManager.instance.unitImg2[3];
                unit.PARAMS[UNIT_COST] = 5;
                unit.PARAMS[UNIT_HP_MAX] = 5;
                unit.PARAMS[UNIT_HP_REGEN] = 1;
                unit.PARAMS[UNIT_ATTACK_DELAY] = 15;
                unit.PARAMS[UNIT_SPEED] = 0.4f;
                unit.PARAMS[UNIT_DAMAGE] = 5;
                unit.PARAMS[UNIT_CRIT_DAMAGE] = 1.2f;
                unit.PARAMS[UNIT_CRIT_CHANCE] = 5;
                unit.PARAMS[UNIT_DISTANCE] = 5;
                unit.PARAMS[UNIT_ARMOR] = 5;
                unit.PARAMS[UNIT_FIRE_RESIST] = 5;
                unit.PARAMS[UNIT_ICE_RESIST] = 5;
                unit.PARAMS[UNIT_ELECTRIC_RESIST] = 5;
                break;
            case UnitType.ZombieV1:
                unit.type = type;
                unit.prefab = ResManager.instance.units[4];
                unit.img = ResManager.instance.unitImg[4];
                unit.img2 = ResManager.instance.unitImg2[4];
                unit.PARAMS[UNIT_COST] = 10;
                unit.PARAMS[UNIT_HP_MAX] = 10;
                unit.PARAMS[UNIT_HP_REGEN] = 2;
                unit.PARAMS[UNIT_ATTACK_DELAY] = 10;
                unit.PARAMS[UNIT_SPEED] = 0.4f;
                unit.PARAMS[UNIT_DAMAGE] = 10;
                unit.PARAMS[UNIT_CRIT_DAMAGE] = 1.5f;
                unit.PARAMS[UNIT_CRIT_CHANCE] = 10;
                unit.PARAMS[UNIT_DISTANCE] = 10;
                unit.PARAMS[UNIT_ARMOR] = 10;
                unit.PARAMS[UNIT_FIRE_RESIST] = 10;
                unit.PARAMS[UNIT_ICE_RESIST] = 10;
                unit.PARAMS[UNIT_ELECTRIC_RESIST] = 10;
                break;
            case UnitType.ZombieV2:
                unit.type = type;
                unit.prefab = ResManager.instance.units[5];
                unit.img = ResManager.instance.unitImg[5];
                unit.img2 = ResManager.instance.unitImg2[5];
                unit.PARAMS[UNIT_COST] = 15;
                unit.PARAMS[UNIT_HP_MAX] = 15;
                unit.PARAMS[UNIT_HP_REGEN] = 3;
                unit.PARAMS[UNIT_ATTACK_DELAY] = 5;
                unit.PARAMS[UNIT_SPEED] = 0.6f;
                unit.PARAMS[UNIT_DAMAGE] = 15;
                unit.PARAMS[UNIT_CRIT_DAMAGE] = 1.8f;
                unit.PARAMS[UNIT_CRIT_CHANCE] = 15;
                unit.PARAMS[UNIT_DISTANCE] = 15;
                unit.PARAMS[UNIT_ARMOR] = 15;
                unit.PARAMS[UNIT_FIRE_RESIST] = 15;
                unit.PARAMS[UNIT_ICE_RESIST] = 15;
                unit.PARAMS[UNIT_ELECTRIC_RESIST] = 15;
                break;
        }
        return unit;
    }
}
