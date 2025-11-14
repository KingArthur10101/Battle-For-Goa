using System;
using System.Collections;
using UnityEngine;

public class pauseScript : MonoBehaviour
{
    public bool pause = true;
    [SerializeField] AudioClip Psnd;
    [SerializeField] AudioClip UPsnd;


    public void Start()
    {
        GameObject.FindGameObjectWithTag("dialouge").GetComponent<dialougeScript>().startDialouge("level1Intro");
    }

    public void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
        {
            StartCoroutine(endGame(true));
        }
        if (!GameObject.FindGameObjectWithTag("Respawn"))
        {
            StartCoroutine(endGame(false));
        }
    }

    public void pauseGame(bool noSound = false)
    {
        pause = true;
        if (!noSound)
        {
            GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(Psnd);        
        }
    }
    public void unpauseGame(bool noSound = false)
    {
        pause = false;
        if (!noSound)
        {
            GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(UPsnd);        
        }
    }
    public void switchPause()
    {
        if (pause)
        {
            unpauseGame();
        }
        else
        {
            pauseGame();
        }
    }

    IEnumerator endGame(bool win)
    {
        yield return new WaitForSeconds(2f);
        string txt;
        if (win){ txt = "level1Win";} else { txt = "level1Lose";}
        GameObject.FindGameObjectWithTag("dialouge").GetComponent<dialougeScript>().startDialouge(txt);        
    }
}
