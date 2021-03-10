using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : ManagedBehaviour
{

    public float power;
    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = collision.transform.position - transform.position;
            rb.AddForce(direction.normalized * power, ForceMode.Impulse);
            MeshRenderer targetMesh = GetComponent<MeshRenderer>();
            StartCoroutine(FlashColor(targetMesh));
        }
        
    }

    IEnumerator FlashColor(MeshRenderer targetMesh)
    {
        if (targetMesh.material.color != Color.black)
        {
            Color currColor = targetMesh.material.color;
            targetMesh.material.color = Color.black;
            audioSource.Play();
            yield return new WaitForSeconds(1);
            targetMesh.material.color = currColor;
        }
    }
}
