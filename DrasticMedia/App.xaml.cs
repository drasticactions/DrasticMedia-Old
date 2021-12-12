namespace DrasticMedia;

public partial class App : Application
{
    private IServiceProvider services;

    public App(IServiceProvider services)
    {
        this.InitializeComponent();
        this.services = services;
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        return new MediaWindow() { Page = new PlayerPage(services) };
    }
}
