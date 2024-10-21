using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float speedJump;

    public AudioSource footSteps;

    public bool collidedStop;

    public GameObject Canva;

    public GameObject CanvaFT;

    public string bookID;

    public bool colidirEscada;

    public string tema;

    private bool grounded;
    private Vector2 initialScale;
    private Rigidbody2D body;

    // Adicione uma referência ao AudioSource
    private AudioSource audioSource;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        initialScale = transform.localScale;

        // Obtenha o componente AudioSource
        audioSource = GetComponent<AudioSource>();


    }

    private void Update()
    {
        if (collidedStop == false)
        {
            float horizontalInput = Input.GetAxis("Horizontal");

            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

            // Flip player when moving left
            if (horizontalInput < -0.01f)
            {
                transform.localScale = initialScale;
            }
            else if (horizontalInput > 0.01f)
            {
                transform.localScale = new Vector2(initialScale.x * -1, initialScale.y);
            }

            if (Input.GetKeyDown(KeyCode.Space) && grounded)
                Jump();
        }
    }

    private void Jump()
    {
        body.AddForce(new Vector2(0, speedJump));
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;

            //footSteps.Play();

        }

        if (collision.gameObject.tag == "book1")
        {
            Canva.SetActive(true);
            collidedStop = true;

            // Tocar o som quando colidir com "book1"
            audioSource.Play();
        }

        if (collision.gameObject.tag == "escadas")
        {
            CanvaFT.SetActive(true);
            collidedStop = true;
            colidirEscada = true;
            Debug.Log("cccc");
        }
    }
}
