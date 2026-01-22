using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class mainMenuScipt : MonoBehaviour
{
    public GameObject Main;
    public GameObject LevelSelect;
    public GameObject Settings;
    public GameObject Load;
    public GameObject About;
    public AudioClip back;
    public AudioClip confirm;
    public AudioClip num3;
    public int section = 0;
    public bool audioS = true;

    /*
    0: Title
    1: Level Select
    2: Settings
    3: About
    */

    public void changeSect(int chg)
    {
        section += chg;
        if (chg <= -1)
        {
            GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(back);
        }
        else
        {
            GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(confirm);
        }
        updateScrn();
        Debug.Log($"SECTION CHANGE: {chg}");
    }

    public void changeAudio()
    {
        GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(num3);
        audioS = !audioS;
        if (audioS)
        {
            Settings.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "music: on";
            GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().startMusic();
        }
        else
        {
            Settings.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "music: off";
            GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().stopMusic();
        }
        GameObject.FindGameObjectWithTag("Load").GetComponent<loadScript>().musicOn = audioS;
        Debug.Log($"AUDIO CHANGE");
    }

    public void updateScrn()
    {
        switch (section)
        {
            case 0:
                Main.SetActive(true);
                LevelSelect.SetActive(false);
                Settings.SetActive(false);
                About.SetActive(false);
                Debug.Log($"TURNED ON MAIN");
                break;
            case 1:
                Main.SetActive(false);
                LevelSelect.SetActive(true);
                Settings.SetActive(false);
                About.SetActive(false);
                Debug.Log($"TURNED ON LVL SELECT");
                break;
            case 2:
                Main.SetActive(false);
                LevelSelect.SetActive(false);
                Settings.SetActive(true);
                About.SetActive(false);
                Debug.Log($"TURNED ON SETTINGS");
                break;
            case 3:
                Main.SetActive(false);
                LevelSelect.SetActive(false);
                Settings.SetActive(false);
                About.SetActive(true);
                Debug.Log($"TURNED ON ABOUT");
                break;
        }
    }
}
