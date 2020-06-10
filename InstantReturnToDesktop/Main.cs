using ColossalFramework.UI;
using Harmony;
using ICities;
using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace InstantReturnToDesktop
{
    public class ModInfo : LoadingExtensionBase, IUserMod
    {
        private readonly string harmonyId = "cgameworld.instantreturntodesktop";
        private HarmonyInstance harmony;

        public string Name => "Instant Return To Desktop";
        public string Description => "Terminates the game executable immediately when returning to desktop";

        public void OnSettingsUI(UIHelperBase helper)
        {
            UIHelperBase group;

            group = helper.AddGroup(Name);

            group.AddCheckbox("Floating Terminate Button (Restart Required)", false, sel =>
            {
                //config.ShnGSleeperReplace = sel;
                //Configuration<ModConfiguration>.Save();
                Debug.LogError("d");
            });

            group.AddSpace(5);

            group.AddButton("Reset Button Position", () =>
            {
                //ModProperties.Instance.ResetInfoPanelPosition();
            });
        }

        public void OnEnabled()
        {
            harmony = HarmonyInstance.Create(harmonyId);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            //add to load only if xml true
            GameObject returnToDesktop = new GameObject("ReturnToDesktop");
            returnToDesktop.AddComponent<PersistentButton>();
            UnityEngine.Object.DontDestroyOnLoad(returnToDesktop);
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
            return Tools.Terminate();
        }
    }

    public class Tools
    {
        public static bool Terminate()
        {
            try
            {
                LoadingManager.instance.autoSaveTimer.Stop();
                Process.GetCurrentProcess().Kill();
                return false;
            }
            catch (Exception e)
            {
                Debug.Log("IRD: -\n" + e);
                Tools.ShowErrorWindow("Instant Return to Desktop Error", e + "\n\nThis mod was unable to terminate the game executable on your computer\n\nReport this error by uploading your output_log.txt file and paste the link in the steam workshop description of this mod as described here: https://steamcommunity.com/workshop/filedetails/ \n?id=463645931");
            }

            return false;

        }
        public static ExceptionPanel ShowErrorWindow(string header, string message)
        {
            ExceptionPanel panel = UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel");
            panel.SetMessage(header, message, false);
            panel.GetComponentInChildren<UISprite>().spriteName = "IconError";
            return panel;
        }
    }

    public class PersistentButton : MonoBehaviour
    {
        //also only call when option enabled!
        Rect windowBounds;

        void Start()
        {
            windowBounds = new Rect(0, 0, 120, 30);
            // windowBounds = new Rect(0, Screen.height-30, 120, 30);
        }

        void OnGUI()
        {
            Debug.Log("onguicalled!");
            windowBounds = GUI.Window(321, windowBounds, DoWindow, "");
        }

        void DoWindow(int windowID)
        {
            if (GUI.Button(new Rect(5, 5, 110, 20), "Terminate"))
            {
                Tools.Terminate();
            }
            GUI.DragWindow(new Rect(0, 0, 10000, 20));          
        }

        //get coords of window on drag! - windowBounds so saving can be done!

    }

}