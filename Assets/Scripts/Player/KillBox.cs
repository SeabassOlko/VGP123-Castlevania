using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    bool canAttack;
    // Start is called before the first frame update
    void Start()
    {
        canAttack = false;
    }
    public void attack()
    {
        canAttack = true;
    }
    public void finishAttack()
    {
        canAttack = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (canAttack) 
        { 
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(1);
            }
            else if (collision.gameObject.CompareTag("Environment"))
            {
                collision.gameObject.GetComponent<RandomItem>().spawnRandomItem();
            }
        }
    }
}
