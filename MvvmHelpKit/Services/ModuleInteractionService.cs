using EnvDTE;
using FileNavigation;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmHelpKit.Services
{
    public class ModuleInteractionService
    {
        public List<string> GetFilesInModule(string csProjPath)
        {
            var directory = new DirectoryInfo(GetModulePath(csProjPath));
            //return fileTypes.SelectMany(directory.EnumerateFiles).ToList();

            return Directory
                 .EnumerateFiles(directory.FullName)
                 .Where(file => file.ToLower().EndsWith(FileTypes.Cs))
                 .ToList();
        }

        public void GetModuleContents(string modulePath)
        {
            //var files = GetFilesInModule(modulePath, FileTypes.Cs);
        }

        public string GetModulePathOfCurrentDocument()
        {
            DTE dte = Package.GetGlobalService(typeof(SDTE)) as DTE;
            var activeDocument = dte.ActiveDocument;

            return activeDocument.ActiveWindow.Project.FullName;


            //return Path.Combine(activeDocument.Path, activeDocument.Name); 
        }

        private string GetModulePath(string csprojFilePath)
        {
            return Directory.GetParent(csprojFilePath).FullName;

            //string fullPath = Path.GetFullPath(csprojFilePath);
            //var directories = fullPath.Split(Path.DirectorySeparatorChar);
            //return directories[directories.Length - 2];
        }
    }
}
