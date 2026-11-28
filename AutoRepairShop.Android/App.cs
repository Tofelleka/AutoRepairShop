using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace AutoRepairShop.Android;

public class App : Application
{
    protected override Window CreateWindow(IActivationState activationState) => new Window(new MainPage());
}
