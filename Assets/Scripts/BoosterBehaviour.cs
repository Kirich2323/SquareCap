using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterBehaviour : MonoBehaviour {

    private GameManager.State currentState = GameManager.State.Red;
    private SpriteRenderer spriteRenderer;
    private BoosterScriptable boosterScriptable;
    private bool isTouchable = true;
    private float timeInUntouchableState = 0.0f;
    private bool isGrey = true;

    // Start is called before the first frame update
    void Awake() {
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        gameObject.tag = "Booster";
    }

    // Update is called once per frame
    void Update() {
        if (!isTouchable) {
            float newTime = timeInUntouchableState + Time.deltaTime;
            if (Mathf.Floor(newTime / boosterScriptable.UntouchableChangeColorCooldown) > Mathf.Floor(timeInUntouchableState / boosterScriptable.UntouchableChangeColorCooldown)) {
                timeInUntouchableState -= boosterScriptable.UntouchableChangeColorCooldown;
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
            if (timeInUntouchableState >= boosterScriptable.UntouchableTime) {
                SetSprite(currentState);
                timeInUntouchableState = 0.0f;
                isTouchable = true;
            }
        }
    }

    void ChangeState(GameManager.State state) {
        SetSprite(state);
        currentState = state;
    }

    void SetSprite(GameManager.State state) {
        isGrey = false;
        switch (state) {
            case GameManager.State.Red:
                spriteRenderer.sprite = boosterScriptable.boosterSprites[0];
                break;
            case GameManager.State.Blue:
                spriteRenderer.sprite = boosterScriptable.boosterSprites[1];
                break;
            case GameManager.State.Green:
                spriteRenderer.sprite = boosterScriptable.boosterSprites[2];
                break;
            case GameManager.State.White:
                spriteRenderer.sprite = boosterScriptable.boosterSprites[3];
                break;
            case GameManager.State.Grey:
                spriteRenderer.sprite = boosterScriptable.boosterSprites[4];
                isGrey = true;
                break;
        }
    }

    public GameManager.State GetState() {
        return currentState;
    }

    public void BoostPlayer() {
        if (isTouchable) {
            var player = FindObjectOfType<PlayerBehaviour>();
            if (player.GetState() == currentState) {
                player.ReceiveDamage(-1);
            }
            FindObjectOfType<PlayerBehaviour>().ChangeState(currentState);
            destroyItself();
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            if (isTouchable) {
                BoostPlayer();
            }
        }
    }

    void destroyItself() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        Destroy(gameObject);
    }

    void RandomState() {
        ChangeState((GameManager.State)Random.Range(0, (int)GameManager.State.Grey));
    }

    public void SetBoosterScriptable(ScriptableObject scriptable) {
        boosterScriptable = scriptable as BoosterScriptable;
        RandomState();
        MakeUntouchable();
    }

    public void MakeUntouchable() {
        isTouchable = false;
        timeInUntouchableState = 0.0f;
    }
}
