# TankAttack_TeamVS
<h1>포톤 서버를 사용하는 실시간 팀 대전게임입니다.</h1>


https://user-images.githubusercontent.com/63942174/158009028-db73771c-60b2-451e-92df-ee2eece69623.mp4

### 어택구동을 위한 코드구현

``` C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    //포탄의 속도
    public float speed = 6000.0f;
    //폭발 효과 프리팹 연결 변수
    public GameObject expEffect;

    private CapsuleCollider _collider;
    private Rigidbody _rigidbody;

    //포탄을 발사한 플레이어의 ID 저장
    [HideInInspector] public int AttackerId = -1; //-1 이면 방 밖으로 나갔다는 뜻
    [HideInInspector] public string AttackerTeam = "blue"; //어느팀에서 쏜 총알인지?

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();

        GetComponent<Rigidbody>().AddForce(transform.forward * speed);

        //3초가 지난 후 자동 폭발하는 코루틴 실행
        StartCoroutine(this.ExplosionCannon(3.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider a_Col)
    {
        //지연 또는 적 탱크에 충돌한 경우 즉시 폭발하도록 코루틴 실행
        StartCoroutine(this.ExplosionCannon(0.0f));
    }

    IEnumerator ExplosionCannon(float tm)
    {
        yield return new WaitForSeconds(tm);

        //충돌 콜백 함수가 발생하지 zmff않도록 Collider를 비활성화
        if (_collider != null)
            _collider.enabled = false;
        //물리엔진의 영향을 받을 필요 없음
        //_rigidbody.isKinematic = true;
        if (_rigidbody != null)
            _rigidbody.velocity = Vector3.zero;

        //폭발 프리팹 동적 생성
        GameObject obj = (GameObject)Instantiate(expEffect,
            transform.position - (transform.forward * 9.0f),
            Quaternion.identity);

        Destroy(obj, 1.0f);

        //Trail Renderer가 소멸되기까지 잠시 대기 후 삭제 처리
        Destroy(this.gameObject, 1.0f);
    }
}

```

##### 이코드를 구현기위해 어저쩌구저쩌구 3줄이상 적지마

https://user-images.githubusercontent.com/63942174/158009037-f89d4479-681c-4ad2-ba7c-d7dad37e16a9.mp4



https://user-images.githubusercontent.com/63942174/158009039-0544b1ff-a517-4366-8940-bac2abd058d9.mp4



https://user-images.githubusercontent.com/63942174/158009047-5d57414b-86ff-4e49-bf21-8c76be54010b.mp4



https://user-images.githubusercontent.com/63942174/158009151-db954426-246e-41d1-94bf-97da24cf1ac8.mp4



https://user-images.githubusercontent.com/63942174/158009155-eda94d24-4a5a-4753-ae5c-a98548b435c0.mp4



https://user-images.githubusercontent.com/63942174/158009159-2b67ae94-18ff-42d9-8701-974a5ee699ea.mp4



https://user-images.githubusercontent.com/63942174/158009163-0d9fc372-713f-48f5-8556-d11025875dd5.mp4

