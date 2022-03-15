using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
//SmoothFollow 스크립트를 사용하기 위해 네임스페이스 추가
using UnityStandardAssets.Utility;

//Pun2에서 TankMove를 이용해서 Tank의 Position과 Rotation을 중계하려면
//MonoBehaviourPunCallbacks, IPunObservable 추가해 주어야 한다.
//IPunObservable --> OnPhotonSerializeView() 함수가 정상적으로 호출되게 추가해 줘야 한다.
public class TankMove : MonoBehaviourPunCallbacks, IPunObservable
{
    //PhotonView 컴포넌트를 할당할 변수
    private PhotonView pv = null;
    //메인 카메라가 추적할 CamPivot 게임오브젝트
    public Transform camPivot;

    //탱크의 이동 및 회전 속도를 나타내는 변수
    public float moveSpeed = 20.0f;
    public float rotSpeed = 50.0f;

    //참조할 컴포넌트를 할당할 변수
    [HideInInspector] public Rigidbody rbody;
    [HideInInspector] public Transform tr;

    //키보드 입력값 변수
    private float h, v;

    Vector3 a_CacPos = Vector3.zero;

    //위치 정보를 송수신할 때 사용할 변수 선언 및 초깃값 설정
    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;

    TankDamage tankDamage = null;

    //tr 변수에 Transform 컴포넌트가 할당되기 전에 OnPhotonSerializeView 콜백 함수가
    //호출될 경우에는 Null Reference 오류가 발생할 수 있다. 따라서 Start 함수는 Awake
    //함수로 변경해 가장 먼저 수행하게 한다. 또한 currPos와 currRot 변수도 탱크가 생성된
    //위치와 회전값으로 초깃값을 설정한다.
    void Awake()
    {
        //컴포넌트 할당
        rbody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();

        //PhotonView 컴포넌트 할당
        pv = GetComponent<PhotonView>();
        //pv.observableSearch = PhotonView.ObservableSearch.Manual;  
        //<--이건 유니티 에디터에서 먼저 설정되어 있어야 정상 동작한다.
        pv.ObservedComponents[0] = this;
        //pv.synchronization = ViewSynchronization.UnreliableOnChange; //udp 방식으로 변화가 있을 때만 패킷 전송 방식
        //ViewSynchronization.Off  //실시간 데이터 송수신을 하지 않는다.
        //ViewSynchronization.ReliableDeltaCompressed //데이터를 정확히 송수신한다.(TCP 프로토콜)
        //ViewSynchronization.Unreliable //데이터의 정확성을 보장할 수 없지만 속도가 빠르다.(UDP 프로토콜)


        //유저가 조정하고 있는 로컬에서 만들어진 탱크의 PhotonView일 경우
        if (pv.IsMine)
        {
            //동적으로 만들어진 프리팹이 로컬 플레이어가 만든 것인지 아니면 네트워크에 접속한 
            //원격 플레이어에 의해 만들어진 것인지 여부는 해당 프리팹에 추가된 
            //PhotonView 컴포넌트의 IsMine 속성으로 판단한다.

            //메인 카메라에 추가된 SmoothFollow 스크립트에 추적 대상을 연결
            Camera.main.GetComponent<SmoothFollow>().target = camPivot;
        }

        //원격 탱크의 위치 및 회전 값을 처리할 변수의 초깃값 설정
        currPos = tr.position;
        currRot = tr.rotation;

        tankDamage = this.GetComponent<TankDamage>();
    }

    // Start is called before the first frame update
    void Start()
    {

        if (!pv.IsMine) //내가 조정하고 있는 탱크가 아닌 경우
        {
            //원격 네트워크 플레이어의 탱크는 물리력을 이용하지 않음
            rbody.isKinematic = true;
        }

        //Rigidbody의 무게중심을 낮게 설정
        rbody.centerOfMass = new Vector3(0.0f, -2.5f, 0.0f);
    }

    //------------ 탱크끼리 구충돌로 밀리게 하기 코드 부분
    float a_Radius = 8.5f;
    GameObject[] a_tanks = null;
    Vector3 a_fCacDist = Vector3.zero;
    float a_CacDistLen = 0.0f;
    float a_ShiftLen = 0.0f;
    TankDamage a_TkDamage = null;
    //------------ 탱크끼리 구충돌로 밀리게 하기 코드 부분

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine) //내가 로컬에서 만든 탱크인 경우에만 조정이 가능하게 한다.
        {
            if (GameMgr.m_GameState != GameState.GS_Playing)
                return;

            if (tankDamage != null)
            {
                if (tankDamage.currHp <= 0)
                    return;
            }

            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            //회전과 이동처리
            tr.Rotate(Vector3.up * rotSpeed * h * Time.deltaTime);
            tr.Translate(Vector3.forward * v * moveSpeed * Time.deltaTime);
            //Default 값 Space.Self

            //------------탱크끼리 구충돌로 밀리게 해서 물리엔진이 발동하지 않게 하기...
            a_tanks = GameObject.FindGameObjectsWithTag("TANK");
            foreach (GameObject tank in a_tanks)
            {
                if (this.gameObject == tank)
                    continue;

                a_TkDamage = tank.GetComponent<TankDamage>();
                if (a_TkDamage == null)
                    continue;

                if (a_TkDamage.currHp <= 0)
                    continue;

                a_fCacDist = tr.position - tank.transform.position;

                if (a_fCacDist.y < 0.0f)
                    a_fCacDist.y = 0.0f;
                a_CacDistLen = a_fCacDist.magnitude;
                if (a_CacDistLen < (a_Radius + a_Radius))
                {
                    a_ShiftLen = 15.0f * Time.deltaTime;
                    tr.position = tr.position + (a_fCacDist.normalized * a_ShiftLen);
                }
            }
            //------------탱크끼리 구충돌로 밀리게 해서 물리엔진이 발동하지 않게 하기...

            //------------탱크가 지형을 벗어나지 못하게 막기...
            a_CacPos = tr.position;
            //a_CacPos.x = Mathf.Clamp(a_CacPos.x, -245.0f, 245.0f);
            //a_CacPos.z = Mathf.Clamp(a_CacPos.z, -245.0f, 245.0f);

            if (245.0f < tr.position.x)
            {
                a_CacPos.x = 245.0f;
            }
            if (245.0f < tr.position.z)
            {
                a_CacPos.z = 245.0f;
            }
            if (tr.position.x < -245.0f)
            {
                a_CacPos.x = -245.0f;
            }
            if (tr.position.z < -245.0f)
            {
                a_CacPos.z = -245.0f;
            }
            tr.position = a_CacPos;
            //------------탱크가 지형을 벗어나지 못하게 막기...
        }// if (pv.IsMine)
        else //원격으로 만들어진 탱크들...
        { //좌표를 중계 받아 움직일 것임

            if (10.0f < (tr.position - currPos).magnitude)
            {
                tr.position = currPos;
            }
            else
            {
                //원격 플레이어의 탱크를 수신받은 위치까지 부드럽게 이동시킴
                tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
            }
            //원격 플레이어의 탱크를 수신받은 각도만큼 부트럽게 회전시킴
            tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 10.0f);

        }//원격에서 만들어진 탱크들...

    } //void Update()

    //기본설정은 SendRate 1초 20번, SerializtionRate 1초에 10번으로 알고있습니다.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //로컬 플레이어의 위치 정보 송신
        if (stream.IsWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else //원격 플레이어의 위치 정보 수신
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
