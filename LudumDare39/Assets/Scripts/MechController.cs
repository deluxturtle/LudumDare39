using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Author: Andrew Seba
/// Description: When in the mech this becomes activated to control the mech.
/// </summary>
public class MechController : MonoBehaviour {

    public float speed = 5f;
    public float hurtAgainDelay = 1f;
    public float hitForce = 300f;
    public float power = 100f;
    public float powerDeductionRate = 4f;

    [Header("AudioSettings")]
    public AudioClip mechOnline;
    public AudioClip mechOutaPower;

    private GameObject mechUI;
    private Slider mechPwrSlider;
    private AudioSource audioSrc;
    private Animator mechAnimator;
    private FacingDirection facingDir = FacingDirection.DOWN;
    private float horizontal = 0;
    private float vertical = 0;
    private bool lockDirection = false;
    private bool canGetHurt = true;
    

    // Use this for initialization
    void Start ()
    {
        
        mechAnimator = GetComponentInChildren<Animator>();
        audioSrc = GetComponent<AudioSource>();
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        GetInput();

        switch (facingDir)
        {
            case FacingDirection.UP:
                mechAnimator.SetInteger("Facing", 0);
                break;
            case FacingDirection.DOWN:
                mechAnimator.SetInteger("Facing", 1);
                break;
            case FacingDirection.LEFT:
                mechAnimator.SetInteger("Facing", 2);
                break;
            case FacingDirection.RIGHT:
                mechAnimator.SetInteger("Facing", 3);
                break;
            default:
                break;
        }


    }

    IEnumerator PowerDeductor()
    {
        while (true)
        {
            //Deduct power
            if (power >= 0)
            {
                power -= powerDeductionRate * Time.deltaTime;
                UpdateMechUI();
            }
            else
            {
                audioSrc.clip = mechOutaPower;
                audioSrc.Play();
                power = 0;
                break;
            }
            yield return null;
        }
    }



    private void FixedUpdate()
    {
        if(power > 0)
            transform.position += new Vector3(horizontal, vertical).normalized * Time.deltaTime * speed;
    }

    private void OnEnable()
    {
        //Grab ui references.
        mechUI = Camera.main.GetComponent<UIHook>().mechPwrPanel;
        mechPwrSlider = Camera.main.GetComponent<UIHook>().mechPwrSlider.GetComponent<Slider>();

        //Enable mech UI
        mechUI.SetActive(true);
        if(power > 0)
        {
            if (audioSrc == null)
            {
                audioSrc = GetComponent<AudioSource>();
            }
            audioSrc.clip = mechOnline;
            audioSrc.Play();
            StartCoroutine("PowerDeductor");
        }
    }

    private void OnDisable()
    {
        StopCoroutine("PowerDeductor");
        mechUI.SetActive(false);
    }

    void UpdateMechUI()
    {
        mechPwrSlider.value = power;
    }


    void GetInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Input.GetKey("right") || horizontal > 0 && !Input.GetKey("left"))
        {
            horizontal = 1;
            if (!lockDirection)
                facingDir = FacingDirection.RIGHT;
        }
        if (Input.GetKey("left") || horizontal < 0 && !Input.GetKey("right"))
        {
            horizontal = -1;
            if (!lockDirection)
                facingDir = FacingDirection.LEFT;
        }
        if (Input.GetKey("up") || vertical > 0 && !Input.GetKey("down"))
        {
            vertical = 1;
            if (!lockDirection)
                facingDir = FacingDirection.UP;
        }
        if (Input.GetKey("down") || vertical < 0 && !Input.GetKey("up"))
        {
            vertical = -1;
            if (!lockDirection)
                facingDir = FacingDirection.DOWN;
        }

        //Button UP
        if (Input.GetButtonUp("Fire1"))
            ResetAttack();
        //Button Down
        if (Input.GetButtonDown("Fire1"))
            Attack();
        if (Input.GetButtonUp("Fire1"))
            lockDirection = false;
    }

    void Attack()
    {
        mechAnimator.SetTrigger("Attack");
        lockDirection = true;
    }

    void ResetAttack()
    {
        lockDirection = false;
    }

    void ResetHurt()
    {
        canGetHurt = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            if (canGetHurt)
            {
                canGetHurt = false;
                Invoke("ResetHurt", hurtAgainDelay);

                //Push back
                Vector2 direction = transform.position - other.transform.position;
                direction = direction.normalized;
                GetComponent<Rigidbody2D>().AddForce(direction * hitForce);
            }
        }
    }
}
