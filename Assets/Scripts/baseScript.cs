using UnityEngine;

public class baseScript : MonoBehaviour
{

    public GameObject[] units;
    public int maxUnits;
    public GameObject constructing;

    public int money = 0;

    public int maxHealth;
    public int perSecondCash;
    public int level = 1;
    public int health;
    public int exp = 0;
    public int nextExp = 5;
    
    private float timer = 0;
    

    void Start()
    {
        health = maxHealth;
        perSecondCash = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.FindGameObjectWithTag("pause").GetComponent<pauseScript>().pause)
        {
            timer += Time.deltaTime;
        }
        if (timer >= 1)
        {
            timer = 0;
            money += perSecondCash;
        }

    }
}
