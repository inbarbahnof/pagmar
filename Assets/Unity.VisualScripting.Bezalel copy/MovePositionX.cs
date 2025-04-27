using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


[UnitCategory("Bezalel/Physics")]
[UnitSurtitle("Rigidbody 2D")]
[UnitTitle("Move Position X")]
[UnitShortTitle("Move Position X")]
[TypeIcon(typeof(Rigidbody2D))]
public sealed class MovePositionX : Unit
{

    [DoNotSerialize]
    [PortLabelHidden]
    public ControlInput enter { get; private set; }
    
    [DoNotSerialize]
    [PortLabelHidden]
    [NullMeansSelf]
    public ValueInput valueRigidBody2D; 
    
    [DoNotSerialize]
    [PortLabelHidden]
    public ValueInput valueIn;
    
    [DoNotSerialize]
    [PortLabelHidden]
    public ValueOutput valueOut;

    [DoNotSerialize]
    [PortLabelHidden]
    public ControlOutput exit { get; private set; }


    protected override void Definition()
    {
        
        valueRigidBody2D = ValueInput<Rigidbody2D>(nameof(valueRigidBody2D),null).NullMeansSelf();
        valueIn = ValueInput<float>("X", 0);
        valueOut = ValueOutput<Vector2>(nameof(valueOut));
        
        enter = ControlInput(nameof(enter), Enter);
        exit = ControlOutput(nameof(exit));
    }

    private ControlOutput Enter(Flow flow)
    {
        var body = flow.GetValue<Rigidbody2D>(valueRigidBody2D);
        var x = flow.GetValue<float>(valueIn);
        var y = body.position.y;
        var newPosition = new Vector2(x, y);
        
        // note MovePosition updates on fixed and not immediately
        body.MovePosition(newPosition);
        
        flow.SetValue(valueOut, newPosition);
        return exit;
    }

}