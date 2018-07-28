using System.ComponentModel;

namespace UI.ViewModel
{
    public interface ITooltipPropertyViewModel : INotifyPropertyChanged
    {
        string Name { get; }
        string Value { get; }
    }

    public class TooltipPropertyViewModel : ObservableObject, ITooltipPropertyViewModel
    {
        private string _value;

        public string Name { get; }

        public string Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(); }
        }

        public TooltipPropertyViewModel(string name, string value)
        {
            Name = name;
            _value = value;
        }
    }
}