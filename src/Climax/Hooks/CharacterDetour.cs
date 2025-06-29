using MonoDetour.HookGen;

namespace Climax.Hooks;

[MonoDetourTargets(typeof(Character))]
public static class CharacterDetour
{
    private static bool _wasSprinting = false;
    private static bool _isCurrentlySprinting = false;
    
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