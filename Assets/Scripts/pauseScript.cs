using UnityEngine;

public class pauseScript : MonoBehaviour
{
    public bool pause = true;
    [SerializeField] AudioClip Psnd;
    [SerializeField] AudioClip UPsnd;

    public void pauseGame()
    {
        pause = true;
        GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(Psnd);
    }
    public void unpauseGame()
    {
        pause = false;
        GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(UPsnd);
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
}
