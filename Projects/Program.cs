using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Pulumi;
using static System.Boolean;

internal class Program
{
    private static Task<int> Main()
    {
        TryParse(Environment.GetEnvironmentVariable("PULUMI_DEBUG"), out var debug);
        
        if (debug)
        {
            Console.WriteLine("Awaiting debugger to attach...");
            while (!Debugger.IsAttached)
                Thread.Sleep(100);
            Console.WriteLine("Running with debug mode.");
        }

        return Deployment.RunAsync(Stack.RunAsync);
    }
}