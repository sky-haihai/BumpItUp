using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class ChangeColorByTime : MonoBehaviour {
    private Image _img;

    private void Start() {
        _img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update() {
        var c = Time.time*200f % 360f;
        c /= 360f;
        _img.color = Color.HSVToRGB(c, 1f, 1f);
        //Debug.Log(c);
    }
}