using UnityEngine;

public class zoneColliderScript : MonoBehaviour
{
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
