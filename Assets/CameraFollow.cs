using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class CameraFollow : MonoBehaviour
 {
     public Transform playerTransform;
 
    //code to follow beind the player
     // Update is called once per frame
     void Update()
     {
         if(playerTransform != null)
         {
             //move camera to the back of the player
             transform.position = playerTransform.position - playerTransform.forward*4.0f + playerTransform.up*1.0f;
             //look at the player
             transform.LookAt(playerTransform.position);
         }
     }
 
     public void setTarget(Transform target)
     {
         playerTransform = target;
     }
 }