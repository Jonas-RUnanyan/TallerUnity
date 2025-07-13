using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 1f;
    private Rigidbody rigid;
    public MapGenerator mapGen;
    private Camera camera;
    
    private void Start()
    {
        //Generamos el mapa nada más empezar la simulación para que se renderice correctamente
        mapGen.GenerateMap();
        
        //Obtenemos la referencia del Ridigbody de la bola 
        rigid = gameObject.GetComponent<Rigidbody>();
 
        //Ocultamos el cursor para que no aparezca mientras jugamos
        Cursor.visible = false;
    }
 
    private void Update()
    {
        //Al pulsar la tecla Esc volvemos a mostrar el cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
        }

        Vector3 direction = Vector3.zero;
        //Movimiento con WASD
        if (Input.GetKey(KeyCode.D))
        {
            direction+=Camera.main.transform.right*speed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction-=Camera.main.transform.right*speed;
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            direction+=Camera.main.transform.forward*speed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction-=Camera.main.transform.forward*speed;
        }

        direction.y = 0;
        
        rigid.AddForce(direction);
    }
}