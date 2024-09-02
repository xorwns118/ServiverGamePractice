using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.PackageManager;
using UnityEngine;

public class PickAxeController : CloseWeaponController
{
    // 활성화 여부 
    public static bool isActive = true;

    private void Start()
    {
        WeaponManager.currentWeapon = currenntCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currenntCloseWeapon.anim;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
            TryAttack();
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                if (hitInfo.transform.tag == "Rock")
                    hitInfo.transform.GetComponent<Rock>().Mining();
                else if (hitInfo.transform.tag == "WeekAnimal")
                {
                    SoundManager.instance.PlaySE("Animal_Hit");
                    hitInfo.transform.GetComponent<WeekAnimal>().Damage(1, transform.position);
                }
                /*else if (hitInfo.transform.tag == "StrongAnimal")
                {
                    SoundManager.instance.PlaySE("Animal_Hit");
                    hitInfo.transform.GetComponent<WeekAnimal>().Damage(1, transform.position);
                }*/
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActive = true;
    }
}
