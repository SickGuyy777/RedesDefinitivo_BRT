using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class DisparoRebote : MonoBehaviour
{
    public float velocidad = 5f; // Velocidad de la bala
    public int maxRebotes = 3;   // Número máximo de rebotes

    private int rebotesRealizados = 0;

    void Update()
    {
        // Mueve la bala en la dirección hacia adelante
        transform.Translate(Vector2.up * velocidad * Time.deltaTime);

        // Detecta colisiones
        DetectarColision();
    }

    void DetectarColision()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 0.1f);

        if (hit.collider != null && hit.collider.CompareTag("Pared"))
        {
            // Si colisiona con algo con el tag "pared", rebota
            if (rebotesRealizados < maxRebotes)
            {
                Rebote(hit.normal);
                rebotesRealizados++;
            }
            else
            {
                // Si alcanza el número máximo de rebotes, destruye la bala
                Destroy(gameObject);
            }
        }
    }

    void Rebote(Vector2 normalDeColision)
    {
        // Refleja la dirección de la bala en la normal de la superficie de colisión
        Vector2 direccionReflejada = Vector2.Reflect(transform.up, normalDeColision);
        transform.up = direccionReflejada;

        // Ajusta la posición ligeramente para evitar colisiones continuas
        transform.Translate(Vector2.up * 0.1f);
    }

}
