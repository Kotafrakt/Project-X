public static class GeneralText
{
    public static string GetName(int id)
    {
        string name = "";
        switch (id)
        {
            case 0:
                if (GameManager.instance.isRussian)
                {
                    name = "Чучело";
                }
                else
                {
                    name = "Чучело";
                }
                break;
        }
        return name;
    }

    public static string GetDescription(int id)
    {
        string desc = "";
        switch (id)
        {
            case 0:
                if (GameManager.instance.isRussian)
                {
                    desc = "Шагающее мясо";
                }
                else
                {
                    desc = "Шагающее мясо";
                }
                break;
        }
        return desc;
    }
}
