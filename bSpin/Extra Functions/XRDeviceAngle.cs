using UnityEngine;
using UnityEngine.XR;
using static bSpin.Extentions.TransformExtensions;

namespace bSpin.Extra_Functions {
    internal static class XRDeviceAngle {
        internal static Vector3 AnglesFromController(ControllerAngleMode mode, bool? leftHand = null) {
            var angs = new Vector3();

            var HandPos = new Vector3();
            var HeadPos = new Vector3();

            var Hand = InputDevices.GetDeviceAtXRNode(XRNode.Head);
            var Head = InputDevices.GetDeviceAtXRNode(XRNode.Head);
            var LeftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            var RightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

            Head.TryGetFeatureValue(CommonUsages.devicePosition, out HeadPos);
            switch (leftHand) {
                case true:
                    Hand = LeftHand;
                    LeftHand.TryGetFeatureValue(CommonUsages.devicePosition, out HandPos);
                    break;
                case false:
                    Hand = RightHand;
                    RightHand.TryGetFeatureValue(CommonUsages.devicePosition, out HandPos);
                    break;
            }

            switch (mode) {
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

        internal enum ControllerAngleMode {
            Point,
            ArmPoint,
            Look
        }
    }
}