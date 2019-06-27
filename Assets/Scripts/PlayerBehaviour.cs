using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

    public List<Sprite> playerSprites;
    public GameEvent BoosterEvent;

    public PlayerScriptable playerScriptable;

    private SpriteRenderer spriteRenderer;
    //private float speed = 4.0f;
    private GameManager.State currentState;

    //public int maxHealth = 10;
    private int health;

    private GameManager gm;
    
    // Start is called before the first frame update
    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeState(GameManager.State.White);
        health = playerScriptable.MaxHealth;
        gm = FindObjectOfType<GameManager>();
        UpdateHealthUI();
    }

    // Update is called once per frame
    void Update() {
        if (health > 0) {
            if (Input.GetKey("w")) {
                Move(new Vector2(0, 1));
            }
            if (Input.GetKey("a")) {
                Move(new Vector2(-1, 0));
            }
            if (Input.GetKey("s")) {
                Move(new Vector2(0, -1));
            }
            if (Input.GetKey("d")) {
                Move(new Vector2(1, 0));
            }
        }
        else {
            gm.Lose();
        }
    }

    public void ChangeState(GameManager.State state) {
        currentState = state;
        switch (state) {
            case GameManager.State.Red:
                spriteRenderer.sprite = playerSprites[0];
                break;
            case GameManager.State.Blue:
                spriteRenderer.sprite = playerSprites[1];
                break;
            case GameManager.State.Green:
                spriteRenderer.sprite = playerSprites[2];
                break;
            case GameManager.State.White:
                spriteRenderer.sprite = playerSprites[3];
                break;
            case GameManager.State.Grey:
                spriteRenderer.sprite = playerSprites[4];
                break;
        }
    }

    void Move(Vector2 dir) {
        dir *= Time.deltaTime *  playerScriptable.StateToSpeed[(int)currentState];
        Vector3 newPos = new Vector3(transform.position.x + dir.x, transform.position.y + dir.y, transform.position.z);
        if (gm.IsInBoundaries(newPos)) {
            transform.position = new Vector3(transform.position.x + dir.x, transform.position.y + dir.y, transform.position.z);
        }
    }

    public GameManager.State GetState() {
        return currentState;
    }

    public void ReceiveDamage(int amount) {
        health -= amount;
        if (health > playerScriptable.MaxHealth) {
            health = playerScriptable.MaxHealth;
        }
        UpdateHealthUI();
    }

    public void UpdateHealthUI() {
        gm.UpdateUIHp(health);
    }
}
