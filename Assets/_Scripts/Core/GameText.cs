public static class GameText
{
    public static string GetUnitName(UnitType type)
    {
        string name = "name";
        if (GameManager.instance.isRussian)
        {
            switch (type)
            {
                case UnitType.SkeletonV0:
                    name = "Скилет";
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case UnitType.SkeletonV0:
                    name = "Skeleton";
                    break;
            }
        }
        return name;
    }

    public static string GetUnitDescription(UnitType type)
    {
        string description = "name";
        if (GameManager.instance.isRussian)
        {
            switch (type)
            {
                case UnitType.SkeletonV0:
                    description = "Скилет первого уровня";
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case UnitType.SkeletonV0:
                    description = "The skeleton of the first level";
                    break;
            }
        }
        return description;
    }

    public static string GetEnemyName(EnemyType type)
    {
        string name = "name";
        if (GameManager.instance.isRussian)
        {
            switch (type)
            {
                case EnemyType.war0:
                    name = "Воин";
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case EnemyType.war0:
                    name = "Warrior";
                    break;
            }
        }
        return name;
    }

    public static string GetEnemyDescription(EnemyType type)
    {
        string description = "name";
        if (GameManager.instance.isRussian)
        {
            switch (type)
            {
                case EnemyType.war0:
                    description = "Воин первого уровня";
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case EnemyType.war0:
                    description = "First-level warrior";
                    break;
            }
        }
        return description;
    }
}
