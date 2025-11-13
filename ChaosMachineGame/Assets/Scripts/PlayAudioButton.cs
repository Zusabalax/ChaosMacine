using UnityEngine;

public class PlayAudioButton : MonoBehaviour
{
   


    public void PlayButtonSound(string name)
    {
        SoundControler.Instance.PlayAudio(name);    
    }

   
}
