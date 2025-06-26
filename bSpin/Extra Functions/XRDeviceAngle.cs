using UnityEngine;
using UnityEngine.XR;
using static bSpin.Extentions.TransformExtensions;

namespace bSpin.Extra_Functions
{
    static class XRDeviceAngle
    {
        internal enum ControllerAngleMode
        {
            Point,
            ArmPoint,
            Look
        }
        internal static Vector3 AnglesFromController(ControllerAngleMode mode, bool? leftHand = null)
        {
            Vector3 angs = new Vector3();

            Vector3 HandPos = new Vector3();
            Vector3 HeadPos = new Vector3();

            InputDevice Hand = InputDevices.GetDeviceAtXRNode(XRNode.Head);
            InputDevice Head = InputDevices.GetDeviceAtXRNode(XRNode.Head);
            InputDevice LeftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            InputDevice RightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

            Head.TryGetFeatureValue(CommonUsages.devicePosition, out HeadPos);
            switch (leftHand)
            {
                case true:
                    Hand = LeftHand;
                    LeftHand.TryGetFeatureValue(CommonUsages.devicePosition, out HandPos);
                    break;
                case false:
                    Hand = RightHand;
                    RightHand.TryGetFeatureValue(CommonUsages.devicePosition, out HandPos);
                    break;
            }
            switch (mode)
            {
                case ControllerAngleMode.ArmPoint:
                    angs = HeadPos.PointingAt(HandPos);
                    break;
                case ControllerAngleMode.Point:
                    Quaternion tempAngs;
                    Hand.TryGetFeatureValue(CommonUsages.deviceRotation, out tempAngs);
                    angs = tempAngs.eulerAngles;
                    break;
                case ControllerAngleMode.Look:
                    Quaternion tempAngs2;
                    Head.TryGetFeatureValue(CommonUsages.deviceRotation, out tempAngs2);
                    angs = tempAngs2.eulerAngles;
                    break;
            }
            return angs;
        }


    }
}
