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
###### 로비화면의 Make Room버튼을 누르면 방을 만들 수 있도록 구현하였다. 만들어진 방은 다른 플레이어들이 볼수 있고  
###### 방을 클릭하면 해당 방에 접속할 수 있다.

https://user-images.githubusercontent.com/63942174/158361325-c7fa9025-d939-433f-93c3-f8e82386f4a0.mp4

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
    <summary>사운드 ㅂ</summary>

```C#
    
```
    
 </details>  
    
## 5.게임시작  

https://user-images.githubusercontent.com/63942174/158361547-473582dc-5ed9-4b3e-a020-986cfd3ce74c.mp4

  
<details>  
    <summary>랜덤방 입장</summary>

```C#
    
    
```
    
 </details>  
    
## 6.게임중 나가기  

https://user-images.githubusercontent.com/63942174/158361614-af9bfcd3-866e-4320-8263-bab11f5ab0b9.mp4

  
<details>  
    <summary>랜덤방 입장</summary>

```C#
    
    
```
    
 </details>  
    
## 7.적공격  

https://user-images.githubusercontent.com/63942174/158361758-0b3e8f61-7d3b-408e-a889-6ef53706b9a1.mp4

*  
<details>  
    <summary>랜덤방 입장</summary>

```C#
    
    
```
    
 </details>  **
    
## 8.이동과 카메라 회전  

https://user-images.githubusercontent.com/63942174/158361799-9cb3bf1c-8fa2-49ba-9935-400b23727e87.mp4

      
<details>  
    <summary>랜덤방 입장</summary>

```C#
    
    
```
    
 </details>  

## 9.미니맵 생성  

https://user-images.githubusercontent.com/63942174/158361902-0618f85d-ab83-44aa-b93c-ec21983daf28.mp4

      
<details>  
    <summary>랜덤방 입장</summary>

```C#
    
    
```
    
 </details>  

## 10.승리  

https://user-images.githubusercontent.com/63942174/158362001-97c7fa5c-9de9-4452-8724-399135141cd5.mp4

  
<details>  
    <summary>랜덤방 입장</summary>

```C#
    
    
```
    
 </details>  
