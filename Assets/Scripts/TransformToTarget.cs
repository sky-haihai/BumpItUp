using System.Collections;
using UnityEngine;

public class TransformToTarget : MonoBehaviour {
    public Transform target;
    public Transform lookAt;
    
    private void Start() {
        EventManager.Instance.Register("OnMusicSelected", o => StartCoroutine(nameof(StartTransformCo)));
    }

    IEnumerator StartTransformCo() {
        var t = 0f;
        var interpo = 120f;
        while (t < interpo * Time.deltaTime) {
            var deltaPosition = target.position - transform.position;
            deltaPosition /= interpo;
            transform.position += deltaPosition;
            
            transform.LookAt(lookAt);

            t += Time.deltaTime;
            yield return null;
        }

        transform.position = target.position;
        transform.rotation = target.rotation;

        EventManager.Instance.Invoke("OnCameraSet", null);
    }

    // private void OnDestroy()
    // {
    //     Debug.Log("!!!!!!!!!!!!!");
    //     EventManager.Instance.UnRegister("OnMusicSelected", o => StartCoroutine(nameof(StartTransformCo)));
    // }
}