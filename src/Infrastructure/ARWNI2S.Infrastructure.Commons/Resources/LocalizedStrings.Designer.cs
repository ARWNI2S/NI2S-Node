﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ARWNI2S.Infrastructure.Resources {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class LocalizedStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal LocalizedStrings() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ARWNI2S.Infrastructure.Resources.LocalizedStrings", typeof(LocalizedStrings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a &quot;{0}. MESSAGE TRUNCATED AT THIS POINT!! Max message size = {1}&quot;.
        /// </summary>
        internal static string Logger_LogMessageTruncated_Format {
            get {
                return ResourceManager.GetString("Logger_LogMessageTruncated_Format", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Exception = .
        /// </summary>
        internal static string Logging_ConsoleText_WriteError {
            get {
                return ResourceManager.GetString("Logging_ConsoleText_WriteError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Completed async task timer callback for timer {0}.
        /// </summary>
        internal static string TimerAfterCallbackAsync {
            get {
                return ResourceManager.GetString("TimerAfterCallbackAsync", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Completed sync timer callback for timer {0}.
        /// </summary>
        internal static string TimerAfterCallbackSync {
            get {
                return ResourceManager.GetString("TimerAfterCallbackSync", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a About to make task timer callback for timer {0}.
        /// </summary>
        internal static string TimerBeforeCallback {
            get {
                return ResourceManager.GetString("TimerBeforeCallback", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a About to make async task timer callback for timer {0}.
        /// </summary>
        internal static string TimerBeforeCallbackAsync {
            get {
                return ResourceManager.GetString("TimerBeforeCallbackAsync", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a About to make sync timer callback for timer {0}.
        /// </summary>
        internal static string TimerBeforeCallbackSync {
            get {
                return ResourceManager.GetString("TimerBeforeCallbackSync", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Changing timer {0} to dueTime={1} period={2}.
        /// </summary>
        internal static string TimerChanging {
            get {
                return ResourceManager.GetString("TimerChanging", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Creating timer {0} with dueTime={1} period={2}.
        /// </summary>
        internal static string TimerChanging_Creating {
            get {
                return ResourceManager.GetString("TimerChanging_Creating", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a About to QueueNextTimerTick for timer {0}.
        /// </summary>
        internal static string TimerChanging_Queue {
            get {
                return ResourceManager.GetString("TimerChanging_Queue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Queued next tick for timer {0} in {1}.
        /// </summary>
        internal static string TimerNextTick {
            get {
                return ResourceManager.GetString("TimerNextTick", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Timer {0} is now stopped and disposed.
        /// </summary>
        internal static string TimerStopped {
            get {
                return ResourceManager.GetString("TimerStopped", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Assert failed with message = .
        /// </summary>
        internal static string TraceLogger_Assert_FailMessage {
            get {
                return ResourceManager.GetString("TraceLogger_Assert_FailMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Internal contract assertion has failed!.
        /// </summary>
        internal static string TraceLogger_Assert_Message {
            get {
                return ResourceManager.GetString("TraceLogger_Assert_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Log code {0} occurred {1} additional time{2} in the previous {3}.
        /// </summary>
        internal static string TraceLogger_CheckBulkMessageLimits_Format {
            get {
                return ResourceManager.GetString("TraceLogger_CheckBulkMessageLimits_Format", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a No logger config data provided..
        /// </summary>
        internal static string TraceLogger_ConfigDataMissing {
            get {
                return ResourceManager.GetString("TraceLogger_ConfigDataMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a INTERNAL FAILURE! About to crash! Fail message is: .
        /// </summary>
        internal static string TraceLogger_Fail_ErrorMessage {
            get {
                return ResourceManager.GetString("TraceLogger_Fail_ErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Internal Fail!.
        /// </summary>
        internal static string TraceLogger_Fail_Message {
            get {
                return ResourceManager.GetString("TraceLogger_Fail_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Unrecoverable failure: .
        /// </summary>
        internal static string TraceLogger_Fail_UnrecoverableMessage {
            get {
                return ResourceManager.GetString("TraceLogger_Fail_UnrecoverableMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a No logger config data provided..
        /// </summary>
        internal static string TraceLogger_Initialize_ArgumentNullException {
            get {
                return ResourceManager.GetString("TraceLogger_Initialize_ArgumentNullException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a CHUNKED MESSAGE Part {0}: {1}.
        /// </summary>
        internal static string TraceLogger_LogWithoutBulkingAndTruncating_Format {
            get {
                return ResourceManager.GetString("TraceLogger_LogWithoutBulkingAndTruncating_Format", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a No LoaderExceptions found.
        /// </summary>
        internal static string TraceLogger_PrintException_Helper_NoExceptionsMesage {
            get {
                return ResourceManager.GetString("TraceLogger_PrintException_Helper_NoExceptionsMesage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Exc level {0}: {1}: {2}{3}.
        /// </summary>
        internal static string TraceLogger_PrintOneException_Format {
            get {
                return ResourceManager.GetString("TraceLogger_PrintOneException_Format", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a &quot;Exception while passing a log message to log consumer. TraceLogger type:{0}, name:{1}, severity:{2}, message:{3}, error code:{4}, message exception:{5}, log consumer exception:{6}&quot;.
        /// </summary>
        internal static string TraceLogger_WriteLogMessage_ExceptionFormat {
            get {
                return ResourceManager.GetString("TraceLogger_WriteLogMessage_ExceptionFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Previous log message was truncated - Max size = .
        /// </summary>
        internal static string TraceLogger_WriteLogMessage_MessageTruncatedText {
            get {
                return ResourceManager.GetString("TraceLogger_WriteLogMessage_MessageTruncatedText", resourceCulture);
            }
        }
    }
}