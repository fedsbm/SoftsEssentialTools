using UnityEngine;

namespace Packages.SoftsEssentialKit.Runtime.Extensions
{
    public static class VectorExtensions
    {
        public static float _AngleFromUp(this Vector2 vector)
        {
            float angle = Vector2.Angle(Vector2.up,  vector);
            if (vector.x < 0) return -angle;
            return angle;
        }
    }
}
