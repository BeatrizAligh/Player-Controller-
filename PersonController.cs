using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PersonController : MonoBehaviour
{
    //llamado al input manager
    private PlayerInput playerInput;


    //movimiento de personaje
    public CharacterController controller;
    public float speed = 1f;
    public Rigidbody rb;


    //visor de camara
    public Camera cam;
    public float xRotation = 0f;

    public float xSensivitivity = 30f;
    public float ySensivitivity = 30f;

    //MOVIMIENTO FINALIZADO-----------------------------

    //INTERACTIVOS----------------------------

    public GameObject indicador;


    //INTERACTIVOS FINALIZADO--------------------



    //CAMINO-----------------
    public Transform player; // Transform del jugador
    public Transform destination; // Transform del destino
    public LineRenderer pathRenderer; // Referencia al LineRenderer
    public float pathWidth = 0.1f; // Ancho del camino
    public Color pathColor = Color.yellow; // Color del camino




    // Start is called before the first frame update
    void Start()
    {
        //llamar a los componente que tiene el player
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();


        //CONFIGURACION DEL CAMINO
        // Configurar el LineRenderer
        pathRenderer.startWidth = pathWidth;
        pathRenderer.endWidth = pathWidth;
        pathRenderer.material.color = pathColor;

    }

    void Update()
    {
        // Actualizar el camino
        UpdatePath();
    }

    //asociar funciones de movimiento de personaje y camara con el input system 
    void FixedUpdate()
    {
        ProcessMove(playerInput.actions["Walk"].ReadValue<Vector2>());

    }

    private void LateUpdate()
    {
        ProcessLook(playerInput.actions["Look"].ReadValue<Vector2>());

    }


    public void ProcessMove(Vector2 input)
    {
        //llamado al imput system las direcciones que poseen
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        //controlador de movimiento dependiendo de la direccion y veloocidad
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

    }

    public void ProcessLook(Vector2 input)
    {
        //llamado al movimiento de mouse
        float mouseX = input.x;
        float mouseY = input.y;
        //identificacion de movimiento 
        xRotation -= (mouseY * Time.deltaTime) * ySensivitivity;
        xRotation = Mathf.Clamp(xRotation, -30f, 50f);
        //rotacion y movimiento de la camara con respecto al mouse
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensivitivity);
    }


    //MOVIMIENTO FINALIZADO------------------------------------------------------------------

    //INTERACTIVOS--------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Activadores")
        {
            Debug.Log("ACTIVACION DE ANIMACION");
            Destroy(indicador);
        }
    }

    //INTERATIVOS FINALIZADO-------------------

    //CAMINO
    void UpdatePath()
    {
        // Establecer la posición inicial del camino en la posición del jugador
        pathRenderer.SetPosition(0, player.position);

        // Establecer la posición final del camino en la posición del destino
        pathRenderer.SetPosition(1, destination.position);
    }




}
