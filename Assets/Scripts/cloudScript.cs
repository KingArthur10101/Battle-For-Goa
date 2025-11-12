using System.Collections;
using UnityEngine;

public class cloudScript : MonoBehaviour
{
    [SerializeField] private float timer;
    [SerializeField] private float deathTime;
    private bool isDying;
    void Start()
    {
        isDying = false;
        deathTime += Random.Range(-1f, 1f);
    }
    void Update()
    {
        if (isDying)
        {
            timer += Time.deltaTime;
        }
        Color currentColor = gameObject.GetComponent<SpriteRenderer>().color;
        currentColor.a = Mathf.Lerp(1f, 0f, timer / deathTime);
        GetComponent<SpriteRenderer>().color = currentColor;
    }
    public void removeCloud()
    {
        isDying = true;
        StartCoroutine(cloudRemovalServiceNice());
    }
    IEnumerator cloudRemovalServiceNice()
    {
        
        yield return new WaitForSeconds(deathTime+0.01f);

        Destroy(gameObject);
    }
}
