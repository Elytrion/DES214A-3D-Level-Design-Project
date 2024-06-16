using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleBlackBars : MonoBehaviour
{
    float targetSize;
    float changeAmount;
    RectTransform top;
    RectTransform bottom;
    private bool isActive = false;
    private void Awake()
    {
        GameObject go = new GameObject("Top", typeof(Image));
        go.transform.SetParent(transform, false);
        go.GetComponent<Image>().color = Color.black;
        top = go.GetComponent<RectTransform>();
        top.anchorMin = new Vector2(0, 1);
        top.anchorMax = new Vector2(1, 1);
        top.sizeDelta = new Vector2(0, 0);

        go = new GameObject("Bottom", typeof(Image));
        go.transform.SetParent(transform, false);
        go.GetComponent<Image>().color = Color.black;
        bottom = go.GetComponent<RectTransform>();
        bottom.anchorMin = new Vector2(0, 0);
        bottom.anchorMax = new Vector2(1, 0);
        bottom.sizeDelta = new Vector2(0, 0);
    }

    public void show(float targetSz, float time)
    {
        this.targetSize = targetSz;
        changeAmount = (targetSize - top.sizeDelta.y) / time;
        isActive = true;
    }

    public void hide(float time)
    {
        this.targetSize = 0.0f;
        changeAmount = (targetSize - top.sizeDelta.y) / time;
        isActive = true;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            Vector2 szdelta = top.sizeDelta;
            szdelta.y += changeAmount * Time.deltaTime;
            if (changeAmount > 0)
            {
                if (szdelta.y >= targetSize)
                {
                    szdelta.y = targetSize;
                    isActive = false;
                }
            }
            else
            {
                if (szdelta.y <= targetSize)
                {
                    szdelta.y = targetSize;
                    isActive = false;
                }
            }
            top.sizeDelta = szdelta;
            bottom.sizeDelta = szdelta;
        }

    }

}
