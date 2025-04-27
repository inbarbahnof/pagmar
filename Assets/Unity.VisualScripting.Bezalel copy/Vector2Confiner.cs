using Unity.VisualScripting;
using UnityEngine;

// namespace Unity.VisualScripting.Bezalel
// {

    [UnitCategory("Bezalel")]
    [UnitTitle("Confine")]
    [UnitSubtitle("Vector2")]
    [TypeIcon(typeof(Vector2))]
    public sealed class Vector2Confiner : Unit
    {

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueInput valueIn;

        [DoNotSerialize]
        public ValueInput valueMin;

        [DoNotSerialize]
        public ValueInput valueMax;

        [DoNotSerialize]
        public ValueOutput valueOut;

        private Vector2 resultValue;


        protected override void Definition()
        {
            valueIn = ValueInput<Vector2>("In", Vector2.zero);
            valueMin = ValueInput<Vector2>("Min", Vector2.zero);
            valueMax = ValueInput<Vector2>("Max", Vector2.zero);

            valueOut = ValueOutput<Vector2>("Out", ReturnValue);
        }

        Vector2 ReturnValue(Flow flow)
        {
            resultValue.x = Mathf.Clamp(flow.GetValue<Vector2>(valueIn).x, flow.GetValue<Vector2>(valueMin).x,
                flow.GetValue<Vector2>(valueMax).x);
            resultValue.y = Mathf.Clamp(flow.GetValue<Vector2>(valueIn).y, flow.GetValue<Vector2>(valueMin).y,
                flow.GetValue<Vector2>(valueMax).y);

            return resultValue;
        }

    }
    
// }