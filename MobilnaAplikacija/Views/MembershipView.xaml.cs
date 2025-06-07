using MobilnaAplikacija.ViewModels;

namespace MobilnaAplikacija.Views;

public partial class MembershipView : ContentPage
{
    private readonly MembershipViewModel _viewModel;

    public MembershipView(MembershipViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}