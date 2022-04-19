using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript: MonoBehaviour {

    public string displayName = "";
    public float rotationSpeed = 360f;
    public float collisionTimer = 0.5f;
    private float timer;
    private bool flag;

    void Start() {
        gameObject.GetComponent<Collider2D>().enabled = false;
        timer = collisionTimer;
        flag = false;
    }

    void Update() {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        timer -= Time.deltaTime;
        if (timer <= 0 && !flag) {
            gameObject.GetComponent<Collider2D>().enabled = true;
            flag = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (!collider.gameObject.tag.Equals("Ground")) {
            return;
        }
        AudioManager.Instance.Play("break");
        Destroy(gameObject);
    }
}
