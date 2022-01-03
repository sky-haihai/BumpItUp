using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WalkPath : MonoBehaviour
{
    public int interpoNum;
    public float radius;
    public float stopDistance = 0.5f;
    public float speed = 1f;
    public float yOffset = 0.2f;

    public PlatformPath path;

    private CharacterController _cc;
    private Animator _animator;

    private Transform _dest = null;
    private int _current;

    private int _areaCount;
    private int _cachedArea;
    private int _area; //current area on the platform

    private bool _moving;

    void Start()
    {
        _cc = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();

        EventManager.Instance.Register("OnPathCreated", (o =>
        {
            if (path.PathNodes.Count != 0)
            {
                _dest = path.PathNodes[0];
            }

            ResetPosition();

            StartMovement();
        }));

        EventManager.Instance.Register("OnCubeSpawned", (o =>
        {
            _areaCount = (int) o;
            //Debug.Log(_areaCount);
        }));

        StopMovement();
    }

    private void Update()
    {
        if (_moving)
        {
            MoveOnPath();
        }

        if (_areaCount > 0)
        {
            ComputeArea(_areaCount);
        }
    }

    private void ComputeArea(int divideNum)
    {
        var delta = transform.position - path.transform.position;
        var angle = Vector3.SignedAngle(Vector3.right, delta, Vector3.up);
        if (angle < 0)
        {
            angle = -angle;
        }
        else if (angle > 0)
        {
            angle = -angle + 360;
        }
        else
        {
            angle = 0;
        }

        var step = 360 / divideNum;
        _area = (int) (angle / (float) step);
        _area = Mathf.Clamp(_area, 0, divideNum - 1);

        if (_cachedArea == _area) return;

        //Debug.LogWarning("changed");
        _cachedArea = _area;
        EventManager.Instance.Invoke("OnPlayerAreaChanged", _area);
    }

    private void MoveOnPath()
    {
        var delta = _dest.position - transform.position;
        delta.y = 0;
        var distance = delta.magnitude;
        //Debug.Log("remain:" + distance);
        //Debug.Log("stop:" + stopDistance);
        if (distance < stopDistance)
        {
            //Debug.LogWarning("Changed Dest");
            _current = (_current + 1) % interpoNum;
            _dest = path.PathNodes[_current];

            //change face direction
            transform.forward = _dest.position - transform.position;
        }

        _cc.Move(delta.normalized * (speed * Time.deltaTime));
        transform.position =
            new Vector3(transform.position.x, path.transform.position.y + yOffset, transform.position.z);
    }

    private void UpdateAnimator()
    {
        _animator.SetBool("Moving", _moving);
    }

    public void ResetPosition()
    {
        transform.position = path.PathNodes[0].position;
    }

    public void StartMovement()
    {
        _moving = true;
        UpdateAnimator();
    }

    public void StopMovement()
    {
        _moving = false;
        UpdateAnimator();
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        foreach (var point in path.PathNodes)
        {
            Gizmos.DrawWireSphere(point.position, stopDistance);
        }
    }
}