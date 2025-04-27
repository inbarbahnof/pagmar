using Unity.VisualScripting;
using UnityEngine;
using System;


[UnitTitle("On Trigger Enter 2D")]
[TypeIcon(typeof(Unity.VisualScripting.OnTriggerEnter2D))]
public sealed class OnTriggerEnter2D : TriggerEvent2DUnit
{
    
    public override Type MessageListenerType => typeof(UnityOnTriggerEnter2DMessageListener);
    protected override string hookName => EventHooks.OnTriggerEnter2D;
    
    // [DoNotSerialize]
    // public ValueOutput name { get; private set; }
    //
    // [DoNotSerialize]
    // public ValueOutput tag { get; private set; }
    //
    // protected override void Definition()
    // {
    //     base.Definition();
    //
    //     name = ValueOutput<string>("name");
    //     tag = ValueOutput<string>("tag");
    // }
    //
    // protected override void AssignArguments(Flow flow, Collider2D other)
    // {
    //     flow.SetValue(name, other.name);
    //     flow.SetValue(tag, other.tag);
    // }
    
}
