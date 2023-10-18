using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class moveCamera : MonoBehaviour
{

    XboxController controls;

    Vector2 move;
    Vector2 rotate;
    CapsuleCollider coll;
    //AudioSource a;
    //AudioSource b;
    //bool count;

    void Awake()
    {
        //count = true;
        //a = GetComponent<AudioSource>();
        //b = GetComponent<AudioSource>();
        coll = GetComponent<CapsuleCollider>();
        controls = new XboxController();


        controls.playerControl.Jump.performed += ctx => Jump();
        controls.playerControl.Sink.performed += ctx => Sink();

        controls.playerControl.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.playerControl.Move.canceled += ctx => move = Vector2.zero;

        controls.playerControl.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        controls.playerControl.Rotate.canceled += ctx => move = Vector2.zero;
    }

    void Jump()
    {
        transform.position += Vector3.one;

    }
    void Sink()
    {
        transform.position -= Vector3.one;

    }

    private void Update()
    {
        Vector3 m = new Vector3(move.x, 0, move.y) * 18 * Time.deltaTime;
        transform.Translate(m, Space.Self);

        Vector3 r = new Vector3(0, rotate.x, 0) * 75 * Time.deltaTime;
        transform.Rotate(r, Space.Self);


    }

    void OnEnable()
    {
        controls.playerControl.Enable();
    }
    void OnDisable()
    {
        controls.playerControl.Disable();
    }
    void OnTriggerEnter(Collider collision)
    {
        collision.gameObject.GetComponent<AudioSource>().Play();
    }
}
