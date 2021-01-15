using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Defines;


public class Level : MonoBehaviour, IMessage
{
    [SerializeField]
    private string levelName;
    [SerializeField]
    private int needFinished;
    [HideInInspector]
    public bool isWinner = false;
    [HideInInspector]
    public bool isRoad;

    public GameObject[] spawnPoint;
    public SpawnBtn[] spawnButtons;
    public Transform[] wayPointsUnit0;
    public Transform[] wayPointsUnit1;
    public Transform[] wayPointsUnit2;
    public Transform finish;
    public Transform canvas;
    GameObject levelBossResult;

    public GameObject finished;
    GameObject playPanelUp;
    GameObject playPanelDown;
    GameObject startLevel;
    GameObject levelMenu;
    List<Button> selectedunits = new List<Button>();
    List<Image> selectedunitsImg = new List<Image>();
    List<Text> selectedunitsText = new List<Text>();
    List<UnitBtn> selectedunitBtn = new List<UnitBtn>();
    List<Button> avableunits = new List<Button>();
    List<Image> avableunitsImg = new List<Image>();
    List<Text> avableunitsText = new List<Text>();
    List<UnitBtn> avableunitBtn = new List<UnitBtn>();
    Button btnStart;
    Button btnCancel;
    Button btnGlobal;
    Button btnMenu;
    Button btnSelect;
    Button btnSelected;
    Button продолжить;
    Button fin;
    Button бежать;
    Button настройки;
    Button выход;
    Button пауза;
    Button ускорение;
    Button босс;
    bool pause;
    int speedUp;
    Text mana;
    Text textRight;
    Text textLeft;
    Transform selectedUnitPanel;
    Transform avableUnitPanel;

    GameObject messagePanel;
    Text messageText;
    Button messageButton;

    private GameObject buttonHero;
    private List<GameObject> buttonsGeneral = new List<GameObject>();
    private List<GameObject> buttonsUnit = new List<GameObject>();

    void Start()
    {
        Time.timeScale = 1;
        pause = false;
        speedUp = 0;
        CreatePanelUp();
        CreatePanelDown();
        CreatePanelStartLevel();
        СоздатьЛевелМеню();
        CreateFinished();
        LevelManager.instance.StartLevel(levelName, wayPointsUnit0, wayPointsUnit1, wayPointsUnit2, finish, spawnPoint, needFinished, mana, spawnButtons, this);
    }

    void Update()
    {

    }

    void CreatePanelUp()
    {
        playPanelUp = Instantiate(ResManager.instance.playPanelUp, canvas);
        btnMenu = playPanelUp.transform.GetChild(0).GetComponent<Button>();
        btnMenu.onClick.AddListener(delegate { OpenMenu(); });
        mana = playPanelUp.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();
        пауза = playPanelUp.transform.GetChild(2).GetComponent<Button>();
        пауза.onClick.AddListener(delegate { Пауза(); });
        ускорение = playPanelUp.transform.GetChild(3).GetComponent<Button>();
        ускорение.onClick.AddListener(delegate { Ускорение(); });
        босс = playPanelUp.transform.GetChild(5).GetComponent<Button>();
        босс.onClick.AddListener(delegate { Босс(); });
    }
    void Босс()
    {
        SceneManager.LoadScene(levelName + "B");
    }
    void Пауза()
    {
        pause = !pause;
        if (pause)
        {
            Time.timeScale = 0;
            пауза.transform.GetComponent<Image>().color = Color.green;
        }
        else
        {
            Time.timeScale = 1;
            пауза.transform.GetComponent<Image>().color = Color.white;
            speedUp = 0;
        }
    }
    void Ускорение()
    {
        switch (speedUp)
        {
            case 0:
                speedUp = 1;
                Time.timeScale = 2;
                pause = false;
                break;
            case 1:
                speedUp = 2;
                Time.timeScale = 4;
                pause = false;
                break;
            case 2:
                speedUp = 3;
                Time.timeScale = 8;
                pause = false;
                break;
            case 3:
                speedUp = 4;
                Time.timeScale = 1;
                pause = false;
                break;
            case 4:
                speedUp = 0;
                Time.timeScale = 0.5f;
                pause = false;
                break;
        }
    }
    void СоздатьЛевелМеню()
    {
        levelMenu = Instantiate(ResManager.instance.levelMenu, canvas);
        продолжить = levelMenu.transform.GetChild(0).GetComponent<Button>();
        продолжить.onClick.AddListener(delegate { Продолжить(); });
        бежать = levelMenu.transform.GetChild(1).GetComponent<Button>();
        бежать.onClick.AddListener(delegate { Бежать(); });
        настройки = levelMenu.transform.GetChild(2).GetComponent<Button>();
        настройки.onClick.AddListener(delegate { Настройки(); });
        выход = levelMenu.transform.GetChild(3).GetComponent<Button>();
        выход.onClick.AddListener(delegate { Выход(); });
    }

    void CreateFinished()
    {
        finished = Instantiate(ResManager.instance.finished, canvas);
        fin = finished.transform.GetChild(1).GetComponent<Button>();
        fin.onClick.AddListener(delegate { LevelManager.instance.Fin(); });
    }
    void OpenMenu()
    {
        if (levelMenu.activeSelf)
        {
            Time.timeScale = 1;
            levelMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            levelMenu.SetActive(true);
        }
    }
    void Продолжить()
    {
        Time.timeScale = 1;
        levelMenu.SetActive(false);
    }
    void Бежать()
    {
        Time.timeScale = 1;
        LevelManager.instance.UnitList.Clear();
        LevelManager.instance.Lose();
    }
    void Настройки()
    {

    }
    void Выход()
    {
        LevelManager.instance.UnitList.Clear();
        LevelManager.instance.Lose();
        Application.Quit();
    }

    void CreatePanelDown()
    {
        playPanelDown = Instantiate(ResManager.instance.playPanelDown, canvas);
        buttonHero = playPanelDown.transform.GetChild(0).gameObject;
        for (int i = 1; i < 4; i++)
        {
            buttonsGeneral.Add(playPanelDown.transform.GetChild(i).gameObject);
        }

        for (int i = 4; i < 14; i++)
        {
            buttonsUnit.Add(playPanelDown.transform.GetChild(i).gameObject);
        }
    }

    void CreatePanelStartLevel()
    {
        startLevel = Instantiate(ResManager.instance.startLevel, canvas);
        selectedUnitPanel = startLevel.transform.GetChild(0);
        avableUnitPanel = startLevel.transform.GetChild(1);
        for (int i = 3; i < 13; i++)
        {
            GameObject slot = selectedUnitPanel.GetChild(i).gameObject;
            slot.GetComponent<Button>().onClick.AddListener(delegate { SelectUnit(slot.GetComponent<Button>()); });
            selectedunits.Add(slot.GetComponent<Button>());
            selectedunitsImg.Add(slot.GetComponent<Image>());
            selectedunitsText.Add(slot.transform.GetChild(0).GetComponent<Text>());
            selectedunitBtn.Add(slot.GetComponent<UnitBtn>());
        }
        for (int i = 0; i < 20; i++)
        {
            GameObject slot = avableUnitPanel.GetChild(i).gameObject;
            slot.GetComponent<Button>().onClick.AddListener(delegate { SelectedUnit(slot.GetComponent<Button>()); });
            avableunits.Add(slot.GetComponent<Button>());
            avableunitsImg.Add(slot.GetComponent<Image>());
            avableunitsText.Add(slot.transform.GetChild(0).GetComponent<Text>());
            avableunitBtn.Add(slot.GetComponent<UnitBtn>());
        }
        btnStart = startLevel.transform.GetChild(3).GetComponent<Button>();
        btnStart.onClick.AddListener(delegate { PlayLevel(); });
        textRight = startLevel.transform.GetChild(4).GetComponent<Text>();
        btnCancel = startLevel.transform.GetChild(5).GetComponent<Button>();
        btnCancel.onClick.AddListener(delegate { CancelSelected(); });
        textLeft = startLevel.transform.GetChild(6).GetComponent<Text>();
        btnGlobal = startLevel.transform.GetChild(7).GetComponent<Button>();
        btnGlobal.onClick.AddListener(delegate { OpenMenu(); });
        for (int i = 0; i < GameManager.instance.units.Count; i++)
        {
            avableunitsImg[i].sprite = GameManager.instance.units[i].img;
            avableunitsText[i].text = GameManager.instance.units[i].PARAMS[UNIT_COUNT].ToString() + " ";
            avableunitBtn[i].sprite = GameManager.instance.units[i].img;
            avableunitBtn[i].unit = GameManager.instance.units[i];
            avableunits[i].interactable = true;
        }
    }

    void SelectUnit(Button btn)
    {
        UnitBtn uBtn = btn.GetComponent<UnitBtn>();
        if (uBtn.unit != null)
        {
            uBtn.btn.interactable = true;
            btn.transform.GetComponent<Image>().sprite = ResManager.instance.zeroSlot;
            btn.transform.GetChild(0).GetComponent<Text>().text = "";
            uBtn.sprite = null;
            uBtn.unit = null;
            uBtn.btn = null;
        }
        btnSelect = btn;
        selectedUnitPanel.gameObject.SetActive(false);
        btnStart.gameObject.SetActive(false);
        textRight.gameObject.SetActive(false);
        avableUnitPanel.gameObject.SetActive(true);
        btnCancel.gameObject.SetActive(true);
        textLeft.gameObject.SetActive(true);
    }
    void SelectedUnit(Button btn)
    {
        btnSelected = btn;
        avableUnitPanel.gameObject.SetActive(false);
        btnCancel.gameObject.SetActive(false);
        textLeft.gameObject.SetActive(false);
        btn.interactable = false;

        btnSelect.GetComponent<Image>().sprite = btn.transform.GetComponent<UnitBtn>().sprite;
        btnSelect.transform.GetChild(0).GetComponent<Text>().text = btn.transform.GetChild(0).GetComponent<Text>().text;
        btnSelect.GetComponent<UnitBtn>().sprite = btn.transform.GetComponent<UnitBtn>().sprite;
        btnSelect.GetComponent<UnitBtn>().unit = btn.transform.GetComponent<UnitBtn>().unit;
        btnSelect.GetComponent<UnitBtn>().btn = btn;

        selectedUnitPanel.gameObject.SetActive(true);
        btnStart.gameObject.SetActive(true);
        textRight.gameObject.SetActive(true);
    }
    void CancelSelected()
    {
        avableUnitPanel.gameObject.SetActive(false);
        btnCancel.gameObject.SetActive(false);
        textLeft.gameObject.SetActive(false);
        selectedUnitPanel.gameObject.SetActive(true);
        btnStart.gameObject.SetActive(true);
        textRight.gameObject.SetActive(true);
    }
    void PlayLevel()
    {
        for (int i = 0; i < selectedunits.Count; i++)
        {
            if (selectedunits[i].GetComponent<UnitBtn>().unit != null)
            {
                buttonsUnit[i].GetComponent<Image>().sprite = selectedunits[i].GetComponent<UnitBtn>().sprite;
                buttonsUnit[i].transform.GetChild(0).GetComponent<Text>().text = selectedunits[i].transform.GetChild(0).GetComponent<Text>().text;
                buttonsUnit[i].GetComponent<SelectUnitButton>().unit = selectedunits[i].GetComponent<UnitBtn>().unit;
                buttonsUnit[i].GetComponent<SelectUnitButton>().interactable = true;
            }
            else
            {
                buttonsUnit[i].GetComponent<Image>().sprite = ResManager.instance.zeroSlot;
            }
        }
        startLevel.SetActive(false);
    }
    void ToGlobal()
    {
        
        SceneManager.LoadScene("GlobalMap");
    }

    public void SetSpawnPoint(SpawnBtn spoint)
    {
        LevelManager.instance.SetSpawnPoint(spoint);
    }

    public void LevelBossResult()
    {
        levelBossResult = Instantiate(ResManager.instance.levelBossResult, canvas);
        Button принять = levelBossResult.transform.GetChild(0).GetComponent<Button>();
        принять.onClick.AddListener(delegate { Принять(); });
        Text resultText = levelBossResult.transform.GetChild(1).GetComponent<Text>();
        Transform trophysPanel = levelBossResult.transform.GetChild(2);
        List<Transform> trophys = new List<Transform>();
        for (int i = 0; i < trophysPanel.childCount; i++)
        {
            trophys.Add(trophysPanel.GetChild(i));
            trophys[i].gameObject.SetActive(false);
        }
        if (isWinner)
        {
            resultText.text = GameText.VictoryText();
            List<TrophyRes> tRes = Trophy.GetRes(levelName);
            Debug.Log("tResCount: " + tRes.Count);
            List<TrophyRes> tResSort = new List<TrophyRes>();
            for (int i = 0; i < tRes.Count; i++)
            {
                if (tRes[i].count > 0)
                {
                    tResSort.Add(tRes[i]);
                }
            }
            Debug.Log("tResSortCount: " + tResSort.Count);
            for (int i = 0; i < tResSort.Count; i++)
            {
                InfoResources res = GetInfoResources.GetInfo(tResSort[i].num);
                trophys[i].GetChild(0).GetComponent<Image>().sprite = res.img;
                trophys[i].GetChild(1).GetComponent<Text>().text = tResSort[i].count.ToString();
                GameManager.resource[tResSort[i].num] += tResSort[i].count;
                trophys[i].transform.GetComponent<ResBtn>().Name = res.name;
                trophys[i].transform.GetComponent<ResBtn>().message = this;
                trophys[i].gameObject.SetActive(true);
            }
        }
        else
        {
            resultText.text = GameText.LoseText();
        }            
    }

    void Принять()
    {
        Time.timeScale = 1f;
        if(isRoad)
        {
            SceneManager.LoadScene("GlobalMap");
        }
        else
        {
            if (isWinner) SceneManager.LoadScene(levelName + "B");
            else SceneManager.LoadScene("GlobalMap");
        }        
    }

    void CreateMessagePanel()
    {
        messagePanel = Instantiate(ResManager.instance.message, canvas);
        messageText = messagePanel.transform.GetChild(0).transform.GetComponent<Text>();
        messageButton = messagePanel.transform.GetChild(1).transform.GetComponent<Button>();
        messageButton.onClick.AddListener(delegate { CloseMessage(); });
        messagePanel.SetActive(false);
    }

    void CloseMessage()
    {
        messagePanel.SetActive(false);
    }

    public void OpenMessage(string text)
    {
        messageText.text = text;
        messagePanel.SetActive(true);
    }

    public void Send(string s)
    {
        if (messagePanel == null)
        {
            CreateMessagePanel();
        }
        OpenMessage(s);
    }
}
