using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopSprite : MonoBehaviour {
    public float threshold;
    public float step;

    private bool _loop;
    private Vector3 _camPos;

    private void Start() {
        EventManager.Instance.Register("OnCameraSet", o => {
            _camPos = Camera.main.transform.position;
            _loop = true;
        });
    }

    private void Update() {
        if (!_loop) {
            return;
        }

        var delta = Camera.main.transform.position - _camPos;
        if (delta.y > threshold) {
            var last = transform.GetChild(2);
            last.localPosition += new Vector3(0f, step, 0f);
            last.SetAsFirstSibling();
            _camPos = Camera.main.transform.position;
        }
    }
    //private void OnDisable()
    //{
    //    EventManager.Instance.UnRegister("OnCameraSet", o => {
    //        _camPos = Camera.main.transform.position;
    //        _loop = true;
    //    });
    //}
}