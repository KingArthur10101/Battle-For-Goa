using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class baseScript : MonoBehaviour
{
    private Dictionary<string, float> timeToBuild = new Dictionary<string, float>
    {
        {"Normal", 5},
        {"Fast", 2},
        {"Tank", 7}
    };
    public GameObject[] unitsToBuild;
    [SerializeField] private AudioClip explode;
    [SerializeField] private GameObject fire;
    public List<GameObject> units = new List<GameObject>();
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
        if (health <= 0)
        {
            die();
        }
    }

    public void damage(int dmg)
    {
        health -= dmg;
    }

    public void die()
    {
        GameObject.FindGameObjectWithTag("audioManager").GetComponent<soundScript>().playClip(explode);
        StartCoroutine(dieAnimation());

    }
    IEnumerator dieAnimation()
    {
        List<GameObject> fires = new List<GameObject>();
        GameObject fire_ = Instantiate(fire, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), Quaternion.identity);
        fires.Append(fire_);

        yield return new WaitForSeconds(0.25f);


        for (int i = 0; i < 4; i++)
        {
            fire_ = Instantiate(fire, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), Quaternion.identity);
            fires.Append(fire_);

            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(0.7f);

        foreach (GameObject fire in fires) {
            Destroy(fire);
        }
    }

}
