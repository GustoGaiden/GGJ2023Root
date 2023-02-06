using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject CameraTop;

    public GameObject CameraBottom;
    public GameObject CameraReset;

    public GameObject CameraContainer;

    private GameObject CamTarget;

    public AnimationCurve curve;

    public float totalAnimTimeDig = 500f;
    public float totalAnimTimeReset = 2000f;
    
    private float currentAnimTime = 0.0f;
    private float totalAnimTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        ResetCameraToTop();
    }

    public void ResetCameraToTop()
    {
        CamTarget = CameraTop;
        totalAnimTime = totalAnimTimeReset;
        CameraContainer.transform.position = CameraReset.transform.position;
    }

    public void SetCameraToDig()
    {
        CamTarget = CameraBottom;
        totalAnimTime = totalAnimTimeDig;
        currentAnimTime = 0f;
    }

    public void Update()
    {
        if (currentAnimTime <= totalAnimTime)
        {
            currentAnimTime = Mathf.Clamp(currentAnimTime + Time.deltaTime, 0, totalAnimTime);
            float normalizedProgress = Mathf.Clamp01(currentAnimTime / totalAnimTime) ; // 0-1
            CameraContainer.transform.position =
                Vector3.Lerp(CameraContainer.transform.position, CamTarget.transform.position, normalizedProgress);    
        }
        
    }
}
