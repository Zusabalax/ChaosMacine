using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AlgoritimoGargula : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D Gargula;
    private bool Cd;

    [SerializeField]
    private float Forca, CDtime;

    
            

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cd = true;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Cd)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Gargula.AddForce(Vector2.up * Forca, ForceMode2D.Impulse);
                StartCoroutine(CD());
            }

    }
    IEnumerator CD()
    {
        Cd = false;
        yield return new WaitForSeconds(CDtime);
        Cd = true;
    }

    public void GargulaFly()
    {
        if (Cd && Gargula.gameObject.activeSelf)
        {
               Gargula.AddForce(Vector2.up * Forca, ForceMode2D.Impulse);
                StartCoroutine(CD());
            Debug.Log("GargulaButao");
        }
    }

}
