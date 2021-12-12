namespace DrasticMedia;

public partial class App : Application
{
    private IServiceCollection collection;

    public App(IServiceCollection collection)
    {
        this.InitializeComponent();
        this.collection = collection;
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        return new MediaWindow() { Page = new PlayerPage(this.collection) };
    }
}
