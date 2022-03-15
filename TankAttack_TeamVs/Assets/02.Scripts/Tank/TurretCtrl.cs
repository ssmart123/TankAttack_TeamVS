using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurretCtrl : MonoBehaviourPunCallbacks, IPunObservable
{
    private Transform tr;
    //광선(Ray)이 지면에 맞은 위치를 저장할 변수
    private RaycastHit hit;

    //터렛의 회전 속도
    public float rotSpeed = 5.0f;

    //PhotonView 컴포넌트 변수
    private PhotonView pv = null;
    //원격 네트워크 탱크의 터렛 회전값을 저장할 변수
    private Quaternion currRot = Quaternion.identity;

    // Start is called before the first frame update
    void Awake()
    {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();


        //초기 회전값 설정
        currRot = tr.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        //자신의 탱크일 때만 조정
        if (pv.IsMine == true)
        {
            if (PhotonInit.isFocus == false) //윈도우 창이 비활성화 되어 있다면...
                return;

            //메인 카메라에서 마우스 커서의 위치로 캐스팅되는 Ray를 생성
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //생성된 Ray를 Scene 뷰에 녹색 광선으로 표현
            Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("TERRAIN")))
            {
                //Ray에 맞은 위치를 로컬좌표로 변환
                Vector3 relative = tr.InverseTransformPoint(hit.point);
                //역탄젠트 함수인 Atan2로 두 점 간의 각도를 계산
                float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
                //rotSpeed 변수에 지정된 속도로 회전
                tr.Rotate(0, angle * Time.deltaTime * rotSpeed, 0);
            }
            else
            {
                Vector3 a_OrgVec = ray.origin + ray.direction * 2000.0f;
                ray = new Ray(a_OrgVec, -ray.direction);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity,
                                            1 << LayerMask.NameToLayer("TURRETPICKOBJ")))
                {
                    //Ray에 맞은 위치를 로컬좌표로 변환
                    Vector3 relative = tr.InverseTransformPoint(hit.point);
                    //역탄젠트 함수인 Atan2로 두 점 간의 각도를 계산
                    float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
                    //rotSpeed 변수에 지정된 속도로 회전
                    tr.Rotate(0, angle * Time.deltaTime * rotSpeed, 0);
                }
            } //else

        }//  if (pv.IsMine == true)
        else //원격 네트워크 플레이어의 탱크일 경우
        {
            //현재 회전각도에서 수신받은 실시간 회전각도로 부드럽게 회전
            tr.localRotation = Quaternion.Slerp(tr.localRotation, currRot, Time.deltaTime * 10.0f);
        }
    } //void Update()

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
