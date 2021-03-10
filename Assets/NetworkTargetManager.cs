using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class NetworkTargetManager : NetworkManager{

    public override void OnClientConnect(NetworkConnection conn){
        Debug.Log("client connected");
    }
}
