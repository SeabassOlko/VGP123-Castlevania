using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    //Player Gameplay Variables
    private int _coins;
    private int _health;
    private bool _knife;
    private bool _axe;

    public int coins
    {
        get => _coins;
        set
        {
            _coins += value;
            Debug.Log("Current Coins: " + _coins);
        }
    }
    public int health
    {
        get => _health;
        set 
        {
            if (value <= 0) GameOver();
            if (value > maxHealth) _health = maxHealth;
            _health = value;
            Debug.Log("Current Health: " + _health);
        }
    }
    private void GameOver()
    {
        Debug.Log("You died");
    }

    public bool knife
    {
        get => _knife;
        set
        {
            _knife = value;
            Debug.Log("Knife is set to: " + _knife);
        }
    }
    public bool axe
    {
        get => _axe;
        set
        {
            _axe = value;
            Debug.Log("Axe is set to: " +_axe);
        }
    }

    [SerializeField] private int maxHealth = 10;

    //Movement Variables
    [SerializeField] private int speed;
    [SerializeField] private int jumpForce;

    //Ground check
    [SerializeField] private Transform groundCheck;
    [SerializeField] private bool isGrounded;
    [SerializeField] private LayerMask isGroundLayer;
    [SerializeField] private float groundCheckRadius;

    //Attack variables
    [SerializeField]private Cooldown cooldown;
    private bool canThrow = true;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private Throw thrw;

    // Start is called before the first frame update
    void Start()
    {
        //Set player variables
        _coins = 0;
        _health = 100;
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
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);
        //Get and set movement direction and velocity
        float xInput = Input.GetAxis("Horizontal");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

        if (curPlayingClips.Length > 0)
        {
            if (curPlayingClips[0].clip.name == "Attack")
                rb.velocity = Vector2.zero;
            else if (!Input.GetKey(KeyCode.S))
            {
                Vector2 moveDirection = new Vector2(xInput * speed, rb.velocity.y);
                rb.velocity = moveDirection;
            }
        }
        
        //Player will jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Sprite flipping
        if (xInput != 0)
        {
            sr.flipX = (xInput < 0);
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

        //Set animations to play depending on variables
        anim.SetBool("isAttacking", Input.GetButtonDown("Fire1"));
        anim.SetBool("isShooting", Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire3"));
        anim.SetBool("isDucking", Input.GetKey(KeyCode.S));
        anim.SetFloat("speed", Mathf.Abs(xInput));
        anim.SetBool("isGrounded", isGrounded);
    }
}
