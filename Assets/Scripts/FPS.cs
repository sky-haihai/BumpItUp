using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using static System.String;

public class FPS : MonoBehaviour
{
    public Text text;
    private float last;

    private void Update()
    {
        if (Time.time - last > 0.5f)
        {
            text.text = ((int) (1 / Time.deltaTime)).ToString();
            last = Time.time;
        }
    }
}