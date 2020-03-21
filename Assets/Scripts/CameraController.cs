using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Vector2 step = Vector2.zero;
    public Vector2 offset = Vector2.zero;

    public enum Behaviour { follow }
    public Behaviour state = Behaviour.follow;

    private void Awake() {
        if (target == null) {
            this.enabled = false;
        }
    }

    private void FixedUpdate() {
        switch (state) {
            case Behaviour.follow:
                FollowTarget();
                break;
        }
    }

    void FollowTarget() {
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, target.position.x + offset.x, step.x), 
            Mathf.Lerp(transform.position.y, target.position.y + offset.y, step.y), transform.position.z);
    }
}
