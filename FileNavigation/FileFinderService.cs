using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace FileNavigation
{
    public class FileFinderService
    {
        public string FindCorrectPotentialFile(string activeDocument)
        {
           var files = Directory
                .EnumerateFiles(Directory.GetCurrentDirectory())
                .Where(file => file.ToLower().EndsWith(FileTypes.Cs) || file.ToLower().EndsWith(FileTypes.Xaml))
                .ToList();

            if (activeDocument.Contains(MvvmTypes.ViewModel))
            {
                return filesContainingActiveDocumentName(files, activeDocument, MvvmTypes.View);
            }
            else
            {
                return filesContainingActiveDocumentName(files, activeDocument, MvvmTypes.ViewModel);
            }
        }

        private string RemoveEverythingAfterView(string activeDocument)
        {
            var word = activeDocument.Substring(0, activeDocument.LastIndexOf(MvvmTypes.View));
            return word;
        }

        private string filesContainingActiveDocumentName(List<string> files, string activeDocument, string type)
        {
            var activeDocumentName = RemoveEverythingAfterView(activeDocument);
            return files.Where(a => a.Contains(activeDocumentName) && a.Contains(type)).FirstOrDefault();
        }
    }
}
