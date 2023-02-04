using UnityEngine;
using UnityEngine.UI;


public class JuiceManager : MonoBehaviour
{
    public Slider JuiceUiSlider;
    public float CurrentJuice;
    public float MaxJuice;
    
    private void Awake()
    {
        resetToMax(100f);
    }
    
    void Update()
    {
        JuiceUiSlider.value = CurrentJuice;
    }

    public void DepleteJuice(float depleteValue)
    {
        CurrentJuice -= depleteValue;
        CurrentJuice = Mathf.Max(CurrentJuice -= depleteValue, 0f);
    }

    public bool isEmpty()
    {
        return CurrentJuice <= 0;
    }

    public void resetToMax(float newMaxValue)
    {
        MaxJuice = newMaxValue;
        JuiceUiSlider.maxValue = MaxJuice;
        JuiceUiSlider.value = MaxJuice;
    }
}
