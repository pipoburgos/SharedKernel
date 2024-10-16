//using System.Diagnostics;

//namespace SharedKernel.Testing.Docker;

//public class DockerComposeCmdHook : IDisposable
//{
//    private string? _error;
//    private string? _info;

//    public DockerComposeCmdHook()
//    {
//        if (!Execute("docker", "compose up -d"))
//            throw new Exception(_error ?? _info ?? "Error starting docker");
//    }

//    private bool Execute(string file, string arguments)
//    {
//        var processStartInfo = new ProcessStartInfo
//        {
//            FileName = file,
//            Arguments = arguments,
//            RedirectStandardOutput = true,
//            RedirectStandardError = true,
//            UseShellExecute = false,
//            CreateNoWindow = true
//        };

//        using var process = new Process();
//        process.StartInfo = processStartInfo;
//        process.OutputDataReceived += (_, e) => _info = e.Data;
//        process.ErrorDataReceived += (_, e) => _error = e.Data;

//        process.Start();
//        process.BeginOutputReadLine();
//        process.BeginErrorReadLine();

//        process.WaitForExit();

//        return process.ExitCode == 0;
//    }

//    public void Dispose()
//    {
//        Execute("docker", "compose down -d");
//    }
//}
