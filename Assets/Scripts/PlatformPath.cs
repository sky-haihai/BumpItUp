using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPath : MonoBehaviour
{
    public int divideNum;
    public float radius;
    public float yOffset; //let player float a bit above the platform when needed

    public GameObject nodePrefab;

    public List<Transform> PathNodes { get; private set; }

    private List<Renderer> _pathRenders;

    private void Start()
    {
        PathNodes = new List<Transform>();
        _pathRenders = new List<Renderer>();

        SpawnPath();

        EventManager.Instance.Register("OnCubeSpawned", SpawnArea);
        EventManager.Instance.Register("OnPlayerAreaChanged", ChangeActiveArea);
    }

    private void ChangeActiveArea(object obj)
    {
        if (_pathRenders.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < _pathRenders.Count; i++)
        {
            if (i == (int) obj)
            {
                _pathRenders[i].material.SetColor("_EmissionColor", Color.green);
            }
            else
            {
                _pathRenders[i].material.SetColor("_EmissionColor", Color.grey);
            }
        }
    }

    private void SpawnArea(object obj)
    {
        //Debug.LogWarning("SpawnArea");
        var count = (int) obj;

        for (int i = 0; i < count; i++)
        {
            var go = Instantiate(nodePrefab, transform, true);
            var rad = i * Mathf.PI * 2f / count;
            var pos = new Vector3(Mathf.Cos(rad) * radius, 0.2f + yOffset, Mathf.Sin(rad) * radius);
            go.transform.localPosition = pos;
            go.transform.LookAt(transform.position);
            var render = go.GetComponent<Renderer>();
            // render.material.color = Color.grey;
            render.material.SetColor("_EmissionColor", Color.grey);
            _pathRenders.Add(render);
        }
    }


    private void SpawnPath()
    {
        for (int i = 0; i < divideNum; i++)
        {
            var rad = i * Mathf.PI * 2f / divideNum;
            var pos = new Vector3(Mathf.Sin(rad) * radius, yOffset, Mathf.Cos(rad) * radius);
            GameObject go = new GameObject("pathPT" + i.ToString());
            go.transform.SetParent(transform);
            go.transform.localPosition = pos;
            PathNodes.Add(go.transform);
        }

        EventManager.Instance.Invoke("OnPathCreated", null);
    }
}