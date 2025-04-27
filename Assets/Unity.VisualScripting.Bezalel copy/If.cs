using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Unity.VisualScripting.Bezalel
{
    
    
[UnitCategory("Bezalel/Control")]
[UnitTitle("If")]
[TypeIcon(typeof(VisualScripting.If))]
public class If : Unit
{

    public enum BranchType { Boolean, Number, String }


    [SerializeAs(nameof(BranchingType))]
    private BranchType _branchingType;

    [DoNotSerialize]
    [Inspectable, UnitHeaderInspectable]
    public BranchType BranchingType { get { return _branchingType; } set { _branchingType = value; } }

    
    [DoNotSerialize]
    [PortLabelHidden]
    public ControlInput enter { get; private set; }

    [PortLabel("A = B")]
    [DoNotSerialize]
    public ControlOutput exitNumberEqual { get; private set; }

    [PortLabel("A > B")]
    [DoNotSerialize]
    public ControlOutput exitNumberGreater { get; private set; }
    
    [PortLabel("A < B")]
    [DoNotSerialize]
    public ControlOutput exitNumberSmaller { get; private set; }
    
    [PortLabel("A = B")]
    [DoNotSerialize]
    public ControlOutput exitStringEqual { get; private set; }

    [PortLabel("A ≠ B")]
    [DoNotSerialize]
    public ControlOutput exitStringNotEqual { get; private set; }
    
    [PortLabel("True")]
    [DoNotSerialize]
    public ControlOutput exitBooleanTrue { get; private set; }

    [PortLabel("False")]
    [DoNotSerialize]
    public ControlOutput exitBooleanFalse { get; private set; }
    
    [PortLabel("Else")]
    [DoNotSerialize]
    public ControlOutput exitElse { get; private set; }
    
    [DoNotSerialize]
    public ValueInput valueA; 

    [DoNotSerialize] 
    public ValueInput valueB;  
    
    [DoNotSerialize]
    [PortLabel("")]
    [PortLabelHidden] 
    public ValueInput valueBoolean; 
    

    protected override void Definition()
    {
        
        // Debug.Log(BranchingType);
        
        switch(BranchingType){
        
            case(BranchType.Boolean):
                enter = ControlInput("", (flow) =>
                {
                    var a = flow.GetValue<bool>(valueBoolean);

                    return a switch
                    {
                        true when exitBooleanTrue.hasValidConnection => exitBooleanTrue,
                        false when exitBooleanFalse.hasValidConnection => exitBooleanFalse,
                        _ => exitElse
                    };
                });
                
                exitBooleanTrue = ControlOutput("exitBooleanTrue");
                exitBooleanFalse = ControlOutput("exitBooleanFalse");
                exitElse = ControlOutput("exitElse");
                
                valueBoolean = ValueInput<bool>("A", false);
                return;
            
            case(BranchType.Number):
                enter = ControlInput("", (flow) =>
                {
                    var a = flow.GetValue<float>(valueA);
                    var b = flow.GetValue<float>(valueB);
                    
                    // A = B ?
                    if (Mathf.Approximately(a, b) && exitNumberEqual.hasValidConnection)
                        return exitNumberEqual;

                    // A > B ?
                    if (a > b && exitNumberGreater.hasValidConnection)
                        return exitNumberGreater;

                    // A < B
                    if (a < b && exitNumberSmaller.hasValidConnection)
                        return exitNumberSmaller;
                    
                    return exitElse;
                });

                exitNumberEqual = ControlOutput("exitNumberEqual");
                exitNumberGreater = ControlOutput("exitNumberGreater");
                exitNumberSmaller = ControlOutput("exitNumberSmaller");
                exitElse = ControlOutput("exitElse");

                
                valueA = ValueInput<float>("A", 0);
                valueB = ValueInput<float>("B", 0);
                return;
            
            case(BranchType.String):
                enter = ControlInput("", (flow) =>
                {
                    var a = flow.GetValue<string>(valueA);
                    var b = flow.GetValue<string>(valueB);
                    
                    if (a == b && exitStringEqual.hasValidConnection)
                        return exitStringEqual;
                    
                    if (a != b && exitStringNotEqual.hasValidConnection)
                        return exitStringEqual;
                    
                    return exitElse;
                });

                exitStringEqual = ControlOutput("exitStringEqual");
                exitStringNotEqual = ControlOutput("exitStringNotEqual");
                exitElse = ControlOutput("exitElse");
        
                valueA = ValueInput<string>("A", string.Empty);
                valueB = ValueInput<string>("B", string.Empty);
                return;
            
        }
    }
    
}
}
