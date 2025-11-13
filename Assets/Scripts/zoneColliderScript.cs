using UnityEngine;

public class zoneColliderScript : MonoBehaviour
{
    void Update()
    {
        if (transform.parent.GetComponent<moveScript>().isAttacking)
        {
            GetComponent<CircleCollider2D>().radius = 0.25f;
        }
        else
        {
            GetComponent<CircleCollider2D>().radius = transform.parent.GetComponent<moveScript>().eyesight;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            transform.parent.gameObject.GetComponent<moveScript>().setTarget(col.gameObject);
        }
        else if (col.gameObject.tag == "Cloud")
        {
            col.gameObject.GetComponent<cloudScript>().removeCloud();
        }
    }
}
