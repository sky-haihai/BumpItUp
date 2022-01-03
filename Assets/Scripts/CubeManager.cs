using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Transactions;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using Random = UnityEngine.Random;

public class CubeManager : MonoBehaviour {
    public CubeBehaviour heightCubePrefab;
    public CubeBehaviour audioCubePrefab;
    public Transform platform;
    [Range(1, 64)] public int cubeCount;
    [Range(1f, 10f)] public float radius;
    [Range(0.01f, 5f)] public float increaseSpeed = 0.1f;
    public float spinSpeed;

    private List<CubeBehaviour> _heightCubes;
    private List<CubeBehaviour> _audioCubes;
    private float _currentHeight = 0.1f; //当前最高高度
    private int _currentBlock;

    private Transform _cachedTransform;
    private Transform _audioRoot;

    private bool _allowFire = true;

    private void Start() {
        _cachedTransform = GetComponent<Transform>();
        // EventManager.Instance.Register("OnHeightChanged", OnHeightChanged);
        EventManager.Instance.Register("OnCurrentHeightSet", OnCurrentHeightSet);
        EventManager.Instance.Register("OnPlayerAreaChanged", (o => { _currentBlock = (int) o; }));
        EventManager.Instance.Register("OnCubeSpawned", (o) =>
            InvokeRepeating(nameof(UpdateAudioCubes), 0f, Time.deltaTime)
        );

        //EventManager.Instance.Register("OnSpinStarted", (o => _allowFire = true));

        //    StartCoroutine(nameof(SpawnCubeCoroutine));
        SpawnCube();

        //InvokeRepeating(nameof(EnableControl),0f,360f/Mathf.Abs(spinSpeed));
        StartCoroutine(nameof(EnableControl));
    }

    void SpawnCube() {
        _heightCubes = new List<CubeBehaviour>();
        _audioCubes = new List<CubeBehaviour>();

        _heightCubes.Clear();
        _audioCubes.Clear();

        var heightParent = new GameObject("height cubes").transform;
        heightParent.SetParent(_cachedTransform, false);
        var audioParent = new GameObject("audio cubes").transform;
        audioParent.SetParent(_cachedTransform, false);

        _audioRoot = audioParent;

        for (int i = 0; i < cubeCount; i++) {
            var cube = Instantiate(heightCubePrefab, heightParent);
            var audio = Instantiate(audioCubePrefab, audioParent);

            var rad = i * Mathf.PI * 2f / cubeCount;

            var pos = new Vector3(Mathf.Cos(rad) * radius, 0, Mathf.Sin(rad) * radius);
            cube.SetPosition(pos);
            audio.SetPosition(pos);
            //Debug.Log(rad);

            cube.transform.LookAt(_cachedTransform);
            audio.transform.LookAt(_cachedTransform);

            _heightCubes.Add(cube);
            _audioCubes.Add(audio);
        }

        EventManager.Instance.Invoke("OnCubeSpawned", cubeCount);
    }

    private void OnCurrentHeightSet(object currentHeight) {
        if (!(currentHeight is float current)) {
            return;
        }

        //Debug.LogWarning(_currentBlock);
        // var height = _cubes[_currentBlock].AddHeight(increaseSpeed * current);
        foreach (var cube in (_heightCubes)) {
            cube.AddHeight(increaseSpeed * current);
        }

        var height = _heightCubes[0].GetHeight();

        // EventManager.Instance.Invoke("OnHeightChanged", height);
        if (_currentHeight < (float) height) {
            _currentHeight = (float) height;
        }
    }

    private void Update() {
        HandleControl();

        UpdateRootPositions();

        DataManager.Instance.SetData("height", _currentHeight);
    }

    IEnumerator EnableControl() {
        //Debug.Log("true");
        var t = 0f;
        while (true) {
            DataManager.Instance.SetData("CurrentHitTime", t / (360f / Mathf.Abs(spinSpeed)));

            t += Time.deltaTime;

            if (t >= (360f / Mathf.Abs(spinSpeed))) {
                _allowFire = true;
                EventManager.Instance.Invoke("OnAllowNextHit", 0f);
                t = 0f;
            }

            yield return null;
        }
    }

    private void UpdateRootPositions() {
        if (_audioRoot != null) {
            var delta = new Vector3(transform.position.x, _currentHeight, transform.position.z) -
                        _audioRoot.position;
            delta /= 5f;
            _audioRoot.position += delta;
        }

        if (platform != null) {
            var delta = new Vector3(transform.position.x, _currentHeight + 50f * increaseSpeed, transform.position.z) -
                        platform.position;
            delta /= 5f;
            platform.position += delta;
        }
    }

    private void HandleControl() {
        if (Input.GetKeyDown(KeyCode.Space) && _allowFire) {
            _allowFire = false;
            //set height
            var height = GetCubeHeight(270f, Mathf.Abs(spinSpeed));
            EventManager.Instance.Invoke("OnCurrentHeightSet", height);
            DataManager.Instance.SetData("currentHeight", height);
        }
    }

    private void UpdateAudioCubes() {
        if (_audioCubes == null) {
            return;
        }

        if (!(DataManager.Instance.GetData("cubeTransforms") is Transform[] trans)) {
            return;
        }

        var length = (trans.Length > cubeCount ? true : false) ? trans.Length : cubeCount;

        for (int i = 0; i < length; i++) {
            _audioCubes[i].SetHeight(trans[i].transform.localScale.y * increaseSpeed);
            var h = (1f / length * i + Time.time) % 1f;
            //Debug.Log(i + ":" + h);
            _audioCubes[i].GetComponentInChildren<Renderer>().material
                .SetColor("_EmissionColor", Color.HSVToRGB(h, 0.8f, 1f));
        }
    }

    private float GetCubeHeight(float targetAngle, float aps) { //angle per frame
        var trans = DataManager.Instance.GetData("cubeTransforms") as Transform[];
        if (trans == null) {
            return 0f;
        }

        var unitBound = 180f / cubeCount;
        for (int i = 0; i < cubeCount; i++) {
            var time = Time.time % (360f / aps);
            var angle = time * aps + i * (360f / cubeCount);
            angle %= 360f;
            var delta = angle - targetAngle;
            if ((delta > 0 && delta < unitBound) || (delta < 0 && delta > -unitBound)) {
                //StartCoroutine(nameof(JumpCoroutine), _audioCubes[i].transform);
                return trans[i].localScale.y;
            }
        }

        return 0f;
    }

    // IEnumerator JumpCoroutine(Transform trans) {
    //     // Debug.Log("trans");
    //     trans.GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", Color.black);
    //     yield return null;
    //     var origin = trans.localPosition;
    //     var target = trans.localPosition + new Vector3(0, 0.2f, 0);
    //     while ((trans.localPosition - target).magnitude > float.Epsilon) {
    //         var delta = target - trans.localPosition;
    //         delta /= 5f;
    //         trans.localPosition += delta;
    //         yield return null;
    //     }
    //
    //     while ((trans.localPosition - origin).magnitude > float.Epsilon) {
    //         var delta = origin - trans.localPosition;
    //         delta /= 5f;
    //         trans.localPosition += delta;
    //         yield return null;
    //     }
    // }

    private void OnDestroy() {
        //Debug.Log("!!!!!!!!!!!!!");
        CancelInvoke(nameof(UpdateAudioCubes));
    }
}