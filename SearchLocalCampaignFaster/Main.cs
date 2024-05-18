using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using UnityEngine.SceneManagement;
using UnityModManagerNet;
using Utility;

namespace SearchLocalCampaignFaster
{
    internal static class Main
    {
        public static UnityModManager.ModEntry mod;
        public static bool enabled;
        public static Settings settings;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.OnToggle = OnToggle;
            mod = modEntry;
            settings = Settings.Load<Settings>(modEntry);

            try
            {
                var harmony = new Harmony(modEntry.Info.Id);
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                mod.Logger.Log("Failed to Patch Harmony\n" + ex.ToString());
            }

            return true;
        }
        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Draw(modEntry);
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }
    }

    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw(Label = "Search Levels Only In Top Folder", Type = DrawType.Toggle)]
        public bool searchOnlyInTopFolder = true;
        public int maxSearchDepth = 1000;

        public void OnChange()
        { }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }

    [HarmonyPatch(typeof(BrowseCampaigns))]
    public static class BrowseCampaignsPatch
    {
        // Broforce seperate teh CampaginHeader and Campaign datas with the character number 31.
        public const byte HEADER_SEPARATOR = 31;
        public const string LEVEL_TOP_FOLDER = "Levels/";
        // Header is on average 500 characters long.
        // 1000 characters is to be sure we don't go through the whole file but that we go enough to find the end of the header.
        public static int MaxSearchDepth
        {
            get => Main.settings.maxSearchDepth;
        }

        public static bool SearchOnlyInTopFolder
        {
            get => Main.settings.searchOnlyInTopFolder;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(LoadOfflineCampaigns))]
        public static bool LoadOfflineCampaigns()
        {
            if (!Main.enabled)
                return true;
            try
            {
                // Load Published Campaigns
                FilterFiles(".bfg", out string[] publishedCampaignFiles, out List<CampaignHeader> publishedCampaignHeaders);
                // Load Published Deathmatch Campaigns
                FilterFiles(".bfd", out string[] publishedDeathmatchFiles, out List<CampaignHeader> publishedDeathmatchHeaders);
                // Load Unpublished Campaigns
                FilterFiles(".bfc", out string[] unpublishedCampaignsFiles, out List<CampaignHeader> unpublishedCampaignsHeaders);

                SingletonMono<NewCustomCampaignMenu>.Instance.ReceiveUpdatedEntries(
                    unpublishedCampaignsHeaders, publishedCampaignHeaders, publishedDeathmatchHeaders,
                    unpublishedCampaignsFiles, publishedCampaignFiles, publishedDeathmatchFiles
                    );
                return false;
            }
            catch (Exception e)
            {
                Main.mod.Logger.LogException(e);
            }
            return true;
        }

        public static void FilterFiles(string fileExtension, out string[] files, out List<CampaignHeader> headers)
        {
            List<string> tempFiles = new List<string>(
                Directory.GetFiles(LEVEL_TOP_FOLDER, "*" + fileExtension, SearchOnlyInTopFolder ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories)
                );

            Dictionary<string, CampaignHeader> campaigns = new Dictionary<string, CampaignHeader>();

            for (int i = 0; i < tempFiles.Count; i++)
            {
                try
                {
                    // Broforce need the file name to start AFTER 'Levels/' and no extension.
                    string fileName = tempFiles[i].Remove(0, LEVEL_TOP_FOLDER.Length);
                    fileName = fileName.Remove(fileName.Length - fileExtension.Length);

                    CampaignHeader header = GetCampaignHeader(LEVEL_TOP_FOLDER, fileName + fileExtension);
                    if (header != null && !string.IsNullOrEmpty(header.name))
                    {
                        campaigns.Add(fileName, header);
                    }
                }
                catch (Exception ex)
                {
                    Main.mod.Logger.LogException($"Can't load header of file '{Path.GetFileName(tempFiles[i])}'", ex);
                }
            }

            Dictionary<string, CampaignHeader> sorted = campaigns.OrderBy(kvp => kvp.Key).ToDictionary(x => x.Key, x => x.Value);
            files = sorted.Keys.ToArray();
            headers = sorted.Values.ToList();
        }
        public static CampaignHeader GetCampaignHeader(string directory, string file)
        {
            return GetCampaignHeader(Path.Combine(directory, file));
        }
        public static CampaignHeader GetCampaignHeader(string path)
        {
            CampaignHeader header = null;

            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            List<byte> buffer = new List<byte>();

            // Retrieve only the header from the file
            for (int i = 0; i < MaxSearchDepth; i++)
            {
                int val = fileStream.ReadByte();

                if (val == HEADER_SEPARATOR)
                {
                    break;
                }
                buffer.Add((byte)val);
            }
            if (buffer.Count == 0 || buffer.Count >= MaxSearchDepth)
            {
                throw new Exception("Unit separator character not founded");
            }

            // convert the bytes to a string
            string xml = Encoding.Default.GetString(buffer.ToArray());
            // deserialize the header from the string
            XmlSerializer serializer = new XmlSerializer(typeof(CampaignHeader));
            using (StringReader reader = new StringReader(xml))
            {
                header = (CampaignHeader)serializer.Deserialize(reader);
            }

            return header;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(NewCustomCampaignMenu), "LaunchOfflineCampaign")]
        private static bool LaunchOfflineCustomCampaign(NewCustomCampaignMenu __instance, CampaignHeader header, string fileName)
        {
            LevelSelectionController.ResetLevelAndGameModeToDefault();

            PlayerProgress.Instance.lastOnlineLevelProgress = 0;
            GameState.Instance.sceneToLoad = LevelSelectionController.JoinScene;

            __instance.highlightState = NewCustomCampaignMenu.HighlightState.Downloading;
            GameModeController.publishRun = false;
            if (header != null)
            {
                GameModeController.GameMode = header.gameMode;
            }
            else
            {
                GameModeController.GameMode = GameMode.Campaign;
            }
            LevelSelectionController.campaignToLoad = fileName;
            LevelSelectionController.loadCustomCampaign = true;
            LevelSelectionController.loadPublishedCampaign = header.isPublished;
            LevelSelectionController.CurrentLevelNum = 0;

            GameState.Instance.loadMode = MapLoadMode.Campaign;
            Traverse.Create(typeof(StatisticsController)).Method("ResetScore").GetValue();
            LevelEditorGUI.levelEditorActive = false;
            try
            {   // Sometimes the game throw a number overflow exxeption from the CLZF2.Decompress .
                if (LevelSelectionController.loadPublishedCampaign)
                {
                    LevelSelectionController.currentCampaign = FileIO.LoadPublishedCampaignFromDisk(fileName, ".bfg");
                }
                else
                {
                    LevelSelectionController.currentCampaign = FileIO.LoadCampaignFromDisk(fileName);
                }

            }
            catch (Exception ex)
            {
                __instance.mapDetails.text = "failed to load campaign : " + ex.Message;

                __instance.highlightState = NewCustomCampaignMenu.HighlightState.Listing;
                Main.mod.Logger.Log(ex.ToString());
                return false;
            }

            Traverse.Create(__instance).Method("ClearEntries").GetValue();
            SceneLoader.LoadScene(LevelSelectionController.JoinScene, LoadSceneMode.Single);

            return false;
        }
    }
}
