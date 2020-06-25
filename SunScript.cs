using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunScript : MonoBehaviour
{
    [SerializeField] float secondPerRealTime;   //상대시간
    bool isNight = false;
    [SerializeField] float fogDensityCalc;      //증감량
    [SerializeField] float nightFogDensity;     //밤의 최대안개량
    float dayFogDensity;                        //낮의 기본 안개량
    float currentFogDensity=0.01f;                    //현재 안개량
    // Start is called before the first frame update
    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right,0.1f*secondPerRealTime*Time.deltaTime);

        if (transform.eulerAngles.x >= 170)
            isNight = true;
        else if (transform.eulerAngles.x <= 10)
            isNight = false;

        if(isNight&&currentFogDensity<=nightFogDensity)
        {
            currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
            RenderSettings.fogDensity = currentFogDensity;
        }
        else if(!isNight&&currentFogDensity>=dayFogDensity)
        {
            currentFogDensity-= 0.1f * fogDensityCalc * Time.deltaTime;
            RenderSettings.fogDensity = currentFogDensity;
        }
    }
}
