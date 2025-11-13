using Unity.VisualScripting;
using UnityEngine;

public class spawnerScript : MonoBehaviour
{
    [SerializeField] private float timer = 0f;
    [SerializeField] private float spawnInterval;
    public float maxHealth;
    public float health;
    public GameObject enemy;

    void Start()
    {
        health = maxHealth;
    } 
    void Update()
    {
        if (!GameObject.FindGameObjectWithTag("pause").GetComponent<pauseScript>().pause)
        {
//            timer += Time.deltaTime;
        }
        if (timer >= spawnInterval)
        {
            Instantiate(enemy, transform.position, Quaternion.identity);
            timer = 0f;
        }
    }
}
