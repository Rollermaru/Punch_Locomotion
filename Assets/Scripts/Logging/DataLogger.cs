using System;
using System.IO;
using UnityEngine;

public static class DataLogger
{
    private static readonly string csvPath;
    private static bool hasStartedFirstTeleport = false;
    private static float teleportStartTime = 0f;
    private static bool experimentInProgress = false;

    static DataLogger()
    {
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var fileName = $"punch_data_{timestamp}.csv";
        var folder = Application.persistentDataPath;
        csvPath = Path.Combine(folder, fileName);

        // Ensure the directory exists
        var dir = Path.GetDirectoryName(csvPath);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        // If the file does not exist, write the header first
        if (!File.Exists(csvPath))
        {
            var header = "Trial,Event,Time,FlagName,Distance,TimeSinceTeleport\n";
            File.WriteAllText(csvPath, header);
        }

        Debug.Log($"[DataLogger] CSV file initialized at: {csvPath}");
    }

    // Call this when starting a new trial
    public static void StartExperiment()
    {
        if (!experimentInProgress)
        {
            experimentInProgress = true;
            Debug.Log("[DataLogger] Experiment started");
        }
    }

    public static void LogTeleportStart(int trialNumber)
    {
        // Only start timing on the first punch ever
        if (experimentInProgress && !hasStartedFirstTeleport)
        {
            hasStartedFirstTeleport = true;
            teleportStartTime = Time.time;

            // Write to CSV, opening in append mode with 'true' parameter
            var line = $"{trialNumber},TeleportStart,{Time.time},,0,0\n";
            File.AppendAllText(csvPath, line);

            Debug.Log($"[DataLogger] Trial {trialNumber}: First teleport logged at {teleportStartTime}");
        }
    }

    public static void LogFlagHit(int trialNumber, string flagName, Vector3 flagPos, Vector3 handPos)
    {
        // Calculate the center distance
        float dist = Vector3.Distance(flagPos, handPos);

        // Calculate time since first teleport
        float timeSinceTeleport = 0f;
        if (hasStartedFirstTeleport)
        {
            timeSinceTeleport = Time.time - teleportStartTime;
        }
        // Output event, time, flag name, distance, and time since teleport
        var line = string.Format(
            "{0},FlagHit,{1},{2},{3:F2},{4:F2}\n",
            trialNumber,
            Time.time,
            flagName,
            dist,
            timeSinceTeleport
        );
        File.AppendAllText(csvPath, line);

        Debug.Log($"[DataLogger] Trial {trialNumber}: Flag '{flagName}' hit, distance: {dist:F2}m, time: {timeSinceTeleport:F2}s");
    }

    // Reset for next trial
    public static void ResetForNextTrial()
    {
        hasStartedFirstTeleport = false;
        teleportStartTime = 0f;
        Debug.Log("[DataLogger] Reset for next trial");
    }

}
