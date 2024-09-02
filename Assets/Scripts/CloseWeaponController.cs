using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 미완성 클래스 = 추상 클래스 (컴포넌트 추가 불가능, 자식 객체로 완성 시켜야 함)
public abstract class CloseWeaponController : MonoBehaviour
{
    // 현재 장착된 Hand형 무기 
    [SerializeField]
    protected CloseWeapon currenntCloseWeapon; // protected : 상속받은 스크립트에서만 사용 가능

    // 공격중
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;
    [SerializeField]
    protected LayerMask layerMask;

    protected void TryAttack()
    {
        if (!Inventroy.inventoryActivated)
        {
            if (Input.GetButton("Fire1"))
            {
                if (!isAttack)
                {
                    StartCoroutine(AttackCoroutine());
                }
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currenntCloseWeapon.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currenntCloseWeapon.attackDelayA);
        isSwing = true;

        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currenntCloseWeapon.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currenntCloseWeapon.attackDelay
                                        - currenntCloseWeapon.attackDelayA - currenntCloseWeapon.attackDelayB);

        isAttack = false;
    }

    // abstract : 미완성 , 추상 코루틴으로 남김 (자식 클래스가 완성 시켜야 함)
    // interface : Multi-abstract 로 이해
    protected abstract IEnumerator HitCoroutine();

    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position,
                           transform.forward/* == transform.TransformDirection(Vector3.forward) */,
                           out hitInfo, currenntCloseWeapon.range, layerMask))
        {
            return true;
        }
        return false;
    }

    // 완성 함수이지만 추가 편집이 가능한 함수 
    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currenntCloseWeapon = _closeWeapon;
        WeaponManager.currentWeapon = currenntCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currenntCloseWeapon.anim;

        currenntCloseWeapon.transform.localPosition = Vector3.zero;
        currenntCloseWeapon.gameObject.SetActive(true);
    }
}
