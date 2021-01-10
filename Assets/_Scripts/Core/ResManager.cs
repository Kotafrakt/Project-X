using UnityEngine;

public class ResManager : MonoBehaviour
{
    public static ResManager instance = null;
    [Header("Игровые меню")]
    //[Tooltip("Всплывающая подсказка на переменной в инспекторе")]
    public GameObject playPanelDown;
    public GameObject startLevel;
    public GameObject startBossLevel;
    public GameObject playPanelUp;
    public GameObject playBossPanelUp;
    public GameObject playBossPanelDown;
    public GameObject levelMenu;
    public GameObject levelBossMenu;
    public GameObject levelBossResult;
    public GameObject finished;
    public GameObject DamageText;

    [Header("Юниты")]
    public GameObject[] units;
    public Sprite[] unitImg;
    public Sprite[] unitImg2;

    [Header("Генералы")]
    public GameObject[] gens;
    public Sprite[] genImg;
    public Sprite[] genImg2;

    [Header("ГенералыВраги")]
    public GameObject[] gensE;
    public Sprite[] genEImg;
    public Sprite[] genEImg2;

    [Header("Игровые объекты")]
    public GameObject portal;
    public GameObject Explosion;
    public Sprite zeroSlot;

    [Header("Картинки вещей Компании")]
    public Sprite[] imgHead;
    public Sprite[] imgTors;
    public Sprite[] imgPants;
    public Sprite[] imgBots;
    public Sprite[] imgWeapon0;
    public Sprite[] imgWeapon1;
    public Sprite[] imgAmulet;
    public Sprite[] imgRing;
    public Sprite[] imgArt;

    [Header("Картинки вещей Арены")]
    public Sprite[] imgHeadArena;
    public Sprite[] imgTorsArena;
    public Sprite[] imgPantsArena;
    public Sprite[] imgBotsArena;
    public Sprite[] imgWeapon0Arena;
    public Sprite[] imgWeapon1Arena;
    public Sprite[] imgAmuletArena;
    public Sprite[] imgRingArena;
    public Sprite[] imgArtArena;

    [Header("Картинки Ресурсов")]
    public Sprite[] imgRes;

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
    }
}
