// See https://aka.ms/new-console-template for more information
using DotNetIsolator;
using System.Runtime.InteropServices;
using Wasmtime;

var wasiConfig = new WasiConfiguration()
                    .WithInheritedStandardOutput()
                    .WithPreopenedDirectory(@"c:\temp", "/");

using var host = new IsolatedRuntimeHost()
                    .WithBinDirectoryAssemblyLoader()
                    .WithWasiConfiguration(wasiConfig);

using var runtime = new IsolatedRuntime(host);

Console.WriteLine("Hello, World!");

new EnvironmentInfo().LogEnviromentInfo();

var obj = runtime.CreateObject<EnvironmentInfo>();
obj.InvokeVoid("LogEnviromentInfo");

runtime.Invoke(() =>
{
    Console.WriteLine($"I'm running on {RuntimeInformation.OSArchitecture} ");

    var rootDir = Path.GetPathRoot(Environment.CurrentDirectory);
    var rootDirFiles = Directory.GetFiles(rootDir);

    Console.WriteLine($"The root directory is {rootDir} and contains {rootDirFiles.Length} files");
    foreach (var file in rootDirFiles)
    {
        Console.WriteLine($" - {file}");
    }
});

var path = "/";

runtime.Invoke(() =>
{
    Console.WriteLine($"I'm running on {RuntimeInformation.OSArchitecture} ");

    var rootDirFiles = Directory.GetFiles(path);

    Console.WriteLine($"The root directory is {path} and contains {rootDirFiles.Length} files");
    foreach (var file in rootDirFiles)
    {
        Console.WriteLine($" - {file}");
    }
});

var files = runtime.Invoke(() => new { Path = path, Files = Directory.GetFiles(path)});

foreach (var file in files.Files)
{
    Console.WriteLine($" - {file}");
}



Console.ReadLine();

class EnvironmentInfo
{
    public void LogEnviromentInfo()
    {
        Console.WriteLine("OS: " + Environment.OSVersion);
        Console.WriteLine("OS Version: " + Environment.OSVersion.Version);
        Console.WriteLine("OS Version String: " + Environment.OSVersion.VersionString);
        Console.WriteLine("OS Platform: " + Environment.OSVersion.Platform);
        Console.WriteLine("OS Service Pack: " + Environment.OSVersion.ServicePack);
        Console.WriteLine("OS Version Major: " + Environment.OSVersion.Version.Major);
        Console.WriteLine("OS Version Minor: " + Environment.OSVersion.Version.Minor);
        Console.WriteLine("OS Version Build: " + Environment.OSVersion.Version.Build);
        Console.WriteLine("OS Version Revision: " + Environment.OSVersion.Version.Revision);
        Console.WriteLine("OS Version Major Revision: " + Environment.OSVersion.Version.MajorRevision);
        Console.WriteLine("OS Version Minor Revision: " + Environment.OSVersion.Version.MinorRevision);
        Console.WriteLine($"I'm running on {RuntimeInformation.OSArchitecture} ");

        var rootDir = Path.GetPathRoot(Environment.CurrentDirectory);
        var rootDirFiles = Directory.GetFiles(rootDir);

        Console.WriteLine($"The root directory is {rootDir} and contains {rootDirFiles.Length} files");
        foreach(var file in rootDirFiles)
        {
            Console.WriteLine($" - {file}");
        }
    }
}