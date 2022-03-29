using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour {

    public static GameManager Instance;
    public GameState gameState;
    public static event Action<GameState> OnGameStateChanged;

    private void Awake() {
        Instance = this;
    }

    void Start() {

    }

    void Update() {

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
