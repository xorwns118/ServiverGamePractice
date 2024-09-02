using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    public string closeWeaponName; // 근접 무기 이름

    // 무기 유형 
    public bool isHand;
    public bool isAxe;
    public bool isPickaxe;

    public float range; // 공격 범위 
    public int daamage; // 공격력
    public float workSpeed; // 작업 속도
    public float attackDelay; // 공격 딜레이 
    public float attackDelayA; // 공격 활성화 시점 (주먹을 휘두르는 모션을 취할때 데미지를 입힘)
    public float attackDelayB; // 공격 비활성화 시점 (주먹을 휘두르는 모션이 끝나면 데미지를 입히지 않음)

    public Animator anim; // 애니메이션 
}
