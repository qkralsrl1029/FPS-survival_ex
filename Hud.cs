using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    [SerializeField] GunController theGunController;
    GunScript currentGun;

    [SerializeField] GameObject goBulletHUD;        //필요시 hud호출
    [SerializeField] Text[] bulletText;


    // Update is called once per frame
    void Update()
    {
        checkBullet();
    }

    void checkBullet()
    {
        currentGun = theGunController.GetGun();
        bulletText[0].text = currentGun.carryBulletCount.ToString();
        bulletText[1].text = currentGun.reloadBulletCount.ToString();
        bulletText[2].text = currentGun.currentBulletCount.ToString();
    }
}
