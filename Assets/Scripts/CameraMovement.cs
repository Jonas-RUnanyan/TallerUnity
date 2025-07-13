using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //Referencia al GameObject del jugador
    public GameObject player;
    
    //Sensibilidad de la cámara, determina cuán rápido se moverá cuando movamos el ratón
    private readonly float camSens = 3f;
    //Vector que guarda la diferencia en posición entre el jugador y la cámara
    private Vector3 offset;

    void Start()
    {
        //La posición en la que estén el jugador y la cámara cuando empiece el juego determinará cómo se coloque la cámara
        //a lo largo del juego
        offset = transform.position - player.transform.position;
    }
    
    void Update()
    {
        //Actualizamos la posición de la cámara
        transform.position = player.transform.position + offset;
        
        //Recalculamos la posición de la cámara
        Rotate();
    }


    void Rotate()
    { 
        //Modificamos el offset para que, según el movimiento horizontal del ratón, se mueva alrededor del jugador
        //rotando alrededor del eje vertical
        offset=Quaternion.AngleAxis(Input.GetAxis("Mouse X")*camSens, Vector3.up)*offset;
        
        //Hacemos que la cámara mire hacia el jugador
        transform.LookAt(player.transform);
    }
}