using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager: MonoBehaviour {

    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private RectTransform _healthBar;
    [SerializeField]
    public Text _deathText;

    // Start is called before the first frame update
    void Start() {
        Debug.Log(_ammoText);
    }

    // Update is called once per frame
    void Update() {

    }

    public void UpdateAmmoUI() {
        Queue<BulletScript> bulletQueue = GameManager.Instance.bulletQueue;
        _ammoText.text = "Ammo Queue:";
        foreach (BulletScript b in bulletQueue.ToArray()) {
            _ammoText.text += "\n" + b.displayName;
        }
    }

    public void UpdateHealthUI() {
        float maxHealth = GameManager.Instance.maxHealth;
        float health = GameManager.Instance.health;
        _healthBar.rect.Set(-1*(maxHealth/2) - 10, _healthBar.rect.y, maxHealth + 5, _healthBar.rect.height);
        RectTransform _health = _healthBar.gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        _health.offsetMin = new Vector2(maxHealth - health + 2, _health.offsetMin.y);
    }
}
