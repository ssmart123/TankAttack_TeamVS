using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardMiniMapCanvas : MonoBehaviour
{
    private Transform tr;
    private Transform RenderCamTr;

    private void Start()
    {
        tr = GetComponent<Transform>();
        RenderCamTr = GameObject.FindGameObjectWithTag("RENDERCAMERA").transform;
    }

    void LateUpdate()
    {
        tr.forward = RenderCamTr.forward;
    }
}
