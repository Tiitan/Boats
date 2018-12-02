using System;
using System.ComponentModel;

namespace UI.ViewModel
{
    public interface ICommandViewModel : INotifyPropertyChanged
    {
        string Text { get; }
        Action Action { get; }
        bool Enabled { get; }
    }

    public class CommandViewModel : ObservableObject, ICommandViewModel
    {
        private bool _enabled = true;

        public string Text { get; }
        public Action Action { get; }

        public bool Enabled
        {
            get => _enabled;
            set { _enabled = value; OnPropertyChanged(); }
        }

        public CommandViewModel(string text, Action action)
        {
            Text = text;
            Action = action;
        }
    }
}