using UnityEngine;

public class spawnerScript : MonoBehaviour
{
    [SerializeField] private float timer = 0f;
    [SerializeField] private float spawnInterval = 2f;
    public GameObject enemy;

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
