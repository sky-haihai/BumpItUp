using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorControl : MonoBehaviour {
    public Renderer comp;

    // Start is called before the first frame update
    void Start() {
        EventManager.Instance.Register("OnMusicSelected", (o => comp.enabled = true));
        //EventManager.Instance.Register("",OnCurrentHeightSet);
    }

    private void Update() {
        if (DataManager.Instance.GetData("height") is float) {
            var h = (float) DataManager.Instance.GetData("height");
            var pos = transform.position;
            pos.y = h;
            transform.position = pos;
        }
    }
}