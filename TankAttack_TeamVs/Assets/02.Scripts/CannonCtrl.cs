using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CannonCtrl : MonoBehaviourPunCallbacks, IPunObservable
{
    private Transform tr;
    public float rotSpeed = 5.0f; //100.0f;

    private RaycastHit hit;

    private PhotonView pv = null;

    //원격 네트워크 탱크의 포신 회전 각도를 저장할 변수
    private Quaternion currRot = Quaternion.identity;

    // Start is called before the first frame update
    void Awake()
    {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();

        //Photon View의 Observed 속성을 이 스크립트로 지정
        //pv.ObservedComponents[0] = this;
        //Photon View의 동기화 속성을 설정
        //pv.Synchronization = ViewSynchronization.UnreliableOnChange; 
        //udp 방식으로 변화가 있을 때만 패킷 전송 방식

        //초기 회전값 설정
        currRot = tr.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        //자신이 만든 네트워크 게임오브젝트가 아닌 경우는 키보드 조작 루틴을 나감
        if (pv.IsMine)
        {
            if (PhotonInit.isFocus == false) //윈도우 창이 비활성화 되어 있다면...
                return;

            //float angle = -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * rotSpeed;
            //tr.Rotate(angle, 0, 0);

            //메인 카메라에서 마우스 커서의 위치로 캐스팅되는 Ray를 생성
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //ray.origin //ray.direction  //레이를 벡터로...
            //Ray ray = new Ray(transform.position, transform.forward); //벡터를 레이로...
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("TERRAIN")))
            {
                Vector3 a_CacVec = hit.point - tr.position;
                Quaternion a_rotate = Quaternion.LookRotation(a_CacVec.normalized);
                a_rotate.eulerAngles = new Vector3(a_rotate.eulerAngles.x, tr.eulerAngles.y,
                                        tr.eulerAngles.z);
                tr.rotation = Quaternion.Slerp(tr.rotation, a_rotate, Time.deltaTime * 10.0f);

                tr.localEulerAngles = new Vector3(tr.localEulerAngles.x, 0.0f, 0.0f);
            }
            else
            {
                Vector3 a_OrgVec = ray.origin + ray.direction * 2000.0f;
                ray = new Ray(a_OrgVec, -ray.direction);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity,
                                                        1 << LayerMask.NameToLayer("TURRETPICKOBJ")))
                {
                    Vector3 a_CacVec = hit.point - tr.position;
                    Quaternion a_rotate = Quaternion.LookRotation(a_CacVec.normalized);
                    a_rotate.eulerAngles = new Vector3(a_rotate.eulerAngles.x, tr.eulerAngles.y,
                                            tr.eulerAngles.z);
                    tr.rotation = Quaternion.Slerp(tr.rotation, a_rotate, Time.deltaTime * 10.0f);

                    tr.localEulerAngles = new Vector3(tr.localEulerAngles.x, 0.0f, 0.0f);
                }
            }

            //포신 각도 제한...
            Vector3 a_Angle = tr.localEulerAngles;
            if (a_Angle.x < 180.0f)   //포신 각도를 내려가려는 경우
            {
                if (5.0f < a_Angle.x)
                    a_Angle.x = 5.0f;
            }
            else                      //포신 각도를 올리려는 경우
            {
                if (a_Angle.x < 330.0f)  //값을 더 줄이면 각도가 제한이 더된다.
                    a_Angle.x = 330.0f;
            }

            tr.localEulerAngles = a_Angle;

        }//if (pv.IsMine)
        else
        {
            //현재 회전 각도에서 수신받은 실시간 회전 각도로 부드럽게 회전
            tr.localRotation = Quaternion.Slerp(tr.localRotation, currRot, Time.deltaTime * 10.0f);
        }
    }

    //송수신 콜백 함수
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //로컬 플레이어의 위치 정보 송신
        if (stream.IsWriting)
        {
            stream.SendNext(tr.localRotation);
        }
        else //원격 플레이어의 위치 정보 수신
        {
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
