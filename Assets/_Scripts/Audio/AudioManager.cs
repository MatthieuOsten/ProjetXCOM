using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
    public class Aliase 
    {
        
        public string name;
        public string description;
        public AudioMixerGroup MixerGroup;
        public AudioClip[] audio;

        public bool bypassEffects;
        public bool bypassListenerEffects;
        public bool bypassReverbZones;
        [Range(0,256)]
        public float priority;
        [Range(0,1)]
        public float volume = 0.8f;
        public bool isLooping;
        [Range(-3,3)]
        public float minPitch = 1f;
        [Range(-3,3)]
        public float maxPitch = 1.01f;
        [Range(-1,1)]
        public float stereoPan = 0;
        [Range(0,1)]
        public float spatialBlend = 0;
        
        [Range(0,1.1f)]
        public float reverbZoneMix = 1;
        [Header("3D Sound Settings")]
        [Range(0,5)]
        public float dopplerLevel = 1;
        [Range(0,360)]
        public float Spread = 1;
        [Range(0,10000)]
        public float MinDistance = 1;
        [Range(0,10000)]
        public float MaxDistance = 500;
        public AudioRolloffMode CurveType = AudioRolloffMode.Logarithmic;
        public AnimationCurve distanceCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1), new Keyframe(1, 0) });

        [Header("Subtitle")]
        public string Text;
        public float customDuration;

        public bool isInit;
        public bool isPlaholder = true;

        public Aliase ()
        {
            name = "newAliase";
            volume = 0.8f;
            minPitch = 1f;
            maxPitch = 1.01f;
            reverbZoneMix = 1;
            dopplerLevel = 1;
            Spread = 1;
            MinDistance = 1;
            MaxDistance = 500;
        }

    }

//[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Util;

    [SerializeField] static Aliases[] aliasesArray = new Aliases[0];
    [SerializeField] List<GameObject> audioSource;

    [SerializeField] int audioSourcePoolSize = 32; // 32 is a good start
    [Header("Debug")] 
    [SerializeField]  Aliases[] TableAliasesLoaded = new Aliases[0];

    bool _isPaused;


    public bool IsPaused
    {
        set{ _isPaused = value;}
    }
    // Add aliases to load
    public static void AddAliases(Aliases newAliases)
    {
        // Check if not already exist
        for(int i = 0 ; i < aliasesArray.Length;i++)
        {
            if(aliasesArray[i] == newAliases)
            {
                Debug.Log("AudioManager : L'aliases a déja était ajouté");
                return;
            }    
        }
        // We can add it
        Aliases[] TempAliasesArray = new Aliases[aliasesArray.Length+1];
        for(int i = 0 ; i < aliasesArray.Length;i++)
        {
            TempAliasesArray[i] = aliasesArray[i];
        }
        TempAliasesArray[TempAliasesArray.Length-1] = newAliases;
        aliasesArray = TempAliasesArray;
        Debug.Log("AudioManager : Aliases added");
    }

    void Awake()
    {
        //if(Util == null)
        {
            Util = this;
           // DontDestroyOnLoad(gameObject);
            InitAudioSources();
        }      
        //else
         //   Destroy(gameObject);


        
    }

    void InitAudioSources()
    {
        audioSource = new List<GameObject>();

         for(int i = 0 ; i < audioSourcePoolSize; i++)
        {
            GameObject newAudioSource = new GameObject("Audio Source "+i);
            newAudioSource.transform.SetParent(transform);
            AudioSource audioS = newAudioSource.AddComponent<AudioSource>();
            Util.audioSource.Add(newAudioSource);
            newAudioSource.SetActive(false);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Util != this)
            Util = this;   

        TableAliasesLoaded = aliasesArray;    

        if(_isPaused)
        {
            PauseAllAudio();
        }
        
        else
        {
            UnPauseAllAudio();
            DisableInusedAudioSource();
        }
        
    }

    void DisableInusedAudioSource()
    {
        foreach(GameObject aS in audioSource)
        {
            if(!aS.GetComponent<AudioSource>().isPlaying)
            {
                aS.SetActive(false);
            }     
        }
    }

    public static Aliase GetSoundByAliase(string name)
    {
        for(int i = 0 ; i < aliasesArray.Length; i++)
        {
            foreach(Aliase alias in aliasesArray[i].aliases )
                if(alias.name == name)
                    return alias ;
        }

        Debug.LogWarning("AudioManager : Aliase: "+name+" not found.");
        return null;
    }

    static AudioSource GetAudioSource()
    {
        foreach(GameObject aS in Util.audioSource)
        {  
            AudioSource audio = aS.GetComponent<AudioSource>();
            if(!audio.isPlaying)
            {
                return audio;
            }  
        }
        return null;
    }
    public static void PlayMultiplesAtPosition(string[] aliasesName, Vector3 position, bool waitFinish = true)
    {

    }

    public static void PauseAllAudio()
    {
        foreach(GameObject aS in Util.audioSource )
        {
            AudioSource audio = aS.GetComponent<AudioSource>();
            if(audio.isPlaying)
            {
                audio.Pause();
            }  
        }
    }
    public static void UnPauseAllAudio()
    {
        foreach(GameObject aS in Util.audioSource )
        {
            AudioSource audio = aS.GetComponent<AudioSource>();
            //if(audio.UnPause)
            {
                audio.UnPause();
            }  
        }
    }
    public static Aliase PlaySoundAtPosition(string aliaseName, Vector3 position)
    {
        if( aliaseName == "" || aliaseName == null)
        {
            Debug.LogError("AudioManager : Un son a voulu être jouer sans d'aliaseName, il faut en assigné un dans le script qui a exécuté la function");
            return null;
        }

        Aliase clip = GetSoundByAliase(aliaseName);
        if( clip == null || clip.audio.Length == 0)
        {
            Debug.LogError("AudioManager : Aliase: "+aliaseName+" contains no sounds.");
            return null;
        }
        if(clip.isPlaholder)
        {
            Debug.LogWarning("Un son placeholder a été jouer, il faut le changer , nom de l'aliase "+aliaseName );
        }

        AudioSource audioS = GetAudioSource();
        if(audioS == null)
        {
            Debug.LogWarning($"AudioManager : Limits exceded for audioSource, maybe you need to increase your audioSourcePoolSize (Size = {Util.audioSourcePoolSize})");
            return null;
        }
        audioS.volume = clip.volume;
        audioS.loop = clip.isLooping;
        audioS.pitch = Random.Range(clip.minPitch, clip.maxPitch);

        audioS.spatialBlend = clip.spatialBlend;
        if(clip.MixerGroup != null)
            audioS.outputAudioMixerGroup = clip.MixerGroup;

        switch(clip.CurveType)
        {
            case AudioRolloffMode.Logarithmic:
            case AudioRolloffMode.Linear:
                audioS.rolloffMode = clip.CurveType;
            break;
            case AudioRolloffMode.Custom:
                audioS.rolloffMode = clip.CurveType;
               audioS.SetCustomCurve(AudioSourceCurveType.CustomRolloff, clip.distanceCurve);  
            break;

        }
        audioS.gameObject.transform.position = position;
        audioS.gameObject.SetActive(true);
        int index = Random.Range(0,clip.audio.Length);
        if(clip.isLooping)
        {
            audioS.clip = clip.audio[index];
            audioS.Play();

        }
        else
        {
            audioS.PlayOneShot(clip.audio[index], clip.volume);
             //audioS.clip = clip.audio[0];
             //audioS.Play();
            //UIManager.CreateSubtitle(clip.Text, clip.audio[index].length);
            
        }
        return clip;
        

    } 
    void PlayLoopSound(string aliaseName, Vector3 position, GameObject attachedTo)
    {
        
    }

    

    
}



