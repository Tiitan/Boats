using System;
using System.ComponentModel;
using UnityEngine;

namespace UI.ViewModel
{
    public interface ICommandViewModel : INotifyPropertyChanged
    {
        string Text { get; }
        Texture2D Icon { get; }
        Action Action { get; }
        IReadOnlyObservableCollection<ICommandViewModel> SubCommands { get; }
        bool Enabled { get; }
    }

    public class CommandViewModel : ObservableObject, ICommandViewModel
    {
        private bool _enabled = true;

        public string Text { get; }
        public Texture2D Icon { get; }
        public Action Action { get; }
        public IReadOnlyObservableCollection<ICommandViewModel> SubCommands => EditableSubCommands;
        public ObservableCollection<CommandViewModel> EditableSubCommands { get; }

        public bool Enabled
        {
            get => _enabled;
            set { _enabled = value; OnPropertyChanged(); }
        }

        public CommandViewModel(Texture2D icon, ObservableCollection<CommandViewModel> subCommands)
        {
            Icon = icon;
            EditableSubCommands = subCommands;
        }

        public CommandViewModel(string text, ObservableCollection<CommandViewModel> subCommands)
        {
            Text = text;
            EditableSubCommands = subCommands;
        }

        public CommandViewModel(Texture2D icon, Action action)
        {
            Icon = icon;
            Action = action;
        }

        public CommandViewModel(string text, Action action)
        {
            Text = text;
            Action = action;
        }
    }
}