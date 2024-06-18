using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    //Player Gameplay Variables
    private bool _knife;
    private bool _axe;
    private bool hurt;

    public bool knife
    {
        get => _knife;
        set
        {
            _knife = value;
            onKnifeValueChange?.Invoke(_knife);
            Debug.Log("Knife is set to: " + _knife);
        }
    }
    public bool axe
    {
        get => _axe;
        set
        {
            _axe = value;
            onAxeValueChange?.Invoke(_axe);
            Debug.Log("Axe is set to: " +_axe);
        }
    }

    public Action<bool> onAxeValueChange;
    public Action<bool> onKnifeValueChange;

    //Movement Variables
    [SerializeField] private int speed;
    [SerializeField] private int jumpForce;
    [SerializeField] private int knockback;

    //Ground check
    [SerializeField] private Transform groundCheck;
    [SerializeField] private bool isGrounded;
    [SerializeField] private LayerMask isGroundLayer;
    [SerializeField] private float groundCheckRadius;

    //Attack variables
    [SerializeField]private Cooldown cooldown;

    //Player hit box variables
    [SerializeField] private KillBox killBoxRight;
    [SerializeField] private KillBox killBoxLeft;

    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip hurtClip;
    [SerializeField] private AudioClip collectClip;
    [SerializeField] private AudioClip whipClip;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private Throw thrw;
    private AudioSource audioSource;

    private bool _paused = false;

    public bool paused
    {
        get => _paused;
        set => _paused = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //Set player variables
        _knife = false;
        _axe = false;

        //Set cooldown
        cooldown.startCooldown();

        if (speed <= 0)
        {
            speed = 5;
            Debug.Log("Forgot to set speed");
        }

        if (jumpForce <= 0)
        {
            jumpForce = 5;
            Debug.Log("Forgot to set jumpForce");
        }

        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.2f;
            Debug.Log("Forgot to set GroundCheckRadius");
        }
        if (groundCheck == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("GroundCheck");
            if (obj != null)
            {
                groundCheck = obj.transform;
                return;
            }
            GameObject newObj = new GameObject();
            newObj.transform.SetParent(transform);
            newObj.transform.localPosition = Vector3.zero;
            newObj.name = "GroundCheck";
            newObj.tag = newObj.name;
            groundCheck = newObj.transform;
            Debug.Log("GroundCheck was made using code, look like you forgot to make a GroundCheck object");
        }
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        thrw = GetComponent<Throw>();
    }

    // Update is called once per frame
    void Update()
    {
        if (paused) return;


        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);
        //Get and set movement direction and velocity
        float xInput = Input.GetAxis("Horizontal");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

        if (curPlayingClips.Length > 0)
        {
            if (!hurt)
            {
                if (curPlayingClips[0].clip.name == "Attack")
                    rb.velocity = Vector2.zero;
                else if (!Input.GetKey(KeyCode.S))
                {
                    Vector2 moveDirection = new Vector2(xInput * speed, rb.velocity.y);
                    rb.velocity = moveDirection;
                }
            }
        }
        
        //Player will jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            audioSource.PlayOneShot(jumpClip);
        }

        // Sprite flipping
        if (xInput != 0)
        {
            sr.flipX = !(xInput < 0);
        }

        if (Input.GetKeyDown(KeyCode.I)) 
        {
            Debug.Log("The Cooldown statement: " + cooldown.isCoolingDown);
        }

        if (Input.GetButtonDown("Fire3") && _knife && cooldown.isCoolingDown)
        {
            thrw.type = Throw.ThrowableType.Knife;
            thrw.Fire();
            anim.SetBool("isShooting", Input.GetButtonDown("Fire3"));
            cooldown.startCooldown();

        }else if (Input.GetButtonDown("Fire2") && _axe && cooldown.isCoolingDown)
        {
            thrw.type = Throw.ThrowableType.Axe;
            thrw.Fire();
            anim.SetBool("isShooting", Input.GetButtonDown("Fire2"));
            cooldown.startCooldown();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            GameManager.Instance.health--;
        }

        //Set animations to play depending on variables

        anim.SetBool("isAttacking", Input.GetButtonDown("Fire1"));
        anim.SetBool("isShooting", Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire3"));
        anim.SetBool("isDucking", Input.GetKey(KeyCode.S));
        anim.SetFloat("speed", Mathf.Abs(xInput));
        anim.SetBool("isGrounded", isGrounded);
        
    }

    public void KillBox()
    {
        if (!sr.flipX)
            killBoxRight.attack();
        else
            killBoxLeft.attack();
    }

    public void KillBoxEnd()
    {
        killBoxLeft.finishAttack();
        killBoxRight.finishAttack();
    }

    public void notHurt()
    {
        anim.SetBool("Hurt", false);
        hurt = false;
    }

    public void hit()
    {
        audioSource.PlayOneShot(hurtClip);
    }

    public void collect()
    {
        audioSource.PlayOneShot(collectClip);
    }
    
    public void whip()
    {
        audioSource.PlayOneShot(whipClip);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<Enemy>().dead) return;
            hurtCheck(collision);
        }
        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            hurtCheck(collision);
        }
    }

    private void hurtCheck(Collision2D collision)
    {
        hurt = true;
        anim.SetBool("Hurt", true);
        GameManager.Instance.health--;
        if (rb.transform.position.x < collision.transform.position.x)
        {
            rb.velocity = Vector3.zero;
            rb.velocity = Vector2.left * knockback;
            rb.velocity += Vector2.up * knockback;
        }
        else
        {
            rb.velocity = Vector3.zero;
            rb.velocity = Vector2.right * knockback;
            rb.velocity += Vector2.up * knockback;
        }
    }
}
