using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager: MonoBehaviour {

    [SerializeField]
    private Text _ammoText;

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
}
