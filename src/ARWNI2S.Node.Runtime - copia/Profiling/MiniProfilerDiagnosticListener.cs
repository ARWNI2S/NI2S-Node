﻿using StackExchange.Profiling;
using StackExchange.Profiling.Internal;
using System.Diagnostics;

namespace ARWNI2S.Runtime.Profiling
{
    public class MiniProfilerDiagnosticListener : IMiniProfilerDiagnosticListener
    {
        public string ListenerName => "Microsoft.AspNetCore";  // TODO: Nombre del listener que deseas observar

        public void OnCompleted()
        {
            // No necesitas hacer nada cuando el listener termina
        }

        public void OnError(Exception error)
        {
            // Maneja cualquier error del listener aquí
            Debug.WriteLine($"Error in {ListenerName}: {error.Message}");
        }

        // TODO: NO NEEDED ASP REQUEST NEED ORLEANS REQUESTS... ¿?¿?
        public void OnNext(KeyValuePair<string, object> value)
        {
            // Aquí se manejan los eventos que el listener captura
            if (value.Key == "Microsoft.AspNetCore.Hosting.BeginRequest")
            {
                var profiler = MiniProfiler.StartNew("ASP.NET Core Request");
                Debug.WriteLine($"Profiler started for request: {value.Value}");
            }

            if (value.Key == "Microsoft.AspNetCore.Hosting.EndRequest")
            {
                MiniProfiler.Current?.Stop();
                Debug.WriteLine("Profiler stopped for request.");
            }
        }
    }
}
