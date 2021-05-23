using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;

namespace bSpin
{
    class LivFinder
    {
        public static Transform FindTracker()
        {
            return UnityEngine.Object.FindObjectsOfType<LIV.SDK.Unity.ExternalCamera>().FirstOrDefault().transform;
        }

        public static float GetCameraAngleOffset(Transform camera)
        {
            float opposite = camera.position.x;
            float adjacent = camera.position.z;

            float opposite1 = opposite;
            //if the x coord is less than 0 (AKA to the left), flip it, so the math doesn't go wonky
            if (opposite < 0)
                opposite1 = -opposite;

            float angle = (float)(Math.Atan(opposite1 / adjacent) * (180 / Math.PI));
            //invert the angle, to account for the way the player is spun
            angle = -angle;

            //flip it, to account for the flip we did before
            if (opposite > 0)
                return angle;
            else
                return -angle;
        }
    }
}
