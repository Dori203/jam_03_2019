using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory
{
    private Victory(string value) { Value = value; }

    public string Value { get; set; }

    public static Victory Extermination { get { return new Victory("Extermination Victory!"); } }
    public static Victory Exploration { get { return new Victory("Exploration Victory!"); } }
    public static Victory Fishing { get { return new Victory("Fishing Victory!"); } }
    public static Victory None { get { return new Victory("None Victory!"); } }
}