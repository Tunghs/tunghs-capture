using Capture.UI.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace Capture.UI.Components
{
    public sealed class ViewModelLocator
    {
        public ShellViewModel? ViewModel
            => Ioc.Default.GetService<ShellViewModel>();

        public CaptureViewModel? CaptureViewModel
            => Ioc.Default.GetService<CaptureViewModel>();
    }
}
