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
        {"Hello, young commander. Welcome to your first battle! There are some crucial instructions for you to follow in order to succeed.",
        "Firstly, when in dialouge or paused, the game is not running, meaning time is not of the essence, as of now.",
        "When in a battle, use the Heads-Up-Display to view your current funds, health of home base, and base level.",
        "Upon clicking this, one can expand or contract the extra details pane, showing 5 important details.",
        "$ per second, base health, EXP to next base level, currently construcing unit, units on the board.",
        "Also in this menu is the PAUSE button, accessible at any time.",
        "Click on units or buildings in game to view them, showing stats of your units, and only the health of enemies.",
        "Upon clicking off a unit you own, the location or unit selected will become the target of that unit.",
        "However, similar to the Heads-Up-Display, the extra details pane can be expanded or contracted by clicking the title of the unit.",
        "On a unit, click retreat to have it return to your base, and on a building, choose what unit you'd like to construct next.",
        "Finally, the minimap in the bottom right can be expanded or contracted using the button tied to it.",
        "Good luck! Find and exterminate all enemy troops and buildings from the board and victory will be yours!"}
        },
        {"level1Win", new string[]
        {"Congrats! First level down!",
        "Now we can move on to level 2!",
        "Try it from the title screen!"}
        },
        {"level1Lose", new string[]
        {"We Lost!\n It's alright, we can try again from the title screen.",
        "Hopefully SOMEone will try harder... next time..."}
        },
        {"level2Intro", new string[]
        {"Welcome back, my lord! A second mission has presented itsself.",
        "Now, many of a similar sort of enemy generator are attacking us from many directions!",
        "Be sure to manage your resources well! Good luck!"}
        },
        {"level2Win", new string[]
        {"Congratulations! The castle is saved once more!",
        "Now, we must take the upper hand, and attack this evil where it originates from!"}
        },
        {"level2Lose", new string[]
        {"My lord! We have failed!",
        "Please forgive us, as we may always try again!"}
        },
        {"level3Intro", new string[]
        {"Ahh, and now we are faced with the final battle! A test of the might of both the armies of light and evil!",
        "May the sun shine bright on the victor, and may the kingdom of Draygonia reign on forever!"}
        },
        {"level3Win", new string[]
        {"You Beat the game!\nthanks for playing!\nthis is only they demo :/\ncome back when its actually finished!"}
        },
        {"level3Lose", new string[]
        {"My lord! We have failed!",
        "Please forgive us, as we may always try again!"}
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
            endDialouge();
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
        if (GameObject.FindGameObjectWithTag("pause").GetComponent<pauseScript>().over)
        {
            GameObject.FindGameObjectWithTag("Load").GetComponent<loadScript>().mainMenu();
        }
    }

}
