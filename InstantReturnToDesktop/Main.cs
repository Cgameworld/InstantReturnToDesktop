using Harmony;
using ICities;
using System.Diagnostics;
using System.Reflection;

namespace InstantReturnToDesktop
{
    public class ModInfo : IUserMod
    {
        public string Name
        {
            get { return "Instant Return To Desktop"; }
        }

        public string Description
        {
            get { return "Kills game executable immediately when returning to desktop"; }
        }
    }
    public class ModLoading : LoadingExtensionBase
    {
        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
            var harmony = HarmonyInstance.Create("cgameworld.instantreturntodesktop");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(ExitConfirmPanel))]
    [HarmonyPatch("OnExitGame")]
    public class Patch
    {
        static void Prefix()
        {
            Process.GetProcessesByName("Cities")[0].Kill();
        }
    }
}