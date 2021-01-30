using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LossConditions
{
    private LossConditions(string value) { Value = value; }

    public string Value { get; set; }

    public static LossConditions Extermination { get { return new LossConditions("YOU WERE OVERWHELMED BY MOSQUITOS"); } }
    public static LossConditions Exploration { get { return new LossConditions("YOU GOT CAUGH BY THE SHARK"); } }
    public static LossConditions Fishing { get { return new LossConditions("YOU FORGOT TO EAT"); } }
    public static LossConditions None { get { return new LossConditions(""); } }
}
