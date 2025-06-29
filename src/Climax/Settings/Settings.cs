using BepInEx.Configuration;

namespace Climax.Config;

public class ClimaxSettings(ConfigFile config)
{
    public readonly ConfigEntry<string> ServerURL = config.Bind("Buttplug", "Server URL", "ws://127.0.0.1:12345", "The URL where the Intiface server is hosted");

    public readonly ConfigEntry<bool> SprintingVibrationEnabled = config.Bind("Vibrations.Sprinting", "Enabled", true, "Whether or not using sprinting makes your toys vibrate");
    public readonly ConfigEntry<float> SprintingVibrationIntensity = config.Bind("Vibrations.Sprinting", "Intensity", 0.1f, "The intensity of the vibration");
    
    public readonly ConfigEntry<bool> JumpingVibrationEnabled = config.Bind("Vibrations.Jumping", "Enabled", true, "Whether or not using jumping makes your toys vibrate");
    public readonly ConfigEntry<float> JumpingVibrationIntensitySmall = config.Bind("Vibrations.Sprinting", "Small Intensity", 0.2f, "The intensity of the vibration when you perform a small jump");
    public readonly ConfigEntry<float> JumpingVibrationIntensityHuge = config.Bind("Vibrations.Sprinting", "Huge Intensity", 0.5f, "The intensity of the vibration when you perform a big jump");
}