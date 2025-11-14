using System.Collections;
using UnityEditor.Animations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnimyController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletAtak;

    [SerializeField]
    private Transform skillPoint;

    [SerializeField]
    private float speed,timePatrol;

    [SerializeField]
    private Animator animator;

    private float timeToAtck ;

    [SerializeField]
    private bool finalBoss, nomalEnimy;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
       if(!nomalEnimy)
        {
            StartCoroutine(Atak());
            StartCoroutine(ChangeDirection());
        }
           
       
    
    }

    void Update()
    {
   

        transform.Translate(Vector2.left * speed * Time.deltaTime);

    }

    IEnumerator Atak()
    {
        while (true)
        {
            // Lógica de ataque aqui
            timeToAtck = Random.Range(1,5);
            yield return new WaitForSeconds(timeToAtck); // Espera 2 segundos entre ataques
            if(finalBoss)
            {
                animator.SetBool("Atak", true);
            }
           
            yield return new WaitForSeconds(0.5f);
            Instantiate(bulletAtak, skillPoint.position, skillPoint.rotation);
            if (finalBoss)
            {
                animator.SetBool("Atak", false);
            }


        }
    }

    IEnumerator ChangeDirection()
    {
        while (true)
        {
            // Lógica de ataque aqui
            yield return new WaitForSeconds(timePatrol); // Espera 2 segundos entre ataques

           
           transform.Rotate(0,180,0);
            
        }
    }
}
