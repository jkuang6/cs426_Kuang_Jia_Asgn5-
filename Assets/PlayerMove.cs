using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerMove : ManagedBehaviour
{
    public float speed = 1000000.0f;
    public float rotationSpeed = 90;
    public float force = 0.01f;
    public float invertCameraSpeed = .5f;

    public float cameraRotaton = 0f;

    Rigidbody rb;
    Transform t;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        t = GetComponent<Transform>();
    }

    void applyGravity(){
        rb.AddForce(10*targetManager.getGravity());
    }

    float gravityDirection(){
        return targetManager.getGravity().normalized.y;
    }

    void FixedUpdate(){
        if (!isLocalPlayer || !isLoaded)
            return;

        //apply gravity here since Update is called a different # of times on the client and server
        float gY = gravityDirection();
        //if gravit has flipped, rotate around the z axis and disable controls so it doesn't break stuff
        if(t.up.y != -gY & (Mathf.Abs(t.up.y + gY) > .001)){
            t.RotateAround(t.position, this.transform.forward, 5);
            //make it fall faster
            rb.AddForce(25*targetManager.getGravity());
        } 

        applyGravity();
    }

    void Update()
    {
        if (!isLocalPlayer || !isLoaded)
            return;

        float gY = gravityDirection();

        //check if gravity just inverted
        if(t.up.y != -gY & (Mathf.Abs(t.up.y + gY) > .001)){
            //don't doo stuff while falling and flipping because that makes controlls weird
        } else{
            //otherwise, do the movement stuff
            if (Input.GetKey(KeyCode.D)){
                t.rotation *= Quaternion.Euler(0, -gY * rotationSpeed * Time.deltaTime, 0);
            }
            else if (Input.GetKey(KeyCode.A)){
                t.rotation *= Quaternion.Euler(0, gY * rotationSpeed * Time.deltaTime, 0);
            }

            if (Input.GetKey(KeyCode.W))
                rb.velocity += this.transform.forward * speed * Time.deltaTime;
            else if (Input.GetKey(KeyCode.S))
                rb.velocity -= this.transform.forward * speed * Time.deltaTime;

            // Quaternion returns a rotation that rotates x degrees around the x axis and so on

            //No jumping - for some reason it likes to glitch out of the game
            // if (Input.GetKeyDown(KeyCode.Space))
            //     rb.AddForce(targetManager.getGravity() * -force/Time.deltaTime);
        }
    }

    public override void OnStartLocalPlayer(){
        //based on onine code, set the main camera target to follow the player
        GetComponent<MeshRenderer>().material.color = Color.red;
        Camera.main.GetComponent<CameraFollow>().setTarget(gameObject.transform);
    }
}