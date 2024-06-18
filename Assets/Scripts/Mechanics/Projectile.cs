using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))]
public class Projectile : MonoBehaviour
{
    public float lifetime;

    //speed value is set by shoot script when the player fires
    [HideInInspector]
    public float xVel;
    [HideInInspector]
    public float yVel;

    [SerializeField] private AudioClip enemyShotClip;
    [SerializeField] private AudioClip playerShotClip;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (lifetime <= 0) lifetime = 2.0f;

        if (CompareTag("Weapon"))
            audioSource.PlayOneShot(playerShotClip);
        if (CompareTag("EnemyProjectile"))
            audioSource.PlayOneShot(enemyShotClip);

        GetComponent<Rigidbody2D>().velocity = new Vector2(xVel, yVel);
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Solid"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy") && CompareTag("Weapon"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(1);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        { 
            Destroy(gameObject);
        }
    }
}
