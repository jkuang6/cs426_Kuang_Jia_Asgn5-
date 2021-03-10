using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Target : ManagedBehaviour
{
    //code for he target the player needs to hit.
    [SyncVar]
    public bool isActive = true;
    // Start is called before the first frame update
    [SyncVar]
    public string address="Placeholder";

    public float gravityDirection = -1;

    public Text addressText;
    public Canvas addressCanvas;
    public Rigidbody rb;

    void Start(){  
        //get the canvas and text attachd to this.  assumes only one
        GameObject txt = transform.Find("Canvas").Find("Text").gameObject;
        addressText = txt.GetComponent<Text>();
        addressCanvas = transform.Find("Canvas").gameObject.GetComponent<Canvas>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate(){ 
        //rotate text to look at the player camera. Don't know if this works with networking
        addressText.text = address;
        addressCanvas.transform.LookAt(Camera.main.transform);
        if(isLoaded){
            rb.AddForce(targetManager.getGravity()*5.0f);
        } else{
            rb.AddForce(new Vector3(0, -50.0f, 0));
        }

    }

    public void invertGravityDirection(){
        gravityDirection = -gravityDirection;
    }

    public void setAddress(string newAddress){
        //show the address (assigned in targetmanager) as a floating text above the object
        address = newAddress;
    }

    public string getAddress(){
        return address;
    }

    public void setInactive(){
        //will turn off the gravity inversion
        isActive = false;
    }

    public void setActive(){
        isActive = true;
    }


}
