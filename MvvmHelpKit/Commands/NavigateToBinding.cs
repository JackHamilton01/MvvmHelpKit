using FileNavigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmHelpKit.Commands
{
    [Command(PackageIds.NavigateToBinding)]
    internal sealed class NavigateToBinding : BaseCommand<NavigateToBinding>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            var docView = await VS.Documents.GetActiveDocumentViewAsync();
            var selection = docView?.TextView.Caret.ContainingTextViewLine;
        }
    }
}
