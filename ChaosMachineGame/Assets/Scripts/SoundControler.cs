
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Audio;
using UnityEngine.UI;



public class SoundControler : MonoBehaviour
{
  public static SoundControler Instance;
    [SerializeField]
    public List<Sound> List;
    [SerializeField]
    public Image Image;

    private bool mute;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);


        SetupAudioSource();
        mute = false;
    }
    private void Start()
    {
        foreach (Sound som in List)
        {
          if(som.IsMusic)
            {
                som.AudioSource.Play();
                return;

            }
                
        }
    }

    private void SetupAudioSource()
    {
        foreach (Sound som in List)
        {
            som.AudioSource = gameObject.AddComponent<AudioSource>();
            som.AudioSource.clip = som.AudioClip;
            som.AudioSource.volume = som.SoundVolume;
            som.AudioSource.pitch = som.SoundPitch;
            som.AudioSource.playOnAwake = som.IsMusic;
            som.AudioSource.loop = som.IsMusic;
        }
        // Adiciona o componente se necessário

        // Configura as propriedades

    }
    public void QuitGamE()
    {
        Application.Quit();
    }

    public void PlayAudio(string name)
    {
        foreach(Sound som in List)
        {
            if(som.SoundName==name)
            {
                som.AudioSource.Play();
            }
        }
    }

    public void StopAudio(string name)
    {
        foreach (Sound som in List)
        {
            if (som.SoundName == name)
            {
                som.AudioSource.Stop();
            }
        }
    }

    public void Mute()
    {
        mute=!mute; 
        if(!mute)
            Image.color = Color.green;
        else
            Image.color = Color.red;

        foreach (Sound som in List)
        {
            
                som.AudioSource.mute=mute;
           
        }
    }
 [System.Serializable]
public class Sound 
{

        public string SoundName;

        public AudioClip AudioClip;

        [UnityEngine.RangeAttribute(0f, 1f)]
        public float SoundVolume = 1f;

        [UnityEngine.RangeAttribute(0.1f, 3f)]
        public float SoundPitch = 1f;

        [HideInInspector]
        public AudioSource AudioSource;


        public bool IsMusic;
        public Sound( AudioClip audioClip,string name, float volume, float pitch, bool isMusic)
        {
                SoundName = name;
                SoundVolume = volume;
                SoundPitch = pitch;
                IsMusic = isMusic;
                AudioClip = audioClip;


           
        }
      

        public Sound() { }

      //  Método para configurar o AudioSource
         



    }
}
