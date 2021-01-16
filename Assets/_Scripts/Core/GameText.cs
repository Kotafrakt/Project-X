using UnityEngine;

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
                    name = "СкелетV0";
                    break;
                case UnitType.SkeletonV1:
                    name = "СкелетV1";
                    break;
                case UnitType.SkeletonV2:
                    name = "СкелетV2";
                    break;
                case UnitType.ZombieV0:
                    name = "ЗомбиV0";
                    break;
                case UnitType.ZombieV1:
                    name = "ЗомбиV1";
                    break;
                case UnitType.ZombieV2:
                    name = "ЗомбиV2";
                    break;
                case UnitType.Engineer:
                    name = "ИгженерV0";
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case UnitType.SkeletonV0:
                    name = "SkeletonV0";
                    break;
                case UnitType.SkeletonV1:
                    name = "SkeletonV1";
                    break;
                case UnitType.SkeletonV2:
                    name = "SkeletonV2";
                    break;
                case UnitType.ZombieV0:
                    name = "ZombieV0";
                    break;
                case UnitType.ZombieV1:
                    name = "ZombieV1";
                    break;
                case UnitType.ZombieV2:
                    name = "ZombieV2";
                    break;
                case UnitType.Engineer:
                    name = "EngineerV0";
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
                    description = "Скелет V0";
                    break;
                case UnitType.SkeletonV1:
                    description = "Скелет V1";
                    break;
                case UnitType.SkeletonV2:
                    description = "Скелет V2";
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case UnitType.SkeletonV0:
                    description = "The skeleton of the V0";
                    break;
                case UnitType.SkeletonV1:
                    description = "The skeleton of the V1";
                    break;
                case UnitType.SkeletonV2:
                    description = "The skeleton of the V2";
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

    public static string VictoryText()
    {
        string description = "";
        int textRnd = Random.Range(0, 2);
        if (GameManager.instance.isRussian)
        {
            switch (textRnd)
            {
                case 0:
                    description = "Победа!";
                    break;
                case 1:
                    description = "Победа! Можно собирать трофеи!";
                    break;
                case 2:
                    description = "Враг повержен!";
                    break;
            }
        }
        else
        {
            switch (textRnd)
            {
                case 0:
                    description = "Victory!!!";
                    break;
                case 1:
                    description = "Victory!!!";
                    break;
                case 2:
                    description = "Victory!!!";
                    break;
            }
        }
        return description;
    }

    public static string LoseText()
    {
        string description = "";
        int textRnd = Random.Range(0, 2);
        if (GameManager.instance.isRussian)
        {
            switch (textRnd)
            {
                case 0:
                    description = "Поражение!";
                    break;
                case 1:
                    description = "Вы потерпели поражение!";
                    break;
                case 2:
                    description = "Может в следующий раз повезёт..";
                    break;
            }
        }
        else
        {
            switch (textRnd)
            {
                case 0:
                    description = "You lose!!!";
                    break;
                case 1:
                    description = "You lose!!!";
                    break;
                case 2:
                    description = "You lose!!!";
                    break;
            }
        }
        return description;
    }
}
