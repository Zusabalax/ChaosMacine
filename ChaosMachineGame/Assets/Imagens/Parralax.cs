using UnityEngine;

public class Parralax : MonoBehaviour


    
{
    [SerializeField]
    private float Movespped;
    [SerializeField]
    private Transform Startposition;
    [SerializeField]
    private Transform Endposition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(-Movespped * Time.deltaTime, 0, 0);

        if (this.transform.position.x <= Endposition.transform.position.x)
            this.transform.position = Startposition.position;
    }
}
