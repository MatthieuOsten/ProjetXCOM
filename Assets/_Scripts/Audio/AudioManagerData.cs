using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioManager Data", menuName = "Audio/Audio Manager Data", order = 2)]

public class AudioManagerData : ScriptableObject
{
    
    public List<Aliase> aliases;
    public Dictionary<string, Queue<Aliase>> aliasesDictionnary;

    private void OnValidate() {
           
    }
    
}
