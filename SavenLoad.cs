using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SavenLoad : MonoBehaviour
{
    SaveData saveData = new SaveData();

    string SAVE_DATA_DIRECTORY;
    string SAVE_FILENAME = "/SaveFle.txt";

    
    playerScript thePlayer;
    Inventory theInven;
    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY); 
    }

    public void SaveData()
    {
        thePlayer = FindObjectOfType<playerScript>();
        theInven = FindObjectOfType<Inventory>();

        saveData.playerPos = thePlayer.transform.position;
        saveData.playerRot = thePlayer.transform.eulerAngles;

        Slot[] slots = theInven.GetSlot();
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item!=null)
            {
                saveData.inventoryArrayNum.Add(i);
                saveData.inventoryItem.Add(slots[i].item.itemName);
                saveData.inventoryItemNum.Add(slots[i].itemCount);
            }
        }

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);

        Debug.Log("저장완료");
    }

    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            thePlayer = FindObjectOfType<playerScript>();
            theInven = FindObjectOfType<Inventory>();
            thePlayer.transform.position = saveData.playerPos;
            thePlayer.transform.eulerAngles = saveData.playerRot;

            for (int i = 0; i < saveData.inventoryItemNum.Count; i++)
            {
                theInven.LoadToInven(saveData.inventoryArrayNum[i], saveData.inventoryItem[i], saveData.inventoryItemNum[i]);
            }
            Debug.Log("로드완료");
        }
    }

}

[System.Serializable]       //데이터 직렬화
public class SaveData
{
    //로드할 데이터들 저장
    public Vector3 playerPos;
    public Vector3 playerRot;

    public List<int> inventoryArrayNum = new List<int>();
    public List<string> inventoryItem = new List<string>();
    public List<int> inventoryItemNum = new List<int>();
}
