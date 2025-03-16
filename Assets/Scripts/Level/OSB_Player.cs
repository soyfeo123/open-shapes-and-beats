using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OSB_Player : MonoBehaviour
{
    float p_targetDirection;
    float p_direction;

    Vector3 p_targetScale = new Vector3();
    Vector3 p_scale = new Vector3();

    public Vector2 p_targetVel = new Vector2();
    public Vector3 p_vel = new Vector3();

    Rigidbody2D rb;

    bool movesDuringTheFrame = false;
    bool movesChange = false;


    bool canDash = true;
    bool isInDash = false;
    int dashCooldownCurrentFrame;
    
    int dashCooldownTimeFrames;

    [Header("Particles")]
    public ParticleSystem MovementParticles;
    public ParticleSystem DashParticles;
    public ParticleSystem CircleParticles;
    public ParticleSystem HitParticles;

    [Header("GameObjects")]
    public GameObject VisualObj;

    [Header("Player Settings")]
    public float Speed = 10;
    public float dashCooldownTime = 0.34f;
    public float dashDuration = 0.18f;
    public float DamageCooldown = 1f;
    int framesDuringCooldownDamage;
    int currentDmgCooldownFrame = 0;
    Color defaultColor;

    [Header("Changed during Runtime Variables")]
    public bool IsInDamageCooldown;
    int effectFrame;
    bool isMagenta = false;

    [Header("Debug")]
    public bool DebugEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultColor = VisualObj.GetComponent<SpriteRenderer>().color;

        dashCooldownTimeFrames = Mathf.FloorToInt(dashCooldownTime * 50);
        framesDuringCooldownDamage = Mathf.FloorToInt(DamageCooldown * 50);
    }

    /*void KeyboardMovement()
    {
        movesDuringTheFrame = false;
        List<float> directions = new List<float>();
        if (Input.GetKey(KeyCode.W))
        {
            directions.Add(0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            directions.Add(90);
        }
        if (Input.GetKey(KeyCode.S))
        {
            directions.Add(180);
        }
        if (Input.GetKey(KeyCode.D))
        {
            directions.Add(270);
        }

        if(directions.Count <= 0)
        {
            p_targetDirection = 0;
            return;
        }
        movesDuringTheFrame = true;
        float total = 0;
        foreach(float dir in directions)
        {
            total += dir;
        }
        total = total / directions.Count;
        p_targetDirection = total;
        
    }*/
    void KeyboardMovement()
    {
        movesDuringTheFrame = false;
        Vector2 movement = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            movement += Vector2.up; // (0,1)
        if (Input.GetKey(KeyCode.A))
            movement += Vector2.right; // (-1,0)
        if (Input.GetKey(KeyCode.S))
            movement += Vector2.down; // (0,-1)
        if (Input.GetKey(KeyCode.D))
            movement += Vector2.left ; // (1,0)

        if (movement != Vector2.zero)
        {
            movesDuringTheFrame = true;
            p_targetDirection = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg;
        }


    }


    // Update is called once per frame
    void Update()
    {
        KeyboardMovement();

        p_direction = Mathf.Lerp(p_direction, p_targetDirection, 35 * Time.deltaTime);
        if (movesDuringTheFrame != movesChange)
        {
            if (movesDuringTheFrame)
            {
                MovementParticles.Play();
                p_targetScale.x = 0.85f;
                p_targetScale.y = 1.15f;
                VisualObj.transform.DOKill();
            }
            else
            {
                MovementParticles.Stop();
                p_targetScale = new Vector3(1,1,1);
                p_scale = p_targetScale;
                p_targetDirection = -90;
                VisualObj.transform.DOScale(1, 1).SetEase(Ease.OutElastic, 5f);
            }

            movesChange = movesDuringTheFrame;
        }

        if(movesDuringTheFrame)
        p_scale = Vector3.Lerp(p_scale, p_targetScale, 15 * Time.deltaTime);
        if(movesDuringTheFrame)
        VisualObj.transform.localScale = p_scale;


        if (movesDuringTheFrame)
        {
            float radians = p_targetDirection * Mathf.Deg2Rad;
            p_targetVel = new Vector2(-(Speed) * Mathf.Sin(radians),
                                       Speed * Mathf.Cos(radians));

        }
        else
        {
            p_targetVel = Vector2.zero;
        }
        p_vel = Vector3.Lerp(p_vel, p_targetVel, 20);

        rb.MovePosition(rb.position + p_targetVel);
        transform.rotation = Quaternion.Euler(0, 0, p_direction);

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            DashParticles.Stop();
            DashParticles.Play();
            CircleParticles.Stop();
            CircleParticles.Play();
            DashStuff();
        }
        
    }

    private void FixedUpdate()
    {
        if (!canDash)
        {
            dashCooldownCurrentFrame--;
            if(dashCooldownCurrentFrame <= 0)
            {
                canDash = true;
            }
        }

        if (IsInDamageCooldown)
        {
            currentDmgCooldownFrame--;
            effectFrame--;
            if (effectFrame <= 0)
            {
                VisualObj.GetComponent<SpriteRenderer>().color = isMagenta ? defaultColor : Color.magenta;
                effectFrame = 2;
                isMagenta = !isMagenta;
            }
            if(currentDmgCooldownFrame <= 0)
            {
                effectFrame = 2;
                VisualObj.GetComponent<SpriteRenderer>().color = defaultColor;
                isMagenta = false;
                IsInDamageCooldown = false;
            }
        }
    }

    void DashStuff()
    {
        float originalSpeed = Speed;
        float dashSpeed = originalSpeed * 4.2f;
        float xPos, yPos;
        xPos = p_targetScale.x;
        yPos = p_targetScale.y;

        p_targetScale.x = 0.5f;
        p_targetScale.y = 1.23f;
        dashCooldownCurrentFrame = dashCooldownTimeFrames;
        canDash = false;
        
        isInDash = true;

        DOTween.To(() => dashSpeed, x =>
        {
            dashSpeed = x;
            Speed = originalSpeed + dashSpeed;
        }, 0f, dashDuration).SetEase(Ease.Linear).OnComplete(()=>
        {
            isInDash = false;
            p_targetScale.x = xPos;
            p_targetScale.y = yPos;
        });

        
        
    }

    public void OnGUI()
    {
        //Debug.Log("Velocity: " + p_vel); // Check if it's updating
        GUI.Label(new Rect(0,0, 900, 900), "Velocity: " + p_vel);
        GUI.Label(new Rect(0, 18, 900, 900), "Target Velocity: " + p_targetVel);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "LevelHitbox" && !IsInDamageCooldown && !isInDash)
        {
            Debug.Log("should get hit");
            HitParticles.Stop();
            HitParticles.Play();
            currentDmgCooldownFrame = framesDuringCooldownDamage;
            IsInDamageCooldown = true;
        }
    }

    
}
