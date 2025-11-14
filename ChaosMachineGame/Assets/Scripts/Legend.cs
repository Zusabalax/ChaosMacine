using System.Collections;
using TMPro;

using UnityEngine;

public class Legend : MonoBehaviour
{
  
    [TextAreaAttribute]
    public  string _legendText;

    [SerializeField]
   private  TextMeshProUGUI _legendTextUI;

    [SerializeField]
    private float textSpeed ;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      StartCoroutine(StartLegend());
    }
    IEnumerator StartLegend()
    {
        _legendTextUI.text = "";
        yield return new WaitForSeconds(1);
        SoundControler.Instance.PlayAudio("Narrative");
        foreach (char letter in _legendText.ToCharArray())
        {
            _legendTextUI.text += letter;
            
           


            yield return new WaitForSeconds(1f / textSpeed);
        }

       SceneTransitionManager.Instance.LoadScene("Fase 1");
    }

    // Update is called once per frame
  
}
