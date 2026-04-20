using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;

    [Header("Health")]
    public int maxHP = 3;
    private int currentHP;

    [Header("UI")]
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Invincible")]
    public float invincibleTime = 1f;
    private bool isInvincible = false;

    [Header("Throw")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 0.3f;
    private float lastFireTime;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip coinSound;
    public AudioClip dashSound;

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

        currentHP = maxHP;

        UpdateHeartsUI();
    }

    void Update()
    {
        if (isDead) return;

        moveInput = 0;
        if (Input.GetKey(KeyCode.A)) moveInput = -1;
        if (Input.GetKey(KeyCode.D)) moveInput = 1;

        if (Input.GetKeyDown(KeyCode.Q) && !isDashing && Time.time >= lastDashTime + dashCooldown)
        {
            StartDash();
        }

        if (Input.GetMouseButtonDown(0) && Time.time > lastFireTime + fireRate)
        {
            lastFireTime = Time.time;
            Throw();
        }

        if (anim != null)
            anim.SetBool("isRunning", moveInput != 0 && !isDashing);
    }

    void FixedUpdate()
    {
        if (isDead) return;

        if (isDashing)
            DashMove();
        else
            NormalMove();
    }

    void NormalMove()
    {
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (moveInput != 0)
            sr.flipX = moveInput < 0;
    }

    void StartDash()
    {
        isDashing = true;
        dashTime = dashDuration;
        lastDashTime = Time.time;

        if (audioSource != null && dashSound != null)
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(dashSound);
        }

        if (anim != null)
            anim.SetTrigger("Slide");
    }

    void DashMove()
    {
        float dir = sr.flipX ? -1f : 1f;

        rb.velocity = new Vector2(dir * dashSpeed, rb.velocity.y);

        dashTime -= Time.fixedDeltaTime;

        if (dashTime <= 0)
            isDashing = false;
    }

    void Throw()
    {
        if (anim != null)
            anim.SetTrigger("Throw");

        if (projectilePrefab == null || firePoint == null) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector2 direction = (mousePos - firePoint.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Projectile p = proj.GetComponent<Projectile>();
        if (p != null)
        {
            p.SetDirection(direction);
        }
    }

    // ===== HEALTH SYSTEM =====
    public void TakeDamage(int damage)
    {
        if (isDead || isDashing || isInvincible) return;

        currentHP -= damage;

        if (currentHP < 0)
            currentHP = 0;

        UpdateHeartsUI();

        Debug.Log("HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
            GameManager.instance.GameOver();
        }
        else
        {
            StartCoroutine(Invincible());
        }
    }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHP)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }

    IEnumerator Invincible()
    {
        isInvincible = true;

        for (int i = 0; i < 5; i++)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sr.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        if (anim != null)
            anim.SetTrigger("Dead");

        rb.velocity = Vector2.zero;

        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject);
            TakeDamage(1);
        }

        if (collision.CompareTag("Coin"))
        {
            GameManager.instance.AddScore(10);
            if (audioSource != null && coinSound != null)
            {
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.PlayOneShot(coinSound);
            }
            Destroy(collision.gameObject);
        }
    }
}