using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnOff : MonoBehaviour
{
    public GameObject cube;
    bool status;
    // Start is called before the first frame update
    void Start()
    {
        //cube = GetComponent<GameObject>();
        status = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (status)
        {
            status = false;
            cube.SetActive(false);
        }
        else
        {
            status = true;
            cube.SetActive(true);
        }

    }
}
