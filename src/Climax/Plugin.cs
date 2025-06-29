using BepInEx;
using BepInEx.Logging;
using MonoDetour;
using MonoDetour.HookGen;
using Climax.Buttplug;

namespace Climax;

[BepInAutoPlugin]
public partial class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; private set; } = null!;
    internal static DeviceManager DeviceManager { get; private set; } = null!;

    private void Awake()
    {
        Log = Logger;
        
        // TODO: Make this configurable.
        DeviceManager = new DeviceManager("Climax", "ws://127.0.0.1:12345");
        DeviceManager.ConnectDevices();
        
        CharacterDetour.Init();
        
        Log.LogInfo($"Plugin {Name} v{Version} is loaded!");
    }
}

[MonoDetourTargets(typeof(Character))]
public static class CharacterDetour
{
    private static bool _wasSprinting = false;
    private static bool _isCurrentlySprinting = false;
    
    [MonoDetourHookInitialize]
    public static void Init()
    {
        On.Character.Update.Prefix(Prefix_Update);
    }
    
    private static void Prefix_Update(Character self)
    {
        _isCurrentlySprinting = self.data.isSprinting;

        switch (_wasSprinting)
        {
            case false when _isCurrentlySprinting:
                _wasSprinting = true;
                Plugin.DeviceManager.VibrateConnectedDevices(0.1f);
                break;
            case true when !_isCurrentlySprinting:
                _wasSprinting = false;
                Plugin.DeviceManager.StopConnectedDevices();
                break;
        }
    }
}
