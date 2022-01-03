using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {
    public string followEvent;
    public Transform target;
    public float step = 2f;

    public bool x;
    public bool y;
    public bool z;

    public float xOffset;
    public float yOffset;
    public float zOffset;

    private bool _follow;

    private void Start() {
        if (string.IsNullOrEmpty(followEvent)) {
            _follow = true;
            return;
        }

        EventManager.Instance.Register(followEvent, o => _follow = true);
    }

    private void LateUpdate() {
        if (!_follow) {
            return;
        }

        var delta = target.position + new Vector3(xOffset, yOffset, zOffset) - transform.position;
        if (!x) {
            delta.x = 0;
        }
        else { }

        if (!y) {
            delta.y = 0;
        }

        if (!z) {
            delta.z = 0;
        }

        delta = delta / step;
        transform.position += new Vector3(delta.x, delta.y, delta.z);
    }
}