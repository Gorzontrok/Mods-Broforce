using UnityEngine;

namespace RocketLib.CustomTriggers
{
    public abstract class CustomTriggerActionInfo : TriggerActionInfo
    {
        public static void ShowGridPointOption(LevelEditorGUI gui, GridPoint point, string label)
        {
            if (GUILayout.Button(string.Concat(new object[] { label, " (currently C ", point.collumn, " R ", point.row, ")" }), new GUILayoutOption[0]))
            {
                gui.settingWaypoint = true;
                gui.waypointToSet = point;
                gui.MarkTargetPoint(point);
            }
        }
    }
}
