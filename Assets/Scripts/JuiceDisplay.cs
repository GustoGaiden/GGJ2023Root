using System;
using UnityEngine;
using UnityEngine.UI;

public class JuiceDisplay : MonoBehaviour
{
    private Slider juiceUiSlider;
    
    public GameObject LightMarkPrefab;
    public GameObject DarkMarkPrefab;
    public GameObject MarkerContainer;

    public float LightMarkInterval = 10; // Every X MaxJuice, make a light Mark.
    public float DarkMarkInterval = 10; // Every X Light marks, make a Dark Mark instead.

    public void Start()
    {
        juiceUiSlider = gameObject.GetComponent<Slider>();
    }

    public void Update()
    {
        juiceUiSlider.value = Juice.CurrentJuice;
    }

    public void ResetToMax()
    {
        if (juiceUiSlider == null)
        {
            juiceUiSlider = gameObject.GetComponent<Slider>();
        }
        
        juiceUiSlider.maxValue = Juice.MaxJuice;
        juiceUiSlider.value = Juice.MaxJuice;
                foreach (Transform trans in MarkerContainer.transform)
        {
            Destroy(trans.gameObject);
        }

        float numMarks = Juice.MaxJuice / LightMarkInterval;
        int lightCount = 0;
        float markSpacing = juiceUiSlider.fillRect.rect.height / numMarks;

        for (int i = 0; i < numMarks; i++)
        {
            bool isDarkMark = lightCount >= DarkMarkInterval;

            lightCount += isDarkMark ? -lightCount : 1;
            GameObject prefab = isDarkMark ? DarkMarkPrefab : LightMarkPrefab;
            Vector2 delta = isDarkMark ? new Vector2(0, 2) : new Vector2(-5, 1);

            GameObject newMark = Instantiate(prefab);
            newMark.transform.SetParent(MarkerContainer.transform);
            RectTransform newRectTrans = newMark.GetComponent<RectTransform>();
            newRectTrans.anchoredPosition = new Vector2(0, i * markSpacing);
            newRectTrans.sizeDelta = delta;

        }
    }
}