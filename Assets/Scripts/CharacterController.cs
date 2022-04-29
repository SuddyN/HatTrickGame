using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController: MonoBehaviour {

    public float movementSpeed = 8f;
    public float movementAccel = 10f;
    public float movementAccelInAir = 10f;
    public float sprintMultiplier = 2.0f;
    public float jumpStrength = 10f;
    public float fireStrength = 10f;
    public float bulletSpeed = 10f;
    public float cursorDist = 2.0f;
    public int maxAmmo = 3;
    public int currAmmo;
    public bool usingController = false;
    public GameObject cursor;
    private Rigidbody2D _rigidbody2D;
    private float lastSpeed = 0;

    void Start() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        currAmmo = maxAmmo;
        GameManager.Instance.player = gameObject;
    }

    void Update() {
        if (!GameManager.Instance.gameState.Equals(GameState.Game)) {
            return;
        }

        // Change movement speed and acceleration based on grounded state
        float modifiedSpeed = movementSpeed;
        float modifiedAccel = movementAccel;
        if (Mathf.Abs(_rigidbody2D.velocity.y) > 0.001f) {
            modifiedAccel = movementAccelInAir;
        }
        if (Input.GetButton("Fire3")) {
            modifiedSpeed *= sprintMultiplier;
        }

        // Get the user inputted movement
        float maxSpeed = Input.GetAxisRaw("Horizontal") * modifiedSpeed;

        // Add it to the current velocity
        Vector2 newVelocity = new Vector2(_rigidbody2D.velocity.x + (maxSpeed * modifiedAccel * Time.deltaTime), _rigidbody2D.velocity.y);
        // Only apply new velocity if it would be less than max speed or less than last speed
        if (Mathf.Abs(newVelocity.x) < modifiedSpeed || Mathf.Abs(newVelocity.x) < Mathf.Abs(lastSpeed)) {
            _rigidbody2D.velocity = newVelocity;
        }

        // Handle jumping
        if (Input.GetButtonDown("Jump") && Mathf.Abs(_rigidbody2D.velocity.y) < 0.001f) {
            _rigidbody2D.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
            AudioManager.Instance.Play("jump");
        }

        // Logic for aim and shoot
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Vector2 aim = new Vector2(mousePos.x, mousePos.y);
        if (usingController) {
            aim = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
        if (aim.magnitude > cursorDist) {
            aim = aim.normalized * cursorDist;
        }
        Vector3 cursorPos = new Vector3(transform.position.x + aim.x, transform.position.y + aim.y, transform.position.z);
        cursor.transform.position = cursorPos;

        if (Input.GetButtonDown("Fire1")) {
            Fire(cursorPos);
        }

        lastSpeed = _rigidbody2D.velocity.x;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        HandlePlayerCollide(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        HandlePlayerCollide(collider);
    }

    private void HandlePlayerCollide(Collider2D collider) {
        switch (collider.gameObject.tag) {
            case "Ground":
                TouchGround();
                AudioManager.Instance.Play("land");
                break;
            case "MaxAmmo":
                MaxAmmo();
                collider.GetComponent<SpriteRenderer>().enabled = false;
                collider.GetComponent<Collider2D>().enabled = false;
                if (collider.GetComponent<BulletScript>() != null)
                    Destroy(collider.gameObject);
                AudioManager.Instance.Play("collect");
                break;
            case "AddAmmo":
                AddAmmo();
                collider.GetComponent<SpriteRenderer>().enabled = false;
                collider.GetComponent<Collider2D>().enabled = false;
                if (collider.GetComponent<BulletScript>() != null)
                    Destroy(collider.gameObject);
                AudioManager.Instance.Play("collect");
                break;
            case "Kill":
                GameManager.Instance.UpdateGameState(GameState.Death);
                break;
            case "Enemy":
                TakeDamage();
                break;
            case "Checkpoint":
                break;
            case "Victory":
                GameManager.Instance.LoadNextLevel();
                AudioManager.Instance.Play("win");
                break;
            default:
                break;
        }
    }

    private void Fire(Vector3 cursorPos) {
        if (currAmmo <= 0) {
            return;
        }
        currAmmo--;
        Vector2 aimVector = new Vector3(cursorPos.x - transform.position.x, cursorPos.y - transform.position.y);
        if (aimVector.magnitude < 0.001f) {
            return;
        }
        _rigidbody2D.velocity = _rigidbody2D.velocity + (aimVector.normalized * fireStrength * -1.0f);

        Queue<BulletScript> bulletQueue = GameManager.Instance.bulletQueue;
        GameObject bullet = Instantiate(bulletQueue.Dequeue().gameObject);
        bullet.transform.position = transform.position;
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.velocity = bulletRB.velocity + (aimVector.normalized * bulletSpeed);

        UpdateAmmo();

        AudioManager.Instance.Play("fire");
    }

    private void TouchGround() {
        MaxAmmo();
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("AddAmmo")) {
            g.GetComponent<SpriteRenderer>().enabled = true;
            g.GetComponent<Collider2D>().enabled = true;
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("MaxAmmo")) {
            g.GetComponent<SpriteRenderer>().enabled = true;
            g.GetComponent<Collider2D>().enabled = true;
        }
    }

    private void MaxAmmo() {
        currAmmo = maxAmmo;
        UpdateAmmo();
    }

    private void AddAmmo() {
        currAmmo++;
        currAmmo = (currAmmo < 0) ? 0 : currAmmo;
        currAmmo = (currAmmo > maxAmmo) ? maxAmmo : currAmmo;
        UpdateAmmo();
    }

    private void TakeDamage() {
        GameManager.Instance.health -= 36.7f;
    }

    private void UpdateAmmo() {
        List<BulletScript> bulletList = GameManager.Instance.bullets;
        Queue<BulletScript> bulletQueue = GameManager.Instance.bulletQueue;
        while (bulletQueue.Count < currAmmo) {
            bulletQueue.Enqueue(bulletList[Random.Range(0, bulletList.Count)]);
        }
        GameManager.UIManager.UpdateAmmoUI();
    }
}
