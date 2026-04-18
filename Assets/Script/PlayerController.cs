using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;

    private float dashTime;
    private float lastDashTime;

    private bool isDashing = false;
    private bool isDead = false;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isDead) return;

        // รับ input ซ้ายขวา
        moveInput = 0;
        if (Input.GetKey(KeyCode.A)) moveInput = -1;
        if (Input.GetKey(KeyCode.D)) moveInput = 1;

        // Dash (กด Q)
        if (Input.GetKeyDown(KeyCode.Q) && !isDashing && Time.time >= lastDashTime + dashCooldown)
        {
            StartDash();
        }

        // Animation วิ่ง
        anim.SetBool("isRunning", moveInput != 0 && !isDashing);
    }

    void FixedUpdate()
    {
        if (isDead) return;

        if (isDashing)
        {
            DashMove();
        }
        else
        {
            NormalMove();
        }
    }

    void NormalMove()
    {
        // เดินปกติ
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // หันซ้ายขวา (ใช้ flipX แทน scale)
        if (moveInput != 0)
        {
            sr.flipX = moveInput < 0;
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTime = dashDuration;
        lastDashTime = Time.time;

        anim.SetTrigger("Slide");
    }

    void DashMove()
    {
        // ใช้ทิศจาก flipX
        float dir = sr.flipX ? -1f : 1f;

        // พุ่งไปข้างหน้า (ไม่ยุ่งแกน Y = ไม่ลอย)
        rb.velocity = new Vector2(dir * dashSpeed, rb.velocity.y);

        dashTime -= Time.fixedDeltaTime;

        if (dashTime <= 0)
        {
            isDashing = false;
        }
    }

    // ===== OPTIONAL =====

    public void TakeDamage()
    {
        if (isDashing) return; // dash กันดาเมจได้

        Debug.Log("โดนดาเมจ!");
    }

    public void Die()
    {
        isDead = true;
        anim.SetTrigger("Dead");
        rb.velocity = Vector2.zero;
    }
}