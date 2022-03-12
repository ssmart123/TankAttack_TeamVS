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
