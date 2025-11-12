using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using DG.Tweening;
using UnityEngine.InputSystem;


public class PlayerControl : MonoBehaviour

{
    public static bool fly;
    public static bool Jump;
    [SerializeField]
    private Rigidbody2D Player;       
    [SerializeField]
    private float Forca,FlyForce, Jumptime;
    [SerializeField]
    private float speed;
   


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Jump = false;
        fly = false;
           

    }

    // Update is called once per frame
    void Update()
    {
        if (!fly)
        {
                     

            if (Jump)
            {


                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Player.AddForce(Vector2.up * Forca, ForceMode2D.Impulse);
                    Jump = false;
                }
            }

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Player.AddForce(Vector2.up * FlyForce, ForceMode2D.Impulse);
                StartCoroutine(Cdfly()); 
                
                
            }


        }
        Player.AddForce(Vector2.right * speed, ForceMode2D.Force);
    }
    IEnumerator Cdfly()
    {
        Jump = false;
        yield return new WaitForSeconds(Jumptime);
        Jump = true;
    }


}
