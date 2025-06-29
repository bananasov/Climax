using BepInEx.Configuration;

namespace Climax.Config;

public class ClimaxSettings(ConfigFile config)
{
    public ConfigEntry<string> ServerURL = config.Bind("Buttplug", "Server URL", "ws://127.0.0.1:12345", "The URL where the Intiface server is hosted");

    public ConfigEntry<bool> SprintingVibrationEnabled = config.Bind("Vibrations.Sprinting", "Enabled", true, "Whether or not using sprinting makes your toys vibrate");
}