using System.ComponentModel;
using Framework.Mvvm;
using Models;

namespace UI.ViewModel
{
    public interface ITooltipViewModel : INotifyPropertyChanged
    {
        string Name { get; }
        string Description { get; }
        IReadOnlyObservableDictionary<string, ITooltipPropertyViewModel> Properties { get; }
    }

    public class TooltipViewModel : ObservableObject, ITooltipViewModel
    {
        public string Name { get; }
        public string Description { get; }

        public IReadOnlyObservableDictionary<string, ITooltipPropertyViewModel> Properties => EditableProperties;
        public ObservableDictionary<string, TooltipPropertyViewModel> EditableProperties { get; }

        public TooltipViewModel(string name, string description = null,
            ObservableDictionary<string, TooltipPropertyViewModel> properties = null)
        {
            Name = name;
            Description = description;
            EditableProperties = properties ?? new ObservableDictionary<string, TooltipPropertyViewModel>();
        }

        public TooltipViewModel(TooltipBaseInfo tooltipBaseInfo,
            ObservableDictionary<string, TooltipPropertyViewModel> properties = null)
        {
            Name = tooltipBaseInfo.Name;
            Description = tooltipBaseInfo.Description;
            EditableProperties = properties ?? new ObservableDictionary<string, TooltipPropertyViewModel>();
        }
    }
}