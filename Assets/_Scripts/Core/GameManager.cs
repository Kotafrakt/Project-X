using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public bool isRussian;
    public static bool isFirstLoad = false;
    public int LevelAvable;
    public Dictionary<string, int> locations = new Dictionary<string, int>();
    public bool IsMapFirstRun = false;
    [HideInInspector]
    public string HeroPos = "C0";
    [HideInInspector]
    public List<Item> itemsArena = new List<Item>();
    [HideInInspector]
    public List<Item> itemsCompany = new List<Item>();
    [HideInInspector]
    public List<General> generals = new List<General>();

    public Save save = new Save();

    /// Castle Hiro
    [HideInInspector]
    public List<Unit> units = new List<Unit>(); //Доступное войско в замке
    [HideInInspector]
    public int resource;

    private void Awake()
    {
        if (instance == null)
        { // Экземпляр менеджера не был найден
            instance = this; // Задаем ссылку на экземпляр объекта
        }
        else
        { // Экземпляр объекта уже существует на сцене
            Destroy(gameObject); // Удаляем объект
        }
        DontDestroyOnLoad(gameObject);

        if (!PlayerPrefs.HasKey("Language"))
        {
            if (Application.systemLanguage == SystemLanguage.Russian || Application.systemLanguage == SystemLanguage.Ukrainian || Application.systemLanguage == SystemLanguage.Belarusian)
            {
                PlayerPrefs.SetString("Language", "ru_RU");
                isRussian = true;
            }
            else
            {
                PlayerPrefs.SetString("Language", "en_US");
                isRussian = false;
            }
        }
        else
        {
            if (PlayerPrefs.GetString("Language") == "ru_RU")
                isRussian = true;
            else
                isRussian = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    #region БЛОК СОХРАНЕНИЯ И ЗАГРУЗКИ

    [Serializable]
    public class Save
    {
        public int gameCoint;
        public Dictionary<string, int> locations = new Dictionary<string, int>();
        public List<Unit> units = new List<Unit>();
        public bool isReview;
        public bool isPremium;
        public DateTime saveData;
    }

    public void SaveGame()
    {
        save.gameCoint = 15;
        save.locations = locations;
        save.units = units;
        save.isReview = false;
        save.isPremium = false;
        save.saveData = DateTime.Now;
        PlayerPrefs.SetString("Save", JsonConvert.SerializeObject(save, Formatting.None));
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("Save"))
        {
            Debug.Log("LoadSave");
            string loadSave = PlayerPrefs.GetString("Save");
            save = JsonConvert.DeserializeObject<Save>(loadSave);
            Debug.Log("gameCoint = " + save.gameCoint);
        }
    }

    public void FirstGame()
    {
        GameResources.gold = 0;
        GameResources.wood = 0;
        GameResources.rock = 0;
        GameResources.iron = 0;
        GameResources.bones = 100;
        GameResources.souls = 0;
        GameResources.body = 0;
        GameResources.real = 50;

        GameResources.partsArtV0 = 0;
        GameResources.artV0 = 0;
        GameResources.partsArtV1 = 0;
        GameResources.artV1 = 0;
        GameResources.partsArtV2 = 0;
        GameResources.artV2 = 0;

        GameResources.partsArmorV0 = 0;
        GameResources.armorV0 = 0;
        GameResources.partsArmorV1 = 0;
        GameResources.armorV1 = 0;
        GameResources.partsArmorV2 = 0;
        GameResources.armorV2 = 0;

        GameResources.partsMobsV0 = 0;
        GameResources.mobsV0 = 0;
        GameResources.partsMobsV1 = 0;
        GameResources.mobsV1 = 0;
        GameResources.partsMobsV2 = 0;
        GameResources.mobsV2 = 0;

        units.Add(CreateUnit.UnitCreate(UnitType.SkeletonV0));
        units[0].PARAMS[UNIT_COUNT] = 50;
        units.Add(CreateUnit.UnitCreate(UnitType.ZombieV0));
        units[1].PARAMS[UNIT_COUNT] = 30;

        itemsArena.Add(CreateItem.Head_0());
        itemsArena.Add(CreateItem.Head_1());
        itemsArena.Add(CreateItem.Tors_0());
        itemsArena.Add(CreateItem.Pants_0());
        itemsArena.Add(CreateItem.Bots_0());
        itemsArena.Add(CreateItem.Weapon0_0());
        itemsArena.Add(CreateItem.Weapon1_0());
        itemsArena.Add(CreateItem.Amulet_0());
        itemsArena.Add(CreateItem.Ring_0());
        itemsArena.Add(CreateItem.Art_0());

        itemsCompany.Add(CreateItem.Head_0());
        itemsCompany.Add(CreateItem.Head_1());
        itemsCompany.Add(CreateItem.Tors_0());
        itemsCompany.Add(CreateItem.Pants_0());
        itemsCompany.Add(CreateItem.Bots_0());
        itemsCompany.Add(CreateItem.Weapon0_0());
        itemsCompany.Add(CreateItem.Weapon1_0());
        itemsCompany.Add(CreateItem.Amulet_0());
        itemsCompany.Add(CreateItem.Ring_0());
        itemsCompany.Add(CreateItem.Art_0());

        Hero.PARAMS[ARENA_HERO_DAMAGE] = 100;
        Hero.PARAMS[ARENA_HERO_HP] = 100;
        Hero.PARAMS[ARENA_HERO_HP_CURRENT] = Hero.PARAMS[ARENA_HERO_HP];
        Hero.PARAMS[HERO_MANA_MAX] = 100;
        Hero.PARAMS[HERO_MANA_CURRENT] = Hero.PARAMS[HERO_MANA_MAX];
        Hero.PARAMS[HERO_MANA_REGEN] = 0.2f;

        generals.Add(CreateGeneral.GenarateGeneral());
        generals.Add(CreateGeneral.GenarateGeneral());
        generals.Add(CreateGeneral.GenarateGeneral());
        generals.Add(CreateGeneral.GenarateGeneral());
        generals.Add(CreateGeneral.GenarateGeneral());
        CleanMap();
    }

    #endregion

    public void CleanMap()
    {
        locations.Add("C0", 2);
        locations.Add("C1", 2);
        locations.Add("C2", 2);
        locations.Add("C3", 2);
        locations.Add("C4", 2);
        locations.Add("C5", 2);
        locations.Add("C6", 2);

        locations.Add("V0", 2);
        locations.Add("V1", 2);
        locations.Add("V2", 2);
        locations.Add("V3", 2);
        locations.Add("V4", 2);
        locations.Add("V5", 2);
        locations.Add("V6", 2);
        locations.Add("V7", 2);
        locations.Add("V8", 2);
        locations.Add("V9", 2);
        locations.Add("V10", 2);
        locations.Add("V11", 2);
        locations.Add("V12", 2);
        locations.Add("V13", 2);
        locations.Add("V14", 2);
        locations.Add("V15", 2);
        locations.Add("V16", 2);
        locations.Add("V17", 2);
        locations.Add("V18", 2);

        locations.Add("K0", 2);
        locations.Add("K1", 2);
        locations.Add("K2", 2);
        locations.Add("K3", 2);
        locations.Add("K4", 2);
        locations.Add("K5", 2);
        locations.Add("K6", 2);

        locations.Add("R0", 2);
        locations.Add("R1", 2);
        locations.Add("R2", 2);
        locations.Add("R3", 2);
        locations.Add("R4", 2);
        locations.Add("R5", 2);
        locations.Add("R6", 2);
        locations.Add("R7", 2);
        locations.Add("R8", 2);
        locations.Add("R9", 2);
        locations.Add("R10", 2);
        locations.Add("R11", 2);
        locations.Add("R12", 2);
        locations.Add("R13", 2);
        locations.Add("R14", 2);
        locations.Add("R15", 2);
        locations.Add("R16", 2);
        locations.Add("R17", 2);
        locations.Add("R18", 2);
        locations.Add("R19", 2);
        locations.Add("R20", 2);
        locations.Add("R21", 2);
        locations.Add("R22", 2);
        locations.Add("R23", 2);
        locations.Add("R24", 2);
        locations.Add("R25", 2);
        locations.Add("R26", 2);
        locations.Add("R27", 2);
        locations.Add("R28", 2);
        locations.Add("R29", 2);
        locations.Add("R30", 2);
        locations.Add("R31", 2);
        locations.Add("R32", 2);
        locations.Add("R33", 2);
        locations.Add("R34", 2);
        locations.Add("R35", 2);
        locations.Add("R36", 2);
        locations.Add("R37", 2);
        locations.Add("R38", 2);
        locations.Add("R39", 2);
        locations.Add("R40", 2);
        locations.Add("R41", 2);
        locations.Add("R42", 2);
        locations.Add("R43", 2);
        locations.Add("R44", 2);
        locations.Add("R45", 2);
        locations.Add("R46", 2);
        locations.Add("R47", 2);
        locations.Add("R48", 2);
        locations.Add("R49", 2);
        locations.Add("R50", 2);
        locations.Add("R51", 2);
        locations.Add("R52", 2);
        locations.Add("R53", 2);
        locations.Add("R54", 2);
        locations.Add("R55", 2);
        locations.Add("R56", 2);
        locations.Add("R57", 2);
        locations.Add("R58", 2);
        locations.Add("R59", 2);
        locations.Add("R60", 2);
        locations.Add("R61", 2);
        locations.Add("R62", 2);
    }
}
