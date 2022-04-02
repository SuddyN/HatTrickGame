using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager: MonoBehaviour {

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
        _instance = this;
        UIManager = gameObject.GetComponent<UIManager>();
    }

    void Start() {

    }

    void Update() {
        if (gameState.Equals(GameState.Death)) {
            if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Submit")) {
                this.gameState = GameState.Game;
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
}

public enum GameState {
    Menu,
    Game,
    Death
}
