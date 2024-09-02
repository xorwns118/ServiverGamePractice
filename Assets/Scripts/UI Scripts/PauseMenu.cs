using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject go_BaseUI;
    [SerializeField] private SaveNLoad theSaveNLoad;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!GameManager.isPause)
            {
                CallMenu();
            }
            else
            {
                CloseMenu();
            }
        }
    }

    private void CallMenu()
    {
        GameManager.isPause = true;
        go_BaseUI.SetActive(true);
        Time.timeScale = 0; // 시간흐름 = 0
    }

    private void CloseMenu()
    {
        GameManager.isPause = false;
        go_BaseUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void ClickSave()
    {
        Debug.Log("저장됨");
        theSaveNLoad.SaveData();
    }

    public void ClickLoad()
    {
        theSaveNLoad.LoadData();
        Debug.Log("불러옴");
    }

    public void ClickExit()
    {
        Debug.Log("종료됨");
        Application.Quit();
    }
}
