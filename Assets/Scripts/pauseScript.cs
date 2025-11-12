using UnityEngine;

public class pauseScript : MonoBehaviour
{
    public bool pause = true;

    public void pauseGame()
    {
        pause = true;
    }
    public void unpauseGame()
    {
        pause = false;
    }
    public void switchPause()
    {
        pause = !pause;
    }
}
