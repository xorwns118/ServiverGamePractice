using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true; // 플레이어의 움직임 제어

    public static bool isOpenInventory = false; // 인벤토리 활성화
    public static bool isOpenCraftManual = false; // 건축 메뉴창 활성화 

    public static bool isNight = false;
    public static bool isWater = false;

    public static bool isPause = false; // 메뉴 호출 시 true

    private WeaponManager theWM;
    private bool flag = false;

    // Update is called once per frame
    void Update()
    {
        if (isOpenInventory || isOpenCraftManual || isPause)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            canPlayerMove = false;
        }
        else
        {
            canPlayerMove = true;
        }

        if (isWater)
        {
            if (!flag)
            {
                StartCoroutine(theWM.WeaponInCoroutine());
                flag = true;
            }
        }
        else
        {
            if (flag)
            {
                theWM.WeaponOut();
                flag = false;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 커서를 잠금 
        Cursor.visible = false; // 커서를 숨김
        theWM = FindObjectOfType<WeaponManager>();
    }

    
}
