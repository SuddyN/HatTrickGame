using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow: MonoBehaviour {
    public GameObject followObject; // The object this camera should follow
    public float speed = 3;         // The movement speed of the camera when the player is ahead of it
    public Vector2 followBoxSize;   // The size of the bounds after which the camera will follow
    public Vector2 followBoxOffset; // The offset of said bounds
    private Vector2 threshold;      // Private vector to store camera threshold

    void Start() {
        threshold = calculateThreshold();
    }

    void FixedUpdate() {
        // Get the x,y pos of the object to follow and offset it by the camera offset
        Vector2 follow = followObject.transform.position;
        follow -= followBoxOffset;

        // Calculate the distance between camera's pos and the object
        float xDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * follow.x);
        float yDifference = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * follow.y);

        // Set a new position, updated only if the object is outside the follow bounds
        Vector3 newPosition = transform.position;
        if (Mathf.Abs(xDifference) >= threshold.x) {
            newPosition.x = follow.x;
        }
        if (Mathf.Abs(yDifference) >= threshold.y) {
            newPosition.y = follow.y;
        }

        // Move the camera towards the object
        Rigidbody2D rb = followObject.GetComponent<Rigidbody2D>();
        float moveSpeed = rb.velocity.magnitude > speed ? rb.velocity.magnitude : speed;
        transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
    }

    // Calculates the camera follow bounds
    private Vector2 calculateThreshold() {
        Rect aspect = Camera.main.pixelRect;
        Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
        t.x -= followBoxSize.x;
        t.y -= followBoxSize.y;
        return t;
    }

    // Draws a helpful gizmo in the editor, so we can see the camera follow box
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Vector2 border = calculateThreshold();
        Vector3 center = transform.position;
        center.x += followBoxOffset.x;
        center.y += followBoxOffset.y;
        Gizmos.DrawWireCube(center, new Vector3(border.x * 2, border.y * 2, 1));
    }
}
