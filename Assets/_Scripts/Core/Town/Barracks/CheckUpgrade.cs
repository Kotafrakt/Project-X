
public static class CheckUpgrade
{
    public static UnitType CheckUnit(UnitType type)
    {
        UnitType uType = UnitType.None;

        switch(type)
        {
            case UnitType.SkeletonV0 :
                uType = UnitType.SkeletonV1;
                break;
            case UnitType.SkeletonV1:
                uType = UnitType.SkeletonV2;
                break;
            case UnitType.SkeletonV2:
                uType = UnitType.None;
                break;
            case UnitType.ZombieV0:
                uType = UnitType.ZombieV1;
                break;
            case UnitType.ZombieV1:
                uType = UnitType.ZombieV2;
                break;
            case UnitType.ZombieV2:
                uType = UnitType.None;
                break;
        }

        return uType;
    }
}
