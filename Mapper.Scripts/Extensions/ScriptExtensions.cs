using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mapper.Core.Extensions;

namespace Mapper.Scripts.Extensions
{
    public static class ScriptExtensions
    {
        public static async Task<string> GetTextFromEmbeddedResource(this string resource)
        {
            string text = string.Empty;
            var assembly = Assembly.GetExecutingAssembly();

            using var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}{resource}");
            using var reader = new StreamReader(stream);

            text = await reader.ReadToEndAsync();

            return text;
        }

        public static async Task<ConsoleOutput> ExecuteCommand(this Command command)
        {
            try
            {
                var iss = InitialSessionState.CreateDefault();
                iss.ImportPSModule("Microsoft.PowerShell.Archive");
                using var rs = RunspaceFactory.CreateRunspace(iss);

                rs.Open();

                using var ps = PowerShell.Create();

                ps.Runspace = rs;
                ps.Commands.AddCommand(command);
                await ps.InvokeAsync();

                var output = await ps.GetPowershellOutput();
                return output;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.GetExceptionChain());
            }         
        }

        static Task<ConsoleOutput> GetPowershellOutput(this PowerShell ps) => Task.Run(() =>
        {
            var info = new StringBuilder();
            var warning = new StringBuilder();
            var error = new StringBuilder();
            var output = new ConsoleOutput();

            output.HasError = ps.HadErrors;

            foreach (var err in ps.Streams.Error)
            {
                error.AppendLine(err.GetCategoryInfo());
                error.AppendLine(err.Exception.GetExceptionChain());
            }

            output.Error = error.ToString();

            foreach (var i in ps.Streams.Information)
            {
                info.AppendLine(i.ToString());
            }

            output.Information = info.ToString();

            foreach (var w in ps.Streams.Warning)
            {
                warning.AppendLine(w.Message);
            }

            output.Warning = warning.ToString();

            return output;
        });

        static string GetCategoryInfo(this ErrorRecord error)
        {
            var output = new StringBuilder();

            output.AppendLine($"Activity: {error.CategoryInfo.Activity}");
            output.AppendLine($"Category: {error.CategoryInfo.Category.ToString()}");
            output.AppendLine($"Reason: {error.CategoryInfo.Reason}");
            output.AppendLine($"TargetName: {error.CategoryInfo.TargetName}");

            return output.ToString();
        }
    }
}