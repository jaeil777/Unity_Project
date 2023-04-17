using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public float rotateSpeed;
    
  
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up* rotateSpeed*Time.deltaTime,Space.World);
    }
    private void OnTriggerEnter(Collider other)
    {
       
    }
}
