using BepInEx.Logging;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;


namespace SteamApiUpgrade;

public static class Patcher {
    const string GUID = "com.lalabuff.lethal.steamapiupgrade";
    const string NAME = "Steam API Upgrade";
    const string VERSION = "0.0.1";

    internal static ManualLogSource Log;

    const string STEAMAPI = "steam_api64_patched";
    const string STEAMWORKS = "Facepunch.Steamworks.Win64";
    const string TRANSPORT = "Facepunch Transport for Netcode for GameObjects";

    public static IEnumerable<string> TargetDLLs => [
        STEAMWORKS + ".dll",
        TRANSPORT + ".dll"
    ];

    public static void Initialize() {
        Log = Logger.CreateLogSource(NAME);
        Log.LogInfo($"Loading prepatcher for {NAME} ({GUID}) v{VERSION}...");

        var currentDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        var steamApiPath = Path.Combine(currentDir, STEAMAPI + ".dll");

        Log.LogDebug($"Attempting to load [{steamApiPath}]...");
        var steamApi = Native.LoadLibrary(steamApiPath);
        if(steamApi == IntPtr.Zero) {
            int errorCode = Marshal.GetLastWin32Error();
            Log.LogError($"Failed to load [{STEAMAPI}.dll] with error code [{errorCode}].");
        } else {
            Log.LogInfo($"Successfully loaded [{STEAMAPI}.dll].");
        }
    }

    public static void Patch(ref AssemblyDefinition assembly) {
        string name = assembly.Name.Name;
        byte[] raw = name switch {
            STEAMWORKS => Properties.Resources.Facepunch_Steamworks_Win64,
            TRANSPORT => Properties.Resources.FacepunchTransport,
            _ => []
        };

        if(raw.Length == 0) {
            Log.LogError($"Failed to patch unexpected assembly [{name}].");
            return;
        }

        if(raw == null) {
            Log.LogError($"Something went horribly wrong. Got null assembly for [{name}].");
            return;
        }

        var stream = new MemoryStream(raw);
        var patch = AssemblyDefinition.ReadAssembly(stream);
        patch.Name = assembly.Name;
        assembly = patch;
        Log.LogInfo($"Patched assembly [{name}].");
    }

    public static void Finish() {
        Log.LogInfo($"Finished prepatching {NAME} ({GUID}) v{VERSION}!");
    }
}
