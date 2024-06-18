using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(BoxCollider2D))]

public class PickUp : MonoBehaviour
{
    public enum PickupType
    {
        Health,
        RedBag,
        PurpleBag,
        TanBag,
        Axe,
        Knife,
        Rosary
    }

    [SerializeField] private PickupType type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            PlayerController pc = collision.GetComponent<PlayerController>();
            pc.collect();
            switch (type)
            {
                case PickupType.Health:
                    GameManager.Instance.health++;
                    break;
                case PickupType.RedBag:
                    GameManager.Instance.coins = 50;
                    break;
                case PickupType.PurpleBag:
                    GameManager.Instance.coins = 100;
                    break;
                case PickupType.TanBag:
                    GameManager.Instance.coins = 250;
                    break;
                case PickupType.Axe:
                    pc.axe = true;
                    break;
                case PickupType.Knife:
                    pc.knife = true;
                    break;
                case PickupType.Rosary:
                    Debug.Log("Clear all enemies on screen");
                    break;
            }
            Destroy(gameObject);
        }
    }
}
