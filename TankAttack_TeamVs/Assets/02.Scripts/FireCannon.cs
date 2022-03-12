using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireCannon : MonoBehaviourPunCallbacks
{
    //cannon 프리팹을 연결할 변수
    public GameObject cannon = null;
    //포탄 발사 사운드 파일
    private AudioClip fireSfx = null;
    //AudioSource 컴포턴트를 할당할 변수
    private AudioSource sfx = null;
    //cannon 발사 지점
    public Transform firePos;

    //PhotonView 컴포넌트를 할당할 변수
    private PhotonView pv = null;

    TankDamage tankDamage;

    // Start is called before the first frame update
    void Awake()
    {
        //cannon 프리팹을 Resources 폴더에서 불러와 변수에 할당
        cannon = (GameObject)Resources.Load("cannon");

        //포탄 발사 사운드 파일을 Resources 폴더에서 불러와 변수에 할당
        fireSfx = Resources.Load<AudioClip>("CannonFire");
        //AudioSource 컴포넌트를 할당
        sfx = GetComponent<AudioSource>();

        //PhotonView 컴포넌트를 pv 변수에 할당
        pv = GetComponent<PhotonView>();

        tankDamage = this.GetComponent<TankDamage>();
    }

    // Update is called once per frame
    void Update()
    {
        //마우스 왼쪽 버튼 클릭 시 발사 로직 수행
        if (pv.IsMine && Input.GetMouseButtonDown(0))
        {
            if (tankDamage != null)
            {
                if (GameMgr.m_GameState != GameState.GS_Playing)
                    return;  //못쏘게...

                if (tankDamage.currHp <= 0)  //죽었으면 못쏘게...
                    return;

                if (0.0f < tankDamage.m_ReSetTime)
                    return;
            }

            Fire();

            //원격 네트워크 플레이어의 탱크에 RPC로 원격 Fire 함수를 호출
            pv.RPC("Fire", RpcTarget.Others, null);
        }
    }

    [PunRPC]
    void Fire()
    {
        //발사 사운드 발생
        sfx.PlayOneShot(fireSfx, 1.0f);

        GameObject _cannon = Instantiate(cannon, firePos.position, firePos.rotation);
        _cannon.GetComponent<Cannon>().AttackerId = pv.Owner.ActorNumber; //ownerId;
        if (pv.Owner.CustomProperties.ContainsKey("MyTeam") == true)
        {
            _cannon.GetComponent<Cannon>().AttackerTeam
                = (string)pv.Owner.CustomProperties["MyTeam"];
        }
    }
}
