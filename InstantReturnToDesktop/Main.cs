using ColossalFramework.UI;
using Harmony;
using ICities;
using System;
using System.Diagnostics;
using System.Reflection;
using Debug = UnityEngine.Debug;

namespace InstantReturnToDesktop
{
    public class ModInfo : LoadingExtensionBase, IUserMod
    {
        private readonly string harmonyId = "cgameworld.instantreturntodesktop";
        private HarmonyInstance harmony;

        public string Name
        {
            get { return "Instant Return To Desktop"; }
        }

        public string Description
        {
            get { return "Terminates the game executable immediately when returning to desktop"; }
        }

        public void OnEnabled()
        {
            harmony = HarmonyInstance.Create(harmonyId);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public void OnDisabled()
        {
            harmony.UnpatchAll(harmonyId);
            harmony = null;
        }
    }

    [HarmonyPatch(typeof(LoadingManager))]
    [HarmonyPatch("QuitApplication")]
    public class Patch
    {
        static bool Prefix()
        {
            try
            {
                LoadingManager.instance.autoSaveTimer.Stop();
                Process.GetCurrentProcess().Kill();
            }
            catch (Exception e)
            {
                Debug.Log("IRD: -\n" + e);
                Tools.ShowErrorWindow("Instant Return to Desktop Error", e+ "\n\nThis mod was unable to terminate the game executable on your computer\n\nReport this error by uploading your output_log.txt file and paste the link in the steam workshop description of this mod as described here: https://steamcommunity.com/workshop/filedetails/ \n?id=463645931");
            }

            return false;
        }
    }

    public class Tools
    {
        public static ExceptionPanel ShowErrorWindow(string header, string message)
        {
            ExceptionPanel panel = UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel");
            panel.SetMessage(header, message, false);
            panel.GetComponentInChildren<UISprite>().spriteName = "IconError";
            return panel;
        }
    }
}