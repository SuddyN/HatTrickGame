using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript: MonoBehaviour {

    public string displayName = "";
    public float rotationSpeed = 360f;
    public float collisionTimer = 0.5f;
    public float splashRadius = 1.0f;
    public float power = 34.0f;
    private float timer;
    private bool flag;

    private enum BulletType {
        Default,
        Teleport,
        Damage,
        SplashDamage,
        SplashHeal
    }
    [SerializeField]
    private BulletType bulletType;

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
        if (!collider.gameObject.tag.Equals("Ground") && !collider.gameObject.tag.Equals("Player")) {
            return;
        }
        switch (bulletType) {
            case BulletType.Teleport:
                if (!collider.gameObject.tag.Equals("Ground")) {
                    return;
                }
                GameManager.Instance.player.transform.position = new Vector3(transform.position.x, transform.position.y, GameManager.Instance.player.transform.position.z);
                break;
            case BulletType.Damage:
                if (!collider.gameObject.tag.Equals("Player")) {
                    break;
                }
                GameManager.Instance.health -= power;
                GameManager.UIManager.UpdateHealthUI();
                break;
            case BulletType.SplashDamage:
                if (!collider.gameObject.tag.Equals("Ground")) {
                    return;
                }
                if (Mathf.Abs(Vector2.Distance(GameManager.Instance.player.transform.position, transform.position)) <= splashRadius) {
                    GameManager.Instance.health -= power;
                    GameManager.UIManager.UpdateHealthUI();
                }
                break;
            case BulletType.SplashHeal:
                if (!collider.gameObject.tag.Equals("Ground")) {
                    return;
                }
                if (Mathf.Abs(Vector2.Distance(GameManager.Instance.player.transform.position, transform.position)) <= splashRadius) {
                    GameManager.Instance.health += power;
                    GameManager.UIManager.UpdateHealthUI();
                }
                break;
            default:
                break;
        }
        AudioManager.Instance.Play("break");
        Destroy(gameObject);
    }
}
