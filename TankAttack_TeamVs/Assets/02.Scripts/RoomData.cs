using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviour
{
    //외부 접근을 위해 public으로 선언했지만 Inspector에 노출하지 않음
    [HideInInspector]
    public string roomName = "";
    [HideInInspector]
    public int connectPlayer = 0;
    [HideInInspector]
    public int maxPlayer = 0;

    //룸 이름 표실할 Text UI 항목
    public Text textRoomName;
    //룸 접속자 수와 최대 접속자 수를 표시할 Text UI 항목
    public Text textConnectInfo;

    [HideInInspector] public string ReadyState = "";  //레디 상태 표시

    // Start is called before the first frame update
    void Start()
    {
        PhotonInit RefPtInit = FindObjectOfType<PhotonInit>();

        this.GetComponent<Button>().onClick.AddListener(
                delegate { 
                    if (RefPtInit != null)          
                        RefPtInit.OnClickRoomItem(roomName); 
                });
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    //룸 정보를 전달한 후 Text UI 항목에 표시하는 함수
    public void DispRoomData(bool a_IsOpen)
    {
        if (a_IsOpen == true)
        {
            textRoomName.color = new Color32(0, 0, 0, 255);
            textConnectInfo.color = new Color32(0, 0, 0, 255);
        }
        else
        {
            textRoomName.color = new Color32(0, 0, 255, 255);
            textConnectInfo.color = new Color32(0, 0, 255, 255);
        }

        textRoomName.text = roomName;
        textConnectInfo.text = "(" + connectPlayer.ToString() + "/" + maxPlayer.ToString() + ")";
    }
}
