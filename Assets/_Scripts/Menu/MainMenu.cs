using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text play;
    public Text settings;
    public Text aboutUs;
    public Text exit;
    public Text outTime;


    // Start is called before the first frame update
    void Start()
    {
        SetLenguage();
        if (!GameManager.isFirstLoad)
        {
            if (PlayerPrefs.HasKey("Save"))
            {
                GameManager.isFirstLoad = true;
                GameManager.instance.LoadGame();
                Debug.Log("Data: " + GameManager.instance.save.saveData);
                TimeSpan ts = DateTime.Now - GameManager.instance.save.saveData;
                TimeSpan tsOut = new TimeSpan(0, 0, (int)ts.TotalSeconds);

                outTime.text = "" + tsOut;
            }
            else
            {
                GameManager.isFirstLoad = true;
                GameManager.instance.FirstGame();
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Town");
    }

    public void ExitGame()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Application.Quit();
#else
        Application.Quit();
#endif
    }

    void SetLenguage()
    {
        if (GameManager.instance.isRussian)
        {
            play.text = "Играть";
            settings.text = "Настройки";
            aboutUs.text = "О нас";
            exit.text = "Выход";
        }
        else
        {
            play.text = "Play";
            settings.text = "Settings";
            aboutUs.text = "About us";
            exit.text = "Exit";
        }
    }

    public void ChangeLaguage()
    {
        GameManager.instance.isRussian = !GameManager.instance.isRussian;
        if (GameManager.instance.isRussian)
        {
            PlayerPrefs.SetString("Language", "ru_RU");
        }
        else
        {
            PlayerPrefs.SetString("Language", "en_US");
        }
        SetLenguage();
    }

    public void SaveButton()
    {
        GameManager.instance.SaveGame();
    }
}
