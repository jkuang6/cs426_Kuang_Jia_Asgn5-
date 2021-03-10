using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TargetManager : NetworkBehaviour
{
    public Target[] targets;
    public SyncListString addresses = new SyncListString();
    public SyncListInt addressOrder = new SyncListInt();
    public GameObject winObject;

    public GameObject winText;

    [SyncVar]
    public float gravityY = -10.0f;
    private int maxScore = 4; //dont include some of the targets
    // Start is called before the first frame update

    //This should probably be made into a singleton pattern is I use it for more than like 2 seconds of gameplay
    void Start()
    {
        //finds all objects with the Target script
        if(isServer){
            targets = (Target[]) (GameObject.FindObjectsOfType(typeof(Target)));

            //use these premade addresses, shuffles them, as assigns them to targets
            List<string> baseAddresses = new List<string> {"0xa1b2c3d4", "0x3A28214A","0x6339392C","0x7363682E","CPU","0x0000000","USB"};
            List<int> baseAddressOrder = new List<int> {3,1,0,2};

            //this will be the end order that the player needs to hit the targets in

            ShuffleString(baseAddresses);
            ShuffleInt(baseAddressOrder);

            int currAddressIdx = 0;
            foreach(var target in targets){
                string currAddress = baseAddresses[currAddressIdx];
                target.setAddress(baseAddresses[currAddressIdx]);

                if(currAddressIdx < maxScore){
                    addresses.Add(currAddress);
                    addressOrder.Add(baseAddressOrder[currAddressIdx]);
                }
                currAddressIdx += 1;
            }
        }
        postAddresses();
    }

    [Command]
    public void CmdInvertGravity(){
        gravityY = -1*gravityY;
        targets = (Target[]) (GameObject.FindObjectsOfType(typeof(Target)));
        foreach(Target target in targets){
            target.invertGravityDirection();
        }
        //Todo: move up/down billboard text
        StartCoroutine(MoveBillBoard());
    }

    IEnumerator MoveBillBoard(){
        int i = 0;
        Vector3 moveVector;
        if(gravityY > 0){
            moveVector = new Vector3(0, 7/2, 0);
        } else{
            moveVector = new Vector3(0,-7/2, 0);
        }
        while(i < 2){
            
            GameObject.Find("Billboard").gameObject.transform.Translate(moveVector);
            GameObject.Find("DirectionsBoard").gameObject.transform.Translate(moveVector);
            i++;
            yield return new WaitForSeconds(1);
        }
    }

    public Vector3 getGravity(){
        return new Vector3(0, gravityY, 0);
    }

    void postAddresses(){
        string billBoardText = "Instructions:\n";
        string directionBoardText = "";

        List<string> postAddresses = new List<string> (new string[addresses.Count+1]);

        for(int i=0; i< addressOrder.Count; i++){
            int addrIdx = addressOrder[i];
            postAddresses[addrIdx] = addresses[i];
        }

        for(int i=0; i < addresses.Count; i++){
            string addr = postAddresses[i];
            int addrIdx = addressOrder[i];
            billBoardText += addr;
            billBoardText += "\n";

            directionBoardText += "goto " + addrIdx + "\n";
        }
        GameObject billBoard = GameObject.Find("BillboardText").gameObject;
        billBoard.GetComponent<Text>().text = billBoardText;

        GameObject dirBoard = GameObject.Find("DirectionsBoardText").gameObject;
        dirBoard.GetComponent<Text>().text = directionBoardText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public SyncListString getAddresses(){
        //used to send addresses to the players
        return addresses;
    }

    [ClientRpc]
    public void RpcWin(int clientID){
        //todo: do stuff in a win event here
        targets = (Target[]) (GameObject.FindObjectsOfType(typeof(Target)));
        foreach(var target in targets){
            // target.setInactive();
            if(winObject != null){
                GameObject fish = (GameObject) Instantiate(winObject.gameObject, target.transform.position, target.transform.rotation);
                Destroy(target.gameObject);
                NetworkServer.Spawn(fish);
            }
        }
        GameObject billBoard = GameObject.Find("BillboardText").gameObject;
        billBoard.GetComponent<Text>().text = "Game over";

        winText.SetActive(true);
        GameObject wText = GameObject.Find("WinnerText").gameObject;

        if(wText != null){
            wText.GetComponent<Text>().text = "Player " + (clientID + 1) + " wins!";
        }
    }

    [ClientRpc]
    public void RpcUpdatePoints(int clientID, int playerScore){
        Text playerText;
        int playerNum = clientID + 1;
        string playerString = "Player " + playerNum;
        playerText = GameObject.Find("ScoreCanvas/"+playerString).GetComponent("Text") as Text;
        if(playerText != null)
            playerText.GetComponent<UnityEngine.UI.Text>().text = playerString + ": " + playerScore.ToString();
    }

    void ShuffleString (List<string> deck) {
        for (int i = 0; i < deck.Count; i++) {
            string temp = deck[i];
            int randomIndex = Random.Range(0, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    void ShuffleInt (List<int> deck) {
        for (int i = 0; i < deck.Count; i++) {
            int temp = deck[i];
            int randomIndex = Random.Range(0, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

}
