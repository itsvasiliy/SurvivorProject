using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Number_IncreaseDecrease : MonoBehaviour
{
    [SerializeField] private TMP_Text numberTextMeshPro;

    [SerializeField] private int minNumber;
    [SerializeField] private int maxNumber;

    [SerializeField] private int currentNumber;

    private void Start()
    {
        numberTextMeshPro.text = currentNumber.ToString();
    }

    public void IncreaseNumber()
    {
        if(currentNumber < maxNumber)
        {
            currentNumber++;
            numberTextMeshPro.text = currentNumber.ToString();
        }
    }

    public void DecreaseNumber()
    {
        if (currentNumber > minNumber)
        {
            currentNumber--;
            numberTextMeshPro.text = currentNumber.ToString();
        }
    }
}
