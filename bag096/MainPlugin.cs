using bag096;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.Features;
using Exiled.Events.Handlers;
using System;
using System.Collections.Generic;

namespace Mask096
{
    public class MainPlugin : Plugin<Config, Translations>
    {
        public override string Name => "SCP096Mask";
        public override string Author => "Liginda updated by apb11";
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(8, 9, 6);
        public override void OnEnabled()
        {
            MainPlugin.Instance = this;
            this.eventHandlers = new EventHandlers();
            Exiled.Events.Handlers.Player.UsingItemCompleted += new CustomEventHandler<UsingItemCompletedEventArgs>(this.eventHandlers.OnUsingItemCompleted);
            Exiled.Events.Handlers.Player.Hurt += new CustomEventHandler<HurtEventArgs>(this.eventHandlers.OnHurt);

            Exiled.Events.Handlers.Player.ChangedItem += new CustomEventHandler<ChangedItemEventArgs>(this.eventHandlers.ChangedItem);
            Scp096.Enraging += new CustomEventHandler<EnragingEventArgs>(this.eventHandlers.OnEnragind);
            Scp096.AddingTarget += new CustomEventHandler<AddingTargetEventArgs>(this.eventHandlers.AddingTarget);
        }
        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.UsingItemCompleted -= new CustomEventHandler<UsingItemCompletedEventArgs>(this.eventHandlers.OnUsingItemCompleted);
            Exiled.Events.Handlers.Player.Hurt -= new CustomEventHandler<HurtEventArgs>(this.eventHandlers.OnHurt);
            Exiled.Events.Handlers.Player.ChangedItem -= new CustomEventHandler<ChangedItemEventArgs>(this.eventHandlers.ChangedItem);

            Scp096.Enraging -= new CustomEventHandler<EnragingEventArgs>(this.eventHandlers.OnEnragind);
            Scp096.AddingTarget -= new CustomEventHandler<AddingTargetEventArgs>(this.eventHandlers.AddingTarget);
            this.eventHandlers = null;
            MainPlugin.Instance = null;
        }

        public static MainPlugin Instance;

        public EventHandlers eventHandlers;

        public static Exiled.API.Features.Player cat = null;
    }
}