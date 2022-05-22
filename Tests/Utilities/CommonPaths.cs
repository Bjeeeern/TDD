using System;
using System.IO;

namespace Tests.Utilities;

public static class CommonPaths
{
    public static string ServerWebRoot()
    {
        return Path.Combine(ServerProject(), "wwwroot");
    }

    public static string ServerProject()
    {
        var relativeSolutionPath = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "..");
        var normalizedSolutionPath = Path.GetFullPath(relativeSolutionPath);
        var serverProjectPath = Path.Combine(normalizedSolutionPath, "Server");
        return serverProjectPath;
    }
}