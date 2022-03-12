using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public enum GameState
{
    GS_Ready = 0,
    GS_Playing,
    GS_GameEnd,
}

public class GameMgr : MonoBehaviourPunCallbacks
{
    //RPC 호출을 위한 PhotonView
    private PhotonView pv;

    //접속된 플레이어 수를 표시할 Text UI 항목 변수
    public Text txtConnect;

    public Button ExitRoomBtn;

    //접속 로그를 표시할 Text UI 항목 변수
    public Text txtLogMsg;

    public InputField textChat;
    bool bEnter = false;

    //--------------- 팀대전 관련 변수들...
    GameState m_OldState = GameState.GS_Ready;
    public static GameState m_GameState = GameState.GS_Ready;

    ExitGames.Client.Photon.Hashtable m_StateProps =
                        new ExitGames.Client.Photon.Hashtable();

    ExitGames.Client.Photon.Hashtable SitPosInxProps =
                        new ExitGames.Client.Photon.Hashtable();

    [HideInInspector] public static Vector3[] m_Team1Pos = new Vector3[4];
    [HideInInspector] public static Vector3[] m_Team2Pos = new Vector3[4];

    ExitGames.Client.Photon.Hashtable m_SelTeamProps =
                        new ExitGames.Client.Photon.Hashtable();

    ExitGames.Client.Photon.Hashtable m_PlayerReady =
                        new ExitGames.Client.Photon.Hashtable();

    //--------------------Team Select 부분
    [Header("--- Team1 UI ---")]
    public GameObject Team1Panel;
    public Button m_Team1ToTeam2;
    public Button m_Team1Ready;
    public GameObject scrollTeam1;

    [Header("--- Team1 UI ---")]
    public GameObject Team2Panel;
    public Button m_Team2ToTeam1;
    public Button m_Team2Ready;
    public GameObject scrollTeam2;

    [Header("--- Tank Node ---")]
    public GameObject m_TkNodeItem;
    //--------------------Team Select 부분

    [Header("--- StartTmer UI ---")]
    public Text m_WaitTmText;           //게임 시작후 카운트 3, 2, 1, 0
    [HideInInspector] float m_GoWaitGame = 4.0f; //게임 시작후 카운트 Text UI

    private int m_RoundCount = 0;       //라운드 5라운드로 진행
    private double m_ChekWinTime = 2.0f; //라운드 시작후 승패 판정은 2초후부터 하기 위해..
    private int IsRoomBuf_Team1Win = 0; //정확히 한번만 ++ 시키기위한 Room 기준의 버퍼 변수
    private int m_Team1Win = 0;         //블루팀 승리 카운트
    private int IsRoomBuf_Team2Win = 0; //정확히 한번만 ++ 시키기위한 Room 기준의 버퍼 변수
    private int m_Team2Win = 0;         //블랙팀 승리 카운트
    [Header("--- WinLossCount ---")]
    public Text m_WinLossCount;         //승리 카운트 표시 Text UI

    public Text m_GameEndText;

    ExitGames.Client.Photon.Hashtable m_Team1WinProps =
                        new ExitGames.Client.Photon.Hashtable();
    ExitGames.Client.Photon.Hashtable m_Team2WinProps =
                        new ExitGames.Client.Photon.Hashtable();
    //--------------- 팀대전 관련 변수들...


    [Header("---MiniMap---")]
    public RawImage MiniMap;
    bool isMiniMapActive = false;
    public float MiniMapShowTime = 60;
    float MiniMapTimeCount = 60;

    void Awake()
    {
        //--------------- 팀대전 관련 변수 초기화
        m_Team1Pos[0] = new Vector3(88.4f, 20.0f, 77.9f);
        m_Team1Pos[1] = new Vector3(61.1f, 20.0f, 88.6f);
        m_Team1Pos[2] = new Vector3(34.6f, 20.0f, 98.7f);
        m_Team1Pos[3] = new Vector3(7.7f, 20.0f, 108.9f);

        m_Team2Pos[0] = new Vector3(-19.3f, 20.0f, -134.1f);
        m_Team2Pos[1] = new Vector3(-43.1f, 20.0f, -125.6f);
        m_Team2Pos[2] = new Vector3(-66.7f, 20.0f, -117.3f);
        m_Team2Pos[3] = new Vector3(-91.4f, 20.0f, -108.6f);

        m_GameState = GameState.GS_Ready;
        //--------------- 팀대전 관련 변수 초기화

        //PhotonView 컴포넌트 할당
        pv = GetComponent<PhotonView>();

        //탱크를 생성하는 함수 호출
        CreateTank();

        //모든 클아우드의 네트워크 메시지 수신을 다시 연결
        PhotonNetwork.IsMessageQueueRunning = true;

        //룸에 입장 후 기존 접속자 정보를 출력
        GetConnectPlayerCount();

        //----- CustomProperties 초기화
        InitSelTeamProps();
        InitReadyProps();
        InitGStateProps();
        InitTeam1WinProps();
        InitTeam2WinProps();
        //----- CustomProperties 초기화
    }

    //탱크를 생성하는 함수 
    void CreateTank()
    {
        float pos = Random.Range(-100.0f, 100.0f);
        PhotonNetwork.Instantiate("Tank",
            new Vector3(pos, 20.0f, pos), Quaternion.identity, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        //-- TeamSetting
        //내가 입장할때 나를 포함한 다른 사람들에게 내 등장을 알린다. 
        //pv.RPC("RefreshPhotonTeam", RpcTarget.AllViaServer);

        //-- 팀1 버튼 처리
        if (m_Team1ToTeam2 != null)
            m_Team1ToTeam2.onClick.AddListener(() =>
            {
                SendSelTeam("black"); //블랙팀으로 이동
                //pv.RPC("RefreshPhotonTeam", RpcTarget.AllViaServer);
            });

        if (m_Team1Ready != null)
            m_Team1Ready.onClick.AddListener(() =>
            {
                SendReady(1);
                //pv.RPC("RefreshPhotonTeam", RpcTarget.AllViaServer);
            });
        //-- 팀1 버튼 처리

        //-- 팀2 버튼 처리
        if (m_Team2ToTeam1 != null)
            m_Team2ToTeam1.onClick.AddListener(() =>
            {
                SendSelTeam("blue"); //블루팀으로 이동
                //pv.RPC("RefreshPhotonTeam", RpcTarget.AllViaServer);
            });

        if (m_Team2Ready != null)
            m_Team2Ready.onClick.AddListener(() =>
            {
                SendReady(1);
                //pv.RPC("RefreshPhotonTeam", RpcTarget.AllViaServer);
            });
        //-- 팀2 버튼 처리
        //-- TeamSetting

        if (ExitRoomBtn != null)
            ExitRoomBtn.onClick.AddListener(OnClickExitRoom);

        //로그 메시지에 출력할 문자열 생성
        string msg = "\n<color=#00ff00>["
                     + PhotonNetwork.LocalPlayer.NickName
                     + "] Connected</color>";
        //RPC 함수 호출
        pv.RPC("LogMsg", RpcTarget.AllBuffered, msg);

        MiniMapTimeCount = MiniMapShowTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGamePossible() == false) //게임 플로어를 돌려도 되는 상태인지 확인한다.
            return;

        if (m_GameState == GameState.GS_Ready)
        {
            if (IsDifferentList() == true)
            {
                RefreshPhotonTeam();  //리스트 UI 갱신
            }
        }//if (m_GameState == GameState.GS_Ready)

        //체팅 구현 예제
        if (Input.GetKeyDown(KeyCode.Return)) //<-- 엔터치면 인풋 필드 활성화
        {
            bEnter = !bEnter;

            if (bEnter == true)
            {
                textChat.gameObject.SetActive(bEnter);
                textChat.ActivateInputField(); //<--- 커서를 인풋필드로 이동시켜 줌
            }
            else
            {
                textChat.gameObject.SetActive(bEnter);

                if (textChat.text != "")
                {
                    EnterChat();
                }
            }
        }//if (Input.GetKeyDown(KeyCode.Return)) 

        AllReadyObserver();

        if (m_GameState == GameState.GS_Playing)
        {
            Team1Panel.SetActive(false);
            Team2Panel.SetActive(false);
            m_WaitTmText.gameObject.SetActive(false);
        }//if (m_GameState == GameState.GS_Playing)

        if (isMiniMapActive == true)
            MiniMapShow();

        WinLoseObserver();

    }// void Update()

    void EnterChat()
    {
        string msg = "\n<color=#ffffff>[" + PhotonNetwork.LocalPlayer.NickName + "] "
              + textChat.text + "</color>";
        pv.RPC("LogMsg", RpcTarget.AllBuffered, msg);

        textChat.text = "";
    }

    //룸 접속자 정보를 조회하는 함수
    void GetConnectPlayerCount()
    {
        //현재 입장한 룸 정보를 받아옴
        Room currRoom = PhotonNetwork.CurrentRoom;  //using Photon.Realtime;

        //현재 룸의 접속자 수와 최대 접속 가능한 수를 문자열로 구성한 후 Text UI 항목에 출력
        txtConnect.text = currRoom.PlayerCount.ToString()
                          + "/"
                          + currRoom.MaxPlayers.ToString();
    }

    //네트워크 플레이어가 접속했을 때 호출되는 함수 
    public override void OnPlayerEnteredRoom(Player a_Player)
    {
        GetConnectPlayerCount();
    }

    //네트워크 플레이어가 룸을 나가거나 접속이 끊어졌을 때 호출되는 함수
    public override void OnPlayerLeftRoom(Player outPlayer)
    {
        GetConnectPlayerCount();

        //누군가 방을 나가는 경우 
        //if (m_GameState == GameState.GS_Ready)
        //{
        //    if (pv != null)
        //        pv.RPC("RefreshPhotonTeam", RpcTarget.AllViaServer);
        //}
    }

    //룸 나가기 버튼 클릭 이벤트에 연결될 함수
    public void OnClickExitRoom()
    {
        //로그 메시지에 출력할 문자열 생성
        string msg = "\n<color=#ff0000>["
                     + PhotonNetwork.LocalPlayer.NickName
                     + "] Disconnected</color>";
        //RPC 함수 호출
        pv.RPC("LogMsg", RpcTarget.AllBuffered, msg);
        //설정이 완료된 후 빌드 파일을 여러개 실행해
        //동일한 룸에 입장해보면 접속 로그가 표기되는 것을 확인할 수 있다.
        //또한 PhotonTarget.AllBuffered 옵션으로
        //RPC를 호출했기 때문에 나중에 입장해도 기존의 접속 로그 메시지가 표시된다.

        //현재 룸을 빠져나가며 생성한 모든 네트워크 객체를 삭제
        PhotonNetwork.LeaveRoom();
    }

    //룸에서 접속 종료됐을 때 호출되는 콜백 함수
    public override void OnLeftRoom()  //PhotonNetwork.LeaveRoom(); 성공했을 때 
    {
        //로비 씬을 호출
        UnityEngine.SceneManagement.SceneManager.LoadScene("scLobby");
    }

    [PunRPC]
    void LogMsg(string msg)
    {
        //로그 메시지 Text UI에 텍스트를 누적시켜 표시
        txtLogMsg.text = txtLogMsg.text + msg;
    }

    bool IsDifferentList() //true면 다르다는 뜻 false면 같다는 뜻
    {
        GameObject[] a_TkNodeItems = GameObject.FindGameObjectsWithTag("TKNODE_ITEM");

        if(a_TkNodeItems == null)
            return true;

        if (PhotonNetwork.PlayerList.Length != a_TkNodeItems.Length)
            return true;

        foreach (Player a_RefPlayer in PhotonNetwork.PlayerList)
        {
            bool a_FindNode = false;
            TankNodeItem TankData = null;
            foreach (GameObject a_Node in a_TkNodeItems)
            {
                TankData = a_Node.GetComponent<TankNodeItem>();
                if (TankData == null)
                    continue;

                if(TankData.m_UniqID == a_RefPlayer.ActorNumber)
                {
                    if (TankData.m_TeamKind != ReceiveSelTeam(a_RefPlayer))
                        return true; //해당 유저의 팀이 변경 되었다면...

                    if (TankData.m_IamReady != ReceiveReady(a_RefPlayer))
                        return true; //해당 Ready 상태가 변경 되었다면...

                    a_FindNode = true;
                    break;
                }
            }//foreach (GameObject a_Node in GameObject.FindGameObjectsWithTag("TKNODE_ITEM"))

            if(a_FindNode == false) 
                return true; //해당 유저가 리스트에 존재하지 않으면....

        }//foreach (Player a_RefPlayer in PhotonNetwork.PlayerList)

        return false; //일치한다는 뜻
    }

    //[PunRPC]
    void RefreshPhotonTeam()
    {
        //if (m_GameState != GameState.GS_Ready) //Update 에서 처리
        //    return;

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("TKNODE_ITEM"))
        {
            Destroy(obj);
        }

        GameObject[] a_tanks = GameObject.FindGameObjectsWithTag("TANK");

        string a_TeamKind = "blue";
        GameObject a_TkNode = null;


        foreach (Player a_RefPlayer in PhotonNetwork.PlayerList)
        {
            a_TeamKind = ReceiveSelTeam(a_RefPlayer);
            a_TkNode = (GameObject)Instantiate(m_TkNodeItem);

            //팀이 뭐냐?에 따라서 스크롤 뷰를 분기 해 준다.
            if (a_TeamKind == "blue")
                a_TkNode.transform.SetParent(scrollTeam1.transform, false);
            else if (a_TeamKind == "black")
                a_TkNode.transform.SetParent(scrollTeam2.transform, false);

            //생성한 RoomItem에 표시하기 위한 텍스트 정보 전달
            TankNodeItem TankData = a_TkNode.GetComponent<TankNodeItem>();
            //텍스트 정보를 표시
            if (TankData != null)
            {
                TankData.m_UniqID = a_RefPlayer.ActorNumber;
                TankData.m_TeamKind = a_TeamKind;
                TankData.m_IamReady = ReceiveReady(a_RefPlayer);
                bool isMine = TankData.m_UniqID == PhotonNetwork.LocalPlayer.ActorNumber;
                TankData.DispPlayerData(a_RefPlayer.NickName, isMine);
            }

            //이름표 색깔 바꾸기
            ChangeTankNameColor(a_tanks, a_RefPlayer.ActorNumber, a_TeamKind);
            // 미니맵 플레이어 색깔 바꾸기
            ChangeMiniMapTankColor(a_tanks, a_RefPlayer.ActorNumber, a_TeamKind);
        }//foreach (Player a_RefPlayer in PhotonNetwork.PlayerList)

        //-------------- Ready Off 하기...
        if (ReceiveReady(PhotonNetwork.LocalPlayer) == true)
        {
            m_Team1Ready.gameObject.SetActive(false);
            m_Team2Ready.gameObject.SetActive(false);

            m_Team1ToTeam2.gameObject.SetActive(false);
            m_Team2ToTeam1.gameObject.SetActive(false);
        }
        else
        {
            a_TeamKind = ReceiveSelTeam(PhotonNetwork.LocalPlayer);
            if (a_TeamKind == "blue")
            {
                m_Team1Ready.gameObject.SetActive(true);
                m_Team2Ready.gameObject.SetActive(false);
                m_Team1ToTeam2.gameObject.SetActive(true);
                m_Team2ToTeam1.gameObject.SetActive(false);
            }
            else if (a_TeamKind == "black")
            {
                m_Team1Ready.gameObject.SetActive(false);
                m_Team2Ready.gameObject.SetActive(true);
                m_Team1ToTeam2.gameObject.SetActive(false);
                m_Team2ToTeam1.gameObject.SetActive(true);
            }
        }
        //-------------- Ready Off 하기...

    }//void RefreshPhotonTeam()

    void ChangeTankNameColor(GameObject[] a_tanks, int ActorNumber, string a_TeamKind)
    {
        //이름표 색깔 바꾸기
        DisplayUserId a_DpUserId = null;
        foreach (GameObject tank in a_tanks)
        {
            a_DpUserId = tank.GetComponent<DisplayUserId>();
            if (a_DpUserId != null)
            {
                //tankDamage.pv.ownerId   ==   //tankDamage.pv.owner.ID
                if (a_DpUserId.pv.Owner.ActorNumber == ActorNumber)
                {
                    if (a_TeamKind == "blue")
                        a_DpUserId.userId.color = new Color32(60, 60, 255, 255);
                    else
                        a_DpUserId.userId.color = Color.black;

                    break;
                }//if (a_DpUserId.pv.Owner.ActorNumber == ActorNumber)
            }//if (a_DpUserId != null)
        }//foreach (GameObject tank in a_tanks)
    }

    ///  ------미니맵 유저 색깔 바꾸기
    void ChangeMiniMapTankColor(GameObject[] a_tanks, int ActorNumber, string a_TeamKind)
    {
        DisplayUserId a_DpUserId = null;

        foreach (GameObject tank in a_tanks)
        {
            a_DpUserId = tank.GetComponent<DisplayUserId>();
            if (a_DpUserId != null)
            {
                //tankDamage.pv.ownerId   ==   //tankDamage.pv.owner.ID
                if (a_DpUserId.pv.Owner.ActorNumber == ActorNumber)
                {
                    if (a_TeamKind == "blue")
                        a_DpUserId.MiniMapUI.color = new Color32(60, 60, 255, 255);
                    else
                        a_DpUserId.MiniMapUI.color = Color.black;

                    break;
                }//if (a_DpUserId.pv.Owner.ActorNumber == ActorNumber)

                // 자신만 빨간색으로 보이게 하기
                //if (a_DpUserId.pv.IsMine)
                //{
                //    a_DpUserId.MiniMapUI.color = Color.red;

                //    break;
                //}
            }//if (a_DpUserId != null)
        }//foreach (GameObject tank in a_tanks)
    }


    #region --------------- Observer Method 모음 
    bool IsGamePossible()  //게임이 가능한 상태인지? 체크하는 함수
    {
        //나가는 타이밍에 포톤 정보들이 한플레임 먼저 사라지고 
        //LoadScene()이 한플레임 늦게 호출되는 문제 해결법
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.LocalPlayer == null)  
            return false; //동기화 가능한 상태 일때만 업데이트를 계산해 준다.

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("GameState") == false ||
            PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Team1Win") == false ||
            PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Team2Win") == false)
            return false;

        //PhotonNetwork.CurrentRoom.CustomProperties 에 저장되어 있는 게임 상태 받아오기
        m_GameState = ReceiveGState();
        m_Team1Win = (int)PhotonNetwork.CurrentRoom.CustomProperties["Team1Win"];
        m_Team2Win = (int)PhotonNetwork.CurrentRoom.CustomProperties["Team2Win"];

        return true;
    }

    // 참가 유저 모두 Ready 버튼 눌렀는지 감시하고 게임을 시작하게 처리하는 함수
    void AllReadyObserver()
    {
        if (m_GameState != GameState.GS_Ready) //GS_Ready 상태에서만 확인한다.
            return;

        int a_OldGoWait = (int)m_GoWaitGame;

        bool a_AllReady = true;
        foreach (Player a_RefPlayer in PhotonNetwork.PlayerList)
        {
            if (ReceiveReady(a_RefPlayer) == false)
            {
                a_AllReady = false;
                break;
            }
        }//foreach (Player a_RefPlayer in PhotonNetwork.PlayerList)

        if (a_AllReady == true) //모두가 준비 버튼을 누르고 기다리고 있다는 뜻 
        {
            //누가 발생시켰든 동기화 시키려고 하면....
            if (m_RoundCount == 0 && PhotonNetwork.CurrentRoom.IsOpen == true)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                //게임이 시작되면 다른 유저 들어오지 못하도록 막는 부분
                //PhotonNetwork.CurrentRoom.IsVisible = false; 
                //로비에서 방 목록에서도 보이지 않게 하기
            }

            //--- 각 플레이어 PC 별로 3, 2, 1, 0 타이머 UI 표시를 위한 코드
            if (0.0f < m_GoWaitGame)  //타이머 카운티 처리
            {
                m_GoWaitGame = m_GoWaitGame - Time.deltaTime;

                if (m_WaitTmText != null)
                {
                    m_WaitTmText.gameObject.SetActive(true);
                    m_WaitTmText.text = ((int)m_GoWaitGame).ToString();
                }

                //마스터 클라이언트는 각 유저의 자리배치를 해 줄 것이다.
                //총 4번만 보낸다. MasterClient가 나갈 경우를 대비해서...
                if (PhotonNetwork.IsMasterClient == true)
                if (0.0f < m_GoWaitGame && a_OldGoWait != (int)m_GoWaitGame) 
                {//자리 배정
                    SitPosInxMasterCtrl();
                }//if(a_OldGoWait != (int)m_GoWaitGame) //자리 배정

                if (m_GoWaitGame <= 0.0f) //이건 한번만 발생할 것이다.
                {//진짜 게임 시작 준비
                    m_RoundCount++;

                    Team1Panel.SetActive(false);
                    Team2Panel.SetActive(false);
                    m_WaitTmText.gameObject.SetActive(false);

                    m_ChekWinTime = 2.0f;
                    m_GoWaitGame = 0.0f;


                    isMiniMapActive = true;
                   // Debug.Log(isMiniMapActive);

                }//if (m_GoWaitGame <= 0.0f)
            }//if (0.0f < m_GoWaitGame) 
            //--- 각 플레이어 PC 별로 타이머 UI 표시를 위한 코드

            //게임이 시작 되었어야 하는데 아직 시작 되지 않았다면....
            if (PhotonNetwork.IsMasterClient == true) //마스터 클라이언트만 체크하고 보낸다.
            if (m_GoWaitGame <= 0.0f) //&& ReceiveGState() == GameState.GS_Ready) //위에서 체크함 
            {
                SendGState(GameState.GS_Playing);
            }
        }//if (a_AllReady == true) //모두가 준비 버튼을 누르고 기다리고 있다는 뜻 

    }//void AllReadyObserver()

    void MiniMapShow()
    {
        // 미니맵 보기 카운트다운 시작
        MiniMapTimeCount = MiniMapTimeCount - Time.deltaTime;

        if (MiniMapTimeCount <= 0)
            MiniMapTimeCount = 0;

        Debug.Log(MiniMapTimeCount);

        if ( MiniMapTimeCount <= 0)
        {
            MiniMap.gameObject.SetActive(true);
        }
    }
    
    //한쪽팀이 전멸했는지 체크하고 승리 / 패배 를 감시하고 처리해 주는 함수
    void WinLoseObserver()
    {
        //------------------- 승리 / 패배 체크
        if (m_GameState == GameState.GS_Playing) //GS_Ready 상태의 중계가 좀 늦게와서 한쪽이 전멸 상태라는 걸 몇번 체크할 수는 있다.
        {
            m_ChekWinTime = m_ChekWinTime - Time.deltaTime;
            if (m_ChekWinTime <= 0.0f) //게임이 시작된 후 2초 뒤부터 판정을 시작하기 위한 부분
            {
                int a_Tm1Count = 0;
                int a_Tm2Count = 0;
                int rowTm1 = 0;
                int rowTm2 = 0;
                int a_CurHP = 0;
                string a_PlrTeam = "blue"; //Player Team

                Player[] players = PhotonNetwork.PlayerList; //using Photon.Realtime;
                foreach (Player _player in players)
                {
                    if (_player.CustomProperties.ContainsKey("MyTeam") == true)
                        a_PlrTeam = (string)_player.CustomProperties["MyTeam"];

                    if (a_PlrTeam == "blue")
                    {
                        if (_player.CustomProperties.ContainsKey("curHp") == true) //모든 캐릭터의 에너지바 동기화
                        {
                            a_CurHP = (int)_player.CustomProperties["curHp"];   //모든 캐릭터... 매플레임 계속 동기화 
                            if (0 < a_CurHP)
                            {
                                rowTm1 = 1;
                            }
                        }
                        a_Tm1Count++;

                    }
                    else if (a_PlrTeam == "black")
                    {
                        if (_player.CustomProperties.ContainsKey("curHp") == true) //모든 캐릭터의 에너지바 동기화
                        {
                            a_CurHP = (int)_player.CustomProperties["curHp"];   //모든 캐릭터... 매플레임 계속 동기화 
                            if (0 < a_CurHP)
                            {
                                rowTm2 = 1;
                            }
                        }
                        a_Tm2Count++;

                    }
                }//foreach (Player _player in players)

                if (0 < a_Tm1Count && 0 < a_Tm2Count)   //양팀에 인원이 존재할 때만...
                {
                    if (rowTm1 == 0 || rowTm2 == 0)     //양 팀중에 한팀이 전멸했다면....
                    {
                        if ((m_Team1Win + m_Team2Win) < 5) //아직 5Round까지 가지 않은 상황이라면...
                        {
                            if (PhotonNetwork.IsMasterClient == true)
                            {
                                SendGState(GameState.GS_Ready);

                                if (rowTm1 == 0)
                                {
                                    if (-99999.0f < m_ChekWinTime) //한번만 ++ 시키기 위한 용도
                                    {
                                        m_Team2Win++;  //여러번 발생하더라도 아직은 업데이트가 안된 상태이기 때문에 이전 값에서 추가될 것이다.
                                        IsRoomBuf_Team2Win = m_Team2Win;
                                        m_ChekWinTime = -150000.0f;  
                                    }
                                    SendTeam2Win(IsRoomBuf_Team2Win);                                    
                                }
                                if (rowTm2 == 0)
                                {
                                    if (-99999.0f < m_ChekWinTime) //한번만 ++ 시키기 위한 용도
                                    {
                                        m_Team1Win++;
                                        IsRoomBuf_Team1Win = m_Team1Win;
                                        m_ChekWinTime = -150000.0f;
                                    }
                                    SendTeam1Win(IsRoomBuf_Team1Win);
                                }

                            }//if (PhotonNetwork.IsMasterClient == true)  

                            m_GoWaitGame = 4.0f; //다시 4초후에 게임이 시작되도록...

                            // 미니맵 생성 초기화
                            MiniMap.gameObject.SetActive(false);
                            isMiniMapActive = false;
                            MiniMapTimeCount = MiniMapShowTime;

                        }//if ((m_Team1Win + m_Team2Win) < 5) //아직 5Round까지 가지 않은 상황이라면...
                    }//if (rowTm1 == 0 || rowTm2 == 0) //양 팀중에 한팀이 전멸했다면....
                }//if (0 < a_Tm1Count && 0 < a_Tm2Count)
                //m_ChekWinTime = 0.0f;
            }// if (m_ChekWinTime <= 0.0f)
        }//if (m_GameState == GameState.GS_Playing) 

        if (m_WinLossCount != null)
            m_WinLossCount.text = "<color=Blue>" + "Team1 : " + m_Team1Win.ToString() + " 승 " + "</color> / "
                                + "<color=Black>" + "Team2 : " + m_Team2Win.ToString() + " 승 " + "</color>";

        if (5 <= (m_Team1Win + m_Team2Win)) //아직 5Round까지 모두 플레이된 상황이라면... 
        {
            //Game Over 처리
            if (PhotonNetwork.IsMasterClient == true)
            {
                //누가 발생시켰든 동기화 시키려고 하면....
                SendGState(GameState.GS_GameEnd); //<--- 여기서는 지금 룸을 의미함

            }

            if (m_GameEndText != null)
            {
                m_GameEndText.gameObject.SetActive(true);
                if (m_Team2Win < m_Team1Win)
                {
                    m_GameEndText.text = "<color=Blue>" + "블루팀 승"+"</color>";
                }
                else if (m_Team1Win < m_Team2Win)
                {
                    m_GameEndText.text = "<color=Black>" + "블랙팀 승"+ "</color>";
                }
            }
            return;
        }
        //------------------- 승리 / 패배 체크

        //-------------- 한 Round가 끝나고 다은 Round의 게임을 시작 시키기 위한 부분... //모든탱크 GS_Ready 상태일 때 모든 탱크 대기 상태로 만들기...
        if (m_OldState != GameState.GS_Ready && m_GameState == GameState.GS_Ready)
        {
            GameObject[] tanks = GameObject.FindGameObjectsWithTag("TANK");
            foreach (GameObject tank in tanks)
            {
                TankDamage tankDamage = tank.GetComponent<TankDamage>();
                if (tankDamage != null)
                    tankDamage.ReadyStateTank(); //다음 라운드 준비 --> 1
            }
        }
        m_OldState = m_GameState;
        //-------------- 한 Round가 끝나고 다은 Round의 게임을 시작 시키기 위한 부분... 
    }//void WinLoseObserver()

    //자리 배정 함수
    void SitPosInxMasterCtrl()
    {
        //if (PhotonNetwork.IsMasterClient == false)
        //    return;  //상위에서 확인하고 있음

        int a_Tm1Count = 0;
        int a_Tm2Count = 0;
        string a_TeamKind = "blue";
        foreach (Player _player in PhotonNetwork.PlayerList) //using Photon.Realtime;
        {
            if (_player.CustomProperties.ContainsKey("MyTeam") == true)
                a_TeamKind = (string)_player.CustomProperties["MyTeam"];

            if (a_TeamKind == "blue")
            {
                SitPosInxProps.Clear();
                SitPosInxProps.Add("SitPosInx", a_Tm1Count);
                _player.SetCustomProperties(SitPosInxProps);
                a_Tm1Count++;
            }
            else if (a_TeamKind == "black")
            {
                SitPosInxProps.Clear();
                SitPosInxProps.Add("SitPosInx", a_Tm2Count);
                _player.SetCustomProperties(SitPosInxProps);
                a_Tm2Count++;
            }
        }
    }//void SitPosInxMasterCtrl()

    #endregion  //--------------- Observer Method 모음


    #region --------------- Team1 Win Count
    void InitTeam1WinProps()
    {
        if (PhotonNetwork.CurrentRoom == null)
            return;

        m_Team1WinProps.Clear();
        m_Team1WinProps.Add("Team1Win", 0);
        PhotonNetwork.CurrentRoom.SetCustomProperties(m_Team1WinProps);
    }

    void SendTeam1Win(int a_WinCount)
    {
        //if (PhotonNetwork.CurrentRoom == null)
        //    return;   //Update에서 체크 하고 있다.

        //if (PhotonNetwork.IsMasterClient == false) //마스터만 보낸다.
        //    return;   //Update에서 체크 하고 있다.

        if (m_Team1WinProps == null)
        {
            m_Team1WinProps = new ExitGames.Client.Photon.Hashtable();
            m_Team1WinProps.Clear();
        }

        if (m_Team1WinProps.ContainsKey("Team1Win") == true)
            m_Team1WinProps["Team1Win"] = a_WinCount;
        else
            m_Team1WinProps.Add("Team1Win", a_WinCount);

        PhotonNetwork.CurrentRoom.SetCustomProperties(m_Team1WinProps);
    }
    #endregion  //--------------- Team1 Win Count

    #region --------------- Team2 Win Count
    void InitTeam2WinProps()
    {
        if (PhotonNetwork.CurrentRoom == null)
            return;

        m_Team2WinProps.Clear();
        m_Team2WinProps.Add("Team2Win", 0);
        PhotonNetwork.CurrentRoom.SetCustomProperties(m_Team2WinProps);
    }

    void SendTeam2Win(int a_WinCount)
    {
        //if (PhotonNetwork.CurrentRoom == null)
        //    return;   //Update에서 체크 하고 있다.

        //if (PhotonNetwork.IsMasterClient == false) //마스터만 보낸다.
        //    return;   //Update에서 체크 하고 있다.

        if (m_Team2WinProps == null)
        {
            m_Team2WinProps = new ExitGames.Client.Photon.Hashtable();
            m_Team2WinProps.Clear();
        }

        if (m_Team2WinProps.ContainsKey("Team2Win") == true)
            m_Team2WinProps["Team2Win"] = a_WinCount;
        else
            m_Team2WinProps.Add("Team2Win", a_WinCount);

        PhotonNetwork.CurrentRoom.SetCustomProperties(m_Team2WinProps);
    }
    #endregion  //--------------- Team2 Win Count


    #region --------------- 게임 상태 동기화 처리
    void InitGStateProps()
    {
        if (PhotonNetwork.CurrentRoom == null)
            return;

        m_StateProps.Clear();
        m_StateProps.Add("GameState", (int)GameState.GS_Ready);
        PhotonNetwork.CurrentRoom.SetCustomProperties(m_StateProps);
    }

    void SendGState(GameState a_GState)
    {
        //if (PhotonNetwork.CurrentRoom == null)
        //    return;   //Update에서 체크 하고 있다.

        //if (PhotonNetwork.IsMasterClient == false) //마스터만 보낸다.
        //    return;   //Update에서 체크 하고 있다.

        if (m_StateProps == null)
        {
            m_StateProps = new ExitGames.Client.Photon.Hashtable();
            m_StateProps.Clear();
        }

        if (m_StateProps.ContainsKey("GameState") == true)
            m_StateProps["GameState"] = (int)a_GState;
        else
            m_StateProps.Add("GameState", (int)a_GState);

        PhotonNetwork.CurrentRoom.SetCustomProperties(m_StateProps);  

    }

    GameState ReceiveGState() //GameState 받아서 처리하는 부분
    {
        GameState a_RmVal = GameState.GS_Ready;

        //if (PhotonNetwork.CurrentRoom == null)  
        //    return a_RmVal;    //Update에서 체크 하고 있다.

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("GameState") == true)
            a_RmVal = (GameState)PhotonNetwork.CurrentRoom.CustomProperties["GameState"];

        return a_RmVal;
    }
    #endregion  //--------------- 게임 상태 동기화 처리


    #region --------------- 팀선택 동기화 처리
    void InitSelTeamProps()
    { //속도를 위해 버퍼를 미리 만들어 놓는다는 의미
        m_SelTeamProps.Clear();
        m_SelTeamProps.Add("MyTeam", "blue");   //기본적으로 나는 블루팀으로 시작한다.
        PhotonNetwork.LocalPlayer.SetCustomProperties(m_SelTeamProps);  
        //캐릭터 별로 동기화 시키고 싶은 경우
    }//void InitSelTeamProps()

    //--------------- Send SelTeam
    void SendSelTeam(string a_Team)
    {
        if (string.IsNullOrEmpty(a_Team) == true)
            return;

        if (m_SelTeamProps == null)
        {
            m_SelTeamProps = new ExitGames.Client.Photon.Hashtable();
            m_SelTeamProps.Clear();
        }

        if (m_SelTeamProps.ContainsKey("MyTeam") == true)
        {
            m_SelTeamProps["MyTeam"] = a_Team;
        }
        else
        {
            m_SelTeamProps.Add("MyTeam", a_Team);
        }

        PhotonNetwork.LocalPlayer.SetCustomProperties(m_SelTeamProps);  //캐릭터 별로 동기화 시키고 싶은 경우
    }
    //--------------- Send SelTeam

    //--------------- Receive SelTeam
    string ReceiveSelTeam(Player a_Player) //SelTeam 받아서 처리하는 부분
    {
        string a_TeamKind = "blue";

        if (a_Player == null)
            return a_TeamKind;

        if (a_Player.CustomProperties.ContainsKey("MyTeam") == true)
            a_TeamKind = (string)a_Player.CustomProperties["MyTeam"];

        return a_TeamKind;
    }
    //--------------- Receive SelTeam
    #endregion  //--------------- 팀선택 동기화 처리


    #region --------------- Ready 상태 동기화 처리
    void InitReadyProps()
    { //속도를 위해 버퍼를 미리 만들어 놓는다는 의미
        m_PlayerReady.Clear();
        m_PlayerReady.Add("IamReady", 0);      //기본적으로 아직 준비전 상태로 시작한다.
        PhotonNetwork.LocalPlayer.SetCustomProperties(m_PlayerReady);  
        //캐릭터 별로 동기화 시키고 싶은 경우
    }//void InitSelTeamProps()

    //--------------- Send Ready 
    void SendReady(int a_Ready = 1)
    {
        if (m_PlayerReady == null)
        {
            m_PlayerReady = new ExitGames.Client.Photon.Hashtable();
            m_PlayerReady.Clear();
        }

        if (m_PlayerReady.ContainsKey("IamReady") == true)
        {
            m_PlayerReady["IamReady"] = a_Ready;
        }
        else
        {
            m_PlayerReady.Add("IamReady", a_Ready);
        }

        PhotonNetwork.LocalPlayer.SetCustomProperties(m_PlayerReady);  //캐릭터 별로 동기화 시키고 싶은 경우
    }
    //--------------- Send Ready

    //--------------- Receive Ready
    bool ReceiveReady(Player a_Player) //Ready 상태를 받아서 처리하는 부분
    {
        if (a_Player == null)
            return false;

        if (a_Player.CustomProperties.ContainsKey("IamReady") == false)
            return false;

        if ((int)a_Player.CustomProperties["IamReady"] == 0)
            return false;
        else
            return true;
    }
    //--------------- Receive Ready
    #endregion  //--------------- Ready 상태 동기화 처리

    private void OnGUI()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            // 게임이 아직 시작되지 않은 경우는 각 "유저의 별명 : 킬수 : 사망상태"는 표시하지 않는다.
            if (PhotonNetwork.CurrentRoom.IsOpen == true)
            {
                return;
            }

            // 현재 입장한 룸에 접속한 모든 네트워크 플레이어 정보를 저장
            int a_CurHP = 0;
            int currKillCount = 0;
            Player[] players = PhotonNetwork.PlayerList; // using Photon.RealTime;
            string PlayerTeam = "blue";

            foreach (Player _player in players)
            {
                currKillCount = 0;
                if (_player.CustomProperties.ContainsKey("KillCount") == true)
                    currKillCount = (int)_player.CustomProperties["KillCount"];

                PlayerTeam = "blue";
                if (_player.CustomProperties.ContainsKey("MyTeam") == true)
                    PlayerTeam = (string)_player.CustomProperties["MyTeam"];

                if (_player.CustomProperties.ContainsKey("curHp") == true) //모든 캐릭터의 에너지바 동기화
                {
                    a_CurHP = (int)_player.CustomProperties["curHp"];   //모든 캐릭터... 매플레임 계속 동기화 

                    if (a_CurHP <= 0)
                    {
                        if (PlayerTeam == "blue")
                        {
                            GUILayout.Label("<color=Blue><size=25>" +
                                "[" + _player.ActorNumber + "]" + _player.NickName + " "
                                + currKillCount + " kill" + "</size></color>"
                                + "<color=Red><size=25>" + " <Die>" + "</size></color>");
                        }

                        else
                        {
                            GUILayout.Label("<color=Black><size=25>" +
                                "[" + _player.ActorNumber + "]" + _player.NickName + " "
                                + currKillCount + " kill" + "</size></color>"
                                + "<color=Red><size=25>" + " <Die>" + "</size></color>");

                        }
                        continue;
                    }
                }

                if (PlayerTeam == "blue")
                {
                    GUILayout.Label("<color=Blue><size=25>" + "[" + _player.ActorNumber + "]"
                        + _player.NickName + " " + currKillCount + " kill" + "</size></color>");
                }

                else
                {

                    GUILayout.Label("<color=Black><size=25>" + "[" + _player.ActorNumber + "]"
                        + _player.NickName + " " + currKillCount + " kill" + "</size></color>");
                }
            }
        }
    }
}
