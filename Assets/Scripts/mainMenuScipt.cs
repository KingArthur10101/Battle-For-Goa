using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainMenuScipt : MonoBehaviour
{
    public GameObject Main;
    public GameObject LevelSelect;
    public GameObject Settings;
    public GameObject About;

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
        updateScrn();
    }

    public void changeAudio()
    {
        audioS = !audioS;
        if (audioS)
        {
            Settings.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "audio: on";
        }
        else
        {
            Settings.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "audio: off";
        }
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
                break;
            case 1:
                Main.SetActive(false);
                LevelSelect.SetActive(true);
                Settings.SetActive(false);
                About.SetActive(false);
                break;
            case 2:
                Main.SetActive(false);
                LevelSelect.SetActive(false);
                Settings.SetActive(true);
                About.SetActive(false);
                break;
            case 3:
                Main.SetActive(false);
                LevelSelect.SetActive(false);
                Settings.SetActive(false);
                About.SetActive(true);
                break;

        }
    }

    public void updateDialouge()
    {
        
    }

}
