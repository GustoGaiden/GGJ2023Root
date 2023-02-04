using UnityEngine;
using UnityEngine.UI;


public class JuiceManager : MonoBehaviour
{
    public Slider JuiceUiSlider;
    public float CurrentJuice;
    public float MaxJuice;

    void Update()
    {
        JuiceUiSlider.value = CurrentJuice;
    }

    public void DepleteJuice(float value)
    {
        CurrentJuice = Mathf.Max(CurrentJuice - value, 0f);
    }

    public bool isEmpty()
    {
        return CurrentJuice <= 0;
    }

    public void resetToMax(float newMaxValue)
    {
        MaxJuice = newMaxValue;
        CurrentJuice = MaxJuice;
        JuiceUiSlider.maxValue = MaxJuice;
        JuiceUiSlider.value = MaxJuice;
    }
}
