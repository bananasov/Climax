using System;
using Mono.Cecil.Cil;
using MonoDetour.Cil;
using MonoDetour.HookGen;
using MonoMod.Cil;

namespace Climax.Hooks;

[MonoDetourTargets(typeof(CharacterAfflictions))]
public static class CharacterAfflictionsDetour
{
    public static void Init()
    {
        // NOTE: We could also have used CharacterMovement.CheckFallDamage, but this is more expandable upon.
        On.CharacterAfflictions.AddStatus.ILHook(ILHook_AddStatus_AfflictionVibration);
    }

    private static void ILHook_AddStatus_AfflictionVibration(ILManipulationInfo info)
    {
        ILWeaver w = new(info);

        // GUIManager.instance.AddStatusFX(statusType, amount);
        Instruction inst = null!;
        var matchResult = w.MatchRelaxed(
            x => x.MatchLdsfld<GUIManager>(nameof(GUIManager.instance)), 
            x => x.MatchLdarg(1),
            x => x.MatchLdarg(2),
            x => x.MatchCallvirt<GUIManager>(nameof(GUIManager.AddStatusFX)) && w.SetInstructionTo(ref inst, x)
            );
        matchResult.ThrowIfFailure();

        // Insert `VibrateYerBooty(statusType, amount)` after `GUIManager.instance.AddStatusFX(statusType, amount)`
        var callInst = w.CreateCall(VibrateYerBooty);
        w.InsertAfter(inst, Instruction.Create(OpCodes.Ldarg_1), Instruction.Create(OpCodes.Ldarg_2), callInst);
    }

    // NOTE: `amount` is a value between 0f and 1f
    //       `STATUSTYPE.Cold` is not added due to it being a REALLY REALLY low value that no one will literally
    //        ever feel.
    //
    //        Feel like something is missing? Make an issue <3
    private static void VibrateYerBooty(CharacterAfflictions.STATUSTYPE statusType, float amount)
    {
        float vibrationTime;
        switch (statusType)
        {
            case CharacterAfflictions.STATUSTYPE.Injury:
                vibrationTime = 1f;
                break;
            case CharacterAfflictions.STATUSTYPE.Hot:
                vibrationTime = 0.2f;
                break;
            default:
                return;
        }
        
        Plugin.Log.LogInfo($"Vibrating devices with {amount} intensity for {vibrationTime} seconds");
        Plugin.DeviceManager.VibrateConnectedDevicesWithDuration(amount, vibrationTime);
    }
}