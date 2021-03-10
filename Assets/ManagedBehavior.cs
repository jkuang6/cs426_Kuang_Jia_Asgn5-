using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ManagedBehaviour : NetworkBehaviour
{

    public TargetManager targetManager;
    public bool isLoaded = false;

    public override void OnStartClient(){
        StartCoroutine(WaitForTargetManager());
    }

    private IEnumerator WaitForTargetManager(){
        while(GameObject.FindObjectOfType(typeof(TargetManager)) == null){
            yield return null;
        }
        targetManager = (TargetManager) GameObject.FindObjectOfType(typeof(TargetManager));
        isLoaded = true;
    }
}
