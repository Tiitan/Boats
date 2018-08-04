using UnityEngine;

namespace UI
{
    public interface IView<in TViewModel>
    {
        void Initialize(TViewModel viewModel);
    }

    public static class ViewManager
    {
        public static TView Instantiate<TView, TViewModel> (TViewModel viewModel, GameObject viewPrefab, Transform parent)
        {
            var propertyGameObject = Object.Instantiate(viewPrefab, parent);
            var view = propertyGameObject.GetComponent<TView>();
            ((IView<TViewModel>)view).Initialize(viewModel);
            return view;
        }
    }
}
