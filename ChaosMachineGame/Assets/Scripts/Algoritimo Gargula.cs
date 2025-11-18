using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AlgoritimoGargula : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D Gargula;
    private bool cd;//////chupra cu
        
            
    [SerializeField]
    private float Forca, cdtime;

    
            

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cd = true;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (cd)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Gargula.AddForce(Vector2.up * Forca, ForceMode2D.Impulse);
                StartCoroutine(cd2());
            }


    }
    IEnumerator cd2()
    {
        cd = false;
        yield return new WaitForSeconds(cdtime);
        cd = true;
    }

    public void GargulaFly()
    {
        if (cd && Gargula.gameObject.activeSelf)
        {
               Gargula.AddForce(Vector2.up * Forca, ForceMode2D.Impulse);
                StartCoroutine(cd2());
            Debug.Log("GargulaButao");
        }
    }

}
