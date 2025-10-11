using UnityEngine;

namespace RocketLib.CustomTriggers
{
    public abstract class LevelStartTriggerActionInfo : CustomTriggerActionInfo
    {
        public bool RunAtLevelStart = true;

        public override void ShowGUI(LevelEditorGUI gui)
        {
            RunAtLevelStart = GUILayout.Toggle(RunAtLevelStart, "Run at level start");
            GUILayout.Space(10);
        }
    }
}
