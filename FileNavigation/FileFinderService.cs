using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileNavigation
{
    public class FileFinderService
    {
        public string FindCorrectPotentialFile(string activeDocument)
        {
            var allCSFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.cs", SearchOption.AllDirectories).ToList();
            var allXAMLFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.xaml", SearchOption.AllDirectories).ToList();

            var activeDocumentName = RemoveEverythingAfterView(activeDocument);
            string potentialMatch;

            if (activeDocument.Contains(FileType.ViewModel))
            {
                potentialMatch = allXAMLFiles.Where(a => a.Contains(activeDocumentName) && a.Contains(FileType.View)).FirstOrDefault();
            }
            else
            {
                potentialMatch = allCSFiles.Where(a => a.Contains(activeDocumentName) && a.Contains(FileType.ViewModel)).FirstOrDefault();
            }

            return potentialMatch;
        }

        private string RemoveEverythingAfterView(string activeDocument)
        {
            var word = activeDocument.Substring(0, activeDocument.LastIndexOf(FileType.View));
            return word;
        }
    }
}
