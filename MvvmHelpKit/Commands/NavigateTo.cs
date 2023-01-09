using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FileNavigation;

namespace MvvmHelpKit
{
    [Command(PackageIds.NavigateTo)]
    internal sealed class NavigateTo : BaseCommand<NavigateTo>
    {
        private FileFinderService fileFinderService;
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            fileFinderService = new FileFinderService();

            var potentialMatch = fileFinderService.FindCorrectPotentialFile(GetActiveFile());
            await VS.Documents.OpenAsync(potentialMatch);
        }

        private string GetActiveFile()
        {
            DTE dte = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SDTE)) as DTE;
            return dte.ActiveDocument.Name;
        }
    }
}
