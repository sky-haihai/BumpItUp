using System;
using TMPro;
using UnityEngine;
using Random = System.Random;


public class CubeBehaviour : MonoBehaviour {
    private float _height = 0.1f;

    private void Start() {
        SetHeight(0.1f);
    }

    private void Update() {
        var delta = _height - transform.localScale.y;
        delta /= 5f;
        transform.localScale += new Vector3(0, delta, 0);
    }

    public void SetPosition(Vector3 localPos) {
        transform.localPosition = localPos;
    }

    public void SetHeight(float height) {
        // transform.localScale = new Vector3(transform.localScale.x, height, transform.localScale.z);
        _height = height;
    }

    public float AddHeight(float delta) {
        _height += delta;

        return _height;
    }

    public float GetHeight() {
        return _height;
    }
}