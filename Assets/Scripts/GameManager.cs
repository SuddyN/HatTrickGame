using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager: MonoBehaviour {

    public float health = 100f;
    public float maxHealth = 100f;
    public List<BulletScript> bullets;
    public Queue<BulletScript> bulletQueue;

    private static GameManager _instance;
    public static GameManager Instance {
        get {
            if (_instance == null) {
                Debug.LogError("GameManager is null!");
            }
            return _instance;
        }
    }
    public static UIManager UIManager;
    public GameState gameState;
    public static event Action<GameState> OnGameStateChanged;

    private void Awake() {

        if (_instance == null) {
            _instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        UIManager = gameObject.GetComponent<UIManager>();
        bulletQueue = new Queue<BulletScript>();
    }

    void Start() {
        health = maxHealth;
    }

    void Update() {
        if (health <= 0) {
            gameState = GameState.Death;
        }
        if (gameState.Equals(GameState.Death)) {
            if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Submit")) {
                this.gameState = GameState.Game;
                health = maxHealth;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void UpdateGameState(GameState newState) {
        this.gameState = newState;
        switch (newState) {
            case GameState.Menu:
                break;
            case GameState.Game:
                break;
            case GameState.Death:
                break;
            default:
                break;
        }
        OnGameStateChanged?.Invoke(newState);
    }

    public void LoadNextLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}

public enum GameState {
    Menu,
    Game,
    Death
}
