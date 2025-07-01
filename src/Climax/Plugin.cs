using BepInEx;
using BepInEx.Logging;
using Climax.Buttplug;
using Climax.Config;
using Climax.Hooks;

namespace Climax;

[BepInAutoPlugin]
public partial class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; private set; } = null!;
    internal static DeviceManager DeviceManager { get; private set; } = null!;
    internal static ClimaxSettings Settings { get; private set; } = null!;

    private void Awake()
    {
        Log = Logger;
        Settings = new ClimaxSettings(Config);
        
        // TODO: Make this configurable.
        DeviceManager = new DeviceManager("Climax", Settings.ServerURL.Value);
        DeviceManager.ConnectDevices();
        
        CharacterDetour.Init();
        CharacterAfflictionsDetour.Init();
        
        Log.LogInfo($"Plugin {Name} v{Version} is loaded!");
    }
}


