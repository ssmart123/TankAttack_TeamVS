# TankAttack_TeamVS
포톤 서버를 사용하는 실시간 팀 대전게임입니다.

### <플레이 방법>
#### 최대 8명이서 플레이할 수 있습니다.  
#### 키보드로 탱크를 이동시킬 수 있고 마우스 좌클릭을 눌러 포탄을 발사할 수 있습니다.  
#### 총 5라운드를 진행하며 블루팀과 레드팀의 승리 수의 합이 5가 되면 승리 수가 많은 팀이 이기게 됩니다.  
#### 라운드 시작 후 10초가 지나기 전까진 공격을 할 수 없습니다.  
#### 라운드 시작 후 30초가 지나면 미니맵이 활성화되어 아군과 적군의 위치를 알 수 있습니다.  

--------------------------

## 1.방 생성  

https://user-images.githubusercontent.com/63942174/158361325-c7fa9025-d939-433f-93c3-f8e82386f4a0.mp4
``` C#
void Awake()
    {
        //포톤 클라우드 서버 접속 여부 확인
        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();  //1. 포톤 클라우드에 접속
        
        userId.text = GetUserId();  //사용자 이름 설정

        //룸 이름을 무작위로 설정
        roomName.text = "Room_" + Random.Range(0, 999).ToString("000");
    }

```

## 2.랜덤방 입장  

https://user-images.githubusercontent.com/63942174/158361351-8a318f42-bbbd-47c3-8636-2c99131d8c59.mp4



## 3.볼륨조절  

https://user-images.githubusercontent.com/63942174/158361437-9871a9f5-b60e-4c03-8db9-059c4a164ae2.mp4


## 4.팀 이동 및 준비  

https://user-images.githubusercontent.com/63942174/158361475-0e5b83a3-28b5-4035-bcfd-41b239ba9bec.mp4


## 5.게임시작  

https://user-images.githubusercontent.com/63942174/158361547-473582dc-5ed9-4b3e-a020-986cfd3ce74c.mp4


## 6.게임중 나가기  

https://user-images.githubusercontent.com/63942174/158361614-af9bfcd3-866e-4320-8263-bab11f5ab0b9.mp4


## 7.적공격  

https://user-images.githubusercontent.com/63942174/158361758-0b3e8f61-7d3b-408e-a889-6ef53706b9a1.mp4


## 8.이동과 카메라 회전  

https://user-images.githubusercontent.com/63942174/158361799-9cb3bf1c-8fa2-49ba-9935-400b23727e87.mp4


## 9.미니맵 생성  

https://user-images.githubusercontent.com/63942174/158361902-0618f85d-ab83-44aa-b93c-ec21983daf28.mp4


## 10.승리  

https://user-images.githubusercontent.com/63942174/158362001-97c7fa5c-9de9-4452-8724-399135141cd5.mp4

