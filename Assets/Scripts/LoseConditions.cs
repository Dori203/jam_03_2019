using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LossConditions
{
    private LossConditions(string value) { Value = value; }

    public string Value { get; set; }

    public static LossConditions Extermination { get { return new LossConditions("Your were overwhlemed by mosquitoes!"); } }
    public static LossConditions Exploration { get { return new LossConditions("You got caught by the shark!"); } }
    public static LossConditions Fishing { get { return new LossConditions("You forget to eat!"); } }
    public static LossConditions None { get { return new LossConditions("None Loss. "); } }
}
