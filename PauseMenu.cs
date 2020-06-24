using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject go_Base;
    [SerializeField] SavenLoad theSave;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameManager.isPause)
                CallMenu();
            else
                CloseMenu();
        }
    }

    void CallMenu()
    {
        GameManager.isPause = true;
        go_Base.gameObject.SetActive(true);
        Time.timeScale = 0f;        //일시정지창 실행시 타임스케일 0, 시간흐름 정지
    }

    void CloseMenu()
    {
        GameManager.isPause = false;
        go_Base.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ClickSave()
    {
        theSave.SaveData();
    }
    public void ClickLoad()
    {
        theSave.LoadData();
    }
    public void ClickExit()
    {
        Application.Quit();     //게임종료
    }

}
