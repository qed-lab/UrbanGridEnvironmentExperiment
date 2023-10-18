using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Texture2D mainTexture;
    public int mainTexWidth;
    public int mainTexHeight;

    private bool jumpKeyWasPressed;
    private float horizontalInput;
    private float verticalInput;
    private Rigidbody rigidbodyComponent;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
        SetMainTextureSize();
        CreatePattern();
    }

    void SetMainTextureSize()
    {
        mainTexture = new Texture2D(mainTexWidth, mainTexHeight);
    }

    void CreatePattern()
    {
        for (int i = 0; i < mainTexWidth; i++)
        {
            for (int j = 0; j < mainTexWidth; j++)
            {
                if (((i + j) % 2) == 1)
                {
                    mainTexture.SetPixel(i, j, Color.black);
                }
                else
                {
                    mainTexture.SetPixel(i, j, Color.white);
                }
            }
        }
        mainTexture.Apply();
        GetComponent<Renderer>().material.mainTexture = mainTexture;
    }



    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyWasPressed = true;
        }

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

    }

    private void FixedUpdate()
    {
        float xR = rigidbodyComponent.rotation.x;
        float yR = rigidbodyComponent.rotation.y;
        float zR = rigidbodyComponent.rotation.z;
        float wR = rigidbodyComponent.rotation.w;
        //if(yR != 0){
        //    print(horizontalInput);
        //    print(yR);
        //}
        if (horizontalInput > 0)
        {
            //print("hi");
            //print(yR);
            rigidbodyComponent.rotation *= Quaternion.Euler(new Vector3(0, 2, 0));
        }
        if (horizontalInput < 0)
        {
            //print("hi");
            //print(yR);
            rigidbodyComponent.rotation *= Quaternion.Euler(new Vector3(0, -2, 0));
        }
        if (horizontalInput == 0)
        {
            rigidbodyComponent.rotation = new Quaternion(xR, yR, zR, wR);
        }
        print("rigidbodyComponent.velocity");
        print(rigidbodyComponent.velocity);
        print(transform.forward);
        if (verticalInput > 0)
        {
            rigidbodyComponent.velocity = transform.forward * 7 + new Vector3(0, rigidbodyComponent.velocity.y, 0);
        }
        else if (verticalInput < 0)
        {
            rigidbodyComponent.velocity = transform.forward * -7 + new Vector3(0, rigidbodyComponent.velocity.y, 0);
        }
        else
        {
            rigidbodyComponent.velocity = new Vector3(0, rigidbodyComponent.velocity.y, 0);
        }

        if (jumpKeyWasPressed)
        {
            print("helloworld");
            rigidbodyComponent.AddForce(Vector3.up * 7, ForceMode.VelocityChange);
            jumpKeyWasPressed = false;
        }

    }
}