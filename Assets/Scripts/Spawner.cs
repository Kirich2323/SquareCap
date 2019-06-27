using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public SpawnerScriptable spawnerScriptable;

    private float lastSpawnTime = -1;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Time.time - lastSpawnTime > spawnerScriptable.spawnCooldown) {
            Spawn();
        }
    }

    void Spawn() {
        Vector2 pos = FindObjectOfType<GameManager>().GetRandomPosition();
        GameObject currentObject = Instantiate(spawnerScriptable.ObjectToSpawn, new Vector3(pos.x, pos.y, 0.0f), Quaternion.identity);

        if (spawnerScriptable.SpawnedObjectType == "Enemy") {
            currentObject.GetComponent<EnemyBehaviour>().SetEnemyScriptable(getObjectScriptable());
        }
        else if (spawnerScriptable.SpawnedObjectType == "Booster") {
            currentObject.GetComponent<BoosterBehaviour>().SetBoosterScriptable(getObjectScriptable());
        }
        lastSpawnTime = Time.time;
    }

    ScriptableObject getObjectScriptable() {
        float p = Random.Range(0.0f, 1.0f);
        float v = 0.0f;
        int idx = 0;
        foreach (var i in spawnerScriptable.ScriptableProbabilities) {
            v += i;
            if (v >= p) {
                return spawnerScriptable.ScriptableTypes[idx];
            }
            idx++;
        }
        return null;
    }
}
