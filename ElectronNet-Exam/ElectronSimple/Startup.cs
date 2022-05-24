namespace ElectronSimple
{
    using ElectronNET.API;
    using ElectronNET.API.Entities;

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            if (HybridSupport.IsElectronActive)
            {
                Electron.App.Ready += () => ElectronBootstrap();
            }
        }

        public async void ElectronBootstrap()
        {
#if !DEBUG
            Electron.AutoUpdater.OnError += (message) => Electron.Dialog.ShowErrorBox("Error", message);
            Electron.AutoUpdater.OnCheckingForUpdate += async () => await Electron.Dialog.ShowMessageBoxAsync("Checking for Update");
            Electron.AutoUpdater.OnUpdateNotAvailable += async (info) => await Electron.Dialog.ShowMessageBoxAsync("Update not available");
            Electron.AutoUpdater.OnUpdateAvailable += async (info) => await Electron.Dialog.ShowMessageBoxAsync("Update available" + info.Version);
            Electron.AutoUpdater.OnDownloadProgress += (info) =>
            {
                var message1 = "Download speed: " + info.BytesPerSecond + "\n<br/>";
                var message2 = "Downloaded " + info.Percent + "%" + "\n<br/>";
                var message3 = $"({info.Transferred}/{info.Total})" + "\n<br/>";
                var message4 = "Progress: " + info.Progress + "\n<br/>";
                var information = message1 + message2 + message3 + message4;

                var mainWindow = Electron.WindowManager.BrowserWindows.First();
                Electron.IpcMain.Send(mainWindow, "auto-update-reply", information);
            };
            Electron.AutoUpdater.OnUpdateDownloaded += async (info) => await Electron.Dialog.ShowMessageBoxAsync("Update complete!" + info.Version);

            var currentVersion = await Electron.App.GetVersionAsync();
            var updateCheckResult = await Electron.AutoUpdater.CheckForUpdatesAndNotifyAsync();
            var availableVersion = updateCheckResult.UpdateInfo.Version;
            string information = $"Current version: {currentVersion} - available version: {availableVersion}"; 
#endif



            var browserWindow = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                Width = 1152,
                Height = 940,
                Show = true
            }).ConfigureAwait(false);
            browserWindow.OnClosed += () =>
            {
                Electron.App.Quit();
            };

            await browserWindow.WebContents.Session.ClearCacheAsync();

            browserWindow.SetTitle("title");
        }
    }
}
