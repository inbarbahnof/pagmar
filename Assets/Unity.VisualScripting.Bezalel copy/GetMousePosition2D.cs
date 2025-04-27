using Unity.VisualScripting;
using UnityEngine;

// namespace Unity.VisualScripting.Bezalel
// {

    [UnitCategory("Bezalel")]
    [UnitTitle("Get Mouse Position 2D")]
    [UnitSubtitle("Input")]
    [TypeIcon(typeof(Input))]
    public sealed class GetMousePosition2D : Unit
    {

        [DoNotSerialize]
        [PortLabelHidden]
        public ValueOutput result;

        private Vector3 resultValue;


        protected override void Definition()
        {
            result = ValueOutput<Vector2>("result", ReturnValue);
        }

        Vector2 ReturnValue(Flow flow)
        {
            Vector3 mousePosition = Input.mousePosition;

            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            // mousePosition.z = flow.stack.self.transform.position.z;

            return mousePosition;
        }
    }
    
// }