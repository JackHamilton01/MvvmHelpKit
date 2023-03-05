using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FileNavigation;
using MvvmHelpKit.Services;

namespace MvvmHelpKit
{
    [Command(PackageIds.NavigateTo)]
    internal sealed class NavigateTo : BaseCommand<NavigateTo>
    {
        private FileFinderService fileFinderService;
        private ModuleInteractionService moduleInterractionService;
        private ControllerReaderService controllerReaderService;
        private DTE dte;

        public NavigateTo()
        {
            fileFinderService = new FileFinderService();
            moduleInterractionService = new ModuleInteractionService();
            controllerReaderService = new ControllerReaderService();
        }

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            var filesInModule = moduleInterractionService.GetFilesInModule(moduleInterractionService.GetModulePathOfCurrentDocument());
            var registeredTypes = controllerReaderService.GetRegisteredTypes(filesInModule);

            var fileNameWithoutExtension = GetActiveFileNameWithoutExtension();
            var navigationTarget = registeredTypes.Where(r => r.ViewModelName == fileNameWithoutExtension || r.ViewName == fileNameWithoutExtension).FirstOrDefault();

            var parentDirectoryOfModule = Directory.GetParent(moduleInterractionService.GetModulePathOfCurrentDocument());
            var navigationTargetLocation = parentDirectoryOfModule.GetFiles("*", SearchOption.AllDirectories);

            string moduleDirectory = moduleInterractionService.GetModulePathOfCurrentDocument();
            if (navigationTarget.ViewName == fileNameWithoutExtension)
            {
                var result = navigationTargetLocation.Where(n => n.Name == $"{navigationTarget.ViewModelName}.cs").FirstOrDefault();
                await VS.Documents.OpenAsync(result.FullName);
            }
            else
            {
                var result = navigationTargetLocation.Where(n => n.Name == $"{navigationTarget.ViewName}.xaml").FirstOrDefault();
                await VS.Documents.OpenAsync(result.FullName);
            }
        }

        private string GetActiveFileNameWithoutExtension()
        {
            DTE dte = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SDTE)) as DTE;
            return Path.GetFileNameWithoutExtension(dte.ActiveDocument.Name);
        }
    }
}
