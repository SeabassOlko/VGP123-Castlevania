using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(AudioSource))]
public abstract class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected SpriteRenderer sr;

    [SerializeField] protected AudioClip deathClip;
    protected AudioSource audioSource;

    protected int health;
    [SerializeField] protected int maxHealth;

    public bool dead = false;

    public virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (maxHealth <= 0) maxHealth = 10;

        health = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0 && !dead)
        {
            dead = true;
            audioSource.PlayOneShot(deathClip);
            anim.SetTrigger("Death");
            float time = anim.GetCurrentAnimatorStateInfo(0).length;
            Destroy(gameObject, time);
        }
    }
}
