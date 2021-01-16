using static Defines;
public static class NeedResToUnit
{
   public static Resource GetRes(UnitType type)
    {
        Resource res = new Resource();
         switch (type)
        {
            //Грейд 0
            case UnitType.SkeletonV0:
                res.resource[GOLD] = 5;
                res.resource[BONES] = 3;
                break;
            case UnitType.ZombieV0:
                res.resource[GOLD] = 5;
                res.resource[BODY] = 3;
                break;
            //Грейд 1
            case UnitType.SkeletonV1:
                res.resource[GOLD] = 20;
                res.resource[BONES] = 10;
                res.units.Add(UnitType.SkeletonV0, 1);
                break;
            case UnitType.ZombieV1:
                res.resource[GOLD] = 20;
                res.resource[BODY] = 10;
                res.units.Add(UnitType.ZombieV0, 1);
                break;
            //Грейд 2
            case UnitType.SkeletonV2:
                res.resource[GOLD] = 20;
                res.resource[BONES] = 10;
                res.units.Add(UnitType.SkeletonV0, 3);
                res.units.Add(UnitType.SkeletonV1, 1);
                break;
            case UnitType.ZombieV2:
                res.resource[GOLD] = 20;
                res.resource[BODY] = 10;
                res.units.Add(UnitType.ZombieV0, 3);
                res.units.Add(UnitType.ZombieV1, 1);
                break;
            case UnitType.Engineer:
                res.resource[GOLD] = 10;
                res.resource[BODY] = 5;
                break;
        }
        return res;
    }
}
