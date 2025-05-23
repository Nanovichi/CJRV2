using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image UiFill;
    [SerializeField] private TextMeshProUGUI text;

    public int duration; // Total time to count up to

    private int currentTime;

    private void Start()
    {
        Begin();
    }

    private void Begin()
    {
        currentTime = 0;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while ( true)
        {
            text.text = $"{currentTime / 60:00}:{currentTime % 60:00}";
            
            currentTime++;
            yield return new WaitForSeconds(1f);
        }

        
    }

   
}
