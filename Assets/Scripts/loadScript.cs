using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadScript : MonoBehaviour
{
    public bool musicOn;
    public float zoomSpeed;
    public float minZoom;
    public float maxZoom;
    public float panSpeed;
    public float arrowsPanSpeed;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void startLvl(int lvl)
    {
        SceneManager.LoadScene(lvl);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<canvasScript>().setSettings();
        Debug.Log($"STARTED LVL: {lvl}");
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
        Debug.Log($"BACK TO MAIN MENU");
    }

    public void changeSetting(int numSetting, bool upDown){

        string[] settings = new string[] {"musicVol", "zoomSpeed", "panSpeed", "arrowPanSpeed"};
        float[] settingsMins = new float[]{0,          2,           2,          2};
        float[] settingsMaxs = new float[]{10,         5,           5,          5};
        float[] settingsAdj = new float[] {1,          1,           1,          1};

        if (PlayerPrefs.HasKey(settings[numSetting]))
        {
            if (upDown)
            {
                // SET SETTINGS TO TEMP VALUE AND ADD FUNCTION TO APPLY THOSE TO PLAYERPREF VALUES
                Mathf.Clamp(PlayerPrefs.GetFloat(settings[numSetting]) + settingsAdj[numSetting], settingsMins[numSetting], settingsMaxs[numSetting]);
            }
            else
            {
                Mathf.Clamp(PlayerPrefs.GetFloat(settings[numSetting]) - settingsAdj[numSetting], settingsMins[numSetting], settingsMaxs[numSetting]);                
            }
        }
        else
        {
            Debug.Log("Never seen setting: "+settings[numSetting]);
            return;
        }
    }

    public void finishLevel()
    {
        if(!PlayerPrefs.HasKey("unlocked")) return;
        if(PlayerPrefs.GetInt("unlocked") == SceneManager.GetActiveScene().buildIndex)
        {
            PlayerPrefs.SetInt("unlocked", PlayerPrefs.GetInt("unlocked")+1);
        }
        mainMenu();
    }
}