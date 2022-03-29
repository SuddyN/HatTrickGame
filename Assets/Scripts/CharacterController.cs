using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController: MonoBehaviour {

    public float movementSpeed = 10;
    public float jumpStrength = 10;
    public float cursorDist = 1;
    public GameObject cursor;
    private Rigidbody2D _rigidbody2D;

    void Start() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update() {
        // Use x-axis for movement
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, 0) * Time.deltaTime * movementSpeed;

        // Handle jumping
        if (Input.GetButtonDown("Jump") && Mathf.Abs(_rigidbody2D.velocity.y) < 0.001f) {
            _rigidbody2D.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
        }

        Vector2 aim = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if (aim.magnitude > cursorDist) {
            aim = aim.normalized * cursorDist;
        }
        Vector3 cursorPos = new Vector3(transform.position.x + aim.x, transform.position.y + aim.y, transform.position.z);
        cursor.transform.position = cursorPos;
    }
}
