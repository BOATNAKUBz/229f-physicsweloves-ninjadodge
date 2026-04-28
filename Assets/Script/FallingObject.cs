using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallingObject : MonoBehaviour
{
    [Header("Force Settings")]
    public float fallForce = 5f;      // แรงตกลง
    public float sideForce = 2f;      // แรงเหวี่ยงซ้ายขวา
    public float spinForce = 200f;    // แรงหมุน

    [Header("Destroy Settings")]
    public float destroyY = -6f;      // ต่ำกว่านี้ลบ

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // ใส่แรงตกลง
        rb.AddForce(Vector2.down * fallForce, ForceMode2D.Impulse);

        // ใส่แรงสุ่มซ้าย-ขวา
        float randomX = Random.Range(-sideForce, sideForce);
        rb.AddForce(new Vector2(randomX, 0f), ForceMode2D.Impulse);

        // ใส่แรงหมุน (แกน Z)
        float randomSpin = Random.Range(-spinForce, spinForce);
        rb.AddTorque(randomSpin, ForceMode2D.Impulse);
    }

    void Update()
    {
        // ถ้าตกต่ำเกิน ลบทิ้ง
        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }

    // (เสริม) ถ้าชนพื้นแล้วให้หาย
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
