using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Scriptable", menuName = "Enemy Scriptable", order = 2)]
public class EnemyScriptable : ScriptableObject {

    //public List<Sprite> EnemySprites;
    public List<ArmorScriptable> sprtieTypes;
    public int ArmorAmount;
    public float UntouchableChangeColorCooldown = 0.5f;
    public float UntouchableTime = 3.0f;
    public float TurnProbability = 0.15f;

    public List<int> StateToDamage;
    public List<float> ArmorProbability;
    public List<int> ArmorToReward;
    public List<int> StateToSpeed;

    public GameObject ExplosionEffect;
}
