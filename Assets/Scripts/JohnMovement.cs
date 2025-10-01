using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JohnMovement : MonoBehaviour
{
    public float Speed;
    public float JumpForce;
    public GameObject BulletPrefab;

    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private float Horizontal;
    private bool Grounded;
    private float LastShoot;
    private int Health = 5;

    // Guarda la posición de inicio del personaje
    private Vector3 initialPosition;

    private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        // Guarda la posición inicial al comenzar el juego
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Movimiento con las teclas "A" y "D" o las flechas
        Horizontal = Input.GetAxisRaw("Horizontal");

        if (Horizontal < 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (Horizontal > 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        Animator.SetBool("running", Horizontal != 0.0f);

        // Detectar Suelo
        if (Physics2D.Raycast(transform.position, Vector3.down, 0.1f))
        {
            Grounded = true;
        }
        else Grounded = false;

        // Salto con la tecla "W" o la flecha arriba
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && Grounded)
        {
            Jump();
        }
        
        // Agacharse con la tecla "S" o la flecha abajo
        Animator.SetBool("crouching", Input.GetKey(KeyCode.S));

        // Disparar con la tecla Espacio
        if (Input.GetKey(KeyCode.Space) && Time.time > LastShoot + 0.25f)
        {
            Shoot();
            LastShoot = Time.time;
        }

        // Si el personaje cae del mapa, reinicia el juego
        if (transform.position.y < -10)
        {
            RestartGame();
        }
    }

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = new Vector2(Horizontal * Speed, Rigidbody2D.velocity.y);
    }

    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
    }

    private void Shoot()
    {
        Vector3 direction;
        if (transform.localScale.x == 1.0f) direction = Vector3.right;
        else direction = Vector3.left;

        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);
        bullet.GetComponent<BulletScript>().SetDirection(direction);
    }

    public void Hit()
    {
        Health -= 1;
        if (Health == 0)
        {
            // Detén el movimiento del personaje al morir.
            Rigidbody2D.velocity = Vector2.zero;
            // Cambia el Rigidbody a Kinematic para que ignore la gravedad y las fuerzas
            Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            // Desactiva el script para que no se pueda mover ni disparar
            this.enabled = false;
            // Activa la animación de muerte.
            Animator.SetTrigger("Die");
            // Reinicia el juego después de un tiempo para que la animación termine.
            Invoke("RestartGame", 1f); 
        }
    }
    
    // Función para reiniciar el juego
    private void RestartGame()
    {
        // Regresa al personaje a su posición inicial
        transform.position = initialPosition;
        // Restablece la vida
        Health = 5;
        // Habilita el script y la física del personaje
        this.enabled = true;
        Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        
        // Restablece los parámetros del Animator
        Animator.ResetTrigger("Die"); // Borra el trigger de muerte
        Animator.SetBool("running", false);
        Animator.SetBool("crouching", false);
        
        // Vuelve a la animación de "idle"
        Animator.Play("Idle");
    }
}