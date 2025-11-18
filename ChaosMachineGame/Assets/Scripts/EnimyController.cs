using System.Collections;

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

    [SerializeField]
    int health;


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            health--;
            StartCoroutine(Dammmaaage());
            if (health <= 0)
            {

                if (finalBoss)
                {
                    EconomyManager.Instance.AddCurrency(100);
                    SceneTransitionManager.Instance.LoadScene("WIN");
                }
                else if (!nomalEnimy && !finalBoss)
                {
                    EconomyManager.Instance.AddCurrency(50);
                    SceneTransitionManager.Instance.LoadScene("Fase 2");
                }
                else
                    EconomyManager.Instance.AddCurrency(10);






                Destroy(gameObject);

            }
        }
        if (nomalEnimy)
        {
            if (collision.gameObject.CompareTag("Final"))
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            health--;
            StartCoroutine(Dammmaaage());
            if (health <= 0)
            {

                if (finalBoss)
                {
                    EconomyManager.Instance.AddCurrency(100);
                    SceneTransitionManager.Instance.LoadScene("WIN");
                }
                else if (!nomalEnimy && !finalBoss)
                {
                    EconomyManager.Instance.AddCurrency(50);
                    SceneTransitionManager.Instance.LoadScene("Fase 2");
                }
                else
                    EconomyManager.Instance.AddCurrency(10);





               
                Destroy(gameObject);

            }
        } 
        if (nomalEnimy)
        {
            if (collision.gameObject.CompareTag("Final"))
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator Dammmaaage()
    {
      gameObject.GetComponent<SpriteRenderer>().color = Color.red;


        yield return new WaitForSeconds(1);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;


    }


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
