using UnityEngine;
using UnityEngine.SceneManagement; // Necesitas esta línea

public class EndGame : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Comprueba si el objeto que entró en la zona de meta es el jugador
        if (other.CompareTag("Player"))
        {
            // Muestra un mensaje en la consola de Unity
            Debug.Log("¡Juego Terminado!");
            
            // Puedes recargar el nivel actual para reiniciar el juego
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            // O, si quieres que la aplicación se cierre (solo para versiones compiladas)
            // Application.Quit();
        }
    }
}
