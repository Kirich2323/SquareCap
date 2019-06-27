using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Booster Scriptable", menuName = "Booster Scriptable", order = 2)]
public class BoosterScriptable : ScriptableObject {
    public List<Sprite> boosterSprites;
    public float UntouchableChangeColorCooldown = 0.5f;
    public float UntouchableTime = 3.0f;
    public GameEvent BoosterEvent;
}
