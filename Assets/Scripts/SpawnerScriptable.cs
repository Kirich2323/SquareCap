using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Spawner Scriptable", menuName = "Spawner Scriptable", order = 2)]
public class SpawnerScriptable : ScriptableObject {
    public List<ScriptableObject> ScriptableTypes;
    public int spawnCooldown;
    public List<float> ScriptableProbabilities;
    public GameObject ObjectToSpawn;
    public string SpawnedObjectType;
}
