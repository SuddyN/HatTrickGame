using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Written with the following used as a resource:
 * https://medium.com/nerd-for-tech/player-movement-in-unity-2d-using-rigidbody2d-4f6f1693d730
 */
public class InputController : MonoBehaviour
{

    public float playerSpeed = 10.0f;
    public float jumpHeight = 5.0f;
    public float launchPower = 10.0f;

    private Rigidbody2D _rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded()) {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpHeight);
        }
        if (Input.GetButtonDown("Fire1")) {
            Vector3 diffVector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            Vector2 launchVector = new Vector2(diffVector3.x, diffVector3.y).normalized * launchPower * -1.0f;
            _rigidbody2D.velocity = _rigidbody2D.velocity + launchVector;
        }

        // This is a really messy way of allowing launchers to override movement
        float newHorizontalSpeed = Input.GetAxisRaw("Horizontal") * playerSpeed;
        if (newHorizontalSpeed >= 0 && _rigidbody2D.velocity.x >= 0
            || newHorizontalSpeed <= 0 && _rigidbody2D.velocity.x <= 0) {
            if (Mathf.Abs(newHorizontalSpeed) < Mathf.Abs(_rigidbody2D.velocity.x)) {
                // Skipped vel update
            } else {
                _rigidbody2D.velocity = new Vector2(newHorizontalSpeed, _rigidbody2D.velocity.y);
            }
        } else {
            _rigidbody2D.velocity = new Vector2(newHorizontalSpeed, _rigidbody2D.velocity.y);
        }
    }

    private bool IsGrounded() {
        var groundCheck = Physics2D.Raycast(transform.position, Vector2.down, 0.5f);
        return groundCheck.collider != null;
    }
}
