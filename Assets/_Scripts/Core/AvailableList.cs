using System.Collections.Generic;

public static class AvailableList
{
    public static Dictionary<UnitType, bool> availableUnit = new Dictionary<UnitType, bool>();

    public static void StartDictionary()
    {
        //Грейд 0
        availableUnit.Add(UnitType.SkeletonV0, true);
        availableUnit.Add(UnitType.ZombieV0, true);
        availableUnit.Add(UnitType.Engineer, true);

        //Грейд 1
        availableUnit.Add(UnitType.SkeletonV1, false);
        availableUnit.Add(UnitType.ZombieV1, false);

        //Грейд 2
        availableUnit.Add(UnitType.SkeletonV2, true);
        availableUnit.Add(UnitType.ZombieV2, true);
    }

}
