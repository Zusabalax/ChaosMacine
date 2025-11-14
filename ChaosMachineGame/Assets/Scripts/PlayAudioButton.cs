using UnityEngine;

public class PlayAudioButton : MonoBehaviour
{
    
 


    public void PlayButtonSound(string nameSound)
    {
        SoundControler.Instance.PlayAudio(nameSound);    
    }

   
}
