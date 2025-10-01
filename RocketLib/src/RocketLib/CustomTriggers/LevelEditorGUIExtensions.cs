using UnityEngine;

namespace RocketLib.CustomTriggers
{
    public static class LevelEditorGUIExtensions
    {
        public static void MarkTargetPoint(this LevelEditorGUI self, GridPoint targetPoint)
        {
            self.CallMethod("MarkTargetPoint", targetPoint);
        }

        public static void MarkTargetPoint(this LevelEditorGUI self, GridPoint targetPoint, Color color)
        {
            self.CallMethod("MarkTargetPoint", targetPoint, color);
        }
    }
}
