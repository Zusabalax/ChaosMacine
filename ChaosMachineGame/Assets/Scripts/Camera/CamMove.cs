using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class CamMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    Transform pivot;

    [SerializeField]
    Transform waipointsParent;

    [SerializeField]
    Transform []waipoints;


    [SerializeField]
    private float duration,wiatTime;

    Vector3[] points;

    private void Awake()
    {
        waipoints = waipointsParent.GetComponentsInChildren<Transform>();
        points = new Vector3[waipoints.Length];
        for (int i = 0; i < waipoints.Length; i++)
        {
            points[i] = waipoints[i].position;
        }
    }
    private void Start()
    {
        StartCoroutine(StartMoveCam());
    }



    IEnumerator StartMoveCam()
    {

        yield return new WaitForSeconds(5f);
        pivot.DOPath(points, duration, PathType.CatmullRom)
            .SetEase(Ease.Linear);
           
    }
}
