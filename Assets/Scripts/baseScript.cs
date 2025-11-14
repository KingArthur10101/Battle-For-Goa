using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting.ReorderableList;
using Unity.VisualScripting;

public class baseScript : MonoBehaviour
{
    public GameObject[] unitsToBuild;
    [SerializeField] private AudioClip explode;
    [SerializeField] private GameObject fire;
    public List<GameObject> units = new List<GameObject>();
    [SerializeField] private float constructionTimer;
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

    private bool alive = true;
    

    void Start()
    {
        health = maxHealth;
        perSecondCash = 10;
        constructionTimer = 0f;
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
        if (alive && health <= 0)
        {
            die();
        }
        if (constructing && constructionTimer >= constructing.GetComponent<moveScript>().timeToBuild)
        {
            constructionTimer = 0f;
            money -= constructing.GetComponent<moveScript>().costToBuild;
            GameObject newUnit = Instantiate(constructing, transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f)), Quaternion.identity);
            units.Add(newUnit);
        }
        if (constructing)
        {
            if (money >= constructing.GetComponent<moveScript>().costToBuild)
            {                
                constructionTimer += Time.deltaTime;
            }
            else
            {
                constructing = null;
            }
        }
        else
        {
            constructionTimer = 0f;
        }
    }

    public void damage(int dmg)
    {
        health -= dmg;
    }

    public void die()
    {
        alive = false;
        GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(explode);
        StartCoroutine(dieAnimation());

    }
    IEnumerator dieAnimation()
    {
        List<GameObject> fires = new List<GameObject>();

        for (int i = 0; i < 5; i++)
        {
            GameObject fire_ = Instantiate(fire, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), Quaternion.identity);
            fires.Add(fire_);

            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(0.7f);

        foreach (GameObject fire in fires) {
            Destroy(fire);
        }
    }

}
