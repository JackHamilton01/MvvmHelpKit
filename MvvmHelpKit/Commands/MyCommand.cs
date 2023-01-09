using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MvvmHelpKit
{
    [Command(PackageIds.MyCommand)]
    internal sealed class MyCommand : BaseCommand<MyCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            EnvDTE.DTE dte = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(Microsoft.VisualStudio.Shell.Interop.SDTE)) as EnvDTE.DTE;
            string activeDocument = dte.ActiveDocument.Name;
            //var activeDocument = Path.GetFileNameWithoutExtension(VS.Documents.GetActiveDocumentViewAsync().Result.WindowFrame.Caption);
            var potentialMatch = FindCorrectPotentialFile(activeDocument);
            await VS.Documents.OpenAsync(potentialMatch);
            var isgsd = VS.Documents.IsOpenAsync(potentialMatch);

            //await VS.MessageBox.ShowWarningAsync("MvvmHelpKit", "Button clicked");
        }

        private string RemoveEverythingAfterView(string activeDocument)
        {
            var word = activeDocument.Substring(0, activeDocument.LastIndexOf("View"));
            return word;
        }

        private string FindCorrectPotentialFile(string activeDocument)
        {
            var allCSFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.cs", SearchOption.AllDirectories).ToList();
            var allXAMLFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.xaml", SearchOption.AllDirectories).ToList();

            var activeDocumentName = RemoveEverythingAfterView(activeDocument);
            string potentialMatch;

            if (activeDocument.Contains("ViewModel"))
            {
                potentialMatch = allXAMLFiles.Where(a => a.Contains(activeDocumentName) && a.Contains("View")).FirstOrDefault();
            }
            else
            {
                potentialMatch = allCSFiles.Where(a => a.Contains(activeDocumentName) && a.Contains("ViewModel")).FirstOrDefault();
            }

            return potentialMatch;
        }
    }
}
