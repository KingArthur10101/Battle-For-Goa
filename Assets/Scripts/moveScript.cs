using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class moveScript : MonoBehaviour
{

    public bool alive = true;
    public GameObject deathParticles;
    public GameObject goal;
    public GameObject goalPrefab;
    NavMeshAgent agent;
    public int lvl = 1;
    public int maxHealth;
    public int health;
    public float attackSpeed;
    public int exp = 0;
    public int nextExp = 5;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private int damage;
    [SerializeField] private float moveSpeed;
    public float eyesight;
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackTimer = 0f;
    public bool isAttacking = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.stoppingDistance = 2f;
        transform.GetChild(0).GetComponent<CircleCollider2D>().radius = eyesight;
    }
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("pause").GetComponent<pauseScript>().pause)
        {
            agent.speed = 0f;
        }
        else
        {
            agent.speed = moveSpeed;
        }

        if (goal)
        {
            agent.SetDestination(goal.transform.position);
        }
        else
        {
            agent.SetDestination(transform.position);
        }
        if (health <= 0)
        {
            die();
        }
        if (goal != null && Vector2.Distance(transform.position, goal.transform.position) <= attackDistance)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }
        if (isAttacking)
        {
            if (!GameObject.FindGameObjectWithTag("pause").GetComponent<pauseScript>().pause) { 
                attackTimer += Time.deltaTime;
            }
        }
        else
        {
            attackTimer = 0f;
        }
        if (attackTimer >= attackSpeed)
        {
            attackTimer = 0f;
            if (goal != null)
            {

                if (goal.gameObject.GetComponent<enemyScript>() != null)
                {
                    goal.gameObject.GetComponent<enemyScript>().takeDamage(damage);
                    GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(hitSound, true);
                }
            }
        }
        agent.stoppingDistance = (goal != null && goal.tag == "targPrefab") ? 0f : 2f;
    }

    public void setTarget(GameObject newTarg)
    {
        if (goal && goal.tag == "targPrefab")
        {
            Destroy(goal);
        }
        goal = newTarg;
    }

    public void setTarget(Vector3 newTarg)
    {
        if(goal && goal.tag == "targPrefab")
        {
            Destroy(goal);
        }
        goal = Instantiate(goalPrefab, newTarg, Quaternion.identity);
    }

    public void takeDamage(int dmg)
    {
        health -= dmg;
    }

    private void die()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<canvasScript>().units.Remove(this.gameObject);
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<canvasScript>().go == this.gameObject) {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<canvasScript>().go = null;
        }
        alive = false;
        isAttacking = false;
        Component[] comps = GetComponents(typeof(Component));
        agent.SetDestination(transform.position);
        foreach (Component c in comps)
        {
            if (c.GetType() != typeof(Transform) && c.GetType() != typeof(SpriteRenderer) && c.GetType() != typeof(enemyScript))
            {
                Behaviour behC = (Behaviour)c;
                behC.enabled = false;
            }
        }
        StartCoroutine(dieAnimation());
    }
    IEnumerator dieAnimation()
    {
        transform.Translate(Vector3.left * 0.1f);
        yield return new WaitForSeconds(0.05f);

        for (int i = 0; i < 3; i++)
        {
            transform.Translate(Vector3.right * 0.2f);
            yield return new WaitForSeconds(0.05f);
            transform.Translate(Vector3.left * 0.2f);
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.05f);
        transform.Translate(Vector3.right * 0.1f);

        GetComponent<SpriteRenderer>().enabled = false;
        GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        Destroy(particles);
        Destroy(gameObject);
    }
}