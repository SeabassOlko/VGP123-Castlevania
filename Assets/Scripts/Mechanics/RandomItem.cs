using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItem : MonoBehaviour
{
    [SerializeField] public GameObject RedBag;
    [SerializeField] public GameObject PurpleBag;
    [SerializeField] public GameObject TanBag;
    [SerializeField] public GameObject Heart;
    [SerializeField] public GameObject Rosary;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Weapon") 
        {
            int ran = Random.Range(0, 5);
            switch (ran)
            {
                case 0:
                    Instantiate(RedBag, gameObject.transform.position, gameObject.transform.rotation);
                    break;
                case 1:
                    Instantiate(PurpleBag, gameObject.transform.position, gameObject.transform.rotation);
                    break;
                case 2:
                    Instantiate(TanBag, gameObject.transform.position, gameObject.transform.rotation);
                    break;
                case 3:
                    Instantiate(Heart, gameObject.transform.position, gameObject.transform.rotation);
                    break;
                case 4:
                    Instantiate(Rosary, gameObject.transform.position, gameObject.transform.rotation);
                    break;
            }
            Destroy(gameObject);
        }
    }
}
