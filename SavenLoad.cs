using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;        //디렉토리 라이브러리

public class SavenLoad : MonoBehaviour
{
    SaveData saveData = new SaveData();

    string SAVE_DATA_DIRECTORY;     //저장 경로
    string SAVE_FILENAME = "/SaveFile.txt";  //파일 이름

    
    playerScript thePlayer;
    Inventory theInven;
    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";     //현재게임 폴더
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))                 //경로내에 디렉토리가없으면
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);         //새로 생성
    }

    public void SaveData()
    {
        thePlayer = FindObjectOfType<playerScript>();               //플레이어를 우선 참조하고
        theInven = FindObjectOfType<Inventory>();

        saveData.playerPos = thePlayer.transform.position;          //세이브데이타에 저장
        saveData.playerRot = thePlayer.transform.eulerAngles;

        Slot[] slots = theInven.GetSlot();                         //인벤토리에서 슬롯들 정보 받아와서
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item!=null)                                //비어있지 않은 슬롯들 정보 저장
            {
                saveData.inventoryArrayNum.Add(i);
                saveData.inventoryItem.Add(slots[i].item.itemName);
                saveData.inventoryItemNum.Add(slots[i].itemCount);
            }
        }

        string json = JsonUtility.ToJson(saveData);                 //데이터 저장 클래스의 데이터들을 제이슨화
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);   //기존 지정 디렉토리에 제이슨화 되었던 정보들을 기록(물리적인 저장)

        Debug.Log("저장완료");
    }

    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))       //저장된 데이터가 있는 상태에서만 실행
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);        //디렉토리 경로에있는 정보를  제이슨에 저장
            saveData = JsonUtility.FromJson<SaveData>(loadJson);    //역순으로 제이슨화된 정보들을 세이브데이터에 저장

            thePlayer = FindObjectOfType<playerScript>();           //플레이어를 찾아서
            theInven = FindObjectOfType<Inventory>();
            thePlayer.transform.position = saveData.playerPos;      //저장된 위치로 저장
            thePlayer.transform.eulerAngles = saveData.playerRot;

            for (int i = 0; i < saveData.inventoryItemNum.Count; i++)
            {
                theInven.LoadToInven(saveData.inventoryArrayNum[i], saveData.inventoryItem[i], saveData.inventoryItemNum[i]);
            }
            Debug.Log("로드완료");
        }
    }

}

[System.Serializable]       //데이터 직렬화(저장장치에 읽고쓰기 쉬움)
public class SaveData       //데이터를 저장할 클래스
{
    //로드할 데이터들 저장
    public Vector3 playerPos;       //플레이어 위치
    public Vector3 playerRot;       //플레이어 방향

    //플레이어가 소유하고있던 아이템들, 슬롯 자체는 직렬화가 안됨, 하나하나해줘야함
    public List<int> inventoryArrayNum = new List<int>();
    public List<string> inventoryItem = new List<string>();
    public List<int> inventoryItemNum = new List<int>();
}
