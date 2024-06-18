using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PickUp;

public class Throw : MonoBehaviour
{
    public enum ThrowableType
    {
        Axe,
        Knife
    }

    [SerializeField] private ThrowableType _type;

    public ThrowableType type
    {
        get => _type;
        set => _type = value;
    }

    SpriteRenderer sr;

    [Range(0, 10)]
    public float knifeXVel;
    [Range(0, 10)]
    public float knifeYVel;

    [Range(0, 10)]
    public float axeXVel;
    [Range(0, 10)]
    public float axeYVel;

    public Transform spawnPointLeft;
    public Transform spawnPointRight;

    public Projectile knifePrefab;
    public Projectile AxePrefab;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (knifeXVel == 0 && knifeYVel == 0) knifeXVel = 7.0f;
        if (axeXVel == 0 && axeYVel == 0) axeXVel = 7.0f;

        if (!spawnPointRight || !spawnPointLeft || !knifePrefab || !AxePrefab)
            Debug.Log("Please set default values on the shoot script " + gameObject.name);
    }

    public void Fire()
    {
        switch (type)
        {
            case ThrowableType.Knife:
                {
                    if (!sr.flipX)
                    {
                        Projectile curProjectile = Instantiate(knifePrefab, spawnPointRight.position, spawnPointRight.rotation);
                        curProjectile.xVel = -knifeXVel;
                        curProjectile.yVel = knifeYVel;
                    }
                    else
                    {
                        Projectile curProjectile = Instantiate(knifePrefab, spawnPointLeft.position, spawnPointLeft.rotation);
                        curProjectile.xVel = knifeXVel;
                        curProjectile.yVel = knifeYVel;
                    }
                    break;
                }
            case ThrowableType.Axe:
                {
                    if (!sr.flipX)
                    {
                        Projectile curProjectile = Instantiate(AxePrefab, spawnPointRight.position, spawnPointRight.rotation);
                        curProjectile.xVel = -axeXVel;
                        curProjectile.yVel = axeYVel;
                        curProjectile.GetComponent<Animator>().SetBool("right", true);
                    }
                    else
                    {
                        Projectile curProjectile = Instantiate(AxePrefab, spawnPointLeft.position, spawnPointLeft.rotation);
                        curProjectile.xVel = axeXVel;
                        curProjectile.yVel = axeYVel;
                    }
                    break;
                }
        }
        

    }
}
