using MonoDetour.Cil;
using MonoDetour.HookGen;
using MonoMod.Cil;

namespace Climax.Hooks;

[MonoDetourTargets(typeof(Character))]
public static class CharacterDetour
{
    private static bool _wasSprinting = false;
    private static bool _isCurrentlySprinting = false;
    
    public static void Init()
    {
        if (Plugin.Settings.SprintingVibrationEnabled.Value) On.Character.Update.Prefix(Prefix_Update_SprintVibration);
        if (Plugin.Settings.JumpingVibrationEnabled.Value) On.Character.UseStamina.Prefix(Prefix_UseStamina);
    }

    private static void Prefix_UseStamina(Character self, ref float usage, ref bool useBonusStamina)
    {
        if (usage >= 0.15)
        {
            Plugin.DeviceManager.VibrateConnectedDevicesWithDuration(Plugin.Settings.JumpingVibrationIntensityHuge.Value, 0.2f);
        } 
        else if (usage >= 0.05)
        {
            Plugin.DeviceManager.VibrateConnectedDevicesWithDuration(Plugin.Settings.JumpingVibrationIntensitySmall.Value, 0.1f);
        }
    }
    
    private static void Prefix_Update_SprintVibration(Character self)
    {
        if (!self.player.view.IsMine) return;
        
        _isCurrentlySprinting = self.data.isSprinting;

        switch (_wasSprinting)
        {
            case false when _isCurrentlySprinting:
                _wasSprinting = true;
                Plugin.DeviceManager.VibrateConnectedDevices(Plugin.Settings.SprintingVibrationIntensity.Value);
                break;
            case true when !_isCurrentlySprinting:
                _wasSprinting = false;
                Plugin.DeviceManager.StopConnectedDevices();
                break;
        }
    }
}