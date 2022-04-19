using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript: MonoBehaviour {

    public float rotationSpeed = 360f;

    void Start() {

    }

    void Update() {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (!collider.gameObject.tag.Equals("Ground")) {
            return;
        }
        AudioManager.Instance.Play("break");
        Destroy(gameObject);
    }
}
