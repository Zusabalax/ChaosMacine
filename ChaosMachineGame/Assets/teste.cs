using System.Collections.Generic;
using UnityEngine;

public class teste : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  
    

     
    void Start()
    {
       
     
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate( 1*Time.deltaTime,0,0);
        gameObject.transform.Rotate(0, 90*Time.deltaTime, 0);
      
    }
}
