using UnityEngine;

namespace InstantReturnToDesktop
{
    [ConfigurationPath("InstantReturnToDesktop.xml")]
    public class ModSettings
    {
        public bool FloatingButton { get; set; } = false;
        public Rect ButtonRect { get; set; } = new Rect(0, 0, 120, 30);

        //implementation from keallu's mods
        private static ModSettings _instance;
        public static ModSettings instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Configuration<ModSettings>.Load();
                }

                return _instance;
            }
        }

        public void Save()
        {
            Configuration<ModSettings>.Save();
        }
    }
}
