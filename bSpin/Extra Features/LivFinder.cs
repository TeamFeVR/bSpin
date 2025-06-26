using System;
using UnityEngine;

namespace bSpin {
    internal class LivFinder {
        /*
        public static Transform FindTracker()
        {

            return UnityEngine.Object.FindObjectsOfType<LIV.SDK.Unity.ExternalCamera>().FirstOrDefault().transform;
        }
        */
        public static float GetCameraAngleOffset(Transform camera) {
            ///Summary
            ///using trig to find an angle
            ///
            ///         |\
            ///         | \
            ///adjacent |  \
            ///         |   \
            ///         |____\
            ///         opposite
            /// 
            /// geometry class coming in clutch


            var opposite = camera.position.x;
            var adjacent = camera.position.z;

            var opposite1 = opposite;
            //if the x coord is less than 0 (AKA to the left), flip it, so the math doesn't go wonky
            if (opposite < 0)
                opposite1 = -opposite;

            var angle = (float)(Math.Atan(opposite1 / adjacent) * (180 / Math.PI));
            //invert the angle, to account for the way the player is spun
            angle = -angle;

            //flip it, to account for the flip we did before
            if (opposite > 0)
                return angle;
            return -angle;
        }
    }
}