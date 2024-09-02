using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField] private float secondPerRealTimeSecound; // EX) 게임시간 100초 = 현실시간 1초 

    [SerializeField] private float fogDensityCalc; // 증감량 비율 

    [SerializeField] private float nightFogDensity; // 밤 상태의 Fog 밀도 
    private float dayFogDensity; // 낮 상태의 Fog 밀도 
    private float currentFogDensity; // 계산 

    // Start is called before the first frame update
    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
        currentFogDensity = dayFogDensity;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecound * Time.deltaTime);

        if (transform.eulerAngles.x >= 170)
            GameManager.isNight = true;
        else if (transform.eulerAngles.x >= -10) // unity에서는 eulerAngles값이 -180 , 180 의 범위를 가지고 있음 
            GameManager.isNight = false;                     // -10 || 350 둘다 사용 가능 350으로 입력 시 -10으로 적용 

        if (GameManager.isNight)
        {
            if(currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else
        {
            if (currentFogDensity >= dayFogDensity)
            {
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
    }
}
