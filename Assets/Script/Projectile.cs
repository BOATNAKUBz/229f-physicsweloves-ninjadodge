
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D rb;

    [Header("Sound")]
    public AudioClip hitSound;
    private AudioSource audioSource;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>(); 
    }

    public void SetDirection(Vector2 dir)
    {
        rb.velocity = dir.normalized * speed;

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

            Destroy(gameObject, 1f);
        }
    }
}

