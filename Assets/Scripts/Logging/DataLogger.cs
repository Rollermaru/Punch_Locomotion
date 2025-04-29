using System;
using System.IO;
using UnityEngine;

public static class DataLogger
{
    private static readonly string csvPath;

    static DataLogger()
    {
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var fileName = $"punch_data_{timestamp}.csv";
        var folder = Application.persistentDataPath;
        csvPath = Path.Combine(folder, fileName);

        Debug.Log($"[DataLogger] Initialized new CSV at: {csvPath}");

        // Print the file path once
        Debug.Log($"[DataLogger] CSV file path: {csvPath}");

        // Ensure the directory exists
        var dir = Path.GetDirectoryName(csvPath);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        // If the file does not exist, write the header first
        if (!File.Exists(csvPath))
        {
            var header = "Event,Time,FlagName,Distance\n";
            File.WriteAllText(csvPath, header);
        }
    }

    //public static void LogTeleportStart()
    //{
    //    // Print the path again each time logging occurs
    //    Debug.Log($"[DataLogger] Writing TeleportStart to: {csvPath}");
    //    var line = $"TeleportStart,{Time.time}\n";
    //    File.AppendAllText(csvPath, line);
    //}

    public static void LogFlagHit(string flagName, Vector3 flagPos, Vector3 handPos)
    {
        // Print the file path again
        Debug.Log($"[DataLogger] Writing FlagHit to: {csvPath}");
        // Calculate the center distance
        float dist = Vector3.Distance(flagPos, handPos);
        // Only output event, time, flag name, and distance
        var line = string.Format(
            "FlagHit,{0},{1},{2:F2}\n",
            Time.time,
            flagName,
            dist
        );
        File.AppendAllText(csvPath, line);
    }

}
