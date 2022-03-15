using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

//리스폰 되고 나서 약간 딜레이를 준다.

public class TankDamage : MonoBehaviour
{
    [HideInInspector] public PhotonView pv = null;

    //탱크 폭파 후 투명 처리를 위한 MeshRenderer 컴포넌트 배열
    private MeshRenderer[] renderers;

    //탱크 폭발 효과 프리팹을 연결할 변수
    private GameObject expEffect = null;

    //탱크의 초기 생명치
    private int initHp = 200;
    //탱크의 현재 생명치
    int IsMineBuf_CurHp = 0; //IsMine 경우에만 사용될 변수
    public int currHp = 0;
    int m_OldcurHp = 0;

    //탱크 하위의 Canvas 객체를 연결할 변수
    public Canvas hudCanvas;
    //Filled 타입의 Image UI 항목을 연결할 변수
    public Image hpBar;

    //플레이어 Id를 저장하는 변수
    public int playerId = -1;

    //적 탱크 파괴 스코어를 저장하는 변수
    int IsMineBuf_killCount = 0; //IsMine 경우에만 사용될 변수
    public int killCount = 0;    //모든 PC의 내 탱크들의 변수

    //탱크 HUD에 표현할 스코어 Text UI 항목
    public Text txtKillCount;

    ExitGames.Client.Photon.Hashtable CurrHpProps
                        = new ExitGames.Client.Photon.Hashtable();

    ExitGames.Client.Photon.Hashtable KillProps
                        = new ExitGames.Client.Photon.Hashtable();

    [HideInInspector] public float m_ReSetTime = 0.0f;   //부활시간딜레이
    //시작후에도 딜레이 주기 10초동안

    void Awake()
    {
        //PhotonView 컴포넌트 할당
        pv = GetComponent<PhotonView>();

        //탱크 모델의 모든 Mesh Renderer 컴포넌트를 추출한 후 배열에 할당
        renderers = GetComponentsInChildren<MeshRenderer>();

        //현재 생명치를 초기 생명치로 초깃값 설정
        IsMineBuf_CurHp = initHp;
        currHp = initHp;
        m_OldcurHp = initHp;

        //탱크 폭발 시 생성시킬 폭발 효과를 로드
        expEffect = Resources.Load<GameObject>("ExplosionMobile");

        //Filled 이미지 색상을 녹색으로 설정
        hpBar.color = Color.green;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitCustomProperties(pv);

        //PhotonView의 ownerId를 PlayerId에 저장
        //pv.ownerId -> pv.Owner.ActorNumber 
        playerId = pv.Owner.ActorNumber;

        // ReadyStateTank();
    }

    int a_UpdateCk = 2;
    //// Update is called once per frame
    void Update()
    {
        if (0 < a_UpdateCk)
        {
            a_UpdateCk--;
            if(a_UpdateCk <= 0)
            {
                //이 부분은 탱크가 처음 방에 입장할 때 한번만 호출하게 하기 위한 부분
                //우선 탱크의 상태를 파괴된 이후처럼.. 
                //보이지 않게 하고 모두 Ready상태가 되었을 때 시작하게 한다. 
                ReadyStateTank();
                //이상하게 모든 Update를 돌고난 후에 적용해야 UI가 깨지지 않는다.
                //(탱크 생성시 처음 한번만 발생되도록 한다.)
            }
        }//if (0 < a_UpdateCk)

        ReceiveCurHp();

        ReceiveKillCount();

        if (0.0f < m_ReSetTime)
            m_ReSetTime -= Time.deltaTime;

    }// void Update()

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "CANNON")
        {
            Cannon a_RefCanon = coll.GetComponent<Cannon>();
            if (a_RefCanon == null)
                return;

            //자기가 쏜 총알이면 충돌 제외
            if (playerId == a_RefCanon.AttackerId)
                return;

            //if (pv.IsMine == true)
            //{
                TakeDamage(a_RefCanon.AttackerId, a_RefCanon.AttackerTeam);
            //}
        }//if(coll.gameObject.tag == "CANNON")
    }

    public void TakeDamage(int AttackerId, string a_AttTeam = "blue")
    {
        if (pv.IsMine == false)  //위의 함수 호출 부분에서 체크하고 있음
            return;
        //위 조건을 통과했다는 건 내 실행파일에서 스폰시키고 조정하고 있는 
        //탱크일때만 처리하겠다는 뜻

        if (0.0f < m_ReSetTime)  //게임 시작 후 10초 동안 딜레이 주기
            return;

        //나중에 따라가는 값이니까 이것도 죽어 있는 상태면 아직 데미지 차감 대기 해야함
        if (currHp <= 0)  
            return;

        string a_DamageTeam = "blue";
        if (pv.Owner.CustomProperties.ContainsKey("MyTeam") == true)
            a_DamageTeam = (string)pv.Owner.CustomProperties["MyTeam"];

        //지금 데미지를 받는 탱크가 AttackerId 공격자 팀과 다른 팀일때만 데미지가 들어가도록 처리
        if (a_AttTeam == a_DamageTeam)
            return;

        if (0 < IsMineBuf_CurHp)
        {
            //if (AttackerId == playerId) //자기가 쏜 총알은 자신이 맞으면 안되기 때문에...
            //    return; //위의 함수 호출 부분에서 체크하고 있음

            IsMineBuf_CurHp -= 20;
            if (IsMineBuf_CurHp < 0)
                IsMineBuf_CurHp = 0;

            int a_DamPlayerID = -1; //평타
            if (IsMineBuf_CurHp <= 0)  // 막타
            {
                a_DamPlayerID = AttackerId;
            }

            SendCurHp(IsMineBuf_CurHp, a_DamPlayerID);  //브로드 케이팅 
            //<-- 이걸 해 줘야 브로드 케이팅 된다.

        }//if (0 < IsMineBuf_CurHp)
    }//public void TakeDamage(int AttackerId)

    //폭발 효과 생성 및 리스폰 코루틴 함수
    IEnumerator ExplosionTank()
    {
        //폭발 효과 생성
        if (5.0f < Time.time) //게임 시작 후 5초가 지난다음에 이펙트 터지도록.... 
        //게임이 시작하자마자 기존에 죽어 있는 애들 이펙트가 터지니까 이상하다.
        {
            Object effect = GameObject.Instantiate(expEffect,
                                    transform.position, Quaternion.identity);

            Destroy(effect, 3.0f);
        }

        //HUD를 비활성화
        hudCanvas.enabled = false;

        //탱크 투명 처리
        SetTankVisible(false);

        yield return null;

        ////10초 동안 기다렸다가 활성화하는 로직을 수행
        //yield return new WaitForSeconds(10.0f);

        ////Filled 이미지 초깃값으로 환원
        //hpBar.fillAmount = 1.0f;
        ////Filled 이미지 색상을 녹색으로 설정
        //hpBar.color = Color.green;
        ////HUD 활성화
        //hudCanvas.enabled = true;

        //if (pv != null && pv.IsMine == true)
        //{
        //    //리스폰 시 생명 초깃값 설정
        //    IsMineBuf_CurHp = initHp;

        //    SendCurHp(IsMineBuf_CurHp, -1); // 브로드 케이팅
        //}

        ////탱크를 다시 보이게 처리
        //SetTankVisible(true);
    }

    void SetTankVisible(bool isVisible)
    {
        foreach (MeshRenderer _renderer in renderers)
        {
            _renderer.enabled = isVisible;
        }

        Rigidbody[] a_Rigidbody = this.GetComponentsInChildren<Rigidbody>(true);
        foreach (Rigidbody _Rigidbody in a_Rigidbody)
        {
            _Rigidbody.isKinematic = !isVisible;
        }

        BoxCollider[] a_BoxColls = this.GetComponentsInChildren<BoxCollider>(true);
        foreach (BoxCollider _BoxColl in a_BoxColls)
        {
            _BoxColl.enabled = isVisible;
        }
    }

    //자신을 파괴시킨 적 탱크를 검색해 스코어를 증가시키는 함수
    //firePlayerId : Kill 수를 증가 시키기 위한 탱크 ID 캐릭터 찾아오기
    void SaveKillCount(int firePlayerId)
    {
        //TAKE 태그를 지정된 모든 탱크를 가져와 배열에 저장
        GameObject[] tanks = GameObject.FindGameObjectsWithTag("TANK");
        foreach (GameObject tank in tanks)
        {
            var tankDamage = tank.GetComponent<TankDamage>();
            //탱크의 playerId가 포탄의 playerId와 동일한지 판단
            if (tankDamage != null && tankDamage.playerId == firePlayerId)
            {
                //동일한 탱크일 경우 스코어를 증가시킴
                tankDamage.IncKillCount();
                return;
            }
        }
    }//void SaveKillCount(int firePlayerId)

    void IncKillCount() //때린 탱크 입장으로 호출됨
    {
        if (pv != null && pv.IsMine == true)
        {
            IsMineBuf_killCount++;

            SendKillCount(IsMineBuf_killCount);
            //브로드 케이팅 <--//이걸 해 줘야 브로드 케이팅 된다.
        }//if (pv != null && pv.IsMine == true)
    }//void IncKillCount()

    public void ReadyStateTank()
    {
        if (GameMgr.m_GameState != GameState.GS_Ready)
            return;

        //-------마스터 기준으로 한번만 탱크 리스폰 자리를 정해준다.
  
        //-------마스터 기준으로 한번만 탱크 리스폰 자리를 정해준다.

        StartCoroutine(this.WaitReadyTank());
    }

    //게임 시작 대기...
    IEnumerator WaitReadyTank()
    {
        //HUD를 비활성화
        hudCanvas.enabled = false;

        //탱크 투명 처리
        SetTankVisible(false);

        while (GameMgr.m_GameState == GameState.GS_Ready)
        {
            yield return null;
        }

        //탱크 특정한 위치에 리스폰되도록...
        //--------- 탱크 특정한 위치에 리스폰되도록...
        //위치 고정 필요...
        float pos = Random.Range(-100.0f, 100.0f);
        Vector3 a_SitPos = new Vector3(pos, 20.0f, pos);

        string a_TeamKind = "blue";
        if (pv.Owner.CustomProperties.ContainsKey("MyTeam") == true)
            a_TeamKind = (string)pv.Owner.CustomProperties["MyTeam"];

        if (a_TeamKind == "blue")
        {
            if (pv.Owner.CustomProperties.ContainsKey("SitPosInx") == true) 
            {
                int a_SitPosInx = (int)pv.Owner.CustomProperties["SitPosInx"];
                if (0 <= a_SitPosInx && a_SitPosInx < 4)
                {
                    a_SitPos = GameMgr.m_Team1Pos[a_SitPosInx];
                    this.gameObject.transform.eulerAngles = 
                                            new Vector3(0.0f, 201.0f, 0.0f);
                }
            }
        }
        else if (a_TeamKind == "black")
        {
            if (pv.Owner.CustomProperties.ContainsKey("SitPosInx") == true)
            {
                int a_SitPosInx = (int)pv.Owner.CustomProperties["SitPosInx"];
                if (0 <= a_SitPosInx && a_SitPosInx < 4)
                {
                    a_SitPos = GameMgr.m_Team2Pos[a_SitPosInx];
                    this.gameObject.transform.eulerAngles = 
                                            new Vector3(0.0f, 19.5f, 0.0f);
                }
            }
        }

        this.gameObject.transform.position = a_SitPos;
        m_ReSetTime = 10.0f; //게임 시작후에도 딜레이 주기
        //--------- 탱크 특정한 위치에 리스폰되도록...


        //Filled 이미지 초깃값으로 환원
        hpBar.fillAmount = 1.0f;
        //Filled 이미지 색상을 녹색으로 설정
        hpBar.color = Color.green;
        //HUD 활성화
        hudCanvas.enabled = true;

        if (pv != null && pv.IsMine == true)
        {
            //리스폰 시 생명 초깃값 설정
            IsMineBuf_CurHp = initHp;

            SendCurHp(IsMineBuf_CurHp, -1); // 브로드 케이팅
        }

        //탱크를 다시 보이게 처리
        SetTankVisible(true);
    }


    #region --------------- CustomProperties 초기화
    void InitCustomProperties(PhotonView pv)
    { //속도를 위해 버퍼를 미리 만들어 놓는다는 의미
        //pv.IsMine == true 내가 조정하고 있는 탱크이고 스폰시점에...
        if (pv != null && pv.IsMine == true)
        {
            CurrHpProps.Clear();
            CurrHpProps.Add("curHp", IsMineBuf_CurHp);
            CurrHpProps.Add("LastAttackerID", -1);
            pv.Owner.SetCustomProperties(CurrHpProps);

            KillProps.Clear();
            KillProps.Add("KillCount", 0);
            pv.Owner.SetCustomProperties(KillProps);
        }
    }
    #endregion  //--------------- CustomProperties 초기화


    #region --------------- Hp Sync
    //--------------- Send CurHp
    void SendCurHp(int CurHP = 200, int a_LAtt_ID = -1)
    {
        if (pv == null)
            return;

        if (pv.IsMine == false)
            return;
        //내가 조정하고 있는 탱크 입장에서만 보낸다.
        //(즉 내가 조정하는 탱크를 기준으로만 동기화를 맞춘다.)

        if (CurrHpProps == null)
        {
            CurrHpProps = new ExitGames.Client.Photon.Hashtable();
            CurrHpProps.Clear();
        }

        //자기 탱크의 저장 공간의 값을 갱신해서 브로드 케이팅
        if (CurrHpProps.ContainsKey("curHp") == true) //모든 캐릭터의 에너지바 동기화
        {
            CurrHpProps["curHp"] = CurHP;
        }
        else
        {
            CurrHpProps.Add("curHp", CurHP);
        }

        //내가 죽을 때 막타를 친 유저를 찾아서 킬수를 올려주려고...
        if (CurrHpProps.ContainsKey("LastAttackerID") == true)
        {
            CurrHpProps["LastAttackerID"] = a_LAtt_ID;
        }
        else
        {
            CurrHpProps.Add("LastAttackerID", a_LAtt_ID);
        }

        pv.Owner.SetCustomProperties(CurrHpProps);  //브로드 케이팅 
    }//void SendCurHp(int CurHP = 200, int a_LAtt_ID = -1)
    //--------------- Send CurHp

    //--------------- Receive CurHp
    void ReceiveCurHp() //CurHp 받아서 처리하는 부분
    {
        if (pv == null)
            return;

        if (pv.Owner == null)
            return;

        if (pv.Owner.CustomProperties.ContainsKey("curHp") == true)
        {//모든 캐릭터의 에너지바 동기화
            currHp = (int)pv.Owner.CustomProperties["curHp"];

            //현재 생명치 백분율 = (현재 생명치) / (초기 생명치)
            hpBar.fillAmount = (float)currHp / (float)initHp;

            //생명 수치에 따라 Filled 이미지의 색상을 변경
            if (hpBar.fillAmount <= 0.4f)
                hpBar.color = Color.red;
            else if (hpBar.fillAmount <= 0.6f)
                hpBar.color = Color.yellow;
            else
                hpBar.color = Color.green;

            if (0 < m_OldcurHp && currHp <= 0)
            {
                if (pv.Owner.CustomProperties.ContainsKey("LastAttackerID") == true)
                {
                    int a_LastEmID = (int)pv.Owner.CustomProperties["LastAttackerID"];
                    if (0 <= a_LastEmID)
                        SaveKillCount(a_LastEmID);
                    //자신을 파괴시킨 적 탱크의 스코어를 증가시키는 함수를 호출
                }

                StartCoroutine(this.ExplosionTank());
            }//if (0 < m_OldcurHp && currHp <= 0)

            m_OldcurHp = currHp;
        }//if (pv.Owner.CustomProperties.ContainsKey("curHp") == true) 
    }
    //--------------- Receive CurHp
    #endregion  //--------------- Hp Sync


    #region --------------- KillCount
    //--------------- Send KillCount
    void SendKillCount(int a_KillCount = 0)
    {
        if (pv == null)
            return;

        if (pv.IsMine == false)
            return;

        if (KillProps == null)
        {
            KillProps = new ExitGames.Client.Photon.Hashtable();
            KillProps.Clear();
        }

        if (KillProps.ContainsKey("KillCount") == true)
        {
            KillProps["KillCount"] = a_KillCount;
        }
        else
        {
            KillProps.Add("KillCount", a_KillCount);
        }

        pv.Owner.SetCustomProperties(KillProps);
    }
    //--------------- Send KillCount

    //--------------- Receive KillCount
    void ReceiveKillCount() //KillCount 받아서 처리하는 부분
    {
        if (pv == null)
            return;

        if (pv.Owner == null)
            return;

        if (pv.Owner.CustomProperties.ContainsKey("KillCount") == true)
        {
            int a_KillCnt = (int)pv.Owner.CustomProperties["KillCount"];
            if (killCount != a_KillCnt)
            {
                killCount = a_KillCnt;
                if (txtKillCount != null)
                {
                    txtKillCount.text = killCount.ToString();
                }
            }
        }
    }
    //--------------- Receive KillCount
    #endregion  //--------------- KillCount
}
