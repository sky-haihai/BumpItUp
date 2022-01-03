using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Spin : MonoBehaviour {
    public float speed = 1f;

    public bool sendEvent = false;

    // Update is called once per frame
    void Update() {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);

        // if (sendEvent) {
        //     InvokeRepeating(nameof(InvokeSpinStart), 0f, 360f / Mathf.Abs(speed));
        // }
    }

    // void InvokeSpinStart() {
    //     EventManager.Instance.Invoke("OnSpinStarted", null);
    // }
}