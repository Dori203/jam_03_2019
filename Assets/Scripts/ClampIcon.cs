using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClampIcon : MonoBehaviour
{

    public Image image;

    private Canvas canvas;
    public string canvasName;

    private Camera camera;
    public string cameraName;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find(cameraName).GetComponent<Camera>();
        canvas = GameObject.Find(canvasName).GetComponent<Canvas>();
        image = Instantiate(image, canvas.transform);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 iconPos = camera.WorldToScreenPoint(this.transform.position);
        iconPos.x *= canvas.pixelRect.width / (float)camera.pixelWidth;
        iconPos.y *= canvas.pixelRect.height / (float)camera.pixelHeight;

        image.rectTransform.anchoredPosition = iconPos - canvas.GetComponent<RectTransform>().sizeDelta / 2f;
    }
}
