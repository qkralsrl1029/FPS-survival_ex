using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    public static DragSlot instance;        //자기자신 인스턴스화, static값으로 저장
    public Slot dragSlot;

    [SerializeField] Image itemImage;

    private void Start()
    {
        instance=this;
    }

    public void dragSetImage(Image _image)
    {
        itemImage.sprite = _image.sprite;   //이미지 그자체의 형식으로는 대입 불가
        SetColor(1);
    }

    public void SetColor(float alpha)
    {
        Color color=itemImage.color;
        color.a = alpha;
        itemImage.color = color;

    }
}
