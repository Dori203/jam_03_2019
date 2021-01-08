using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkScript : MonoBehaviour
{
    [SerializeField] Sprite iconSprite;
    [SerializeField] private float iconScale;
    private SpriteRenderer iconRenderer;
    private GameObject icon;
    private Vector3 iconlocalRotation;
    private void Start()
    {
        icon = new GameObject("rock Mark");
        icon.transform.position = transform.position;
        icon.transform.Rotate(new Vector3(90f,0f,0f));
        icon.transform.localScale = new Vector3(iconScale, iconScale, iconScale);
        icon.layer = LayerMask.NameToLayer("Map");
        iconRenderer = icon.AddComponent<SpriteRenderer>();
        iconRenderer.sprite = iconSprite;
        icon.transform.SetParent(this.transform);
    }
}