namespace DrasticMedia;

public partial class App : Application
{
    private IServiceProvider services;

    public App(IServiceProvider services)
    {
        this.InitializeComponent();
        this.services = services;
    }

    /// <inheritdoc/>
    protected override Window CreateWindow(IActivationState activationState)
    {
        return new MediaWindow(this.services) { Page = new PlayerPage(this.services) };
    }
}
