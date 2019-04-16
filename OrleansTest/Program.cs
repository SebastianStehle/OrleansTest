using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime;
using OrleansTest.Grains;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace OrleansTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var siloPort = 11111;
            int gatewayPort = 30000;
            var siloAddress = IPAddress.Loopback;

            var serializer = JsonSerializer.CreateDefault();

            var silo =
                new SiloHostBuilder()
                    .UseDevelopmentClustering(options => options.PrimarySiloEndpoint = new IPEndPoint(siloAddress, siloPort))
                    .UseInMemoryReminderService()
                    .ConfigureEndpoints(siloAddress, siloPort, gatewayPort)
                    .Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = "helloworldcluster";
                        options.ServiceId = "1";
                    })
                    .ConfigureServices(services =>
                    {
                        services.AddSingleton(serializer);
                    })
                    .ConfigureApplicationParts(appParts => appParts.AddApplicationPart(typeof(IIndexGrain).Assembly))
                    .Build();

            silo.StartAsync().Wait();

            var client =
                new ClientBuilder()
                    .UseStaticClustering(options => options.Gateways.Add((new IPEndPoint(siloAddress, gatewayPort)).ToGatewayUri()))
                    .Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = "helloworldcluster";
                        options.ServiceId = "1";
                    })
                    .ConfigureServices(services =>
                    {
                        services.AddSingleton(serializer);
                    })
                    .ConfigureApplicationParts(appParts => appParts.AddApplicationPart(typeof(IIndexGrain).Assembly))
                    .Build();

            client.Connect().Wait();

            var cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                var grain = client.GetGrain<IIndexGrain>(Guid.NewGuid());

                while (!cts.IsCancellationRequested)
                {
                    await grain.IndexAsync(Guid.NewGuid(), new J<IndexData>(new IndexData { Data = new NamedContentData() }), true);

                    await Task.Delay(1000, cts.Token);
                }
            });

            Console.WriteLine("Press key to exit...");
            Console.ReadLine();

            cts.Cancel();

            silo.StopAsync().Wait();
        }
    }
}
