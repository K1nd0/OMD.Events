using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OMD.Events.Services;
using OpenMod.API.Plugins;
using OpenMod.Unturned.Plugins;
using System;

[assembly: PluginMetadata("OMD.Events", DisplayName = "OMD.Events", Author = "K1nd")]

namespace OMD.Events.Main;

public class EventsPlugin(ILogger<EventsPlugin> logger, IEventsService eventsService, IServiceProvider serviceProvider) : OpenModUnturnedPlugin(serviceProvider)
{
    protected override UniTask OnLoadAsync()
    {
        eventsService.Init();

        logger.LogInformation("Made with love by K1nd");
        logger.LogInformation("Discord: k1nd_");

        return base.OnLoadAsync();
    }
}
