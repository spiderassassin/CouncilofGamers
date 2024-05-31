using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElement : MonoBehaviour
{
    Vector3 orig;
    private void Start()
    {
        orig = GetComponent<RectTransform>().localScale;
    }

    public void SetOriginalSize()
    {
        GetComponent<RectTransform>().localScale = new Vector3(orig.x, orig.y, orig.z);
    }
    public IEnumerator SizeFlash(float multiplier)   //1.5 for two, 1.2 for one
    {
        Debug.Log("-");
        
        /*
        Vector3 big = new Vector3((float)(orig.x * multiplier), (float)(orig.y * multiplier), (float)(orig.z * multiplier));
        

        for (int i = 0; i < 5; i++)
        {

            GetComponent<RectTransform>().localScale = big;

            yield return new WaitForSeconds(0.25f);
            GetComponent<RectTransform>().localScale = orig;

            yield return new WaitForSeconds(0.25f);
        }
        */

        float pt = 0;
        float newVal = 1;
        float pulseSpeed = 12f;
        float minAlpha = 1f;
        float maxAlpha = multiplier;

        float timer = 0;

        while (timer < 1.5f)
        {
            timer += Time.deltaTime;
            pt += pulseSpeed * Time.deltaTime;
            if (pt >= 6.28f)
            {
                pt = 0;
            }
            newVal = ((Mathf.Sin(pt) + 1) * (maxAlpha - minAlpha)) / 2 + minAlpha;
            GetComponent<RectTransform>().localScale = new Vector3((float)(orig.x * newVal), (float)(orig.y * newVal), (float)(orig.z * newVal));
            yield return null;
            
        }

        while (newVal > 1)
        {
            newVal -= 0.05f;
            GetComponent<RectTransform>().localScale = new Vector3(orig.x * newVal, orig.y * newVal, orig.z * newVal);
        }
        GetComponent<RectTransform>().localScale = new Vector3(orig.x, orig.y, orig.z);
        yield return null;




    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            GetComponent<RectTransform>().localPosition = new Vector2(x, y);
            yield return null;

        }
    }
    /*
        public IEnumerator Flash(GameObject bar)
        {
            Vector3 bar_orig = bar.GetComponent<RectTransform>().localScale;
            Vector3 bar_big = new Vector3((float)(bar_orig.x * 1.2), (float)(bar_orig.y * 1.2), (float)(bar_orig.z * 1.2));
            for (int i = 0; i < 5; i++)
            {
                bar.GetComponent<RectTransform>().localScale = bar_big;
                yield return new WaitForSeconds(0.25f);
                bar.GetComponent<RectTransform>().localScale = bar_orig;
                yield return new WaitForSeconds(0.25f);
            }
        }
    */



}
