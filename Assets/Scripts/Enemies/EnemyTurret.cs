using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : Enemy
{
    [SerializeField] private float projectileFireRate;
    [SerializeField] private float range;
    private float timeSinceLastFire = 0;
    private bool left;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        if (range <= 0) range = 2;
        if (projectileFireRate <= 0) projectileFireRate = 2;
    }

    // Update is called once per frame
    void Update()
    {
        sr.flipX = (GameManager.Instance.PlayerInstance.transform.position.x < transform.position.x) ? false : true;

        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);
        if (curPlayingClips[0].clip.name == "Idle" && Vector3.Distance(GameManager.Instance.PlayerInstance.transform.position, transform.position) <= range)
        {
            if (Time.time >= timeSinceLastFire + projectileFireRate)
            {
                anim.SetTrigger("Fire");
                timeSinceLastFire = Time.time;
            }
        }
    }
}
