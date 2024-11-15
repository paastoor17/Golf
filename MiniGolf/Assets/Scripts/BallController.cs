using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    public GameController gameController;  // Asigna el GameController en el Inspector
    public Slider powerSlider;
    public float maxPower = 1f;
    private Rigidbody2D rb;
    private Vector2 startPos;
    private Vector2 endPos;
    private bool gameActive = true;
    private bool isStuck = false;
    private float stuckTime = 0.5f;
    private float stuckTimer = 0f;

    public GameObject[] paredesDesaparecer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        powerSlider.value = 0;
        powerSlider.maxValue = maxPower;

        if (paredesDesaparecer.Length == 0)
        {
            Debug.LogError("¡No hay paredes asignadas en el array 'paredesDesaparecer'!");
        }

        foreach (var pared in paredesDesaparecer)
        {
            if (pared != null)
            {
                pared.SetActive(true);
            }
            else
            {
                Debug.LogError("¡Una pared está asignada como null!");
            }
        }
    }

    void Update()
    {
        if (gameActive)
        {
            if (isStuck)
            {
                stuckTimer += Time.deltaTime;
                if (stuckTimer >= stuckTime)
                {
                    isStuck = false;
                    stuckTimer = 0f;
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    Debug.Log("La bola ha dejado de estar enganchada a la pared.");
                }
            }

            if (Input.GetMouseButtonDown(0) && !isStuck)
            {
                startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                powerSlider.value = 0;
            }

            if (Input.GetMouseButton(0) && !isStuck)
            {
                endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float distance = Vector2.Distance(startPos, endPos);
                powerSlider.value = Mathf.Clamp(distance, 0, maxPower);
            }

            if (Input.GetMouseButtonUp(0) && !isStuck)
            {
                endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 force = (startPos - endPos).normalized * powerSlider.value * 10;
                rb.AddForce(force, ForceMode2D.Impulse);

                // Registrar el golpe en el GameController
                if (gameController != null)
                {
                    gameController.RegistrarGolpe();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boton"))
        {
            Debug.Log("Botón tocado: " + other.gameObject.name);

            int buttonIndex = int.Parse(other.gameObject.name.Replace("Boton", ""));
            if (buttonIndex >= 0 && buttonIndex < paredesDesaparecer.Length)
            {
                GameObject pared = paredesDesaparecer[buttonIndex];
                if (pared != null)
                {
                    pared.SetActive(false);
                    Debug.Log("¡La pared " + buttonIndex + " ha desaparecido!");
                }
                else
                {
                    Debug.LogError("¡La pared asociada al botón " + buttonIndex + " no está asignada!");
                }
            }
            else
            {
                Debug.LogError("¡Índice de botón no válido o fuera de rango!");
            }
        }

        if (other.CompareTag("Hoyo"))
        {
            Destroy(gameObject);
            gameActive = false;
            Debug.Log("¡Juego terminado!");

            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                foreach (var pared in paredesDesaparecer)
                {
                    if (pared != null)
                    {
                        pared.SetActive(true);
                    }
                    else
                    {
                        Debug.LogWarning("¡Una pared no está asignada!");
                    }
                }

                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.Log("Has completado todos los mapas.");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ParedNegra"))
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;

            isStuck = true;
            Debug.Log("La bola está enganchada a la pared.");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ParedNegra"))
        {
            isStuck = false;
            stuckTimer = 0f;
            rb.bodyType = RigidbodyType2D.Dynamic;
            Debug.Log("La bola ha salido de la pared.");
        }
    }
}
