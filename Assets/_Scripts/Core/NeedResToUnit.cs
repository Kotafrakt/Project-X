using static Defines;
public static class NeedResToUnit
{
   public static Resource Unit(UnitType type)
    {
        Resource res = new Resource();
         switch (type)
        {
            //Грейд 0
            case UnitType.SkeletonV0:
                res.RES[GOLD] = 5;
                res.RES[BONES] = 3;
                break;
            case UnitType.ZombieV0:
                res.RES[GOLD] = 5;
                res.RES[BODY] = 3;
                break;
            //Грейд 1
            case UnitType.SkeletonV1:
                res.RES[GOLD] = 20;
                res.RES[BONES] = 10;
                res.units.Add(UnitType.SkeletonV0, 1);
                break;
            case UnitType.ZombieV1:
                res.RES[GOLD] = 20;
                res.RES[BODY] = 10;
                res.units.Add(UnitType.ZombieV0, 1);
                break;
            //Грейд 2
            case UnitType.SkeletonV2:
                res.RES[GOLD] = 20;
                res.RES[BONES] = 10;
                res.units.Add(UnitType.SkeletonV0, 3);
                res.units.Add(UnitType.SkeletonV1, 1);
                break;
            case UnitType.ZombieV2:
                res.RES[GOLD] = 20;
                res.RES[BODY] = 10;
                res.units.Add(UnitType.ZombieV0, 3);
                res.units.Add(UnitType.ZombieV1, 1);
                break;
        }
        return res;
    }
}
