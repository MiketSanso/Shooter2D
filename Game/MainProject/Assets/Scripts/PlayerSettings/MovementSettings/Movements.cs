using UnityEngine;

public class Movements : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject player;
    [SerializeField] private float going, run, jump, offset;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("IsActivePanels") == 0)
            Move();

#warning Добавь сюда нормальную переменную
        }

    private void Move()
    {
        if (Input.GetButton("Horizontal") && Input.GetKey(KeyCode.LeftControl))
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * run, rb.velocity.y);
            animator.SetBool("run", true);
            animator.SetBool("walk", false);
        }
        else if (Input.GetButton("Horizontal"))
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * going, rb.velocity.y);
            animator.SetBool("walk", true);
            animator.SetBool("run", false);
        }
        else
        {
            animator.SetBool("walk", false);
            animator.SetBool("run", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !animator.GetBool("jump") && !Input.GetKey(KeyCode.LeftControl) && !animator.GetBool("aim"))
        {
            rb.AddForce(transform.up * jump, ForceMode2D.Impulse);
            animator.SetTrigger("jump");
        }
    }
}
