using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeadBlink : MonoBehaviour
{
    public Image panelImage;
    public float speed = 2f;

    private bool isBlinking = false;

    public void StartBlink()
    {
        isBlinking = true;
        StartCoroutine(Blink());
    }

    public void StopBlink()
    {
        panelImage.color = new Color(255, 0, 0, 0);
        isBlinking = false;
    }

    IEnumerator Blink()
    {
        while (isBlinking)
        {
            float alpha = Mathf.PingPong(Time.time * speed, 1f);
            panelImage.color = new Color(255, 0, 0, alpha);
            yield return null;
        }
    }
}
