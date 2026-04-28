using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float mass = 1f; // เพิ่มมวล

    private Rigidbody2D rb;

    [Header("Sound")]
    public AudioClip hitSound;
    private AudioSource audioSource;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        rb.gravityScale = 0f;
    }

    public void SetDirection(Vector2 dir)
    {
        // 🔥 คำนวณแรงจาก F = ma
        float acceleration = speed; // เอา speed เป็น a แบบง่าย
        float force = mass * acceleration;

        // 🔥 แปลงเป็น velocity
        Vector2 velocity = dir.normalized * force;

        rb.velocity = velocity;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject);

            if (audioSource != null && hitSound != null)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(hitSound, 0.5f);
            }

            Destroy(gameObject);
        }
    }
}