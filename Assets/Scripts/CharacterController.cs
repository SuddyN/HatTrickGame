using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController: MonoBehaviour {

    public float movementSpeed = 10;
    public float movementAccel = 0.1f;
    public float jumpStrength = 10;
    public float fireStrength = 10;
    public float cursorDist = 1;
    public bool usingController = false;
    public GameObject cursor;
    private Rigidbody2D _rigidbody2D;

    void Start() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update() {
        // Only allowed to jump or move from the ground
        if (Mathf.Abs(_rigidbody2D.velocity.y) < 0.001f) {

            // Get the user inputted movement
            float maxSpeed = Input.GetAxisRaw("Horizontal") * movementSpeed;
            // Add it to the current velocity
            Vector2 newVelocity = new Vector2(_rigidbody2D.velocity.x + (maxSpeed * movementAccel), _rigidbody2D.velocity.y);
            // Only apply new velocity if it would be less than max speed
            if (Mathf.Abs(newVelocity.x) < movementSpeed) {
                _rigidbody2D.velocity = newVelocity;
            }

            // Handle jumping
            if (Input.GetButtonDown("Jump")) {
                _rigidbody2D.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
            }
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Vector2 aim = new Vector2(mousePos.x, mousePos.y).normalized;
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
    }

    private void Fire(Vector3 cursorPos) {
        Vector2 aimVector = new Vector3(cursorPos.x - transform.position.x, cursorPos.y - transform.position.y);
        if (aimVector.magnitude < 0.001f) {
            return;
        }
        _rigidbody2D.velocity = _rigidbody2D.velocity + (aimVector * fireStrength * -1.0f);
    }
}
