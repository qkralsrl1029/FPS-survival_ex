using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadMe : MonoBehaviour
{
   /*
    * 메인 카메라 하나만 사용할 경우, 플레이어의 시점에 따라 들고있는 무기가 안보이는 버그가 발생,
    * 따라서 무기의 레이어를 따로 만들고 무기만 볼수있는 카메라를 따로 생성. 그리고 메인 카메라는 무기만 볼수없게
    * Culling Mask를 설정하고 이를 합쳐서 보여주면 무기가 온전히 나오는 모습을 볼수 있음.
    */
    
}
