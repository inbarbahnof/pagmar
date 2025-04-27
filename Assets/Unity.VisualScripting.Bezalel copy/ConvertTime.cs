using Unity.VisualScripting;
using UnityEngine;


[UnitCategory("Bezalel/Time")]
[UnitTitle("Format Time")]
[UnitShortTitle("Format Time")]
[TypeIcon(typeof(Time))]
public sealed class ConvertTime : Unit
{

    [DoNotSerialize]
    [PortLabelHidden]
    public ValueInput valueTotalSeconds;

    [DoNotSerialize]
    public ValueOutput valueSeconds;
    
    [DoNotSerialize]
    public ValueOutput valueMinutes;
    
    [DoNotSerialize]
    public ValueOutput valueHours;


    protected override void Definition()
    {
        valueTotalSeconds = ValueInput<float>("Time", 0);

        valueSeconds = ValueOutput<int>("Seconds", ReturnSeconds);
        valueMinutes = ValueOutput<int>("Minutes", ReturnMinutes);
        valueHours = ValueOutput<int>("Hours", ReturnHours);
    }

    int ReturnSeconds(Flow flow)
    {
        float totalSeconds = flow.GetValue<float>(valueTotalSeconds);
        int seconds = Mathf.FloorToInt(totalSeconds % 60);
        
        return seconds;
    }
    
    int ReturnMinutes(Flow flow)
    {
        float totalSeconds = flow.GetValue<float>(valueTotalSeconds);
        int minutes = Mathf.FloorToInt((totalSeconds/60) % 60);
        
        return minutes;
    }

    int ReturnHours(Flow flow)
    {
        float totalSeconds = flow.GetValue<float>(valueTotalSeconds);
        int hours = Mathf.FloorToInt(totalSeconds/3600);
        
        return hours;
    }
}