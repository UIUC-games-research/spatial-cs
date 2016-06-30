using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextRainbowWave : MonoBehaviour
{
    float timeOffset = 0f;
    float waveSpeed = 4f;
    float offsetFixer = 8f;
    Image imageComp;

    // Use this for initialization
    void Start()
    {
        imageComp = GetComponent<Image>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Need to add 8 to the y position (Half of the height.)
        //transform.localPosition = new Vector2(initialPos.x, initialPos.y + offsetFixer + (wavePower * Mathf.Sin(waveSpeed * (Time.time + timeOffset))));
        //transform.localScale = new Vector2(transform.localScale.x * 1.01f, transform.localScale.y * 1.01f);
        float colorR = Mathf.Pow(Mathf.Sin(waveSpeed * (Time.time + timeOffset)), 2);
        float colorG = Mathf.Pow(Mathf.Sin(waveSpeed * (Time.time + timeOffset + 2.09f)), 2);
        float colorB = Mathf.Pow(Mathf.Sin(waveSpeed * (Time.time + timeOffset + 4.188f)), 2);
        imageComp.color = new Color(colorR, colorG, colorB);
    }

    public void SetVars(float timeOffset_, float waveSpeed_)
    {
        timeOffset = timeOffset_;
        waveSpeed = waveSpeed_;
    }

}
