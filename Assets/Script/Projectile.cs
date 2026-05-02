using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public float acceleration = 10f; // a
    public float mass = 1f;          // m

    private Rigidbody2D rb;

    [Header("Sound")]
    public AudioClip hitSound;
    private AudioSource audioSource;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        rb.gravityScale = 1f;       // ให้ตกแบบ projectile
        rb.drag = 0.5f;   // แรงต้านอากาศ (Air Resistance)
    }

    public void SetDirection(Vector2 dir)
    {
        //  F = m * a
        float force = mass * acceleration;

        //  ใช้ AddForce แทน velocity
        rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);

        // หมุนให้หันไปทิศยิง
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // หมุนตอนบิน (Rotational Motion)
        rb.angularVelocity = 300f;

        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject);

            if (audioSource != null && hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position, 0.5f);
                Destroy(gameObject);
            }

            Destroy(gameObject);
        }
    }
}