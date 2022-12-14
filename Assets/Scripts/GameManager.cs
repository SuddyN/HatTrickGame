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
    public GameObject player;
    public GameObject mainCamera;

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
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Start() {
        health = maxHealth;
        UIManager._deathText.enabled = false;
    }

    void Update() {
        if (health <= 0 && this.gameState != GameState.Death) {
            UpdateGameState(GameState.Death);
        }
        if (health >= maxHealth) {
            health = maxHealth;
            UIManager.UpdateHealthUI();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            UpdateGameState(GameState.Game);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void UpdateGameState(GameState newState) {
        this.gameState = newState;
        switch (newState) {
            case GameState.Menu:
                break;
            case GameState.Game:
                player.SetActive(true);
                health = maxHealth;
                UIManager._deathText.enabled = false;
                UIManager.UpdateHealthUI();
                break;
            case GameState.Death:
                AudioManager.Instance.Play("lose");
                player.SetActive(false);
                health = 0;
                UIManager._deathText.enabled = true;
                UIManager.UpdateHealthUI();
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
