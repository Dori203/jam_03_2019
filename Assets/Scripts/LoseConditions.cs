using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LossConditions
{
    private LossConditions(string value) { Value = value; }

    public string Value { get; set; }

    public static LossConditions Extermination { get { return new LossConditions("Extermination Lose, your blood was drained by mosquitoes."); } }
    public static LossConditions Exploration { get { return new LossConditions("Exploration Lose, you have a narrow horizon."); } }
    public static LossConditions Fishing { get { return new LossConditions("Fishing Loss, you ate a toxic fish. RIP!"); } }
    public static LossConditions None { get { return new LossConditions("None Loss. "); } }
}
