using AutoRepairShop;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Storage;

namespace AutoRepairShop.Android;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>();

        GameSave.RootDirectory = FileSystem.AppDataDirectory;
        Profile.RootDirectory = FileSystem.AppDataDirectory;

        return builder.Build();
    }
}
