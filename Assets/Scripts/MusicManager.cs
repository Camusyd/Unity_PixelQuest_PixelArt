using UnityEngine;

public class MusicManager : MonoBehaviour
{
    void Awake()
    {
        // Busca si ya existe un MusicManager en la escena
        GameObject[] objs = GameObject.FindGameObjectsWithTag("MusicManager");

        // Si ya existe otro MusicManager, destrúyete a ti mismo
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        // Si eres el único, haz que no te destruyas al cargar una nueva escena
        DontDestroyOnLoad(this.gameObject);
    }
}
