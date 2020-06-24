using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canMove = true;
    public static bool inventoryOpen = false;       //인벤토리창 오픈
    public static bool isPause = false;             //일시정지창 오픈
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;       //커서 전체 잠그기
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inventoryOpen||isPause) 

        {
            Cursor.lockState = CursorLockMode.None;       
            Cursor.visible = true;
            canMove = !inventoryOpen;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;       //커서 전체 잠그기
            Cursor.visible = false;
            canMove = !inventoryOpen;
        }
    }
}