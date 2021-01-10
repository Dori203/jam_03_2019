using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ExampleScript : MonoBehaviour
{
    [FilePath(Extensions = ".unity")]
    public string ScenePath;

    [Button(ButtonSizes.Large)]
    public void SayHello()
    {
        Debug.Log("Hello button!");
    }
    
    // You can hide fields with the HideInInspector attribute, and show fields and properties with the ShowInInspector attribute.
    
    [HideInInspector]
    public int NormallyVisible;

    [ShowInInspector]
    private bool normallyHidden;

    [ShowInInspector]
    public ScriptableObject Property { get; set; }
    
    // // You can combine multiple attributes, and change the order with the PropertyOrder attribute.
    //
    // [PreviewField, Required, AssetsOnly]
    // public GameObject Prefab;
    //
    // [HideLabel, Required, PropertyOrder(-5)]
    // public string Name { get; set; }
    //
    // [Button(ButtonSizes.Medium), PropertyOrder(-3)]
    // public void RandomName()
    // {
    //     this.Name = Guid.NewGuid().ToString();
    // }
    
    // You can use group attributes to completely change the layout of your properties. See the How to design with group attributes guide for a more in-depth guide on how you can combine your groups.
    
    [HorizontalGroup("Split", Width = 50), HideLabel, PreviewField(50)]
    public Texture2D Icon;

    [VerticalGroup("Split/Properties")]
    public string MinionName;

    [VerticalGroup("Split/Properties")]
    public float Health;

    [VerticalGroup("Split/Properties")]
    public float Damage;
    
    // And many attributes let you reference other fields, properties or methods to extend the inspector with custom behaviour for your use-cases.
    
    [LabelText("$IAmLabel")]
    public string IAmLabel;

    [ListDrawerSettings(
        CustomAddFunction = "CreateNewGUID",
        CustomRemoveIndexFunction = "RemoveGUID")]
    public List<string> GuidList;

    private string CreateNewGUID()
    {
        return Guid.NewGuid().ToString();
    }

    private void RemoveGUID(int index)
    {
        this.GuidList.RemoveAt(index);
    }
}
