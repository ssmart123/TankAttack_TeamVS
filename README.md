
![์บก์ฒ](https://user-images.githubusercontent.com/63942174/161957027-8467343f-a6fa-47a6-87bf-854c43878041.PNG)
# ๐ฎTankAttack_TeamVS๐ฎ  
์ ๋ต์  ์์น๋ฅผ ์ ์ ํ๊ณ  ์ ์ ์ฒ์นํ์ฌ ํ์ ์น๋ฆฌ๋ก ์ด๋์ธ์. ์น๊ตฌ์ ํจ๊นจ ํฌํ์ด ๋๋๋๋ ์ ์ฅ์ ์ง๋ฐฐํด๋ณด์ธ์!

### <๊ฒ์ ์๊ฐ>
    ์ค์๊ฐ ํ ๋์  ํฑํฌ ๊ฒ์์๋๋ค. ์จ๋ผ์ธ์ผ๋ก ๋ฉํฐํ๋ ์ด๋ฅผ ์ฆ๊ธธ ์ ์๋๋ก Photon์ ํ์ฉํ์ฌ ๊ตฌํํ์์ต๋๋ค.

### <ํ๋ ์ด ๋ฐฉ๋ฒ>
##### - ์ต๋ 8๋ช์ด์ ํ๋ ์ดํ  ์ ์์ต๋๋ค.  
##### - ํค๋ณด๋๋ก ํฑํฌ๋ฅผ ์ด๋์ํฌ ์ ์๊ณ  ๋ง์ฐ์ค ์ขํด๋ฆญ์ ๋๋ฌ ํฌํ์ ๋ฐ์ฌํ  ์ ์์ต๋๋ค.  
##### - ์ด 5๋ผ์ด๋๋ฅผ ์งํํ๋ฉฐ ๋ธ๋ฃจํ๊ณผ ๋ ๋ํ์ ์น๋ฆฌ ์์ ํฉ์ด 5๊ฐ ๋๋ฉด ์น๋ฆฌ ์๊ฐ ๋ง์ ํ์ด ์ด๊ธฐ๊ฒ ๋ฉ๋๋ค.  
##### - ๋ผ์ด๋ ์์ ํ 10์ด๊ฐ ์ง๋๊ธฐ ์ ๊น์ง ๊ณต๊ฒฉ์ ํ  ์ ์์ต๋๋ค.  
##### - ๋ผ์ด๋ ์์ ํ 30์ด๊ฐ ์ง๋๋ฉด ๋ฏธ๋๋งต์ด ํ์ฑํ๋์ด ์๊ตฐ๊ณผ ์ ๊ตฐ์ ์์น๋ฅผ ์ ์ ์์ต๋๋ค.  

--------------------------

## 1.๋ฐฉ ์์ฑ  
https://user-images.githubusercontent.com/63942174/158361325-c7fa9025-d939-433f-93c3-f8e82386f4a0.mp4


    ๋ก๋นํ๋ฉด์ Make Room๋ฒํผ์ ๋๋ฅด๋ฉด ๋ฐฉ์ ๋ง๋ค ์ ์๋๋ก ๊ตฌํํ์๋ค. ๋ง๋ค์ด์ง ๋ฐฉ์ ๋ค๋ฅธ ํ๋ ์ด์ด๋ค์ด   
    ๋ณผ์ ์๊ณ  ๋ฐฉ์ ํด๋ฆญํ๋ฉด ํด๋น ๋ฐฉ์ ์ ์ํ  ์ ์๋ค. 
<details>
    <summary>ํฌํค ํด๋ผ์ฐ๋ ์๋ฒ ์ ์๊ณผ ๋ฐฉ์์ฑ์ ์ํ ์ฝ๋(PhotonInit)</summary>
  
``` C#
    
void Awake()
    {
        //ํฌํค ํด๋ผ์ฐ๋ ์๋ฒ ์ ์ ์ฌ๋ถ ํ์ธ
        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();  //ํฌํค ํด๋ผ์ฐ๋์ ์ ์
        
        userId.text = GetUserId();  //์ฌ์ฉ์ ์ด๋ฆ ์ค์ 

        //๋ฃธ ์ด๋ฆ์ ๋ฌด์์๋ก ์ค์ 
        roomName.text = "Room_" + Random.Range(0, 999).ToString("000");
    }
    
    // ๋ฐฉ๋ง๋ค๊ธฐ ๋ฒํผ ํด๋ฆญ ์ ํธ์ถ๋  ํจ์
 public void ClickCreateRoom()
    {
        string _roomName = roomName.text;
        
        if (string.IsNullOrEmpty(roomName.text))  //๋ฃธ ์ด๋ฆ์ด ์๊ฑฐ๋ Null์ผ ๊ฒฝ์ฐ ๋ฃธ ์ด๋ฆ ์ง์ 
            _roomName = "ROOM_" + Random.Range(0, 999).ToString("000");

        PhotonNetwork.LocalPlayer.NickName = userId.text;  //๋ก์ปฌ ํ๋ ์ด์ด์ ์ด๋ฆ์ ์ค์ 
        
        PlayerPrefs.SetString("USER_ID", userId.text);  //ํ๋ ์ด์ด ์ด๋ฆ์ ์ ์ฅ

        //์์ฑํ  ๋ฃธ์ ์กฐ๊ฑด ์ค์ 
        RoomOptions roomOptions = new RoomOptions();  //using Photon.Realtime;
        roomOptions.IsOpen = true;     //์์ฅ ๊ฐ๋ฅ ์ฌ๋ถ
        roomOptions.IsVisible = true;  //๋ก๋น์์ ๋ฃธ์ ๋ธ์ถ ์ฌ๋ถ
        roomOptions.MaxPlayers = 8;    //๋ฃธ์ ์์ฅํ  ์ ์๋ ์ต๋ ์ ์์ ์

        //์ง์ ํ ์กฐ๊ฑด์ ๋ง๋ ๋ฃธ ์์ฑ ํจ์
        PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);  //TypedLobby.Default ์ด๋ ๋ก๋น์ ๋ฐฉ์ ๋ง๋ค๊ป์ง? 
    }
    
     //PhotonNetwork.CreateRoom() ์ด ํจ์๊ฐ ์คํจ ํ๋ฉด ํธ์ถ๋๋ ํจ์(๊ฐ์ ์ด๋ฆ์ ๋ฐฉ์ด ์์ ๋ ์คํจํจ)
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("๋ฐฉ ๋ง๋ค๊ธฐ ์คํจ"); //์ฃผ๋ก ๊ฐ์ ์ด๋ฆ์ ๋ฐฉ์ด ์กด์ฌํ  ๋ ๋ฃธ์์ฑ ์๋ฌ๊ฐ ๋ฐ์๋๋ค.
        Debug.Log(returnCode.ToString()); //์ค๋ฅ ์ฝ๋(ErrorCode ํด๋์ค)
        Debug.Log(message); //์ค๋ฅ ๋ฉ์์ง
    }
    
    //์์ฑ๋ ๋ฃธ ๋ชฉ๋ก์ด ๋ณ๊ฒฝ๋์ ๋ ํธ์ถ๋๋ ์ฝ๋ฐฑ ํจ์(๋ฐฉ ๋ฆฌ์คํธ ๊ฐฑ์ ์ ๋ก๋น์์๋ง ๊ฐ๋ฅํ๋ค.)
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

        //๋ฃธ ๋ชฉ๋ก์ ๋ค์ ๋ฐ์์ ๋ ๊ฐฑ์ ํ๊ธฐ ์ํด ๊ธฐ์กด์ ์์ฑ๋ RoomItem์ ์ญ์ 
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ROOM_ITEM"))
            Destroy(obj);
            
        //์คํฌ๋กค ์์ญ ์ด๊ธฐํ
        scrollContents.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        for (int i = 0; i < myList.Count; i++)
        {
            GameObject room = (GameObject)Instantiate(roomItem);
            
            room.transform.SetParent(scrollContents.transform, false);//์์ฑํ RoomItem ํ๋ฆฌํน์ Parent๋ฅผ ์ง์ 

            //์์ฑํ RoomItem์ ํ์ํ๊ธฐ ์ํ ํ์คํธ ์ ๋ณด ์ ๋ฌ
            RoomData roomData = room.GetComponent<RoomData>();
            roomData.roomName = myList[i].Name;
            roomData.connectPlayer = myList[i].PlayerCount;
            roomData.maxPlayer = myList[i].MaxPlayers;

            //ํ์คํธ ์ ๋ณด๋ฅผ ํ์
            roomData.DispRoomData(myList[i].IsOpen);
        }//for (int i = 0; i < roomCount; i++)
    }// public override void OnRoomListUpdate(List<RoomInfo> roomList)

//RoomItem์ด ํด๋ฆญ๋๋ฉด ํธ์ถ๋  ์ด๋ฒคํธ ์ฐ๊ฒฐ ํจ์
    public void OnClickRoomItem(string roomName)
    {
        //๋ก์ปฌ ํ๋ ์ด์ด์ ์ด๋ฆ์ ์ค์ 
        PhotonNetwork.LocalPlayer.NickName = userId.text;
        //ํ๋ ์ด์ด ์ด๋ฆ์ ์ ์ฅ
        PlayerPrefs.SetString("USER_ID", userId.text);

        //์ธ์๋ก ์ ๋ฌ๋ ์ด๋ฆ์ ํด๋นํ๋ ๋ฃธ์ผ๋ก ์์ฅ
        PhotonNetwork.JoinRoom(roomName);
    }
    
```
    
 </details>

    
## 2.๋๋ค๋ฐฉ ์์ฅ  

https://user-images.githubusercontent.com/63942174/158361351-8a318f42-bbbd-47c3-8636-2c99131d8c59.mp4

    Join Random Room ๋ฒํผ์ ๋๋ฅด๋ฉด ํ์ฌ ๋ง๋ค์ด์ ธ ์๋ ์์์ ๋ฐฉ์ ์ ์ํ๊ฒ ๋๋ค.
<details>  
    <summary>๋๋ค๋ฐฉ ์์ฅ(PhotonInit)</summary>

```C#
     public override void OnConnectedToMaster()
    {
        //๋จ์ ํฌํค ์๋ฒ ์ ์๋ง ๋ ์ํ (ConnectedToMaster)
        Debug.Log("์๋ฒ ์ ์ ์๋ฃ");
        PhotonNetwork.JoinLobby();
    }
    
    
    // PhotonNetwork.JoinLobby() ์ฑ๊ณต์ ํธ์ถ๋๋ ๋ก๋น ์ ์ ์ฝ๋ฐฑํจ์
    public override void OnJoinedLobby()
    {
        Debug.Log("๋ก๋น์ ์์๋ฃ");
        userId.text = GetUserId(); //๋ฐฉ์์ ๋ก๋น๋ก ๋์ฌ ๋๋ ์ ์  ID๋ฅผ ํ๋ ์ํํด ์ฃผ์ด์ผ ํ๋ค.
    }
    
     public override void OnJoinedRoom()
    {
        Debug.Log("๋ฐฉ ์ฐธ๊ฐ ์๋ฃ");
        //๋ฃธ ์ฌ์ผ๋ก ์ด๋ํ๋ ์ฝ๋ฃจํด ์คํ
        StartCoroutine(this.LoadBattleField());
    }

    //PhotonNetwork.JoinRandomRoom() ์ด ํจ์ ์คํจํ ๊ฒฝ์ฐ ํธ์ถ๋๋ ์ฝ๋ฐฑ ํจ์
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("๋๋ค ๋ฐฉ ์ฐธ๊ฐ ์คํจ (์ฐธ๊ฐํ  ๋ฐฉ์ด ์กด์ฌํ์ง ์์ต๋๋ค.)");
    }
    
      //๋ฃธ ์ฌ์ผ๋ก ์ด๋ํ๋ ์ฝ๋ฃจํด ํจ์
    IEnumerator LoadBattleField()        //์ต์ข ๋ฐฐํํ๋ ์ฌ ๋ก๋ฉ
    {
        //์ฌ์ ์ด๋ํ๋ ๋์ ํฌํค ํด๋ผ์ฐ๋ ์๋ฒ๋ก๋ถํฐ ๋คํธ์ํฌ ๋ฉ์์ง ์์  ์ค๋จ
        PhotonNetwork.IsMessageQueueRunning = false;
        //๋ฐฑ๊ทธ๋ผ์ด๋๋ก ์ฌ ๋ก๋ฉ

        Time.timeScale = 1.0f;  //๊ฒ์์ ๋ค์ด๊ฐ ๋๋ ์๋ ์๋๋ก...

        AsyncOperation ao = SceneManager.LoadSceneAsync("scBattleField");

        yield return ao;
    }
```
</details>  
    
    
## 3.ํ๊ฒฝ์ค์ 

https://user-images.githubusercontent.com/63942174/158361437-9871a9f5-b60e-4c03-8db9-059c4a164ae2.mp4

    ํ๋ฉด์ ํ๊ฒฝ์ค์ ๋ฒํผ์ ๋๋ฅด๋ฉด ์จ๊ฒจ์ ธ์๋ ๋ฉ๋ด๊ฐ ์คํฌ๋กค๋๋๋ก ๊ตฌํํ์๋ค.  
    ์ฌ์ด๋ ๊ด๋ จ ๋ฐ์ดํฐ๋ PlayerPrefs์ ์ ์ฅํ์๋ค.
<details>  
    <summary>ํ๊ฒฝ์ค์ ๋ฒํผ ์คํฌ๋กค๊ณผ ์ฌ์ด๋ ์ต์ ๋ก์ปฌ ์ ์ฅ(PhotonInit)</summary>

``` C#
     private void Update()
    {
        //  ๋ฉ๋ด ์์ด์ฝ ํ์  ๊ด๋ จ
        if (isMenuOnOff)
            MenuImg.rectTransform.rotation = Quaternion.Lerp(MenuImg.rectTransform.rotation, Quaternion.Euler(0, 0, 0), MenuRotSpeed * Time.deltaTime);
        else
            MenuImg.rectTransform.rotation = Quaternion.Lerp(MenuImg.rectTransform.rotation, Quaternion.Euler(0, 0, 45), MenuRotSpeed * Time.deltaTime);

        // ๋ฉ๋ด ์คํฌ๋กค ์๋ฐ์ดํธ
        MenuScrollUpdate();
        // ์ฌ์ด๋ ๋ณผ๋ฅจ ์กฐ์ 
        SoundPlay();
    }
    // ๋ฉ๋ด ์คํฌ๋กค ์๋ฐ์ดํธ ๋ฉ์๋
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
    // ์ฌ์ด๋ ๋ณผ๋ฅจ ์กฐ์  ๋ฉ์๋
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
    
    
    ConfigBox์์ ์ฌ์ด๋์ค์ ์ ๋ณ๊ฒฝํ๋ฉด PlayerPreps์ ์ ์ฅ๋๋๋ก ๊ตฌํํ์๋ค.
<details>  
    <summary>ConfigBox ์คํฌ๋ฆฝํธ(ConfigBox)</summary>
    
``` C#
    public class ConfigBox : MonoBehaviour
{
    public Button CancleBtn;
    public Toggle SoundToggle;
    public Slider VolumeSlider;

    public Button OkBtn;

    private void Start()
    {
        if (CancleBtn != null)
            CancleBtn.onClick.AddListener(()=> { Destroy(this.gameObject); });

        if (SoundToggle != null)
            SoundToggle.onValueChanged.AddListener(SoundOnOff);

        bool a_SoundOnOff = System.Convert.ToBoolean(PlayerPrefs.GetInt("SoundOnOff"));
        if (SoundToggle != null)
            SoundToggle.isOn = a_SoundOnOff;

        if (VolumeSlider != null)
            VolumeSlider.onValueChanged.AddListener(ValumSliderCheck);

        float a_SoundVolume = PlayerPrefs.GetFloat("SoundVolume");
        if (VolumeSlider != null)
            VolumeSlider.value = a_SoundVolume;
    }

    // ์ฌ์ด๋ ์์๊ฑฐ ๊ด๋ จ ํจ์
    private void SoundOnOff(bool value)
    {
        if (value == true)
            PlayerPrefs.SetInt("SoundOnOff", 1);
        else
            PlayerPrefs.SetInt("SoundOnOff", 0);

    }
    
    // ์ฌ์ด๋ ๋ณผ๋ฅจ ๊ด๋ จ ํจ์
    private void ValumSliderCheck(float value)
    {
        PlayerPrefs.SetFloat("SoundVolume", value);
    }
}

```
</details>  
    
    
## 4.ํ ์ด๋ ๋ฐ ์ค๋น  

https://user-images.githubusercontent.com/63942174/158361475-0e5b83a3-28b5-4035-bcfd-41b239ba9bec.mp4

    ๋ฐฐํ์ฌ์์ ๋ธ๋ฃจํ๊ณผ ๋ ๋ํ์ผ๋ก  ์ด๋ํ  ์ ์๊ณ  ์ค๋น๋ฅผ ํ ์ ์๋๋ก ๋ง๋ค์๋ค.
<details>  
    <summary>ConfigBox ์คํฌ๋ฆฝํธ(ConfigBox)</summary>
    
``` C#
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

      private void Awake()
    {
        //PhotonView ์ปดํฌ๋ํธ ํ ๋น
        pv = GetComponent<PhotonView>();
    
        //๋ชจ๋  ํด๋ผ์ฐ๋์ ๋คํธ์ํฌ ๋ฉ์์ง ์์ ์ ๋ค์ ์ฐ๊ฒฐ
        PhotonNetwork.IsMessageQueueRunning = true;
    
        //๋ฃธ์ ์์ฅ ํ ๊ธฐ์กด ์ ์์ ์ ๋ณด๋ฅผ ์ถ๋ ฅ
        GetConnectPlayerCount();
    
        //----- CustomProperties ์ด๊ธฐํ
        InitSelTeamProps();
        InitReadyProps();
        InitGStateProps();
        InitTeam1WinProps();
        InitTeam2WinProps();
    
    }
    private void Start()
    {
        //-- TeamSetting
        //-- ํ1 ๋ฒํผ ์ฒ๋ฆฌ
        //๋ ๋ํ์ผ๋ก ์ด๋
        if (m_Team1ToTeam2 != null)
            m_Team1ToTeam2.onClick.AddListener(() => { SendSelTeam("red"); });

        if (m_Team1Ready != null)
            m_Team1Ready.onClick.AddListener(() => { SendReady(1); });

        //-- ํ2 ๋ฒํผ ์ฒ๋ฆฌ
        //๋ธ๋ฃจํ์ผ๋ก ์ด๋
        if (m_Team2ToTeam1 != null)
            m_Team2ToTeam1.onClick.AddListener(() =>{ SendSelTeam("blue");    });

        if (m_Team2Ready != null)
            m_Team2Ready.onClick.AddListener(() => { SendReady(1); });
    }
    
    
      private void Update()
    {
        if (IsGamePossible() == false) //๊ฒ์ ํ๋ก์ด๋ฅผ ๋๋ ค๋ ๋๋ ์ํ์ธ์ง ํ์ธํ๋ค.
            return;

        //๋ฆฌ์คํธ UI ๊ฐฑ์ 
        if (m_GameState == GameState.GS_Ready)
        {
            if (IsDifferentList() == true)
            {
                RefreshPhotonTeam();  
            }
        }//if (m_GameState == GameState.GS_Ready)
        
        //์ฑํ ๊ตฌํ
        if (Input.GetKeyDown(KeyCode.Return))
        {
            bEnter = !bEnter;

            if (bEnter == true)
            {
                textChat.gameObject.SetActive(bEnter);
                textChat.ActivateInputField(); //<--- ์ปค์๋ฅผ ์ธํํ๋๋ก ์ด๋์์ผ ์ค
            }
            else
            {
                textChat.gameObject.SetActive(bEnter);

                if (textChat.text != "")
                    EnterChat();
            }
        }//if (Input.GetKeyDown(KeyCode.Return)) 

        // ์ฐธ๊ฐ ์ ์  ๋ชจ๋ Ready ๋ฒํผ ๋๋ ๋์ง ๊ฐ์ํ๊ณ  ๊ฒ์์ ์์ํ๊ฒ ์ฒ๋ฆฌํ๋ ํจ์
        AllReadyObserver();

        // ๊ฒ์์ด ์์๋์์ ๋
        if (m_GameState == GameState.GS_Playing)
        {
            Team1Panel.SetActive(false);
            Team2Panel.SetActive(false);
            m_WaitTmText.gameObject.SetActive(false);
            WinCountRoot.SetActive(true);
        }//if (m_GameState == GameState.GS_Playing)

        if (isMiniMapActive == true)
            MiniMapShow();

        //ํ์ชฝํ์ด ์ ๋ฉธํ๋์ง ์ฒดํฌํ๊ณ  ์น๋ฆฌ / ํจ๋ฐฐ ๋ฅผ ๊ฐ์ํ๊ณ  ์ฒ๋ฆฌํด ์ฃผ๋ ํจ์
        WinLoseObserver();

        // ๊ฒ์์ด ์ข๋ฃ๋์์๋
        if (m_GameState == GameState.GS_GameEnd)
        {
            m_WaitTmText.gameObject.SetActive(false);

            m_BackLobby = m_BackLobby - Time.deltaTime;

            if (m_BackLobby <= 0)
                OnClickExitRoom();
        }

    }// void Update()

    
     //๋ฃธ ์ ์์ ์ ๋ณด๋ฅผ ์กฐํํ๋ ํจ์
    void GetConnectPlayerCount()
    {
        //ํ์ฌ ์์ฅํ ๋ฃธ ์ ๋ณด๋ฅผ ๋ฐ์์ด
        Room currRoom = PhotonNetwork.CurrentRoom;  //using Photon.Realtime;

        //ํ์ฌ ๋ฃธ์ ์ ์์ ์์ ์ต๋ ์ ์ ๊ฐ๋ฅํ ์๋ฅผ ๋ฌธ์์ด๋ก ๊ตฌ์ฑํ ํ Text UI ํญ๋ชฉ์ ์ถ๋ ฅ
        ConnectTxt.text = currRoom.PlayerCount.ToString()
                          + "/"
                          + currRoom.MaxPlayers.ToString();
    }
    
    bool IsDifferentList() //true๋ฉด ๋ค๋ฅด๋ค๋ ๋ป false๋ฉด ๊ฐ๋ค๋ ๋ป
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
                        return true; //ํด๋น ์ ์ ์ ํ์ด ๋ณ๊ฒฝ ๋์๋ค๋ฉด...

                    if (TankData.m_IamReady != ReceiveReady(a_RefPlayer))
                        return true; //ํด๋น Ready ์ํ๊ฐ ๋ณ๊ฒฝ ๋์๋ค๋ฉด...

                    a_FindNode = true;
                    break;
                }
            }//foreach (GameObject a_Node in GameObject.FindGameObjectsWithTag("TKNODE_ITEM"))

            if(a_FindNode == false) 
                return true; //ํด๋น ์ ์ ๊ฐ ๋ฆฌ์คํธ์ ์กด์ฌํ์ง ์์ผ๋ฉด....

        }//foreach (Player a_RefPlayer in PhotonNetwork.PlayerList)

        return false; //์ผ์นํ๋ค๋ ๋ป
    }

    // ๋ฆฌ์คํธ UI๊ฐฑ์ 
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

            //ํ์ด ๋ญ๋?์ ๋ฐ๋ผ์ ์คํฌ๋กค ๋ทฐ๋ฅผ ๋ถ๊ธฐ ํด ์ค๋ค.
            if (a_TeamKind == "blue")
                a_TkNode.transform.SetParent(scrollTeam1.transform, false);
            else if (a_TeamKind == "red")
                a_TkNode.transform.SetParent(scrollTeam2.transform, false);

            //์์ฑํ RoomItem์ ํ์ํ๊ธฐ ์ํ ํ์คํธ ์ ๋ณด ์ ๋ฌ
            TankNodeItem TankData = a_TkNode.GetComponent<TankNodeItem>();
            //ํ์คํธ ์ ๋ณด๋ฅผ ํ์
            if (TankData != null)
            {
                TankData.m_UniqID = a_RefPlayer.ActorNumber;
                TankData.m_TeamKind = a_TeamKind;
                TankData.m_IamReady = ReceiveReady(a_RefPlayer);
                bool isMine = TankData.m_UniqID == PhotonNetwork.LocalPlayer.ActorNumber;
                TankData.DispPlayerData(a_RefPlayer.NickName, isMine);
            }

            //์ด๋ฆํ ์๊น ๋ฐ๊พธ๊ธฐ
            ChangeTankNameColor(a_tanks, a_RefPlayer.ActorNumber, a_TeamKind);
            // ๋ฏธ๋๋งต ํ๋ ์ด์ด ์๊น ๋ฐ๊พธ๊ธฐ
            ChangeMiniMapTankColor(a_tanks, a_RefPlayer.ActorNumber, a_TeamKind);
        }//foreach (Player a_RefPlayer in PhotonNetwork.PlayerList)

        //-------------- Ready Off ํ๊ธฐ...
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
        //-------------- Ready Off ํ๊ธฐ...

    }//void RefreshPhotonTeam()
    
    
    void ChangeTankNameColor(GameObject[] a_tanks, int ActorNumber, string a_TeamKind)
    {
        //์ด๋ฆํ ์๊น ๋ฐ๊พธ๊ธฐ
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

    
    #region --------------- Ready ์ํ ๋๊ธฐํ ์ฒ๋ฆฌ
    void InitReadyProps()
    { //์๋๋ฅผ ์ํด ๋ฒํผ๋ฅผ ๋ฏธ๋ฆฌ ๋ง๋ค์ด ๋๋๋ค๋ ์๋ฏธ
        m_PlayerReady.Clear();
        m_PlayerReady.Add("IamReady", 0);      //๊ธฐ๋ณธ์ ์ผ๋ก ์์ง ์ค๋น์  ์ํ๋ก ์์ํ๋ค.
        PhotonNetwork.LocalPlayer.SetCustomProperties(m_PlayerReady);  
        //์บ๋ฆญํฐ ๋ณ๋ก ๋๊ธฐํ ์ํค๊ณ  ์ถ์ ๊ฒฝ์ฐ
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

        PhotonNetwork.LocalPlayer.SetCustomProperties(m_PlayerReady);  //์บ๋ฆญํฐ ๋ณ๋ก ๋๊ธฐํ ์ํค๊ณ  ์ถ์ ๊ฒฝ์ฐ
    }
    //--------------- Send Ready

    //--------------- Receive Ready
    bool ReceiveReady(Player a_Player) //Ready ์ํ๋ฅผ ๋ฐ์์ ์ฒ๋ฆฌํ๋ ๋ถ๋ถ
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
    #endregion  //--------------- Ready ์ํ ๋๊ธฐํ ์ฒ๋ฆฌ
    
      #region --------------- ๊ฒ์ ์ํ ๋๊ธฐํ ์ฒ๋ฆฌ
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
        //    return;   //Update์์ ์ฒดํฌ ํ๊ณ  ์๋ค.

        //if (PhotonNetwork.IsMasterClient == false) //๋ง์คํฐ๋ง ๋ณด๋ธ๋ค.
        //    return;   //Update์์ ์ฒดํฌ ํ๊ณ  ์๋ค.

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

    GameState ReceiveGState() //GameState ๋ฐ์์ ์ฒ๋ฆฌํ๋ ๋ถ๋ถ
    {
        GameState a_RmVal = GameState.GS_Ready;

        //if (PhotonNetwork.CurrentRoom == null)  
        //    return a_RmVal;    //Update์์ ์ฒดํฌ ํ๊ณ  ์๋ค.

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("GameState") == true)
            a_RmVal = (GameState)PhotonNetwork.CurrentRoom.CustomProperties["GameState"];

        return a_RmVal;
    }
    #endregion  //--------------- ๊ฒ์ ์ํ ๋๊ธฐํ ์ฒ๋ฆฌ

    
    
    #region --------------- ํ์ ํ ๋๊ธฐํ ์ฒ๋ฆฌ
    void InitSelTeamProps()
    { //์๋๋ฅผ ์ํด ๋ฒํผ๋ฅผ ๋ฏธ๋ฆฌ ๋ง๋ค์ด ๋๋๋ค๋ ์๋ฏธ
        m_SelTeamProps.Clear();
        m_SelTeamProps.Add("MyTeam", "blue");   //๊ธฐ๋ณธ์ ์ผ๋ก ๋๋ ๋ธ๋ฃจํ์ผ๋ก ์์ํ๋ค.
        PhotonNetwork.LocalPlayer.SetCustomProperties(m_SelTeamProps);  
        //์บ๋ฆญํฐ ๋ณ๋ก ๋๊ธฐํ ์ํค๊ณ  ์ถ์ ๊ฒฝ์ฐ
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

        PhotonNetwork.LocalPlayer.SetCustomProperties(m_SelTeamProps);  //์บ๋ฆญํฐ ๋ณ๋ก ๋๊ธฐํ ์ํค๊ณ  ์ถ์ ๊ฒฝ์ฐ
    }
    //--------------- Send SelTeam

    //--------------- Receive SelTeam
    string ReceiveSelTeam(Player a_Player) //SelTeam ๋ฐ์์ ์ฒ๋ฆฌํ๋ ๋ถ๋ถ
    {
        string a_TeamKind = "blue";

        if (a_Player == null)
            return a_TeamKind;

        if (a_Player.CustomProperties.ContainsKey("MyTeam") == true)
            a_TeamKind = (string)a_Player.CustomProperties["MyTeam"];

        return a_TeamKind;
    }
    //--------------- Receive SelTeam
    #endregion  //--------------- ํ์ ํ ๋๊ธฐํ ์ฒ๋ฆฌ


    #region --------------- Ready ์ํ ๋๊ธฐํ ์ฒ๋ฆฌ
    void InitReadyProps()
    { //์๋๋ฅผ ์ํด ๋ฒํผ๋ฅผ ๋ฏธ๋ฆฌ ๋ง๋ค์ด ๋๋๋ค๋ ์๋ฏธ
        m_PlayerReady.Clear();
        m_PlayerReady.Add("IamReady", 0);      //๊ธฐ๋ณธ์ ์ผ๋ก ์์ง ์ค๋น์  ์ํ๋ก ์์ํ๋ค.
        PhotonNetwork.LocalPlayer.SetCustomProperties(m_PlayerReady);  
        //์บ๋ฆญํฐ ๋ณ๋ก ๋๊ธฐํ ์ํค๊ณ  ์ถ์ ๊ฒฝ์ฐ
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

        PhotonNetwork.LocalPlayer.SetCustomProperties(m_PlayerReady);  //์บ๋ฆญํฐ ๋ณ๋ก ๋๊ธฐํ ์ํค๊ณ  ์ถ์ ๊ฒฝ์ฐ
    }
    //--------------- Send Ready

    //--------------- Receive Ready
    bool ReceiveReady(Player a_Player) //Ready ์ํ๋ฅผ ๋ฐ์์ ์ฒ๋ฆฌํ๋ ๋ถ๋ถ
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
    #endregion  //--------------- Ready ์ํ ๋๊ธฐํ ์ฒ๋ฆฌ

```
</details>  
    
## 5.๊ฒ์์์  

https://user-images.githubusercontent.com/63942174/158361547-473582dc-5ed9-4b3e-a020-986cfd3ce74c.mp4

    ๋ชจ๋  ํ์ ๋ ๋๊ฐ ์๋ฃ๋๋ฉด 3์ด๋ค ๊ฒ์์ด ์์ํ๊ฒ ๋ฉ๋๋ค. ์นด์ดํธ๋ค์ด์ด ๋๋๋ฉด UIํ๋ฉด์ ๋๊ณ  ํฑํฌ๊ฐ ์คํฐ๋๊ฒ ๋ฉ๋๋ค.
  
<details>  
    <summary>๊ฒ์์์(GameMgr) </summary>

```C#
    bool IsGamePossible()  //๊ฒ์์ด ๊ฐ๋ฅํ ์ํ์ธ์ง? ์ฒดํฌํ๋ ํจ์
    {
        //๋๊ฐ๋ ํ์ด๋ฐ์ ํฌํค ์ ๋ณด๋ค์ด ํํ๋ ์ ๋จผ์  ์ฌ๋ผ์ง๊ณ  
        //LoadScene()์ด ํํ๋ ์ ๋ฆ๊ฒ ํธ์ถ๋๋ ๋ฌธ์  ํด๊ฒฐ๋ฒ
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.LocalPlayer == null)  
            return false; //๋๊ธฐํ ๊ฐ๋ฅํ ์ํ ์ผ๋๋ง ์๋ฐ์ดํธ๋ฅผ ๊ณ์ฐํด ์ค๋ค.

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("GameState") == false ||
            PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Team1Win") == false ||
            PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Team2Win") == false)
            return false;

        //PhotonNetwork.CurrentRoom.CustomProperties ์ ์ ์ฅ๋์ด ์๋ ๊ฒ์ ์ํ ๋ฐ์์ค๊ธฐ
        m_GameState = ReceiveGState();
        m_Team1Win = (int)PhotonNetwork.CurrentRoom.CustomProperties["Team1Win"];
        m_Team2Win = (int)PhotonNetwork.CurrentRoom.CustomProperties["Team2Win"];

        return true;
    }

    // ์ฐธ๊ฐ ์ ์  ๋ชจ๋ Ready ๋ฒํผ ๋๋ ๋์ง ๊ฐ์ํ๊ณ  ๊ฒ์์ ์์ํ๊ฒ ์ฒ๋ฆฌํ๋ ํจ์
    void AllReadyObserver()
    {
        if (m_GameState != GameState.GS_Ready) //GS_Ready ์ํ์์๋ง ํ์ธํ๋ค.
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

        if (a_AllReady == true) //๋ชจ๋๊ฐ ์ค๋น ๋ฒํผ์ ๋๋ฅด๊ณ  ๊ธฐ๋ค๋ฆฌ๊ณ  ์๋ค๋ ๋ป 
        {
            //๋๊ฐ ๋ฐ์์์ผฐ๋  ๋๊ธฐํ ์ํค๋ ค๊ณ  ํ๋ฉด
            if (m_RoundCount == 0 && PhotonNetwork.CurrentRoom.IsOpen == true)
            {
                //๊ฒ์์ด ์์๋๋ฉด ๋ค๋ฅธ ์ ์  ๋ค์ด์ค์ง ๋ชปํ๋๋ก ๋ง๋ ๋ถ๋ถ
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }

            //--- ๊ฐ ํ๋ ์ด์ด PC ๋ณ๋ก 3, 2, 1, 0 ํ์ด๋จธ UI ํ์๋ฅผ ์ํ ์ฝ๋
            if (0.0f < m_GoWaitGame)  //ํ์ด๋จธ ์นด์ดํฐ ์ฒ๋ฆฌ
            {
                m_GoWaitGame = m_GoWaitGame - Time.deltaTime;

                if (m_WaitTmText != null && m_GameState != GameState.GS_GameEnd)
                {
                    m_WaitTmText.gameObject.SetActive(true);
                    m_WaitTmText.text = ((int)m_GoWaitGame).ToString();
                }

                //๋ง์คํฐ ํด๋ผ์ด์ธํธ๋ ๊ฐ ์ ์ ์ ์๋ฆฌ๋ฐฐ์น๋ฅผ ํด ์ค ๊ฒ์ด๋ค.
                //์ด 4๋ฒ๋ง ๋ณด๋ธ๋ค. MasterClient๊ฐ ๋๊ฐ ๊ฒฝ์ฐ๋ฅผ ๋๋นํด์...
                if (PhotonNetwork.IsMasterClient == true)
                if (0.0f < m_GoWaitGame && a_OldGoWait != (int)m_GoWaitGame) 
                {//์๋ฆฌ ๋ฐฐ์ 
                    SitPosInxMasterCtrl();
                }//if(a_OldGoWait != (int)m_GoWaitGame) //์๋ฆฌ ๋ฐฐ์ 

                if (m_GoWaitGame <= 0.0f) //์ด๊ฑด ํ๋ฒ๋ง ๋ฐ์ํ  ๊ฒ์ด๋ค.
                {//์ง์ง ๊ฒ์ ์์ ์ค๋น
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
            //--- ๊ฐ ํ๋ ์ด์ด PC ๋ณ๋ก ํ์ด๋จธ UI ํ์๋ฅผ ์ํ ์ฝ๋

            //๊ฒ์์ด ์์ ๋์์ด์ผ ํ๋๋ฐ ์์ง ์์ ๋์ง ์์๋ค๋ฉด....
            if (PhotonNetwork.IsMasterClient == true) //๋ง์คํฐ ํด๋ผ์ด์ธํธ๋ง ์ฒดํฌํ๊ณ  ๋ณด๋ธ๋ค.
            if (m_GoWaitGame <= 0.0f) //&& ReceiveGState() == GameState.GS_Ready) //์์์ ์ฒดํฌํจ 
            {
                SendGState(GameState.GS_Playing);
            }
        }//if (a_AllReady == true) //๋ชจ๋๊ฐ ์ค๋น ๋ฒํผ์ ๋๋ฅด๊ณ  ๊ธฐ๋ค๋ฆฌ๊ณ  ์๋ค๋ ๋ป 

    }//void AllReadyObserver()
```
    
 </details>  
    
## 6.๊ฒ์์ค ๋๊ฐ๊ธฐ  

https://user-images.githubusercontent.com/63942174/158361614-af9bfcd3-866e-4320-8263-bab11f5ab0b9.mp4

    ํ๊ฒฝ์ค์  ์คํฌ๋กค์ ๋๋ฅธ ๋ค ๋๊ฐ๊ธฐ ๋ฒํผ์ ํด๋ฆญํ๋ฉด ํฌํค๊ณผ์ ์ ์์ด ์ข๋ฃ๋๊ฒ ๋๊ณ  ์์ฑ๋ ๋ชจ๋  ๋คํธ์ํฌ ๊ฐ์ฒด๋ฅผ ์ญ์ ํ๊ฒ ๋ฉ๋๋ค.
  
<details>  
    <summary>๊ฒ์์ค ๋๊ฐ๊ธฐ</summary>

```C#
     private void Start()
    {
        // ๋๊ฐ๊ธฐ ๋ฒํผ ์ฐ๊ฒฐ
        if (ExitRoomBtn != null)
            ExitRoomBtn.onClick.AddListener(OnClickExitRoom);
    }
     //๋คํธ์ํฌ ํ๋ ์ด์ด๊ฐ ๋ฃธ์ ๋๊ฐ๊ฑฐ๋ ์ ์์ด ๋์ด์ก์ ๋ ํธ์ถ๋๋ ํจ์
    public override void OnPlayerLeftRoom(Player outPlayer)
    {
        GetConnectPlayerCount();
    }
    
    
    //๋ฃธ ๋๊ฐ๊ธฐ ๋ฒํผ ํด๋ฆญ ์ด๋ฒคํธ์ ์ฐ๊ฒฐ๋  ํจ์
    public void OnClickExitRoom()
    {
        //๋ก๊ทธ ๋ฉ์์ง์ ์ถ๋ ฅํ  ๋ฌธ์์ด ์์ฑ
        string msg = "\n<color=#ff0000>[" + PhotonNetwork.LocalPlayer.NickName + "] Disconnected</color>";
        //RPC ํจ์ ํธ์ถ
        pv.RPC("LogMsg", RpcTarget.AllBuffered, msg);
        //ํ์ฌ ๋ฃธ์ ๋น ์ ธ๋๊ฐ๋ฉฐ ์์ฑํ ๋ชจ๋  ๋คํธ์ํฌ ๊ฐ์ฒด๋ฅผ ์ญ์ 
        PhotonNetwork.LeaveRoom();
    }
    
    //๋ฃธ์์ ์ ์ ์ข๋ฃ๋์ ๋ ํธ์ถ๋๋ ์ฝ๋ฐฑ ํจ์
    public override void OnLeftRoom()  //PhotonNetwork.LeaveRoom(); ์ฑ๊ณตํ์ ๋ 
    {
        //๋ก๋น ์ฌ์ ํธ์ถ
        UnityEngine.SceneManagement.SceneManager.LoadScene("scLobby");
    }
    
    
    
```
    
 </details>  
    
## 7.์ ๊ณต๊ฒฉ  

https://user-images.githubusercontent.com/63942174/158361758-0b3e8f61-7d3b-408e-a889-6ef53706b9a1.mp4

    ์ขํด๋ฆญ์ ๋๋ฅด๋ฉด ํฌํ์ ๋ฐ์ฌํ๊ฒ ๋ฉ๋๋ค. UI๋ฅผ ํด๋ฆญํ๋ฉด ํฌํ์ด ๋ฐ์ฌ๋์ง ์๋๋ก ์ค์ ํ์์ต๋๋ค.

<details>  
    <summary>์ ์ ํฅํด ๊ณต๊ฒฉ์ ํ  ์ ์คํ๋๋ ์คํฌ๋ฆฝํธ(FireCannon)</summary>

```C#
    
     void Awake()
    {
        //cannon ํ๋ฆฌํน์ Resources ํด๋์์ ๋ถ๋ฌ์ ๋ณ์์ ํ ๋น
        cannon = (GameObject)Resources.Load("cannon");

        //ํฌํ ๋ฐ์ฌ ์ฌ์ด๋ ํ์ผ์ Resources ํด๋์์ ๋ถ๋ฌ์ ๋ณ์์ ํ ๋น
        fireSfx = Resources.Load<AudioClip>("CannonFire");
        //AudioSource ์ปดํฌ๋ํธ๋ฅผ ํ ๋น
        sfx = GetComponent<AudioSource>();

        //PhotonView ์ปดํฌ๋ํธ๋ฅผ pv ๋ณ์์ ํ ๋น
        pv = GetComponent<PhotonView>();

        tankDamage = this.GetComponent<TankDamage>();
    }

    // Update is called once per frame
    void Update()
    {

        //๋ง์ฐ์ค ์ผ์ชฝ ๋ฒํผ ํด๋ฆญ ์ ๋ฐ์ฌ ๋ก์ง ์ํ
        if (pv.IsMine && Input.GetMouseButtonDown(0))
        {
            // ๋ง์ฐ์ค๊ฐ UI์์ ์์์ ๋ชป์๊ฒ
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (tankDamage != null)
                {
                    if (GameMgr.m_GameState != GameState.GS_Playing)
                        return;  //๋ชป์๊ฒ...

                    if (tankDamage.currHp <= 0)  //์ฃฝ์์ผ๋ฉด ๋ชป์๊ฒ...
                        return;

                    if (0.0f < tankDamage.m_ReSetTime)
                        return;
                }

                Fire();

                //์๊ฒฉ ๋คํธ์ํฌ ํ๋ ์ด์ด์ ํฑํฌ์ RPC๋ก ์๊ฒฉ Fire ํจ์๋ฅผ ํธ์ถ
                pv.RPC("Fire", RpcTarget.Others, null);
            }
        }
    }


    [PunRPC]
    void Fire()
    {
        //๋ฐ์ฌ ์ฌ์ด๋ ๋ฐ์
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

    // ๋ฐ์ฌ์ ์ฌ์ด๋ ์ค์ 
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
    
    ์ด์๊ณผ ํฑํฌ์ ํธ๋ฆฌ๊ฑฐ๊ฐ ์ถฉ๋ํ๋ฉด ์คํ๋๋ ์คํฌ๋ฆฝํธ์๋๋ค. ์ถฉ๋์ ๋ฐ๋ฏธ์ง๋ฅผ ์๊ฒ๋๊ณ  BillboardUI์ ํฑํฌ์ฒด๋ ฅ์ ๊น๊ฒ ๋ฉ๋๋ค.
      
<details>  
    <summary>ํฑํฌ๊ฐ ๊ณต๊ฒฉ์ ๋ฐ์์ ๋ ์คํฌ๋ฆฝํธ(TankDamage)</summary>

```C#
     [HideInInspector] public PhotonView pv = null;

    //ํฑํฌ ํญํ ํ ํฌ๋ช ์ฒ๋ฆฌ๋ฅผ ์ํ MeshRenderer ์ปดํฌ๋ํธ ๋ฐฐ์ด
    private MeshRenderer[] renderers;

    //ํฑํฌ ํญ๋ฐ ํจ๊ณผ ํ๋ฆฌํน์ ์ฐ๊ฒฐํ  ๋ณ์
    private GameObject expEffect = null;

    //ํฑํฌ์ ์ด๊ธฐ ์๋ช์น
    private int initHp = 200;
    //ํฑํฌ์ ํ์ฌ ์๋ช์น
    int IsMineBuf_CurHp = 0; //IsMine ๊ฒฝ์ฐ์๋ง ์ฌ์ฉ๋  ๋ณ์
    public int currHp = 0;
    int m_OldcurHp = 0;

    //ํฑํฌ ํ์์ Canvas ๊ฐ์ฒด๋ฅผ ์ฐ๊ฒฐํ  ๋ณ์
    public Canvas hudCanvas;
    //Filled ํ์์ Image UI ํญ๋ชฉ์ ์ฐ๊ฒฐํ  ๋ณ์
    public Image hpBar;

    //ํ๋ ์ด์ด Id๋ฅผ ์ ์ฅํ๋ ๋ณ์
    public int playerId = -1;

    //์  ํฑํฌ ํ๊ดด ์ค์ฝ์ด๋ฅผ ์ ์ฅํ๋ ๋ณ์
    int IsMineBuf_killCount = 0; //IsMine ๊ฒฝ์ฐ์๋ง ์ฌ์ฉ๋  ๋ณ์
    public int killCount = 0;    //๋ชจ๋  PC์ ๋ด ํฑํฌ๋ค์ ๋ณ์

    //ํฑํฌ HUD์ ํํํ  ์ค์ฝ์ด Text UI ํญ๋ชฉ
    public Text txtKillCount;

    ExitGames.Client.Photon.Hashtable CurrHpProps
                        = new ExitGames.Client.Photon.Hashtable();

    ExitGames.Client.Photon.Hashtable KillProps
                        = new ExitGames.Client.Photon.Hashtable();

    [HideInInspector] public float m_ReSetTime = 0.0f;   //๋ถํ์๊ฐ๋๋ ์ด
    //์์ํ์๋ ๋๋ ์ด ์ฃผ๊ธฐ 10์ด๋์

    void Awake()
    {
        //PhotonView ์ปดํฌ๋ํธ ํ ๋น
        pv = GetComponent<PhotonView>();

        //ํฑํฌ ๋ชจ๋ธ์ ๋ชจ๋  Mesh Renderer ์ปดํฌ๋ํธ๋ฅผ ์ถ์ถํ ํ ๋ฐฐ์ด์ ํ ๋น
        renderers = GetComponentsInChildren<MeshRenderer>();

        //ํ์ฌ ์๋ช์น๋ฅผ ์ด๊ธฐ ์๋ช์น๋ก ์ด๊น๊ฐ ์ค์ 
        IsMineBuf_CurHp = initHp;
        currHp = initHp;
        m_OldcurHp = initHp;

        //ํฑํฌ ํญ๋ฐ ์ ์์ฑ์ํฌ ํญ๋ฐ ํจ๊ณผ๋ฅผ ๋ก๋
        expEffect = Resources.Load<GameObject>("ExplosionMobile");

        //Filled ์ด๋ฏธ์ง ์์์ ๋น์์ผ๋ก ์ค์ 
        hpBar.color = Color.green;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitCustomProperties(pv);

        //PhotonView์ ownerId๋ฅผ PlayerId์ ์ ์ฅ
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
                //์ด ๋ถ๋ถ์ ํฑํฌ๊ฐ ์ฒ์ ๋ฐฉ์ ์์ฅํ  ๋ ํ๋ฒ๋ง ํธ์ถํ๊ฒ ํ๊ธฐ ์ํ ๋ถ๋ถ
                //์ฐ์  ํฑํฌ์ ์ํ๋ฅผ ํ๊ดด๋ ์ดํ์ฒ๋ผ.. 
                //๋ณด์ด์ง ์๊ฒ ํ๊ณ  ๋ชจ๋ Ready์ํ๊ฐ ๋์์ ๋ ์์ํ๊ฒ ํ๋ค. 
                ReadyStateTank();
                //์ด์ํ๊ฒ ๋ชจ๋  Update๋ฅผ ๋๊ณ ๋ ํ์ ์ ์ฉํด์ผ UI๊ฐ ๊นจ์ง์ง ์๋๋ค.
                //(ํฑํฌ ์์ฑ์ ์ฒ์ ํ๋ฒ๋ง ๋ฐ์๋๋๋ก ํ๋ค.)
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

            //์๊ธฐ๊ฐ ์ ์ด์์ด๋ฉด ์ถฉ๋ ์ ์ธ
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
        if (pv.IsMine == false)  //์์ ํจ์ ํธ์ถ ๋ถ๋ถ์์ ์ฒดํฌํ๊ณ  ์์
            return;
        //์ ์กฐ๊ฑด์ ํต๊ณผํ๋ค๋ ๊ฑด ๋ด ์คํํ์ผ์์ ์คํฐ์ํค๊ณ  ์กฐ์ ํ๊ณ  ์๋ 
        //ํฑํฌ์ผ๋๋ง ์ฒ๋ฆฌํ๊ฒ ๋ค๋ ๋ป

        if (0.0f < m_ReSetTime)  //๊ฒ์ ์์ ํ 10์ด ๋์ ๋๋ ์ด ์ฃผ๊ธฐ
            return;

        //๋์ค์ ๋ฐ๋ผ๊ฐ๋ ๊ฐ์ด๋๊น ์ด๊ฒ๋ ์ฃฝ์ด ์๋ ์ํ๋ฉด ์์ง ๋ฐ๋ฏธ์ง ์ฐจ๊ฐ ๋๊ธฐ ํด์ผํจ
        if (currHp <= 0)  
            return;

        string a_DamageTeam = "blue";
        if (pv.Owner.CustomProperties.ContainsKey("MyTeam") == true)
            a_DamageTeam = (string)pv.Owner.CustomProperties["MyTeam"];

        //์ง๊ธ ๋ฐ๋ฏธ์ง๋ฅผ ๋ฐ๋ ํฑํฌ๊ฐ AttackerId ๊ณต๊ฒฉ์ ํ๊ณผ ๋ค๋ฅธ ํ์ผ๋๋ง ๋ฐ๋ฏธ์ง๊ฐ ๋ค์ด๊ฐ๋๋ก ์ฒ๋ฆฌ
        if (a_AttTeam == a_DamageTeam)
            return;

        if (0 < IsMineBuf_CurHp)
        {
            //if (AttackerId == playerId) //์๊ธฐ๊ฐ ์ ์ด์์ ์์ ์ด ๋ง์ผ๋ฉด ์๋๊ธฐ ๋๋ฌธ์...
            //    return; //์์ ํจ์ ํธ์ถ ๋ถ๋ถ์์ ์ฒดํฌํ๊ณ  ์์

            IsMineBuf_CurHp -= 20;
            if (IsMineBuf_CurHp < 0)
                IsMineBuf_CurHp = 0;

            int a_DamPlayerID = -1; //ํํ
            if (IsMineBuf_CurHp <= 0)  // ๋งํ
            {
                a_DamPlayerID = AttackerId;
            }

            SendCurHp(IsMineBuf_CurHp, a_DamPlayerID);  //๋ธ๋ก๋ ์ผ์ดํ 
            //<-- ์ด๊ฑธ ํด ์ค์ผ ๋ธ๋ก๋ ์ผ์ดํ ๋๋ค.

        }//if (0 < IsMineBuf_CurHp)
    }//public void TakeDamage(int AttackerId)

    //ํญ๋ฐ ํจ๊ณผ ์์ฑ ๋ฐ ๋ฆฌ์คํฐ ์ฝ๋ฃจํด ํจ์
    IEnumerator ExplosionTank()
    {
        //ํญ๋ฐ ํจ๊ณผ ์์ฑ
        if (5.0f < Time.time) //๊ฒ์ ์์ ํ 5์ด๊ฐ ์ง๋๋ค์์ ์ดํํธ ํฐ์ง๋๋ก.... 
        //๊ฒ์์ด ์์ํ์๋ง์ ๊ธฐ์กด์ ์ฃฝ์ด ์๋ ์ ๋ค ์ดํํธ๊ฐ ํฐ์ง๋๊น ์ด์ํ๋ค.
        {
            Object effect = GameObject.Instantiate(expEffect,
                                    transform.position, Quaternion.identity);

            Destroy(effect, 3.0f);
        }

        //HUD๋ฅผ ๋นํ์ฑํ
        hudCanvas.enabled = false;

        //ํฑํฌ ํฌ๋ช ์ฒ๋ฆฌ
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

    //์์ ์ ํ๊ดด์ํจ ์  ํฑํฌ๋ฅผ ๊ฒ์ํด ์ค์ฝ์ด๋ฅผ ์ฆ๊ฐ์ํค๋ ํจ์
    //firePlayerId : Kill ์๋ฅผ ์ฆ๊ฐ ์ํค๊ธฐ ์ํ ํฑํฌ ID ์บ๋ฆญํฐ ์ฐพ์์ค๊ธฐ
    void SaveKillCount(int firePlayerId)
    {
        //TAKE ํ๊ทธ๋ฅผ ์ง์ ๋ ๋ชจ๋  ํฑํฌ๋ฅผ ๊ฐ์ ธ์ ๋ฐฐ์ด์ ์ ์ฅ
        GameObject[] tanks = GameObject.FindGameObjectsWithTag("TANK");
        foreach (GameObject tank in tanks)
        {
            var tankDamage = tank.GetComponent<TankDamage>();
            //ํฑํฌ์ playerId๊ฐ ํฌํ์ playerId์ ๋์ผํ์ง ํ๋จ
            if (tankDamage != null && tankDamage.playerId == firePlayerId)
            {
                //๋์ผํ ํฑํฌ์ผ ๊ฒฝ์ฐ ์ค์ฝ์ด๋ฅผ ์ฆ๊ฐ์ํด
                tankDamage.IncKillCount();
                return;
            }
        }
    }//void SaveKillCount(int firePlayerId)

    void IncKillCount() //๋๋ฆฐ ํฑํฌ ์์ฅ์ผ๋ก ํธ์ถ๋จ
    {
        if (pv != null && pv.IsMine == true)
        {
            IsMineBuf_killCount++;

            SendKillCount(IsMineBuf_killCount);
            //๋ธ๋ก๋ ์ผ์ดํ <--//์ด๊ฑธ ํด ์ค์ผ ๋ธ๋ก๋ ์ผ์ดํ ๋๋ค.
        }//if (pv != null && pv.IsMine == true)
    }//void IncKillCount()

    public void ReadyStateTank()
    {
        if (GameMgr.m_GameState != GameState.GS_Ready)
            return;
  
        //-------๋ง์คํฐ ๊ธฐ์ค์ผ๋ก ํ๋ฒ๋ง ํฑํฌ ๋ฆฌ์คํฐ ์๋ฆฌ๋ฅผ ์ ํด์ค๋ค.

        StartCoroutine(this.WaitReadyTank());
    }

    //๊ฒ์ ์์ ๋๊ธฐ...
    IEnumerator WaitReadyTank()
    {
        //HUD๋ฅผ ๋นํ์ฑํ
        hudCanvas.enabled = false;

        //ํฑํฌ ํฌ๋ช ์ฒ๋ฆฌ
        SetTankVisible(false);

        while (GameMgr.m_GameState == GameState.GS_Ready)
        {
            yield return null;
        }

        //ํฑํฌ ํน์ ํ ์์น์ ๋ฆฌ์คํฐ๋๋๋ก...
        //--------- ํฑํฌ ํน์ ํ ์์น์ ๋ฆฌ์คํฐ๋๋๋ก...
        //์์น ๊ณ ์  ํ์...
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
        m_ReSetTime = 10.0f; //๊ฒ์ ์์ํ์๋ ๋๋ ์ด ์ฃผ๊ธฐ
        //--------- ํฑํฌ ํน์ ํ ์์น์ ๋ฆฌ์คํฐ๋๋๋ก...


        //Filled ์ด๋ฏธ์ง ์ด๊น๊ฐ์ผ๋ก ํ์
        hpBar.fillAmount = 1.0f;
        //Filled ์ด๋ฏธ์ง ์์์ ๋น์์ผ๋ก ์ค์ 
        hpBar.color = Color.green;
        //HUD ํ์ฑํ
        hudCanvas.enabled = true;

        if (pv != null && pv.IsMine == true)
        {
            //๋ฆฌ์คํฐ ์ ์๋ช ์ด๊น๊ฐ ์ค์ 
            IsMineBuf_CurHp = initHp;

            SendCurHp(IsMineBuf_CurHp, -1); // ๋ธ๋ก๋ ์ผ์ดํ
        }

        //ํฑํฌ๋ฅผ ๋ค์ ๋ณด์ด๊ฒ ์ฒ๋ฆฌ
        SetTankVisible(true);
    }


    #region --------------- CustomProperties ์ด๊ธฐํ
    void InitCustomProperties(PhotonView pv)
    { //์๋๋ฅผ ์ํด ๋ฒํผ๋ฅผ ๋ฏธ๋ฆฌ ๋ง๋ค์ด ๋๋๋ค๋ ์๋ฏธ
        //pv.IsMine == true ๋ด๊ฐ ์กฐ์ ํ๊ณ  ์๋ ํฑํฌ์ด๊ณ  ์คํฐ์์ ์...
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
    #endregion  //--------------- CustomProperties ์ด๊ธฐํ


    #region --------------- Hp Sync
    //--------------- Send CurHp
    void SendCurHp(int CurHP = 200, int a_LAtt_ID = -1)
    {
        if (pv == null)
            return;

        if (pv.IsMine == false)
            return;
        //๋ด๊ฐ ์กฐ์ ํ๊ณ  ์๋ ํฑํฌ ์์ฅ์์๋ง ๋ณด๋ธ๋ค.
        //(์ฆ ๋ด๊ฐ ์กฐ์ ํ๋ ํฑํฌ๋ฅผ ๊ธฐ์ค์ผ๋ก๋ง ๋๊ธฐํ๋ฅผ ๋ง์ถ๋ค.)

        if (CurrHpProps == null)
        {
            CurrHpProps = new ExitGames.Client.Photon.Hashtable();
            CurrHpProps.Clear();
        }

        //์๊ธฐ ํฑํฌ์ ์ ์ฅ ๊ณต๊ฐ์ ๊ฐ์ ๊ฐฑ์ ํด์ ๋ธ๋ก๋ ์ผ์ดํ
        if (CurrHpProps.ContainsKey("curHp") == true) //๋ชจ๋  ์บ๋ฆญํฐ์ ์๋์ง๋ฐ ๋๊ธฐํ
        {
            CurrHpProps["curHp"] = CurHP;
        }
        else
        {
            CurrHpProps.Add("curHp", CurHP);
        }

        //๋ด๊ฐ ์ฃฝ์ ๋ ๋งํ๋ฅผ ์น ์ ์ ๋ฅผ ์ฐพ์์ ํฌ์๋ฅผ ์ฌ๋ ค์ฃผ๋ ค๊ณ ...
        if (CurrHpProps.ContainsKey("LastAttackerID") == true)
        {
            CurrHpProps["LastAttackerID"] = a_LAtt_ID;
        }
        else
        {
            CurrHpProps.Add("LastAttackerID", a_LAtt_ID);
        }

        pv.Owner.SetCustomProperties(CurrHpProps);  //๋ธ๋ก๋ ์ผ์ดํ 
    }//void SendCurHp(int CurHP = 200, int a_LAtt_ID = -1)
    //--------------- Send CurHp

    //--------------- Receive CurHp
    void ReceiveCurHp() //CurHp ๋ฐ์์ ์ฒ๋ฆฌํ๋ ๋ถ๋ถ
    {
        if (pv == null)
            return;

        if (pv.Owner == null)
            return;

        if (pv.Owner.CustomProperties.ContainsKey("curHp") == true)
        {//๋ชจ๋  ์บ๋ฆญํฐ์ ์๋์ง๋ฐ ๋๊ธฐํ
            currHp = (int)pv.Owner.CustomProperties["curHp"];

            //ํ์ฌ ์๋ช์น ๋ฐฑ๋ถ์จ = (ํ์ฌ ์๋ช์น) / (์ด๊ธฐ ์๋ช์น)
            hpBar.fillAmount = (float)currHp / (float)initHp;

            //์๋ช ์์น์ ๋ฐ๋ผ Filled ์ด๋ฏธ์ง์ ์์์ ๋ณ๊ฒฝ
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
                    //์์ ์ ํ๊ดด์ํจ ์  ํฑํฌ์ ์ค์ฝ์ด๋ฅผ ์ฆ๊ฐ์ํค๋ ํจ์๋ฅผ ํธ์ถ
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
    void ReceiveKillCount() //KillCount ๋ฐ์์ ์ฒ๋ฆฌํ๋ ๋ถ๋ถ
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
    
## 8.์บ๋ฆญํฐ ์กฐ์๊ณผ ์นด๋ฉ๋ผ ํ์   

https://user-images.githubusercontent.com/63942174/158361799-9cb3bf1c-8fa2-49ba-9935-400b23727e87.mp4

    ํฑํฌ์ ์ด๋ ๋ฐ ๋ฒ์์ ํ, ํฑํฌ๋ผ๋ฆฌ์ ์ถฉ๋๋ฒ์๋ฅผ ์ ํํ ์คํฌ๋ฆฝํธ์๋๋ค.  
    ์ข์ฐ๋ฒํผ์ ๋๋ฅด๋ฉด ํฑํฌ๊ฐ ํ์ ํ๊ฒ ๋๊ณ  ์นด๋ฉ๋ผ๊ฐ ํฑํฌ์ ์ ๋ฐฉ์ ํฅํ๊ฒ ์ฒ์ฒํ ๋ฐ๋ผ๊ฐ๋๋ค.
    
<details>  
    <summary>ํฑํฌ ์ด๋ ๊ด๋ จ ์คํฌ๋ฆฝํธ(TankMove)</summary>

```C#
    public class TankMove : MonoBehaviourPunCallbacks, IPunObservable
{
    //PhotonView ์ปดํฌ๋ํธ๋ฅผ ํ ๋นํ  ๋ณ์
    private PhotonView pv = null;
    //๋ฉ์ธ ์นด๋ฉ๋ผ๊ฐ ์ถ์ ํ  CamPivot ๊ฒ์์ค๋ธ์ ํธ
    public Transform camPivot;

    //ํฑํฌ์ ์ด๋ ๋ฐ ํ์  ์๋๋ฅผ ๋ํ๋ด๋ ๋ณ์
    public float moveSpeed = 20.0f;
    public float rotSpeed = 50.0f;

    //์ฐธ์กฐํ  ์ปดํฌ๋ํธ๋ฅผ ํ ๋นํ  ๋ณ์
    [HideInInspector] public Rigidbody rbody;
    [HideInInspector] public Transform tr;

    //ํค๋ณด๋ ์๋ ฅ๊ฐ ๋ณ์
    private float h, v;

    Vector3 a_CacPos = Vector3.zero;

    //์์น ์ ๋ณด๋ฅผ ์ก์์ ํ  ๋ ์ฌ์ฉํ  ๋ณ์ ์ ์ธ ๋ฐ ์ด๊น๊ฐ ์ค์ 
    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;

    TankDamage tankDamage = null;
    
    
    //------------ ํฑํฌ๋ผ๋ฆฌ ๊ตฌ์ถฉ๋๋ก ๋ฐ๋ฆฌ๊ฒ ํ๊ธฐ ์ฝ๋ ๋ถ๋ถ
    float a_Radius = 8.5f;
    GameObject[] a_tanks = null;
    Vector3 a_fCacDist = Vector3.zero;
    float a_CacDistLen = 0.0f;
    float a_ShiftLen = 0.0f;
    TankDamage a_TkDamage = null;

    void Awake()
    {
        //์ปดํฌ๋ํธ ํ ๋น
        rbody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();

        //PhotonView ์ปดํฌ๋ํธ ํ ๋น
        pv = GetComponent<PhotonView>();
        pv.ObservedComponents[0] = this;
                
              
        //์ ์ ๊ฐ ์กฐ์ ํ๊ณ  ์๋ ๋ก์ปฌ์์ ๋ง๋ค์ด์ง ํฑํฌ์ PhotonView์ผ ๊ฒฝ์ฐ
        if (pv.IsMine)
        {
            //๋ฉ์ธ ์นด๋ฉ๋ผ์ ์ถ๊ฐ๋ SmoothFollow ์คํฌ๋ฆฝํธ์ ์ถ์  ๋์์ ์ฐ๊ฒฐ
            Camera.main.GetComponent<SmoothFollow>().target = camPivot;
        }  
    
        //์๊ฒฉ ํฑํฌ์ ์์น ๋ฐ ํ์  ๊ฐ์ ์ฒ๋ฆฌํ  ๋ณ์์ ์ด๊น๊ฐ ์ค์ 
        currPos = tr.position;
        currRot = tr.rotation;

        tankDamage = this.GetComponent<TankDamage>();
    
    void Start()
    {

        if (!pv.IsMine) //๋ด๊ฐ ์กฐ์ ํ๊ณ  ์๋ ํฑํฌ๊ฐ ์๋ ๊ฒฝ์ฐ
        {
            //์๊ฒฉ ๋คํธ์ํฌ ํ๋ ์ด์ด์ ํฑํฌ๋ ๋ฌผ๋ฆฌ๋ ฅ์ ์ด์ฉํ์ง ์์
            rbody.isKinematic = true;
        }

        //Rigidbody์ ๋ฌด๊ฒ์ค์ฌ์ ๋ฎ๊ฒ ์ค์ 
        rbody.centerOfMass = new Vector3(0.0f, -2.5f, 0.0f);
    }


    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine) //๋ด๊ฐ ๋ก์ปฌ์์ ๋ง๋  ํฑํฌ์ธ ๊ฒฝ์ฐ์๋ง ์กฐ์ ์ด ๊ฐ๋ฅํ๊ฒ ํ๋ค.
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

            //ํ์ ๊ณผ ์ด๋์ฒ๋ฆฌ
            tr.Rotate(Vector3.up * rotSpeed * h * Time.deltaTime);
            tr.Translate(Vector3.forward * v * moveSpeed * Time.deltaTime);
            //Default ๊ฐ Space.Self

            //------------ํฑํฌ๋ผ๋ฆฌ ๊ตฌ์ถฉ๋๋ก ๋ฐ๋ฆฌ๊ฒ ํด์ ๋ฌผ๋ฆฌ์์ง์ด ๋ฐ๋ํ์ง ์๊ฒ ํ๊ธฐ...
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
            //------------ํฑํฌ๋ผ๋ฆฌ ๊ตฌ์ถฉ๋๋ก ๋ฐ๋ฆฌ๊ฒ ํด์ ๋ฌผ๋ฆฌ์์ง์ด ๋ฐ๋ํ์ง ์๊ฒ ํ๊ธฐ...

            //------------ํฑํฌ๊ฐ ์งํ์ ๋ฒ์ด๋์ง ๋ชปํ๊ฒ ๋ง๊ธฐ...
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
            //------------ํฑํฌ๊ฐ ์งํ์ ๋ฒ์ด๋์ง ๋ชปํ๊ฒ ๋ง๊ธฐ...
        }// if (pv.IsMine)
        else //์๊ฒฉ์ผ๋ก ๋ง๋ค์ด์ง ํฑํฌ๋ค...
        { //์ขํ๋ฅผ ์ค๊ณ ๋ฐ์ ์์ง์ผ ๊ฒ์

            if (10.0f < (tr.position - currPos).magnitude)
            {
                tr.position = currPos;
            }
            else
            {
                //์๊ฒฉ ํ๋ ์ด์ด์ ํฑํฌ๋ฅผ ์์ ๋ฐ์ ์์น๊น์ง ๋ถ๋๋ฝ๊ฒ ์ด๋์ํด
                tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
            }
            //์๊ฒฉ ํ๋ ์ด์ด์ ํฑํฌ๋ฅผ ์์ ๋ฐ์ ๊ฐ๋๋งํผ ๋ถํธ๋ฝ๊ฒ ํ์ ์ํด
            tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 10.0f);

        }//์๊ฒฉ์์ ๋ง๋ค์ด์ง ํฑํฌ๋ค...
    }//void Update()
    
      //๊ธฐ๋ณธ์ค์ ์ SendRate 1์ด 20๋ฒ, SerializtionRate 1์ด์ 10๋ฒ์ผ๋ก ์๊ณ ์์ต๋๋ค.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //๋ก์ปฌ ํ๋ ์ด์ด์ ์์น ์ ๋ณด ์ก์ 
        if (stream.IsWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else //์๊ฒฉ ํ๋ ์ด์ด์ ์์น ์ ๋ณด ์์ 
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}

                                            
```
    
 </details>  
    
    ํฑํฌ ์ค๋ธ์ ํธ์ ํฐ๋ ์  ํ์ ์์ผ์ฃผ๋ ์คํฌ๋ฆฝํธ์๋๋ค.
      
<details>  
    <summary>ํฐ๋  ํ์  ๊ด๋ จ ์คํฌ๋ฆฝํธ(TurretCtrl)</summary>

```C#
    
public class TurretCtrl : MonoBehaviourPunCallbacks, IPunObservable
{
    private Transform tr;
    //๊ด์ (Ray)์ด ์ง๋ฉด์ ๋ง์ ์์น๋ฅผ ์ ์ฅํ  ๋ณ์
    private RaycastHit hit;

    //ํฐ๋ ์ ํ์  ์๋
    public float rotSpeed = 5.0f;

    //PhotonView ์ปดํฌ๋ํธ ๋ณ์
    private PhotonView pv = null;
    //์๊ฒฉ ๋คํธ์ํฌ ํฑํฌ์ ํฐ๋  ํ์ ๊ฐ์ ์ ์ฅํ  ๋ณ์
    private Quaternion currRot = Quaternion.identity;

    void Awake()
    {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();


        //์ด๊ธฐ ํ์ ๊ฐ ์ค์ 
        currRot = tr.localRotation;
    }

    void Update()
    {
        //์์ ์ ํฑํฌ์ผ ๋๋ง ์กฐ์ 
        if (pv.IsMine == true)
        {
            if (PhotonInit.isFocus == false) //์๋์ฐ ์ฐฝ์ด ๋นํ์ฑํ ๋์ด ์๋ค๋ฉด...
                return;

            //๋ฉ์ธ ์นด๋ฉ๋ผ์์ ๋ง์ฐ์ค ์ปค์์ ์์น๋ก ์บ์คํ๋๋ Ray๋ฅผ ์์ฑ
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //์์ฑ๋ Ray๋ฅผ Scene ๋ทฐ์ ๋น์ ๊ด์ ์ผ๋ก ํํ
            Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("TERRAIN")))
            {
                //Ray์ ๋ง์ ์์น๋ฅผ ๋ก์ปฌ์ขํ๋ก ๋ณํ
                Vector3 relative = tr.InverseTransformPoint(hit.point);
                //์ญํ์  ํธ ํจ์์ธ Atan2๋ก ๋ ์  ๊ฐ์ ๊ฐ๋๋ฅผ ๊ณ์ฐ
                float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
                //rotSpeed ๋ณ์์ ์ง์ ๋ ์๋๋ก ํ์ 
                tr.Rotate(0, angle * Time.deltaTime * rotSpeed, 0);
            }
            else
            {
                Vector3 a_OrgVec = ray.origin + ray.direction * 2000.0f;
                ray = new Ray(a_OrgVec, -ray.direction);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity,
                                            1 << LayerMask.NameToLayer("TURRETPICKOBJ")))
                {
                    //Ray์ ๋ง์ ์์น๋ฅผ ๋ก์ปฌ์ขํ๋ก ๋ณํ
                    Vector3 relative = tr.InverseTransformPoint(hit.point);
                    //์ญํ์  ํธ ํจ์์ธ Atan2๋ก ๋ ์  ๊ฐ์ ๊ฐ๋๋ฅผ ๊ณ์ฐ
                    float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
                    //rotSpeed ๋ณ์์ ์ง์ ๋ ์๋๋ก ํ์ 
                    tr.Rotate(0, angle * Time.deltaTime * rotSpeed, 0);
                }
            } //else

        }//  if (pv.IsMine == true)
        else //์๊ฒฉ ๋คํธ์ํฌ ํ๋ ์ด์ด์ ํฑํฌ์ผ ๊ฒฝ์ฐ
        {
            //ํ์ฌ ํ์ ๊ฐ๋์์ ์์ ๋ฐ์ ์ค์๊ฐ ํ์ ๊ฐ๋๋ก ๋ถ๋๋ฝ๊ฒ ํ์ 
            tr.localRotation = Quaternion.Slerp(tr.localRotation, currRot, Time.deltaTime * 10.0f);
        }
    } //void Update()

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //๋ก์ปฌ ํ๋ ์ด์ด์ ์์น ์ ๋ณด ์ก์ 
        if (stream.IsWriting)
        {
            stream.SendNext(tr.localRotation);
        }
        else //์๊ฒฉ ํ๋ ์ด์ด์ ์์น ์ ๋ณด ์์ 
        {
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }

}
    
```
    
 </details>  
    
    ํฑํฌ์ ํฌ์ ์ด ์์๋๋ก ์์ง์ด๊ฒ ํ๊ธฐ ์ํ ์คํฌ๋ฆฝํธ์๋๋ค.
    
<details>  
    <summary>ํฌ์  ์ปจํธ๋กค ๊ด๋ จ ์คํฌ๋ฆฝํธ(CannonCtrl)</summary>

```C#
    
public class CannonCtrl : MonoBehaviourPunCallbacks, IPunObservable
{
    private Transform tr;
    public float rotSpeed = 5.0f;

    private RaycastHit hit;

    private PhotonView pv = null;

    //์๊ฒฉ ๋คํธ์ํฌ ํฑํฌ์ ํฌ์  ํ์  ๊ฐ๋๋ฅผ ์ ์ฅํ  ๋ณ์
    private Quaternion currRot = Quaternion.identity;

    // Start is called before the first frame update
    void Awake()
    {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();

        //์ด๊ธฐ ํ์ ๊ฐ ์ค์ 
        currRot = tr.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        //์์ ์ด ๋ง๋  ๋คํธ์ํฌ ๊ฒ์์ค๋ธ์ ํธ๊ฐ ์๋ ๊ฒฝ์ฐ๋ ํค๋ณด๋ ์กฐ์ ๋ฃจํด์ ๋๊ฐ
        if (pv.IsMine)
        {
            if (PhotonInit.isFocus == false) //์๋์ฐ ์ฐฝ์ด ๋นํ์ฑํ ๋์ด ์๋ค๋ฉด...
                return;

            //tr.Rotate(angle, 0, 0);

            //๋ฉ์ธ ์นด๋ฉ๋ผ์์ ๋ง์ฐ์ค ์ปค์์ ์์น๋ก ์บ์คํ๋๋ Ray๋ฅผ ์์ฑ
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

            //ํฌ์  ๊ฐ๋ ์ ํ...
            Vector3 a_Angle = tr.localEulerAngles;
            if (a_Angle.x < 180.0f)   //ํฌ์  ๊ฐ๋๋ฅผ ๋ด๋ ค๊ฐ๋ ค๋ ๊ฒฝ์ฐ
            {
                if (5.0f < a_Angle.x)
                    a_Angle.x = 5.0f;
            }
            else                      //ํฌ์  ๊ฐ๋๋ฅผ ์ฌ๋ฆฌ๋ ค๋ ๊ฒฝ์ฐ
            {
                if (a_Angle.x < 330.0f)  //๊ฐ์ ๋ ์ค์ด๋ฉด ๊ฐ๋๊ฐ ์ ํ์ด ๋๋๋ค.
                    a_Angle.x = 330.0f;
            }

            tr.localEulerAngles = a_Angle;

        }//if (pv.IsMine)
        else
        {
            //ํ์ฌ ํ์  ๊ฐ๋์์ ์์ ๋ฐ์ ์ค์๊ฐ ํ์  ๊ฐ๋๋ก ๋ถ๋๋ฝ๊ฒ ํ์ 
            tr.localRotation = Quaternion.Slerp(tr.localRotation, currRot, Time.deltaTime * 10.0f);
        }
    }

    //์ก์์  ์ฝ๋ฐฑ ํจ์
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //๋ก์ปฌ ํ๋ ์ด์ด์ ์์น ์ ๋ณด ์ก์ 
        if (stream.IsWriting)
        {
            stream.SendNext(tr.localRotation);
        }
        else //์๊ฒฉ ํ๋ ์ด์ด์ ์์น ์ ๋ณด ์์ 
        {
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}

    
```
    
 </details>  

## 9.๋ฏธ๋๋งต ์์ฑ  

https://user-images.githubusercontent.com/63942174/158361902-0618f85d-ab83-44aa-b93c-ec21983daf28.mp4

    ๊ฒ์ ์์ ํ 30์ด ๋ค์ ๋ฏธ๋๋งต UI๋ฅผ ํ์ํด์ฃผ๋ ์คํฌ๋ฆฝํธ์๋๋ค.
      
<details>  
    <summary>๋ฏธ๋๋งต ๊ด๋ จ ์คํฌ๋ฆฝํธ(GameMgr)</summary>

```C#
    
    ///  ------๋ฏธ๋๋งต ์ ์  ์๊น ๋ฐ๊พธ๊ธฐ
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
        // ๋ฏธ๋๋งต ๋ณด๊ธฐ ์นด์ดํธ๋ค์ด ์์
        MiniMapTimeCount += Time.deltaTime;

        MiniMapCountText.text = ((int)MiniMapTimeCount).ToString();

        if ( MiniMapTimeCount >= MiniMapShowTime)
        {
            MiniMap.gameObject.SetActive(true);
        }
    }
    
```
    
 </details>  

## 10.์น๋ฆฌ  

https://user-images.githubusercontent.com/63942174/158362001-97c7fa5c-9de9-4452-8724-399135141cd5.mp4

    ๋ผ์ด๋ ์น๋ฆฌ ์ ์น๋ฆฌ ์นด์ดํธ๋ฅผ ํ๋์ฉ ์ฆ๊ฐ์์ผ ์ฃผ๊ณ  ๋ธ๋ฃจํ๊ณผ ๋ ๋ํ์ ์น์์ ํฉ๊ณ๊ฐ 5์ด์์ด๋ฉด   
    ๋ ๋์ ์น๋ฆฌ๋ฅผ ๊ฐ์ ธ๊ฐ ํ์ด ์น๋ฆฌํ๊ฒ ๋๋๋ก ๊ตฌํํ ์คํฌ๋ฆฝํธ์๋๋ค. 
    ์น๋ฆฌ ํ์คํธ๊ฐ ๋์ค๊ณ  5์ด ํ์ ๋ก๋น๋ก ๋๋์๊ฐ๋๋ก ์ค์ ํ์์ต๋๋ค.
    
<details>  
    <summary>์น๋ฆฌ์ ์ฒ๋ฆฌ ์คํฌ๋ฆฝํธ(GameMgr)</summary>

```C#
      //ํ์ชฝํ์ด ์ ๋ฉธํ๋์ง ์ฒดํฌํ๊ณ  ์น๋ฆฌ / ํจ๋ฐฐ ๋ฅผ ๊ฐ์ํ๊ณ  ์ฒ๋ฆฌํด ์ฃผ๋ ํจ์
    void WinLoseObserver()
    {
        //------------------- ์น๋ฆฌ / ํจ๋ฐฐ ์ฒดํฌ
        if (m_GameState == GameState.GS_Playing) //GS_Ready ์ํ์ ์ค๊ณ๊ฐ ์ข ๋ฆ๊ฒ์์ ํ์ชฝ์ด ์ ๋ฉธ ์ํ๋ผ๋ ๊ฑธ ๋ช๋ฒ ์ฒดํฌํ  ์๋ ์๋ค.
        {
            m_ChekWinTime = m_ChekWinTime - Time.deltaTime;
            if (m_ChekWinTime <= 0.0f) //๊ฒ์์ด ์์๋ ํ 2์ด ๋ค๋ถํฐ ํ์ ์ ์์ํ๊ธฐ ์ํ ๋ถ๋ถ
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
                        if (_player.CustomProperties.ContainsKey("curHp") == true) //๋ชจ๋  ์บ๋ฆญํฐ์ ์๋์ง๋ฐ ๋๊ธฐํ
                        {
                            a_CurHP = (int)_player.CustomProperties["curHp"];   //๋ชจ๋  ์บ๋ฆญํฐ... ๋งคํ๋ ์ ๊ณ์ ๋๊ธฐํ 
                            if (0 < a_CurHP)
                            {
                                rowTm1 = 1;
                            }
                        }
                        a_Tm1Count++;

                    }
                    else if (a_PlrTeam == "red")
                    {
                        if (_player.CustomProperties.ContainsKey("curHp") == true) //๋ชจ๋  ์บ๋ฆญํฐ์ ์๋์ง๋ฐ ๋๊ธฐํ
                        {
                            a_CurHP = (int)_player.CustomProperties["curHp"];   //๋ชจ๋  ์บ๋ฆญํฐ... ๋งคํ๋ ์ ๊ณ์ ๋๊ธฐํ 
                            if (0 < a_CurHP)
                            {
                                rowTm2 = 1;
                            }
                        }
                        a_Tm2Count++;

                    }
                }//foreach (Player _player in players)

                if (0 < a_Tm1Count && 0 < a_Tm2Count)   //์ํ์ ์ธ์์ด ์กด์ฌํ  ๋๋ง...
                {
                    if (rowTm1 == 0 || rowTm2 == 0)     //์ ํ์ค์ ํํ์ด ์ ๋ฉธํ๋ค๋ฉด....
                    {
                        if ((m_Team1Win + m_Team2Win) < 5) //์์ง 5Round๊น์ง ๊ฐ์ง ์์ ์ํฉ์ด๋ผ๋ฉด...
                        {
                            if (PhotonNetwork.IsMasterClient == true)
                            {
                                SendGState(GameState.GS_Ready);

                                if (rowTm1 == 0)
                                {
                                    if (-99999.0f < m_ChekWinTime) //ํ๋ฒ๋ง ++ ์ํค๊ธฐ ์ํ ์ฉ๋
                                    {
                                        m_Team2Win++;  //์ฌ๋ฌ๋ฒ ๋ฐ์ํ๋๋ผ๋ ์์ง์ ์๋ฐ์ดํธ๊ฐ ์๋ ์ํ์ด๊ธฐ ๋๋ฌธ์ ์ด์  ๊ฐ์์ ์ถ๊ฐ๋  ๊ฒ์ด๋ค.
                                        IsRoomBuf_Team2Win = m_Team2Win;
                                        m_ChekWinTime = -150000.0f;  
                                    }
                                    SendTeam2Win(IsRoomBuf_Team2Win);                                    
                                }
                                if (rowTm2 == 0)
                                {
                                    if (-99999.0f < m_ChekWinTime) //ํ๋ฒ๋ง ++ ์ํค๊ธฐ ์ํ ์ฉ๋
                                    {
                                        m_Team1Win++;
                                        IsRoomBuf_Team1Win = m_Team1Win;
                                        m_ChekWinTime = -150000.0f;
                                    }
                                    SendTeam1Win(IsRoomBuf_Team1Win);
                                }

                            }//if (PhotonNetwork.IsMasterClient == true)  

                            m_GoWaitGame = 4.0f; //๋ค์ 4์ดํ์ ๊ฒ์์ด ์์๋๋๋ก...

                            // ๋ฏธ๋๋งต ์์ฑ ์ด๊ธฐํ
                            MiniMap.gameObject.SetActive(false);
                            isMiniMapActive = false;
                            MiniMapTimeCount = 0;

                        }//if ((m_Team1Win + m_Team2Win) < 5) //์์ง 5Round๊น์ง ๊ฐ์ง ์์ ์ํฉ์ด๋ผ๋ฉด...
                    }//if (rowTm1 == 0 || rowTm2 == 0) //์ ํ์ค์ ํํ์ด ์ ๋ฉธํ๋ค๋ฉด....
                }//if (0 < a_Tm1Count && 0 < a_Tm2Count)
            }// if (m_ChekWinTime <= 0.0f)
        }//if (m_GameState == GameState.GS_Playing) 

        // ์น๋ฆฌ ์นด์ดํธ ํ์คํธ ์์ 
        if (m_BlueTeamWin != null)
            m_BlueTeamWin.text = m_Team1Win.ToString() + "์น";
        if (m_RedTeamWin != null)
            m_RedTeamWin.text = m_Team2Win.ToString() + "์น";

        if (5 <= (m_Team1Win + m_Team2Win)) //์์ง 5Round๊น์ง ๋ชจ๋ ํ๋ ์ด๋ ์ํฉ์ด๋ผ๋ฉด... 
        {
            //Game Over ์ฒ๋ฆฌ
            if (PhotonNetwork.IsMasterClient == true)
            {
                //๋๊ฐ ๋ฐ์์์ผฐ๋  ๋๊ธฐํ ์ํค๋ ค๊ณ  ํ๋ฉด....
                SendGState(GameState.GS_GameEnd); //<--- ์ฌ๊ธฐ์๋ ์ง๊ธ ๋ฃธ์ ์๋ฏธํจ

            }

            if (m_GameEndText != null)
            {
                m_GameEndText.gameObject.SetActive(true);
                if (m_Team2Win < m_Team1Win)
                {
                    m_GameEndText.text = "<color=Blue>" + "๋ธ๋ฃจํ ์น๋ฆฌ"+"</color>";
                }
                else if (m_Team1Win < m_Team2Win)
                {
                    m_GameEndText.text = "<color=Red>" + "๋ ๋ํ ์น๋ฆฌ"+ "</color>";
                }
            }

            return;
        }
        //------------------- ์น๋ฆฌ / ํจ๋ฐฐ ์ฒดํฌ

        //-------------- ํ Round๊ฐ ๋๋๊ณ  ๋ค์ Round์ ๊ฒ์์ ์์ ์ํค๊ธฐ ์ํ ๋ถ๋ถ... //๋ชจ๋ ํฑํฌ GS_Ready ์ํ์ผ ๋ ๋ชจ๋  ํฑํฌ ๋๊ธฐ ์ํ๋ก ๋ง๋ค๊ธฐ...
        if (m_OldState != GameState.GS_Ready && m_GameState == GameState.GS_Ready)
        {
            GameObject[] tanks = GameObject.FindGameObjectsWithTag("TANK");
            foreach (GameObject tank in tanks)
            {
                TankDamage tankDamage = tank.GetComponent<TankDamage>();
                if (tankDamage != null)
                    tankDamage.ReadyStateTank(); //๋ค์ ๋ผ์ด๋ ์ค๋น --> 1
            }
        }
        m_OldState = m_GameState;
        //-------------- ํ Round๊ฐ ๋๋๊ณ  ๋ค์ Round์ ๊ฒ์์ ์์ ์ํค๊ธฐ ์ํ ๋ถ๋ถ... 
    }//void WinLoseObserver()
    
    
    //์๋ฆฌ ๋ฐฐ์  ํจ์
    void SitPosInxMasterCtrl()
    {
        //if (PhotonNetwork.IsMasterClient == false)
        //    return;  //์์์์ ํ์ธํ๊ณ  ์์

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
        //    return;   //Update์์ ์ฒดํฌ ํ๊ณ  ์๋ค.

        //if (PhotonNetwork.IsMasterClient == false) //๋ง์คํฐ๋ง ๋ณด๋ธ๋ค.
        //    return;   //Update์์ ์ฒดํฌ ํ๊ณ  ์๋ค.

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
        //    return;   //Update์์ ์ฒดํฌ ํ๊ณ  ์๋ค.

        //if (PhotonNetwork.IsMasterClient == false) //๋ง์คํฐ๋ง ๋ณด๋ธ๋ค.
        //    return;   //Update์์ ์ฒดํฌ ํ๊ณ  ์๋ค.

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
