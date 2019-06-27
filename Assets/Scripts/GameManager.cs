using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public enum State { Red, Blue, Green, White, Grey };

    public float Width = 9.5f;
    public float Height = 5.0f;

    public Text HpText;
    public Text ScoreText;
    public Text ResultText;

    public float ScorePerSecond;

    public GameObject GameUI;
    public GameObject ResultUI;

    private float score;
    private float lastUpdateScoreTime;

    private bool hasLost;

    // Start is called before the first frame update
    void Start() {
        GameUI.SetActive(true);
        ResultUI.SetActive(false);
        score = 0f;
        lastUpdateScoreTime = -1f;
        hasLost = false;
    }

    // Update is called once per frame
    void Update() {
        if (hasLost) {
            return;
        }

        if (Time.time - lastUpdateScoreTime >= 1f) {
            score += ScorePerSecond;
            lastUpdateScoreTime = Time.time;
            UpdateUIScore();
        }
    }

    public Vector2 GetRandomPosition() {
        return new Vector2(Width * Random.Range(-1.0f, 1.0f), Height * Random.Range(-1.0f, 1.0f));
    }

    public void Lose() {
        hasLost = true;
        GameUI.SetActive(false);
        ResultUI.SetActive(true);
        ResultText.text = "Lose :( Your score is " + Mathf.Floor(score).ToString();
    }

    public void IncreaseScore(float amount) {
        score += amount;
    }

    public void UpdateUIHp(int hp) {
        if (!hasLost) {
            HpText.text = "HP: " + hp.ToString();
        }
    }

    public void UpdateUIScore() {
        if (!hasLost) {
            ScoreText.text = Mathf.Floor(score).ToString();
        }
    }

    public bool IsInBoundaries(Vector3 pos) {
        return pos.x >= -Width && pos.x <= Width && pos.y >+ -Height && pos.y <= Height;
    }
}
