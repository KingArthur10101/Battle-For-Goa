using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class spawnerScript : MonoBehaviour
{
    [SerializeField] private float timer = 0f;
    [SerializeField] private float spawnInterval;
    [SerializeField] private bool alive = true;
    [SerializeField] private GameObject fire;
    public float maxHealth;
    public float health;
    public GameObject enemy;

    void Start()
    {
        health = maxHealth;
    } 
    void Update()
    {
        if (alive && !GameObject.FindGameObjectWithTag("pause").GetComponent<pauseScript>().pause)
        {
//            timer += Time.deltaTime;
        }
        if (timer >= spawnInterval)
        {
            Instantiate(enemy, transform.position, Quaternion.identity);
            timer = 0f;
        }
        if (alive && health <= 0)
        {
            die();
        }
    }

    public void takeDamage(int damg)
    {
        health -= damg;
    }

    private void die()
    {
        alive = false;
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
            print(fire);
            Destroy(fire);
        }
        Destroy(gameObject);
    }


}
