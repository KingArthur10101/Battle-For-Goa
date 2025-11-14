using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dialougeScript : MonoBehaviour
{
    public int dialAdd = -1;
    public string dialName = "";
    [SerializeField] private AudioClip snd;

    Dictionary<string, string[]> dialouge = new Dictionary<string, string[]>()
    {
        {"level1Intro", new string[]
        {"   My name is Stom, I will guide you through this first mission.",
        "    This is my second, and final pane of text. PP POOPOO"}
        },
        {"level1Win", new string[]
        {"Congrats! First level down!",
        "Now we can move on to level 2!",
        "Try it from the title screen!"}
        },
        {"level1Lose", new string[]
        {"We Lost!\n It's alright, we can try again from the title screen.",
        "Hopefully SOMEone will try harder... next time..."}
        }
    };
    public void startDialouge(string dial)
    {
        GameObject.FindGameObjectWithTag("pause").GetComponent<pauseScript>().pauseGame(true);
        GameObject.FindGameObjectWithTag("GameController").transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = dialouge[dial][0];
        GameObject.FindGameObjectWithTag("GameController").transform.GetChild(0).gameObject.SetActive(false);
        GameObject.FindGameObjectWithTag("GameController").transform.GetChild(1).gameObject.SetActive(true);
        GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(snd);
        dialAdd = 0;
        dialName = dial;
    }

    public void nextDialouge()
    {
        if (dialAdd == -1)
        {
            Debug.Log("Error Accessing Dialouge");
        }
        else if (dialAdd < dialouge[dialName].Length - 1)
        {
            dialAdd += 1;
            GameObject.FindGameObjectWithTag("GameController").transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = dialouge[dialName][dialAdd];
        }
        else
        {
            endDialouge();
        }
    }

    public void endDialouge()
    {
        dialAdd = -1;
        dialName = "";
        GameObject.FindGameObjectWithTag("GameController").transform.GetChild(0).gameObject.SetActive(true);
        GameObject.FindGameObjectWithTag("GameController").transform.GetChild(1).gameObject.SetActive(false);
        GameObject.FindGameObjectWithTag("pause").GetComponent<pauseScript>().unpauseGame(true);
    }

}
