using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTest : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnerPoint;
    [SerializeField] private List<Transform> _spawnerCharacter;
    [SerializeField] private CameraShoulder cameraShoulder;
    [SerializeField] private CameraIsometric cameraIsometric;
    [SerializeField] private GameObject _enemyCamera;
    [SerializeField] public GameObject _character;

    // Start is called before the first frame update
    void Start()
    {
       Spawning();
    }

    private void Spawning()
    {
        for (int j = 0; j < _spawnerCharacter.Count; j++)
        {
            GameObject c = Instantiate(_character, _spawnerCharacter[_spawnerCharacter.Count - 1].position, Quaternion.identity);
            cameraIsometric.CharacterPlayer.Add(c);
        }

        for (int i = 0; i < _spawnerPoint.Count; i++)
        {
            GameObject e = Instantiate(_enemyCamera, _spawnerPoint[_spawnerPoint.Count - 1].position, Quaternion.identity);
            cameraShoulder.Enemy.Add(e);
        }
    }
}
