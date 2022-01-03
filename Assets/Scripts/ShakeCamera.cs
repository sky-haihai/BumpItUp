using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    private void Start() {
        EventManager.Instance.Register("OnCurrentHeightSet",o => CameraEffects.ShakeOnce(0.1f,10f,new Vector3(0.5f,0.5f,0.5f)));
    }
}
