using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
    private GameManager.State currentState;
    private SpriteRenderer spriteRenderer;

    private EnemyScriptable enemyScriptable;
    private int armor;
    private bool isTouchable = true;
    private float timeInUntouchableState = 0.0f;
    private bool isGrey = true;
    private Vector2 direction;
    private int reward;

    void Awake() {
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        gameObject.tag = "Enemy";
        direction = RandomNewDirection();
    }

    void Update() {
        if (armor <= 0) {
            die();
            destroyItself();
            return;
        }

        if (!isTouchable) {
            float newTime = timeInUntouchableState + Time.deltaTime;
            if (Mathf.Floor(newTime / enemyScriptable.UntouchableChangeColorCooldown) > Mathf.Floor(timeInUntouchableState / enemyScriptable.UntouchableChangeColorCooldown)) {
                timeInUntouchableState -= enemyScriptable.UntouchableChangeColorCooldown;
                if (isGrey) {
                    SetSprite(currentState);
                    isGrey = false;
                }
                else {
                    SetSprite(GameManager.State.Grey);
                    isGrey = true;
                }
            }
            timeInUntouchableState = newTime;
            if (timeInUntouchableState >= enemyScriptable.UntouchableTime) {
                SetSprite(currentState);
                timeInUntouchableState = 0.0f;
                isTouchable = true;
            }
        }
        else {
            if (Random.Range(0f, 1.0f) < enemyScriptable.TurnProbability) {
                direction = RandomNewDirection();
            }
            Move(direction);
        }
    }

    public void SetEnemyScriptable(ScriptableObject scriptable) {
        enemyScriptable = scriptable as EnemyScriptable;
        armor = RandomArmor();
        reward = enemyScriptable.ArmorToReward[armor - 1];
        RandomState();
        MakeUntouchable();
    }

    void ChangeState(GameManager.State state) {
        SetSprite(state);
        currentState = state;
    }

    void Move(Vector2 dir) {
        dir *= Time.deltaTime * enemyScriptable.StateToSpeed[(int)currentState];
        Vector3 newPos = new Vector3(transform.position.x + dir.x, transform.position.y + dir.y, transform.position.z);
        if (FindObjectOfType<GameManager>().IsInBoundaries(newPos)) {
            transform.position = new Vector3(transform.position.x + dir.x, transform.position.y + dir.y, transform.position.z);
        }
        else {
            direction = RandomNewDirection();
        }
    }

    Vector2 RandomNewDirection() {
        return new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
    }

    void SetSprite(GameManager.State state) {
        switch (state) {
            case GameManager.State.Red:
                spriteRenderer.sprite = enemyScriptable.sprtieTypes[armor - 1].Sprites[0];
                break;
            case GameManager.State.Blue:
                spriteRenderer.sprite = enemyScriptable.sprtieTypes[armor - 1].Sprites[1];
                break;
            case GameManager.State.Green:
                spriteRenderer.sprite = enemyScriptable.sprtieTypes[armor - 1].Sprites[2];
                break;
            case GameManager.State.White:
                spriteRenderer.sprite = enemyScriptable.sprtieTypes[armor - 1].Sprites[3];
                break;
            case GameManager.State.Grey:
                spriteRenderer.sprite = enemyScriptable.sprtieTypes[armor - 1].Sprites[4];
                break;
        }
    }

    void RandomState() {
        ChangeState((GameManager.State)Random.Range(0, (int)GameManager.State.Grey));
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!isTouchable) {
            return;
        }
        if (collision.gameObject.tag == "Player") {
            var player = FindObjectOfType<PlayerBehaviour>();
            GameManager.State playerState = player.GetState();
            if (playerState == currentState) {
                RecieveDamage();
            }
            else {
                dealDamage(player);
            }
        }
        else {
            Debug.Log("Collision occured");
        }
    }

    public GameManager.State GetState() {
        return currentState;
    }

    public void MakeUntouchable() {
        isTouchable = false;
        timeInUntouchableState = 0.0f;
    }

    void RecieveDamage() {
        armor -= 1;
        SetSprite(currentState);
        MakeUntouchable();
    }

    int RandomArmor() {
        float v = 0.0f;
        float p = Random.Range(0.0f, 1.0f);
        int idx = 1;
        foreach (var i in enemyScriptable.ArmorProbability) {
            v += i;
            if (p <= v) {
                return idx;
            }
            idx++;
        }
        return idx;
    }

    void die() {
        FindObjectOfType<GameManager>().IncreaseScore(reward);
    }

    void destroyItself() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        Destroy(gameObject);
    }

    void dealDamage(PlayerBehaviour player) {
        Instantiate(enemyScriptable.ExplosionEffect, player.transform);
        player.ReceiveDamage(enemyScriptable.StateToDamage[(int)currentState]);
        MakeUntouchable();
    }
}
