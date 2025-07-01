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

        Instruction inst = null!;
        var matchResult = w.MatchRelaxed(
            x => x.MatchLdarg(0),
            x => x.MatchLdnull(),
            x => x.MatchCall(out _),
            x => x.MatchLdcI4(1) && w.SetInstructionTo(ref inst, x),
            x => x.MatchRet() || x.MatchBr(out _));
        matchResult.ThrowIfFailure();

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