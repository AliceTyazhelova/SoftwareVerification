using System.Windows;
using SoftwareVerification.WpfClient.ViewModels;

namespace SoftwareVerification.WpfClient.Views;

public partial class CreateWindow : Window
{
    public CreateWindow()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is CreateWindowViewModel oldVm)
            oldVm.RequestClose -= OnRequestClose;

        if (e.NewValue is CreateWindowViewModel newVm)
            newVm.RequestClose += OnRequestClose;
    }

    private void OnRequestClose()
    {
        if (DataContext is CreateWindowViewModel vm)
            DialogResult = vm.DialogResult;
    }
}