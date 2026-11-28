$ErrorActionPreference = "Stop"

$scriptPath = $MyInvocation.MyCommand.Path
$project = Join-Path $PSScriptRoot "AutoRepairShop.Android\AutoRepairShop.Android.csproj"
$desktop = [Environment]::GetFolderPath("Desktop")
$log = Join-Path $desktop "garage-build.log"

function Write-Log($text)
{
    $text | Tee-Object -FilePath $log -Append
}

if (([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator) -eq $false)
{
    Write-Host "Требуются права администратора — запускаю с повышением..."
    Start-Process -FilePath "powershell.exe" -Verb RunAs -ArgumentList "-NoExit","-ExecutionPolicy","Bypass","-File","`"$scriptPath`""
    exit
}

Write-Log "=== Гараж Народный: сборка APK ==="
Write-Log ("Время: " + (Get-Date))

try
{
    Write-Log "1/3 Установка рабочей нагрузки maui-android..."
    & dotnet workload install maui-android 2>&1 | Tee-Object -FilePath $log -Append

    Write-Log "2/3 Подготовка Android SDK и JDK..."
    & dotnet build $project -t:InstallAndroidDependencies -f net10.0-android -c Release -p:AcceptAndroidSDKLicenses=True 2>&1 | Tee-Object -FilePath $log -Append

    Write-Log "3/3 Сборка APK..."
    & dotnet build $project -f net10.0-android -c Release 2>&1 | Tee-Object -FilePath $log -Append

    $apk = Get-ChildItem -Path (Join-Path $PSScriptRoot "AutoRepairShop.Android\bin\Release\net10.0-android") -Filter "*.apk" -ErrorAction SilentlyContinue |
        Where-Object { $_.Name -like "*Signed*" } |
        Select-Object -First 1

    if ($null -eq $apk)
    {
        $apk = Get-ChildItem -Path (Join-Path $PSScriptRoot "AutoRepairShop.Android\bin\Release\net10.0-android") -Filter "*.apk" -ErrorAction SilentlyContinue |
            Select-Object -First 1
    }

    if ($null -ne $apk)
    {
        $target = Join-Path $desktop "Гараж 2108.apk"
        Copy-Item $apk.FullName $target -Force
        Write-Log ("ГОТОВО: " + $target)
        Write-Host "APK на рабочем столе: $target"
    }
    else
    {
        Write-Log "APK не найден. Смотрите лог: $log"
    }
}
catch
{
    Write-Log ("ОШИБКА: " + $_)
    Write-Log "Лог сохранён на рабочем столе: garage-build.log"
}

Write-Host "Готово. Нажмите Enter, чтобы закрыть окно."
Read-Host
