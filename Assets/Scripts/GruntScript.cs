using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntScript : MonoBehaviour
{
    public Transform John;
    public GameObject BulletPrefab;
    
    // Agrega una referencia al Animator del Grunt
    private Animator anim;
    
    private int Health = 3;
    private float LastShoot;

    void Start()
    {
        // Obtiene el componente Animator al inicio
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (John == null) return;

        Vector3 direction = John.position - transform.position;
        if (direction.x >= 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        else transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);

        float distance = Mathf.Abs(John.position.x - transform.position.x);

        if (distance < 1.0f && Time.time > LastShoot + 0.25f)
        {
            Shoot();
            LastShoot = Time.time;
        }
    }

    private void Shoot()
    {
        Vector3 direction = new Vector3(transform.localScale.x, 0.0f, 0.0f);
        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);
        bullet.GetComponent<BulletScript>().SetDirection(direction);
    }

    public void Hit()
    {
        Health -= 1;
        if (Health == 0)
        {
            // En lugar de destruir el objeto inmediatamente, activa la animación de muerte.
            anim.SetTrigger("Die");
            // Luego, llama a una función para destruir el objeto después de un tiempo.
            Invoke("DestroyObject", 1f); // 1 segundo de retraso, ajústalo según tu animación.
        }
    }
    
    // Esta función se encarga de destruir el GameObject
    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
