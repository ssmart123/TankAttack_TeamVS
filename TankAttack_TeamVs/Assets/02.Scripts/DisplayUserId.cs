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

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        userId.text = pv.Owner.NickName;
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
