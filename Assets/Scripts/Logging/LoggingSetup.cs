using UnityEngine;
using Unity.Logging;
using Unity.Logging.Sinks;
using Unity.Logging.Internal.Debug;

using UnityLogger = Unity.Logging.Logger;
public class LoggingSetup : MonoBehaviour
{
    void Awake()
    {
        // Configure the logging pipeline
        var config = new LoggerConfig()
            .MinimumLevel.Debug()
            .WriteTo.File("Logs/PunchAndTP.log", minLevel: LogLevel.Debug)
            .WriteTo.JsonFile("Logs/PunchAndTP.json", minLevel: LogLevel.Debug)
            .WriteTo.UnityEditorConsole(outputTemplate: "[{Level}] {Message}");

        // Set the global Logger
        Log.Logger = new UnityLogger(config);

        // If the logging system itself fails, output errors via Debug.LogError
        SelfLog.SetMode(SelfLog.Mode.EnabledInUnityEngineDebugLogError);

        // Test log to confirm Awake was actually called
        Log.Debug("LoggingSetup Awake has run");
    }
}
