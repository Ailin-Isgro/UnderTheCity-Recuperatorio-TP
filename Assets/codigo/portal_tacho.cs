using System.Collections.Generic;
using UnityEngine;

public class portal_tacho : MonoBehaviour
{
    public Transform portalDestino;   // A dónde se teletransporta
    public float tiempoEspera = 0.5f; // Segundos antes de poder volver a usar un portal
    public bool estaActivo = true;    // Portal prendido o apagado

    // Guarda cuándo viajó cada objeto (compartido entre todos los tachos)
    static Dictionary<GameObject, float> ultimoViaje = new Dictionary<GameObject, float>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!estaActivo || portalDestino == null) return;

        // Solo viajan estos tags
        if (!other.CompareTag("Vagabundo") && !other.CompareTag("Rata") && !other.CompareTag("Caja")) return;

        // El objeto que tiene el Rigidbody2D (el personaje o la caja completa)
        Rigidbody2D rb = other.attachedRigidbody;
        GameObject objeto = rb != null ? rb.gameObject : other.gameObject;

        // Si viajó hace poco, no hacemos nada (evita el bucle infinito)
        if (ultimoViaje.ContainsKey(objeto) && Time.time - ultimoViaje[objeto] < tiempoEspera) return;
        ultimoViaje[objeto] = Time.time;

        // Teletransportar (mantenemos la Z original)
        Vector3 nuevaPosicion = portalDestino.position;
        nuevaPosicion.z = objeto.transform.position.z;
        objeto.transform.position = nuevaPosicion;
    }

    // Para conectar a un interruptor
    public void AlternarPortal()
    {
        estaActivo = !estaActivo;
    }
}
