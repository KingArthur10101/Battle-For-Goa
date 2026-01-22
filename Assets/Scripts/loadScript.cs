using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loadScript : MonoBehaviour
{
    public bool musicOn;
    public string unlocked = "";
    public GameObject fstButton;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (PlayerPrefs.HasKey("unlocked"))
        {
            unlocked = PlayerPrefs.GetString("unlocked");
            Debug.Log($"SAVE FOUND");
        }
        else
        {
            unlocked = "1";
            PlayerPrefs.SetString("unlocked", unlocked);
            Debug.Log($"SAVE MADE");
        }
        fstButton = GameObject.FindGameObjectWithTag("NOTeditoronly").transform.GetChild(2).GetChild(1).gameObject;
        Debug.Log($"FOUND FSTBUTTON: {fstButton}");
        Debug.Log($"FOUND FSTBUTTON: {fstButton}");
        foreach (char chr in PlayerPrefs.GetString("unlocked"))
        {
            switch (chr){
            case '1':
                fstButton.transform.GetChild(2).GetComponent<Button>().interactable = true;
                break;
            case '2':
                fstButton.GetComponent<Button>().interactable = true;
                break;
            case '3':
                fstButton.transform.GetChild(1).GetComponent<Button>().interactable = true;
                break;
            }
            Debug.Log($"UNLOCKED LVL: {chr}");
        }
    }

    public void startLvl(int lvl)
    {
        fstButton = null;
        SceneManager.LoadScene(lvl);
        Debug.Log($"STARTED LVL: {lvl}");
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
        Debug.Log($"BACK TO MAIN MENU");
    }
}