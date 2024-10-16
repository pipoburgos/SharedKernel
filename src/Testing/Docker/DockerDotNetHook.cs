//using Docker.DotNet;
//using Docker.DotNet.Models;

//namespace SharedKernel.Testing.Docker;

//public sealed class DockerDotNetHook : IDisposable
//{
//    private readonly DockerClient _client;

//    public DockerDotNetHook()
//    {
//        _client = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient();

//        var createContainerParameters = new CreateContainerParameters
//        {
//            Image = "docker/compose:latest",
//            Cmd = ["wsl docker-compose", "-f", "./docker-compose.yml", "up", "-d"]
//        };

//        var response = _client.Containers.CreateContainerAsync(createContainerParameters).GetAwaiter().GetResult();

//        if (response.Warnings != null)
//        {
//            foreach (var warning in response.Warnings)
//            {
//                Console.WriteLine($"Warning: {warning}");
//            }
//        }

//        _client.Containers.StartContainerAsync(response.ID, new ContainerStartParameters()).GetAwaiter().GetResult();
//    }

//    private void Dispose(bool disposing)
//    {
//        if (!disposing)
//            return;

//        _client.Dispose();
//    }

//    public void Dispose()
//    {
//        Dispose(true);
//        GC.SuppressFinalize(this);
//    }
//}

