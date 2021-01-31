using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private UnityEngine.UI.Image image;
    private float fadeSpeed = 0.7f;
    void Start()
    {
        image = transform.GetComponentInChildren<UnityEngine.UI.Image>();
    }

    public void Respawn()
    {
        StartCoroutine(FadeBetween());
    }

    IEnumerator FadeBetween()
    {
        while (image.color.a < 1)
        {
            float alpha = image.color.a + Time.fixedDeltaTime * fadeSpeed;
            image.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(1);
        while (image.color.a > 0)
        {
            float alpha = image.color.a - Time.fixedDeltaTime * fadeSpeed;
            image.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

    }
    
}
