using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Scriptable", menuName = "Player Scriptable", order = 1)]
public class PlayerScriptable : ScriptableObject {
    public List<int> StateToSpeed;
    public int MaxHealth;
}
