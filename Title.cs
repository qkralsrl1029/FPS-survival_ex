using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public static Title instance;
    SavenLoad theSave;

    private void Awake()        //싱글턴화
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }


    public void StartClick()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void LoadClick()
    {
        StartCoroutine(LoadCoroutine());
        
    }

    IEnumerator LoadCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("GameScene");        //로드를하기위한 플레이어가 다음 씬에 있으니 우선 씬 이동
        while (!operation.isDone)       //씬 로딩이 끝나지않았다면
            yield return null;          //대기
        theSave = FindObjectOfType<SavenLoad>();
        theSave.LoadData();                         //씬 이동 후에도 싱글턴화 했기 때문에 객체가 남아있어서 실행 가능
        this.gameObject.SetActive(false);
    }
    public void ExitClick()
    {
        Application.Quit();
    }
}
