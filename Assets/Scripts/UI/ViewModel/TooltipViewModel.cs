using System.ComponentModel;
using Framework.Mvvm;
using Models;
using UnityEngine;

namespace UI.ViewModel
{
    public interface ITooltipViewModel : INotifyPropertyChanged
    {
        Transform Target { get; }
        int PixelRadius { get; }
        string Name { get; }
        string Description { get; }
        IReadOnlyObservableDictionary<string, ITooltipPropertyViewModel> Properties { get; }
    }

    public class TooltipViewModel : ObservableObject, ITooltipViewModel
    {
        public Transform Target { get; }
        public int PixelRadius { get; }
        public string Name { get; }
        public string Description { get; }

        public IReadOnlyObservableDictionary<string, ITooltipPropertyViewModel> Properties => EditableProperties;
        public ObservableDictionary<string, TooltipPropertyViewModel> EditableProperties { get; }

        public TooltipViewModel(Transform target, int pixelRadius, string name, string description = null,
            ObservableDictionary<string, TooltipPropertyViewModel> properties = null)
        {
            Target = target;
            PixelRadius = pixelRadius;
            Name = name;
            Description = description;
            EditableProperties = properties ?? new ObservableDictionary<string, TooltipPropertyViewModel>();
        }

        public TooltipViewModel(Transform target, int pixelRadius, TooltipBaseInfo tooltipBaseInfo,
            ObservableDictionary<string, TooltipPropertyViewModel> properties = null)
            : this(target, pixelRadius,tooltipBaseInfo.Name, tooltipBaseInfo.Description, properties)
        {}
    }
}