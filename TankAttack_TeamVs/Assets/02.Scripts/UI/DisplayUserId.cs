using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DisplayUserId : MonoBehaviour
{
    public Text userId;
    public RawImage MiniMapUI;
    [HideInInspector] public PhotonView pv = null;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        userId.text = pv.Owner.NickName;
    }

}
