//using System.Diagnostics;

//namespace SharedKernel.Testing.Docker;

//public class DockerWslCmdHook : IDisposable
//{
//    private readonly Process? _process;
//    //private bool IsAdmin()
//    //{
//    //    using var identity = WindowsIdentity.GetCurrent();
//    //    var principal = new WindowsPrincipal(identity);
//    //    return principal.IsInRole(WindowsBuiltInRole.Administrator);
//    //}

//    public DockerWslCmdHook()
//    {
//        var startInfo = new ProcessStartInfo
//        {
//            FileName = "cmd.exe",
//            Arguments = "/C wsl docker compose up",
//            RedirectStandardOutput = true,
//            RedirectStandardError = true,
//            UseShellExecute = false,
//            CreateNoWindow = true
//        };

//        _process = new Process
//        {
//            StartInfo = startInfo
//        };

//        try
//        {
//            _process.Start();

//            Thread.Sleep(TimeSpan.FromSeconds(20));
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Se produjo una excepción: {ex.Message}");
//        }
//    }

//    public void Dispose()
//    {
//        _process?.Close();
//        _process?.Dispose();
//    }
//}
