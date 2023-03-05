using MvvmHelpKit.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MvvmHelpKit.Services
{
    public class ControllerReaderService
    {
        public string GetControllerFromFiles(List<string> files)
        {
            string moduleFile = string.Empty;

            foreach (var file in files)
            {
                var allText = File.ReadAllText(file);

                if (allText.Contains("IModule"))
                {
                    moduleFile = file;
                    break;
                }
            }

            return moduleFile;
        }

        public List<ViewModelLink> GetRegisteredTypes(List<string> files)
        {
            List<string> newLines= new List<string>();

            var controllerPath = GetControllerFromFiles(files);

            var allText = File.ReadAllText(controllerPath);
            string output = allText.Substring(allText.IndexOf("RegisterTypes"));

            var registerMethodContents = output.Split('\n').ToList();
            registerMethodContents.RemoveAll(s => !s.Contains("RegisterForNavigation"));

            List<ViewModelLink> viewModelLinks = new List<ViewModelLink>();
            foreach (var item in registerMethodContents)
            {
                string names = new string(item.SkipWhile(c => c != '<')
                                   .Skip(1)
                                   .TakeWhile(c => c != '>')
                                   .ToArray()).Trim();

                var split = names.Split(',');

                viewModelLinks.Add(new ViewModelLink()
                {
                    ViewName = split[0],
                    ViewModelName = split[1].TrimStart()
                });;
            }

            return viewModelLinks;
        }
    }   
}
