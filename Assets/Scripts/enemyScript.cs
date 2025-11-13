using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    public List<GameObject> targetTroops = new List<GameObject>();
    public GameObject deathParticles;
    UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private Transform goal;
    [SerializeField] private float attackTimer = 0f;
    [SerializeField] private float attackTimerMax;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private bool alive = true;
    [SerializeField] private int damage;
    [SerializeField] private float moveSpeed;

    public int maxHealth;
    public int health;

    public Animator animator;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float attackDistance;

    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetTroops = GameObject.FindGameObjectsWithTag("Player").ToList();
        goal = findTarg(targetTroops);
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;
    }

    // Update is called once per frame
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

        if(alive && health <= 0)
        {
            die();
        }
        if (goal == null)
        {
            goal = findTarg(targetTroops);
        }
        targetTroops = GameObject.FindGameObjectWithTag("GameController").GetComponent<canvasScript>().units;
        Color currentColor = spriteRenderer.color;
        currentColor.b = Mathf.Lerp(1f, 0f, attackTimer / attackTimerMax);
        currentColor.g = Mathf.Lerp(1f, 0f, attackTimer / attackTimerMax);
        spriteRenderer.color = currentColor;

        if (alive && goal != null) { 
            agent.SetDestination(goal.position);
            animator.SetFloat("xMove", agent.velocity.normalized.x);
            animator.SetFloat("yMove", agent.velocity.normalized.y);        
        }

        if (goal != null && Vector2.Distance(transform.position, goal.position) <= attackDistance)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }

        if (isAttacking)
        {
            if (!GameObject.FindGameObjectWithTag("pause").GetComponent<pauseScript>().pause)
            {
                attackTimer += Time.deltaTime;
            }
        }
        else
        {
            attackTimer = 0f;
        }
        if (attackTimer >= attackTimerMax)
        {
            attackTimer = 0f;
            if (goal != null)
            {

                if (goal.gameObject.GetComponent<moveScript>() != null)
                {
                    goal.gameObject.GetComponent<moveScript>().takeDamage(damage);
                    GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(hitSound, true);
                }
                if (goal.gameObject.GetComponent<baseScript>() != null)
                {
                    goal.gameObject.GetComponent<baseScript>().damage(damage);
                    GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(hitSound, true);
                }
            }
        }
    }
    private void die()
    {
        alive = false;
        isAttacking = false;
        attackTimer = 0;
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

    public Transform findTarg(List<GameObject> targetTroops_)
    {
        float minDistance = Mathf.Infinity;
        Transform goal_ = GameObject.FindGameObjectWithTag("Respawn").transform;
        foreach (GameObject i in targetTroops_)
        {
            if (Vector2.Distance(transform.position, i.transform.position) < minDistance)
            {
                goal_ = i.transform;
                minDistance = Vector2.Distance(transform.position, i.transform.position);
            }
        }
        return goal_;
    }
    public void takeDamage(int dmg)
    {
        health -= dmg;
    }
}
