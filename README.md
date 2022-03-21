# TankAttack_TeamVS  
포톤 서버를 사용하는 실시간 팀 대전게임입니다.

### <플레이 방법>
##### - 최대 8명이서 플레이할 수 있습니다.  
##### - 키보드로 탱크를 이동시킬 수 있고 마우스 좌클릭을 눌러 포탄을 발사할 수 있습니다.  
##### - 총 5라운드를 진행하며 블루팀과 레드팀의 승리 수의 합이 5가 되면 승리 수가 많은 팀이 이기게 됩니다.  
##### - 라운드 시작 후 10초가 지나기 전까진 공격을 할 수 없습니다.  
##### - 라운드 시작 후 30초가 지나면 미니맵이 활성화되어 아군과 적군의 위치를 알 수 있습니다.  

--------------------------

## 1.방 생성  
https://user-images.githubusercontent.com/63942174/158361325-c7fa9025-d939-433f-93c3-f8e82386f4a0.mp4


#####로비화면의 Make Room버튼을 누르면 방을 만들 수 있도록 구현하였다. 만들어진 방은 다른 플레이어들이   
#####볼수 있고 방을 클릭하면 해당 방에 접속할 수 있다.  


<details>
    <summary>포톤 클라우드 서버 접속과 방생성을 위한 코드</summary>
  
``` C#
void Awake()
    {
        //포톤 클라우드 서버 접속 여부 확인
        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();  //포톤 클라우드에 접속
        
        userId.text = GetUserId();  //사용자 이름 설정

        //룸 이름을 무작위로 설정
        roomName.text = "Room_" + Random.Range(0, 999).ToString("000");
    }
    
    // 방만들기 버튼 클릭 시 호출될 함수
 public void ClickCreateRoom()
    {
        string _roomName = roomName.text;
        
        if (string.IsNullOrEmpty(roomName.text))  //룸 이름이 없거나 Null일 경우 룸 이름 지정
            _roomName = "ROOM_" + Random.Range(0, 999).ToString("000");

        PhotonNetwork.LocalPlayer.NickName = userId.text;  //로컬 플레이어의 이름을 설정
        
        PlayerPrefs.SetString("USER_ID", userId.text);  //플레이어 이름을 저장

        //생성할 룸의 조건 설정
        RoomOptions roomOptions = new RoomOptions();  //using Photon.Realtime;
        roomOptions.IsOpen = true;     //입장 가능 여부
        roomOptions.IsVisible = true;  //로비에서 룸의 노출 여부
        roomOptions.MaxPlayers = 8;    //룸에 입장할 수 있는 최대 접속자 수

        //지정한 조건에 맞는 룸 생성 함수
        PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);  //TypedLobby.Default 어느 로비에 방을 만들껀지? 
    }
    
     //PhotonNetwork.CreateRoom() 이 함수가 실패 하면 호출되는 함수(같은 이름의 방이 있을 때 실패함)
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("방 만들기 실패"); //주로 같은 이름의 방이 존재할 때 룸생성 에러가 발생된다.
        Debug.Log(returnCode.ToString()); //오류 코드(ErrorCode 클래스)
        Debug.Log(message); //오류 메시지
    }
    
    //생성된 룸 목록이 변경됐을 때 호출되는 콜백 함수(방 리스트 갱신은 로비에서만 가능하다.)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) 
                myList.RemoveAt(myList.IndexOf(roomList[i]));
        }

        //룸 목록을 다시 받았을 때 갱신하기 위해 기존에 생성된 RoomItem을 삭제
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ROOM_ITEM"))
            Destroy(obj);
            
        //스크롤 영역 초기화
        scrollContents.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        for (int i = 0; i < myList.Count; i++)
        {
            GameObject room = (GameObject)Instantiate(roomItem);
            
            room.transform.SetParent(scrollContents.transform, false);//생성한 RoomItem 프리팹의 Parent를 지정

            //생성한 RoomItem에 표시하기 위한 텍스트 정보 전달
            RoomData roomData = room.GetComponent<RoomData>();
            roomData.roomName = myList[i].Name;
            roomData.connectPlayer = myList[i].PlayerCount;
            roomData.maxPlayer = myList[i].MaxPlayers;

            //텍스트 정보를 표시
            roomData.DispRoomData(myList[i].IsOpen);
        }//for (int i = 0; i < roomCount; i++)
    }// public override void OnRoomListUpdate(List<RoomInfo> roomList)

//RoomItem이 클릭되면 호출될 이벤트 연결 함수
    public void OnClickRoomItem(string roomName)
    {
        //로컬 플레이어의 이름을 설정
        PhotonNetwork.LocalPlayer.NickName = userId.text;
        //플레이어 이름을 저장
        PlayerPrefs.SetString("USER_ID", userId.text);

        //인자로 전달된 이름에 해당하는 룸으로 입장
        PhotonNetwork.JoinRoom(roomName);
    }
```
 </details>

## 2.랜덤방 입장  

https://user-images.githubusercontent.com/63942174/158361351-8a318f42-bbbd-47c3-8636-2c99131d8c59.mp4

<h3>Join Random Room 버튼을 누르면 현재 만들어져 있는 임의의 방에 접속하게 된다.</h>
  
<details>  
    <summary>랜덤방 입장</summary>

```C#
     public override void OnConnectedToMaster()
    {
        //단순 포톤 서버 접속만 된 상태 (ConnectedToMaster)
        Debug.Log("서버 접속 완료");
        PhotonNetwork.JoinLobby();
    }
    
    
    // PhotonNetwork.JoinLobby() 성공시 호출되는 로비 접속 콜백함수
    public override void OnJoinedLobby()
    {
        Debug.Log("로비접속완료");
        userId.text = GetUserId(); //방에서 로비로 나올 때도 유저 ID를 하나 셋팅해 주어야 한다.
    }
    
     public override void OnJoinedRoom()
    {
        Debug.Log("방 참가 완료");
        //룸 씬으로 이동하는 코루틴 실행
        StartCoroutine(this.LoadBattleField());
    }

    //PhotonNetwork.JoinRandomRoom() 이 함수 실패한 경우 호출되는 콜백 함수
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("랜덤 방 참가 실패 (참가할 방이 존재하지 않습니다.)");
    }
    
      //룸 씬으로 이동하는 코루틴 함수
    IEnumerator LoadBattleField()        //최종 배틀필드 씬 로딩
    {
        //씬을 이동하는 동안 포톤 클라우드 서버로부터 네트워크 메시지 수신 중단
        PhotonNetwork.IsMessageQueueRunning = false;
        //백그라운드로 씬 로딩

        Time.timeScale = 1.0f;  //게임에 들어갈 때는 원래 속도로...

        AsyncOperation ao = SceneManager.LoadSceneAsync("scBattleField");

        yield return ao;
    }
    
```
    
 </details>  
    
    
## 3.환경설정

https://user-images.githubusercontent.com/63942174/158361437-9871a9f5-b60e-4c03-8db9-059c4a164ae2.mp4

      
<details>  
    <summary>환경설정을 위한 config박스 컨트롤</summary>

```C#
     private void Update()
    {
        //  메뉴 아이콘 회전 관련
        if (isMenuOnOff)
            MenuImg.rectTransform.rotation = Quaternion.Lerp(MenuImg.rectTransform.rotation, Quaternion.Euler(0, 0, 0), MenuRotSpeed * Time.deltaTime);
        else
            MenuImg.rectTransform.rotation = Quaternion.Lerp(MenuImg.rectTransform.rotation, Quaternion.Euler(0, 0, 45), MenuRotSpeed * Time.deltaTime);

        // 메뉴 스크롤 업데이트
        MenuScrollUpdate();
        // 사운드 볼륨 조절
        SoundPlay();
    }
    // 메뉴 스크롤 업데이트 메소드
    private void MenuScrollUpdate()
    {
        if (isMenuOnOff && MenuRoot != null && MenuRoot.transform.localPosition.x > MenuPosOrigin.x)
        {
            MenuRoot.transform.localPosition =
                Vector3.MoveTowards(MenuRoot.transform.localPosition, MenuPosOrigin, MenuScrollSpeed * Time.deltaTime);
        }
        else if (!isMenuOnOff && MenuRoot != null && MenuRoot.transform.localPosition.x < MenuPosHide.x)
        {
            MenuRoot.transform.localPosition =
                Vector3.MoveTowards(MenuRoot.transform.localPosition, MenuPosHide, MenuScrollSpeed * Time.deltaTime);
        }
    }
    // 사운드 볼륨 조절 메소드
    private void SoundPlay()
    {
        if (CurAudioSource != null)
        {
            bool a_SoundOnOff = System.Convert.ToBoolean(PlayerPrefs.GetInt("SoundOnOff"));
            CurAudioSource.mute = !a_SoundOnOff;

            float a_SoundVolume = PlayerPrefs.GetFloat("SoundVolume");
            CurAudioSource.volume = a_SoundVolume;
        }
    }
    
```
    
 </details>  

## 4.팀 이동 및 준비  

https://user-images.githubusercontent.com/63942174/158361475-0e5b83a3-28b5-4035-bcfd-41b239ba9bec.mp4

  
<details>  
    <summary>팀 이동 관련 </summary>

```C#
      private void Awake()
    {
        //PhotonView 컴포넌트 할당
        pv = GetComponent<PhotonView>();
    
        //모든 클라우드의 네트워크 메시지 수신을 다시 연결
        PhotonNetwork.IsMessageQueueRunning = true;
    
        //룸에 입장 후 기존 접속자 정보를 출력
        GetConnectPlayerCount();
    
        //----- CustomProperties 초기화
        InitSelTeamProps();
        InitReadyProps();
        InitGStateProps();
        InitTeam1WinProps();
        InitTeam2WinProps();
    
    }
    private void Start()
    {
        //-- TeamSetting
        //-- 팀1 버튼 처리
        //레드팀으로 이동
        if (m_Team1ToTeam2 != null)
            m_Team1ToTeam2.onClick.AddListener(() => { SendSelTeam("red"); });

        if (m_Team1Ready != null)
            m_Team1Ready.onClick.AddListener(() => { SendReady(1); });

        //-- 팀2 버튼 처리
        //블루팀으로 이동
        if (m_Team2ToTeam1 != null)
            m_Team2ToTeam1.onClick.AddListener(() =>{ SendSelTeam("blue");    });

        if (m_Team2Ready != null)
            m_Team2Ready.onClick.AddListener(() => { SendReady(1); });
    }
    
    
      private void Update()
    {
        if (IsGamePossible() == false) //게임 플로어를 돌려도 되는 상태인지 확인한다.
            return;

        //리스트 UI 갱신
        if (m_GameState == GameState.GS_Ready)
        {
            if (IsDifferentList() == true)
            {
                RefreshPhotonTeam();  
            }
        }//if (m_GameState == GameState.GS_Ready)
        
        //채팅 구현
        if (Input.GetKeyDown(KeyCode.Return))
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
                    EnterChat();
            }
        }//if (Input.GetKeyDown(KeyCode.Return)) 

        // 참가 유저 모두 Ready 버튼 눌렀는지 감시하고 게임을 시작하게 처리하는 함수
        AllReadyObserver();

        // 게임이 시작되었을 때
        if (m_GameState == GameState.GS_Playing)
        {
            Team1Panel.SetActive(false);
            Team2Panel.SetActive(false);
            m_WaitTmText.gameObject.SetActive(false);
            WinCountRoot.SetActive(true);
        }//if (m_GameState == GameState.GS_Playing)

        if (isMiniMapActive == true)
            MiniMapShow();

        //한쪽팀이 전멸했는지 체크하고 승리 / 패배 를 감시하고 처리해 주는 함수
        WinLoseObserver();

        // 게임이 종료되었을떄
        if (m_GameState == GameState.GS_GameEnd)
        {
            m_WaitTmText.gameObject.SetActive(false);

            m_BackLobby = m_BackLobby - Time.deltaTime;

            if (m_BackLobby <= 0)
                OnClickExitRoom();
        }

    }// void Update()

    
     //룸 접속자 정보를 조회하는 함수
    void GetConnectPlayerCount()
    {
        //현재 입장한 룸 정보를 받아옴
        Room currRoom = PhotonNetwork.CurrentRoom;  //using Photon.Realtime;

        //현재 룸의 접속자 수와 최대 접속 가능한 수를 문자열로 구성한 후 Text UI 항목에 출력
        ConnectTxt.text = currRoom.PlayerCount.ToString()
                          + "/"
                          + currRoom.MaxPlayers.ToString();
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

    // 리스트 UI갱신
    void RefreshPhotonTeam()
    {
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
            else if (a_TeamKind == "red")
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
            else if (a_TeamKind == "red")
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
                        a_DpUserId.userId.color = Color.red;

                    break;
                }//if (a_DpUserId.pv.Owner.ActorNumber == ActorNumber)
            }//if (a_DpUserId != null)
        }//foreach (GameObject tank in a_tanks)
    }

    
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
```
    
 </details>  
    
## 5.게임시작  

https://user-images.githubusercontent.com/63942174/158361547-473582dc-5ed9-4b3e-a020-986cfd3ce74c.mp4

  
<details>  
    <summary>게임 </summary>

```C#
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
            //누가 발생시켰든 동기화 시키려고 하면
            if (m_RoundCount == 0 && PhotonNetwork.CurrentRoom.IsOpen == true)
            {
                //게임이 시작되면 다른 유저 들어오지 못하도록 막는 부분
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }

            //--- 각 플레이어 PC 별로 3, 2, 1, 0 타이머 UI 표시를 위한 코드
            if (0.0f < m_GoWaitGame)  //타이머 카운티 처리
            {
                m_GoWaitGame = m_GoWaitGame - Time.deltaTime;

                if (m_WaitTmText != null && m_GameState != GameState.GS_GameEnd)
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
```
    
 </details>  
    
## 6.게임중 나가기  

https://user-images.githubusercontent.com/63942174/158361614-af9bfcd3-866e-4320-8263-bab11f5ab0b9.mp4

  
<details>  
    <summary>게임중 나가기</summary>

```C#
     private void Start()
    {
        // 나가기 버튼 연결
        if (ExitRoomBtn != null)
            ExitRoomBtn.onClick.AddListener(OnClickExitRoom);
    }
     //네트워크 플레이어가 룸을 나가거나 접속이 끊어졌을 때 호출되는 함수
    public override void OnPlayerLeftRoom(Player outPlayer)
    {
        GetConnectPlayerCount();
    }
    
    
    //룸 나가기 버튼 클릭 이벤트에 연결될 함수
    public void OnClickExitRoom()
    {
        //로그 메시지에 출력할 문자열 생성
        string msg = "\n<color=#ff0000>[" + PhotonNetwork.LocalPlayer.NickName + "] Disconnected</color>";
        //RPC 함수 호출
        pv.RPC("LogMsg", RpcTarget.AllBuffered, msg);
        //현재 룸을 빠져나가며 생성한 모든 네트워크 객체를 삭제
        PhotonNetwork.LeaveRoom();
    }
    
    //룸에서 접속 종료됐을 때 호출되는 콜백 함수
    public override void OnLeftRoom()  //PhotonNetwork.LeaveRoom(); 성공했을 때 
    {
        //로비 씬을 호출
        UnityEngine.SceneManagement.SceneManager.LoadScene("scLobby");
    }
    
    
    
```
    
 </details>  
    
## 7.적공격  

https://user-images.githubusercontent.com/63942174/158361758-0b3e8f61-7d3b-408e-a889-6ef53706b9a1.mp4


<details>  
    <summary>적을 향해 공격을 할 시 실행되는 스크립트(FireCannon)</summary>

```C#
    
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
            // 마우스가 UI위에 있을시 못쏘게
            if (!EventSystem.current.IsPointerOverGameObject())
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
    }


    [PunRPC]
    void Fire()
    {
        //발사 사운드 발생
        sfx.PlayOneShot(fireSfx);

        PlaySound();

        GameObject _cannon = Instantiate(cannon, firePos.position, firePos.rotation);
        _cannon.GetComponent<Cannon>().AttackerId = pv.Owner.ActorNumber; //ownerId;
        if (pv.Owner.CustomProperties.ContainsKey("MyTeam") == true)
        {
            _cannon.GetComponent<Cannon>().AttackerTeam
                = (string)pv.Owner.CustomProperties["MyTeam"];
        }
    }

    // 발사시 사운드 설정
    private void PlaySound()
    {
        bool a_SoundOnOff = System.Convert.ToBoolean(PlayerPrefs.GetInt("SoundOnOff"));
        float a_SoundVolum = PlayerPrefs.GetFloat("SoundVolume");

        sfx.mute = !a_SoundOnOff;
        sfx.volume = a_SoundVolum;
    }
}
```
    
 </details>  
    
      
<details>  
    <summary>탱크가 공격을 받았을 때 스크립트(TankDamage)</summary>

```C#
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
    
```
    
 </details>  
    
## 8.캐릭터 조작과 카메라 회전  

https://user-images.githubusercontent.com/63942174/158361799-9cb3bf1c-8fa2-49ba-9935-400b23727e87.mp4

      
<details>  
    <summary>탱크 이동 관련 스크립트(TankMove)</summary>

```C#
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
    
    
    //------------ 탱크끼리 구충돌로 밀리게 하기 코드 부분
    float a_Radius = 8.5f;
    GameObject[] a_tanks = null;
    Vector3 a_fCacDist = Vector3.zero;
    float a_CacDistLen = 0.0f;
    float a_ShiftLen = 0.0f;
    TankDamage a_TkDamage = null;

    void Awake()
    {
        //컴포넌트 할당
        rbody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();

        //PhotonView 컴포넌트 할당
        pv = GetComponent<PhotonView>();
        pv.ObservedComponents[0] = this;
                
              
        //유저가 조정하고 있는 로컬에서 만들어진 탱크의 PhotonView일 경우
        if (pv.IsMine)
        {
            //메인 카메라에 추가된 SmoothFollow 스크립트에 추적 대상을 연결
            Camera.main.GetComponent<SmoothFollow>().target = camPivot;
        }  
    
        //원격 탱크의 위치 및 회전 값을 처리할 변수의 초깃값 설정
        currPos = tr.position;
        currRot = tr.rotation;

        tankDamage = this.GetComponent<TankDamage>();
    
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
    }//void Update()
    
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

                                            
```
    
 </details>  
    
      
<details>  
    <summary>터렛 회전 관련 스크립트(TurretCtrl)</summary>

```C#
    
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

    void Awake()
    {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();


        //초기 회전값 설정
        currRot = tr.localRotation;
    }

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
    
```
    
 </details>  
    
    
<details>  
    <summary>포신 컨트롤 관련 스크립트(CannonCtrl)</summary>

```C#
    
public class CannonCtrl : MonoBehaviourPunCallbacks, IPunObservable
{
    private Transform tr;
    public float rotSpeed = 5.0f;

    private RaycastHit hit;

    private PhotonView pv = null;

    //원격 네트워크 탱크의 포신 회전 각도를 저장할 변수
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
        //자신이 만든 네트워크 게임오브젝트가 아닌 경우는 키보드 조작 루틴을 나감
        if (pv.IsMine)
        {
            if (PhotonInit.isFocus == false) //윈도우 창이 비활성화 되어 있다면...
                return;

            //tr.Rotate(angle, 0, 0);

            //메인 카메라에서 마우스 커서의 위치로 캐스팅되는 Ray를 생성
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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

    
```
    
 </details>  

## 9.미니맵 생성  

https://user-images.githubusercontent.com/63942174/158361902-0618f85d-ab83-44aa-b93c-ec21983daf28.mp4

      
<details>  
    <summary>미니맵 관련 스크립트(GameMgr)</summary>

```C#
    
    ///  ------미니맵 유저 색깔 바꾸기
    void ChangeMiniMapTankColor(GameObject[] a_tanks, int ActorNumber, string a_TeamKind)
    {
        DisplayUserId a_DpUserId = null;

        foreach (GameObject tank in a_tanks)
        {
            a_DpUserId = tank.GetComponent<DisplayUserId>();
            if (a_DpUserId != null)
            {
                if (a_DpUserId.pv.Owner.ActorNumber == ActorNumber)
                {
                    if (a_TeamKind == "blue")
                        a_DpUserId.MiniMapUI.color = new Color32(60, 60, 255, 255);
                    else
                        a_DpUserId.MiniMapUI.color = Color.red;

                    break;
                }//if (a_DpUserId.pv.Owner.ActorNumber == ActorNumber)

            }//if (a_DpUserId != null)
        }//foreach (GameObject tank in a_tanks)
    }
    
    
    void MiniMapShow()
    {
        // 미니맵 보기 카운트다운 시작
        MiniMapTimeCount += Time.deltaTime;

        MiniMapCountText.text = ((int)MiniMapTimeCount).ToString();

        if ( MiniMapTimeCount >= MiniMapShowTime)
        {
            MiniMap.gameObject.SetActive(true);
        }
    }
    
```
    
 </details>  

## 10.승리  

https://user-images.githubusercontent.com/63942174/158362001-97c7fa5c-9de9-4452-8724-399135141cd5.mp4

  
<details>  
    <summary>랜덤방 입장</summary>

```C#
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
                    else if (a_PlrTeam == "red")
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
                            MiniMapTimeCount = 0;

                        }//if ((m_Team1Win + m_Team2Win) < 5) //아직 5Round까지 가지 않은 상황이라면...
                    }//if (rowTm1 == 0 || rowTm2 == 0) //양 팀중에 한팀이 전멸했다면....
                }//if (0 < a_Tm1Count && 0 < a_Tm2Count)
            }// if (m_ChekWinTime <= 0.0f)
        }//if (m_GameState == GameState.GS_Playing) 

        // 승리 카운트 텍스트 수정
        if (m_BlueTeamWin != null)
            m_BlueTeamWin.text = m_Team1Win.ToString() + "승";
        if (m_RedTeamWin != null)
            m_RedTeamWin.text = m_Team2Win.ToString() + "승";

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
                    m_GameEndText.text = "<color=Blue>" + "블루팀 승리"+"</color>";
                }
                else if (m_Team1Win < m_Team2Win)
                {
                    m_GameEndText.text = "<color=Red>" + "레드팀 승리"+ "</color>";
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
            else if (a_TeamKind == "red")
            {
                SitPosInxProps.Clear();
                SitPosInxProps.Add("SitPosInx", a_Tm2Count);
                _player.SetCustomProperties(SitPosInxProps);
                a_Tm2Count++;
            }
        }
    }//void SitPosInxMasterCtrl()

    
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

    
```
    
 </details>  
