using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class ParticleManager : MonoBehaviour
{
     private static ParticleManager _instance = null;

    // public GameObject[] FXExplosion;
    // public GameObject[] FXExplosionKamikaze;
    // public GameObject[] FXMuzzleFlash;
    // public GameObject FXTrail;

    [SerializeField] int poolFXSize = 32;
    [SerializeField] int poolFXTrailSize = 16;

    [Range(1,15)]
    [SerializeField] float SpeedTrailTo = 5;
    [Header("POOL")]
    [SerializeField] Queue<GameObject> FXpool;
    [SerializeField] List<GameObject> FXTrailPool;
     [SerializeField] List<Transform> FXTrailPoolTarget;



    public static ParticleManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ParticleManager>();
                // Si vrai, l'instance va étre crée
                if(_instance == null) 
                {
                    var newObjectInstance = new GameObject();
                    newObjectInstance.name = typeof(ParticleManager).ToString();
                    _instance = newObjectInstance.AddComponent<ParticleManager>();
                }
            }
            return _instance;
        }
    }

     public void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = GetComponent<ParticleManager>();
        if(_instance == null)
            return;

        InitVFX();
        //InitParticleSystem();

    }

    void InitVFX()
    {
        FXpool = new Queue<GameObject>();

         for(int i = 0 ; i < poolFXSize; i++)
        {
            GameObject newVFXComponent = new GameObject("Visual Effect "+i);
            newVFXComponent.transform.SetParent(transform);
            VisualEffect audioS = newVFXComponent.AddComponent<VisualEffect>();
            FXpool.Enqueue(newVFXComponent);
            newVFXComponent.SetActive(false);
        }
    }
    void InitParticleSystem()
    {
        FXTrailPool = new List<GameObject>();

         for(int i = 0 ; i < poolFXTrailSize; i++)
        {
            GameObject newVFXComponent = new GameObject("Visual Effect "+i);
            newVFXComponent.transform.SetParent(transform);
            ParticleSystem audioS = newVFXComponent.AddComponent<ParticleSystem>();
            //FXTrailPool.Enqueue(newVFXComponent);
            newVFXComponent.SetActive(false);
        }
    }



    
    static VisualEffect GetVisualEffect()
    {
            GameObject LastElem = Instance.FXpool.Peek();
            VisualEffect audio = LastElem.GetComponent<VisualEffect>();
            Instance.FXpool.Dequeue();
            Instance.FXpool.Enqueue(LastElem);
            return audio;
       
    }

    static ParticleSystem GetParticleSystem()
    {
            // GameObject LastElem = Instance.FXTrailPool.Peek();
            // ParticleSystem audio = LastElem.GetComponent<ParticleSystem>();
            // Instance.FXTrailPool.Dequeue();
            // Instance.FXTrailPool.Enqueue(LastElem);
            // return audio;
       return null;
    }

    public static void PlayFXAtPosition(Vector3 position, VisualEffectAsset[] fxs)
    {   
        if(fxs.Length == 0 )
        {
            return;
        }
        // GameObject Fx = Instantiate(fxs[Random.Range(0, fxs.Length)], position,Quaternion.identity);
        // Instance.FXpool.Add(Fx);
        PlayFXAtPosition(position, fxs[Random.Range(0, fxs.Length)]);

        
    }
    public static void PlayFXAtPosition(Vector3 position, VisualEffectAsset fx)
    {   
        if(fx == null)
            return;

        VisualEffect vfx = GetVisualEffect();
        vfx.visualEffectAsset = fx;
        vfx.gameObject.SetActive(true);
        vfx.gameObject.transform.position = position;
        //GameObject Fx = Instantiate(fx, position,Quaternion.identity);
        //Instance.FXpool.Add(Fx);
        // if(Fx.TryGetComponent<VisualEffect>(out VisualEffect compvfx))
        // {
        //     compvfx.Play();
        // }
        
    }

    public static void PlayTrailFXto(GameObject fxToPlay ,Vector3 startPosition ,Transform target)
    {   
        if(fxToPlay == null || target == null)
            return;

        GameObject Fx = Instantiate(fxToPlay, startPosition,Quaternion.identity);
        Fx.transform.LookAt(target);
        Instance.FXTrailPool.Add(Fx);
        Instance.FXTrailPoolTarget.Add(target);
        // ParticleSystem psGO = fxToPlay.GetComponent<ParticleSystem>();


        // ParticleSystem vfx = GetParticleSystem();

        // vfx. = psGO.main;
        // vfx.gameObject.SetActive(true);
        // vfx.gameObject.transform.position = position;
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(GameManager.GameState != State.InGame )
        //      return;

        // for(int i = 0 ; i < FXpool.Count; i++)
        // {
        //     //Debug.Log(FXpool[i].GetComponent<ParticleSystem>().isPlaying);
        //     if(FXpool[i] != null)
        //     {
        //         if(FXpool[i].TryGetComponent<ParticleSystem>(out ParticleSystem compo) ) 
        //         {
        //             if(!compo.isPlaying)
        //             {
        //                 Destroy(FXpool[i]);
        //                 FXpool.Remove(FXpool[i]);
        //             }  
        //         }
        //         if(FXpool[i].TryGetComponent<VisualEffect>(out VisualEffect compvfx))
        //         {
                    
        //             if(compvfx.pause)
        //             {
                       
        //                 Destroy(FXpool[i]);
        //                 FXpool.Remove(FXpool[i]);
        //             }
        //         }
        //     }
        // }


        for(int i = 0 ; i < FXTrailPool.Count; i++)
        {
            if(FXTrailPool[i] == null || FXTrailPoolTarget[i] == null)
            {
                Destroy(FXTrailPool[i]);
                FXTrailPool.Remove(FXTrailPool[i]);
                FXTrailPoolTarget.Remove(FXTrailPoolTarget[i]);
                return;
            }
            if(Vector3.Distance(FXTrailPool[i].transform.position, FXTrailPoolTarget[i].position) < 1f)
            {
                Debug.Log("oh");
                Destroy(FXTrailPool[i]);
                FXTrailPool[i].GetComponent<ParticleSystem>().Stop(true);
            }      //
            else
            {
                Debug.Log("oh");
                FXTrailPool[i].transform.position = Vector3.Lerp(FXTrailPool[i].transform.position, FXTrailPoolTarget[i].position, 5*Time.deltaTime);
            }   
            // if(!FXTrailPool[i].GetComponent<ParticleSystem>().isPlaying)
            // {
            //     Destroy(FXTrailPool[i]);
            //     FXTrailPool.Remove(FXTrailPool[i]);
            //     FXTrailPoolTarget.Remove(FXTrailPoolTarget[i]);
                 
            // } 
        }
    }
}
