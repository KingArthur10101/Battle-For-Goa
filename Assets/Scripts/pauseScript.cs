using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseScript : MonoBehaviour
{
    public bool pause = true;
    public bool over = false;
    [SerializeField] private AudioClip Psnd;
    [SerializeField] private AudioClip UPsnd;
    [SerializeField] private AudioClip WW;
    [SerializeField] private AudioClip LL;


    public void Start()
    {
        GameObject.FindGameObjectWithTag("dialouge").GetComponent<dialougeScript>().startDialouge($"level{SceneManager.GetActiveScene().buildIndex}Intro");
    }

    public void Update()
    {
        if (!over)
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
    }

    public void pauseGame(bool noSound = false)
    {
        pause = true;
        if (!noSound)
        {
            GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(Psnd);        
        }
        if (GameObject.FindGameObjectWithTag("Load").GetComponent<loadScript>().musicOn)
        {
            GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().stopMusic();
        }
    }
    public void unpauseGame(bool noSound = false)
    {
        pause = false;
        if (!noSound)
        {
            GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(UPsnd);        
        }
        if (GameObject.FindGameObjectWithTag("Load").GetComponent<loadScript>().musicOn)
        {
            GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().startMusic();
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
        over = true;
        pauseGame(true);
        if (win)
        {
            PlayerPrefs.SetString("unlocked", PlayerPrefs.GetString("unlocked") + (SceneManager.GetActiveScene().buildIndex+1));
            GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(WW);
            yield return new WaitForSeconds(2f);
            GameObject.FindGameObjectWithTag("dialouge").GetComponent<dialougeScript>().startDialouge($"level{SceneManager.GetActiveScene().buildIndex}Win");
        }
        else
        {
            GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(LL);
            yield return new WaitForSeconds(2f);
            GameObject.FindGameObjectWithTag("dialouge").GetComponent<dialougeScript>().startDialouge($"level{SceneManager.GetActiveScene().buildIndex}Lose");
        }

    }
}
