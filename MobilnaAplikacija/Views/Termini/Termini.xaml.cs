using MobilnaAplikacija.Models;
using MobilnaAplikacija.ViewModels;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MobilnaAplikacija.Views.Termini;

public partial class Termini : ContentPage
{
    public Termini()
    {
    }

    public Termini(TerminViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((TerminViewModel)BindingContext).LoadTerminiAsync();
    }

    private async void OnDetailsFrameTapped(object sender, TappedEventArgs e)
    {
        var frame = sender as Frame;
        if (frame?.BindingContext is Models.Termin termin)
        {
            await Shell.Current.GoToAsync($"TerminDetails?TerminId={termin.id}");
        }
    }
}

