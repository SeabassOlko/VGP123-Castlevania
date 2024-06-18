using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    SpriteRenderer sr;

    [Range(0, 10)]
    public float XVel;
    [Range(0, 10)]
    public float YVel;

    public float projectileSpeed;
    public Transform spawnPointLeft;
    public Transform spawnPointRight;

    public Projectile projectilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (projectileSpeed <= 0) projectileSpeed = 7;

        if (!spawnPointRight || spawnPointLeft || !projectilePrefab)
            Debug.Log("Please set default values on the shoot script " + gameObject.name);
    }

    void Fire()
    {
        if (sr.flipX)
        {
            Projectile curProjectile = Instantiate(projectilePrefab, spawnPointRight.position, spawnPointRight.rotation);
            curProjectile.xVel = XVel;
            curProjectile.yVel = YVel;
        }
        else if (!sr.flipX)
        {
            Projectile curProjectile = Instantiate(projectilePrefab, spawnPointLeft.position, spawnPointLeft.rotation);
            curProjectile.xVel = -XVel;
            curProjectile.yVel = YVel;
        }
    }
}
