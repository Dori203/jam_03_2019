using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory
{
    private Victory(string value) { Value = value; }

    public string Value { get; set; }

    public static Victory Extermination { get { return new Victory("EXTERMINATION VICTORY"); } }
    public static Victory Exploration { get { return new Victory("EXPLORATION VICTORY"); } }
    public static Victory Fishing { get { return new Victory("FISHING VICTORY"); } }
    public static Victory None { get { return new Victory(""); } }
}