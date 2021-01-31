using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Helpers;

public class ClampIcon : ListeningMonoBehaviour
{
    protected override List<BaseListener> Listeners => new List<BaseListener>() {
        new BaseListener<int> {Event = GameManager.Channels.NewFishCaught.GetPath(), Callback = changeIcon}
    };
    public Image image;

    public Sprite image1;
    public Sprite image2;

    public int fishType;

    private Canvas canvas;
    public string canvasName;

    private Camera camera;
    public string cameraName;

    private float distance;
    public float viewRange;

    public bool limitView;

    // Start is called before the first frame update
    void Awake()
    {
        camera = GameObject.Find(cameraName).GetComponent<Camera>();
        canvas = GameObject.Find(canvasName).GetComponent<Canvas>();
        image = Instantiate(image, canvas.transform);
        image.GetComponent<Image>().sprite = image1;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 iconPos = camera.WorldToScreenPoint(this.transform.position);
        iconPos.x *= canvas.pixelRect.width / (float)camera.pixelWidth;
        iconPos.y *= canvas.pixelRect.height / (float)camera.pixelHeight;

        image.rectTransform.anchoredPosition = iconPos - canvas.GetComponent<RectTransform>().sizeDelta / 2f;

        distance = Vector3.Distance(transform.position, camera.transform.position);

        if (limitView)
        {
            if (distance > viewRange) // Check if the object is within view range
            {
                image.enabled = false;
            }
            else
            {
                var relativePoint = camera.transform.InverseTransformPoint(transform.position);
                if (relativePoint.z < 0.0f) // Check if the object is behind the camera
                {
                    image.enabled = false;

                }
                else
                {
                    image.enabled = true;
                }
            }
        }
        else
        {
            image.enabled = true;
        }
    }

    private void changeIcon(int newFishType) {
        if (fishType == newFishType) {
            image.GetComponent<Image>().sprite = image2;
        }
    }
}
