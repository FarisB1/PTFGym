using Microsoft.Extensions.Logging;
using MobilnaAplikacija.Services;
using MobilnaAplikacija.ViewModels;
using MobilnaAplikacija.Views;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using MobilnaAplikacija.Views.Termini;

namespace MobilnaAplikacija
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<ITerminService, TerminService>();
            builder.Services.AddSingleton<TerminViewModel>();
            builder.Services.AddSingleton<Termini>();
            builder.Services.AddTransient<TerminTrenerViewModel>();
            builder.Services.AddTransient<TerminTreneri>();
            builder.Services.AddTransient<TerminDetails>();
            builder.Services.AddTransient<CreateTermin>();

            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<MainPage>();

            builder.Services.AddSingleton<ISecureStorage>(SecureStorage.Default);
            builder.Services.AddTransient<AppShell>();

            builder.Services.AddTransient<IRezervacijaService, RezervacijaService>();
            builder.Services.AddTransient<RezervacijeViewModel>();
            builder.Services.AddTransient<Rezervacije>();

            builder.Services.AddTransient<ITrenerService, TrenerService>();
            builder.Services.AddTransient<TreneriViewModel>();
            builder.Services.AddTransient<Treneri>();

            builder.Services.AddTransient<INapredakService, NapredakService>();
            builder.Services.AddTransient<NapredakViewModel>();
            builder.Services.AddTransient<Napredak>();
            builder.Services.AddTransient<CreateNapredak>(); 
            builder.Services.AddTransient<CreateNapredakViewModel>();
            
            builder.Services.AddTransient<AdminDashboardViewModel>();
            builder.Services.AddTransient<AdminDashboardPage>();
            builder.Services.AddTransient<UserEditViewModel>();
            builder.Services.AddTransient<UserEditPage>();


            builder.Services.AddSingleton<MembershipService>();
            builder.Services.AddTransient<MembershipViewModel>();
            builder.Services.AddTransient<MembershipView>();

            builder.Services.AddSingleton<IRezervacijaCreateService, RezervacijaCreateService>();
            builder.Services.AddTransient<CreateRezervacijaViewModel>();
            builder.Services.AddTransient<CreateRezervacijaPage>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
