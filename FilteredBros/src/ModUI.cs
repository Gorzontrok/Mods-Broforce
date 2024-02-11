using System.Collections.Generic;
using UnityEngine;
using World.LevelEdit;
using UnityModManagerNet;

namespace FilteredBros
{
    public static class ModUI
    {
        public static int boxHeight = 135;
        public static int spaceBetweenGroups = 20;

        private static string _search = string.Empty;
        private static FuzzySearcher<BroToggle> _fuzzySearcher = new FuzzySearcher<BroToggle>();
        private static List<BroToggle> _campaignBroFiltered = null;
        private static List<BroToggle> _expendaBroFiltered = null;
        private static List<BroToggle> _hideBroFiltered = null;

        internal static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            if (_fuzzySearcher == null)
                _fuzzySearcher = new FuzzySearcher<BroToggle>();

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical("box", GUILayout.Height(boxHeight));
            Main.settings.mod.Draw(modEntry);
            GUILayout.EndVertical();

            GUILayout.BeginVertical("box", GUILayout.Height(boxHeight));
            Main.settings.ui.Draw(modEntry);
            GUILayout.EndVertical();

            GUILayout.BeginVertical("box", GUILayout.Height(boxHeight));
            GUILayout.Label("Informations", UnityModManager.UI.bold); // Header style
            GUILayout.Label("Number of Freed Bros: " + PlayerProgress.Instance.freedBros.ToString());
            int numberOfRescuesToNextUnlock = HeroUnlockController.GetNumberOfRescuesToNextUnlock();
            if (numberOfRescuesToNextUnlock != -1)
            {
                GUILayout.Label($"Next unlock in {numberOfRescuesToNextUnlock} saves");
            }
            else
            {
                GUILayout.Label("Every Bros have been unlocked!");
            }
            GUILayout.Label("Number of bro select : " + BroToggle.BrosEnabled);
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Search", GUILayout.ExpandWidth(false));
            _search = GUILayout.TextField(_search, GUILayout.Width(300));
            GUILayout.EndHorizontal();

            // Draw every bros toggles
            _campaignBroFiltered = _fuzzySearcher.FuzzySearch(BroToggle.Broforce, _search);
            DrawToggleGroup("Broforce", _campaignBroFiltered, UnityModManager.UI.bold);
            GUILayout.Space(spaceBetweenGroups);
            _expendaBroFiltered = _fuzzySearcher.FuzzySearch(BroToggle.Expendabros, _search);
            DrawToggleGroup("Expendabros", _expendaBroFiltered, UnityModManager.UI.bold);
            GUILayout.Space(spaceBetweenGroups);
            _hideBroFiltered = _fuzzySearcher.FuzzySearch(BroToggle.Secret, _search);
            DrawToggleGroup("Unused", _hideBroFiltered, UnityModManager.UI.bold);
            GUILayout.Space(spaceBetweenGroups);
        }

        private static void DrawToggleGroup(string groupeName, List<BroToggle> broToggles, GUIStyle nameStyle)
        {
            GUILayout.BeginHorizontal("box");
            GUILayout.Label(groupeName, nameStyle);
            GUILayout.EndHorizontal();
            if (broToggles == null)
            {
                GUILayout.Label("Filtered Bros encountered an error :(");
                return;
            }
            else if (broToggles.Count == 0)
            {
                GUILayout.Label("No Bros Founded");
                return;
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Select All", GUILayout.ExpandWidth(false)))
                Select(broToggles, true);
            if (GUILayout.Button("Select Nothing", GUILayout.ExpandWidth(false)))
                Select(broToggles, false);
            GUILayout.EndHorizontal();

            // Draw Toggles
            int horizontalIndex = 0;
            bool horizontal = false;
            foreach (BroToggle broToggle in broToggles)
            {
                if (horizontalIndex == 0)
                {
                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
                    horizontal = true;
                }
                broToggle.DrawToggle();
                GUILayout.Space(10);

                horizontalIndex++;
                if (horizontalIndex == Main.settings.ui.numberOfBroPerLine) // if max number -> end horizontal
                {
                    horizontalIndex = 0;
                    GUILayout.EndHorizontal();
                    horizontal = false;
                }
            }
            // Make sure no Horizontal is opened
            if (horizontal)
            {
                GUILayout.EndHorizontal();
            }
        }

        private static void Select(List<BroToggle> list, bool enable)
        {
            foreach (BroToggle toggle in list)
            {
                toggle.enabled = enable;
            }
        }
    }
}
