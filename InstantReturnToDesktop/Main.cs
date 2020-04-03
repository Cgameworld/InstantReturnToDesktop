using Harmony;
using ICities;
using System.Diagnostics;
using System.Reflection;

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
            LoadingManager.instance.autoSaveTimer.Stop();
            Process.GetCurrentProcess().Kill();
            return false;
        }
    }
}