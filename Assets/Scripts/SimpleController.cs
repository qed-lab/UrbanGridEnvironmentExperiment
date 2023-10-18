using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour
{
    public Transform[] target;
    public float speed;
    private int current;
    private float distance;
    // Use this for initialization    
    void Start()
    {
    }
    //float dist(Vector3 A, Vector3 B)
    //{
    //    //return Math.Sqrt((A.x));
    //}
    // Update is called once per frame    
    void Update()
    {
        //print("current");
        //print(current);
        //print(target[current].position);
        //print(transform.position);
        print(target[current].rotation);
        //print(transform.rotation);

        if (transform.position != target[current].position || transform.rotation != target[current].rotation)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, target[current].position, speed);
            GetComponent<Rigidbody>().MovePosition(pos);
            Quaternion q = new Quaternion();
            if (target[current].rotation.y == 90 || target[current].rotation.y == 180)
            {
                q = Quaternion.RotateTowards(transform.rotation, target[current].rotation, speed * 4);
            }
            else
            {
                q = Quaternion.RotateTowards(transform.rotation, target[current].rotation, speed * 4);
            }
            GetComponent<Rigidbody>().MoveRotation(q);
        }
        else
        {
            current = (current + 1);
            //print(target[current].rotation);

            if (current > target.Length - 1)
            {
                current = current - 1;
                //distance = dist(transform.position - target[current].position);

            }
        }
    }
}   

//0°	X = 27.0000	Y = 0.0000ww        266 + 
//2	15°	X = 26.0800	Y = 6.9881ww
//3	30°	X = 23.3827	Y = 13.5000ww
//4	45°	X = 19.0919	Y = 19.0919ww
//5	60°	X = 13.5000	Y = 23.3827ww
//6	75°	X = 6.9881	Y = 26.0800ww
//7	90°	X = 0.0000	Y = 27.0000ww
//8	105°	X = -6.9881	Y = 26.0800
//9	120°	X = -13.5000	Y = 23.3827
//10	135°	X = -19.0919	Y = 19.0919
//11	150°	X = -23.3827	Y = 13.5000
//12	165°	X = -26.0800	Y = 6.9881
//13	180°	X = -27.0000	Y = 0.0000
//{
    //public Texture2D mainTexture;
    //public int mainTexWidth;
    //public int mainTexHeight;

    //private bool jumpKeyWasPressed;
    //private Transform[] target;
    //private int horizontalInput;
    //private bool forward = false;
    //private bool backward = false;
    //private Rigidbody rigidbodyComponent;
    //private int count;
    //private int current;
    //private int speed = 5;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    rigidbodyComponent = GetComponent<Rigidbody>();
    //    SetMainTextureSize();
    //    CreatePattern();
    //    current = 0;
    //    target[0] =;
    //}

    //void SetMainTextureSize()
    //{
    //    mainTexture = new Texture2D(mainTexWidth, mainTexHeight);
    //}

    //void CreatePattern()
    //{
    //    for (int i = 0; i < mainTexWidth; i++)
    //    {
    //        for (int j = 0; j < mainTexWidth; j++)
    //        {
    //            if (((i + j) % 2) == 1)
    //            {
    //                mainTexture.SetPixel(i, j, Color.black);
    //            }
    //            else
    //            {
    //                mainTexture.SetPixel(i, j, Color.white);
    //            }
    //        }
    //    }
    //    mainTexture.Apply();
    //    GetComponent<Renderer>().material.mainTexture = mainTexture;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (transform.position != target[current].position)
    //    {
    //        Vector3 pos = Vector3.MoveTowards(transform.position, target[current].position, speed * Time.deltaTime);
    //        GetComponent<Rigidbody>().MovePosition(pos);
    //    }
    //    else current = (current + 1) % target.Length;

        //print(count);
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    jumpKeyWasPressed = true;
        //}

        //if (Input.GetKeyDown(KeyCode.UpArrow)) {     // UP
        //    backward = false;
        //    forward = !forward;
        //}
        //if (Input.GetKeyDown(KeyCode.DownArrow)) {   // DOWN
        //    forward = false;
        //    backward = !backward;
        //}
        //if (Input.GetKeyDown(KeyCode.LeftArrow))    // LEFT
        //{   
        //    horizontalInput = -1;
        //    count += 45;
        //}
        //if (Input.GetKeyDown(KeyCode.RightArrow))   // RIGHT
        //{  
        //    horizontalInput = 1;
        //    count += 45;
        //}
        //if (Input.GetKeyDown(KeyCode.S))    // STOP
        //{   
        //    count = 0;
        //}

    //}

////    private void FixedUpdate()
////    {
////        float xR = rigidbodyComponent.rotation.x;
////        float yR = rigidbodyComponent.rotation.y;
////        float zR = rigidbodyComponent.rotation.z;
////        float wR = rigidbodyComponent.rotation.w;
////        //if(yR != 0){
////        //    print(horizontalInput);
////        //    print(yR);
////        //}
////        if (count > 0)
////            count -= 1;
////        { 
////            if (horizontalInput == 1)
////            {

////                //print("hi");
////                //print(yR);
////                rigidbodyComponent.rotation *= Quaternion.Euler(new Vector3(0, 0.5f, 0));

////            }
////            if (horizontalInput == -1)
////            {
////                //print("hi");
////                //print(yR);
////                rigidbodyComponent.rotation *= Quaternion.Euler(new Vector3(0, -0.5f, 0));
////            }
////        }
////        if (horizontalInput == 0 || count == 0)
////        {
////            rigidbodyComponent.rotation = new Quaternion(xR, yR, zR, wR);
////        }
////        print("rigidbodyComponent.velocity");
////        print(rigidbodyComponent.velocity);
////        print(transform.forward);
////        if (forward == true )
////        {
////            backward = false;
////            rigidbodyComponent.velocity = transform.forward * 19 + new Vector3(0, rigidbodyComponent.velocity.y, 0);
////        }
////        else if (backward == true)
////        {
////            forward = false;
////            rigidbodyComponent.velocity = transform.forward * -19 + new Vector3(0, rigidbodyComponent.velocity.y, 0);
////        }
////        //else if (forward < 0)
////        //{
////        //    rigidbodyComponent.velocity = transform.forward * -7 + new Vector3(0, rigidbodyComponent.velocity.y, 0);
////        //}
////        else
////        {
////            rigidbodyComponent.velocity = new Vector3(0, rigidbodyComponent.velocity.y, 0);
////        }


////        if (jumpKeyWasPressed)
////        {
////            rigidbodyComponent.AddForce(Vector3.up * 7, ForceMode.VelocityChange);
////            jumpKeyWasPressed = false;
////        }

////    }
//}