using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float speed;
    public VirtualJoystick joystick;

    private Rigidbody rb;
    private int health = 100;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate ()
    {
        float moveHorizontal = joystick.Horizontal(); //Input.GetAxis("Horizontal");
        float moveVertical = joystick.Vertical(); //Input.GetAxis("Vertical");

        Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

        rb.AddForce (movement * speed);
    }
}