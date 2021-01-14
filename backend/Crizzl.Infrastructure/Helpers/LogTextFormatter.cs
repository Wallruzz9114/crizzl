using System.IO;
using Serilog.Events;
using Serilog.Formatting;

namespace Crizzl.Infrastructure.Helpers
{
    public class LogTextFormatter : ITextFormatter
    {
        public void Format(LogEvent logEvent, TextWriter output)
        {
            if (logEvent.Level.ToString() != "Information")
            {
                output.WriteLine("---------------------------------------------------------------------------");
                output.WriteLine($"Timestamp - { logEvent.Timestamp } | Level - { logEvent.Level }                  |");
                output.WriteLine("---------------------------------------------------------------------------");

                foreach (var property in logEvent.Properties)
                    output.WriteLine(property.Key + " : " + property.Value);

                if (logEvent.Exception != null)
                {
                    output.WriteLine("--------------------------- EXCEPTION DETAILS ------------------------------");
                    output.Write("Exception - {0}", logEvent.Exception);
                    output.Write("StackTrace - {0}", logEvent.Exception.StackTrace);
                    output.Write("Message - {0}", logEvent.Exception.Message);
                    output.Write("Source - {0}", logEvent.Exception.Source);
                    output.Write("InnerException -{0}", logEvent.Exception.InnerException);
                }

                output.WriteLine("---------------------------------------------------------------------------");
            }
        }
    }
}