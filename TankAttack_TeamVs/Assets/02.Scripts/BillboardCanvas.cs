using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardCanvas : MonoBehaviour
{
    private Transform tr;
    private Transform mainCameraTr;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        //스테이지에 있는 메인 카메라의 Transform 컴포넌트를 추출
        mainCameraTr = Camera.main.transform;
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    tr.LookAt(mainCameraTr); //절대강좌 교재에서...
    //}

    void LateUpdate() //이쪽에서 계산해 줘야 떨리지 않고 Hp바가 캐릭터를 따라다닌다.
    {
        ////------------------------------------- Hp Bar Billboard 방식 
        tr.forward = mainCameraTr.forward;  //Quad로 만든 평면을 빌보드 하는 방법 
        ////------------------------------------- Hp Bar Billboard 방식 
    }
}
