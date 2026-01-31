using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp096;
using LabApi.Features.Console;
using MEC;
using ProjectMER.Events.Handlers;
using ProjectMER.Features;
using ProjectMER.Features.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using UnityEngine;

namespace Mask096
{

    public class EventHandlers
    {

        public void OnUsingItemCompleted(UsingItemCompletedEventArgs ev)
        {
            bool flag = this.Serials.Contains(ev.Item.Serial);
            if (flag)
            {
                ev.IsAllowed = false;
                IEnumerable<Exiled.API.Features.Player> enumerable = Enumerable.Where<Exiled.API.Features.Player>(Exiled.API.Features.Player.List, (Exiled.API.Features.Player p) => p.Role is Scp096Role && Vector3.Distance(p.Position, ev.Player.Position) < 5f && !this.MaskEquipped.Contains(p));
                bool flag2 = Enumerable.Count<Exiled.API.Features.Player>(enumerable) != 0;
                if (flag2)
                {
                    Exiled.API.Features.Player player = Enumerable.First<Exiled.API.Features.Player>(enumerable);
                    player.EnableEffect(Exiled.API.Enums.EffectType.Ensnared, 999f, true);
                    ev.Player.EnableEffect(Exiled.API.Enums.EffectType.Ensnared, 999f, true);
                    ev.Usable.Destroy();
                    this.Serials.Remove(ev.Usable.Serial);
                    ev.Player.CustomInfo = MainPlugin.Instance.Translation.Scp096Hint;
                    Timing.RunCoroutine(this.Scp096Corountine(ev.Player, player));

                }
            }
        }

        public void OnHurt(HurtEventArgs ev)
        {
            bool flag = MainPlugin.Instance.Config.IsMaskOffByDamage && this.MaskEquipped.Contains(ev.Player);
            if (flag)
            {
                this.MaskEquipped.Remove(ev.Player);
                ev.Player.CustomInfo = "";
                SchematicManager.DespawnForPlayer(ev.Player);
            }
        }

        public void AddingTarget(AddingTargetEventArgs ev)
        {
            bool flag = this.MaskEquipped.Contains(ev.Player);
            if (flag)
            {
                ev.IsAllowed = false;
            }
        }

        public void OnEnragind(EnragingEventArgs ev)
        {
            bool flag = this.MaskEquipped.Contains(ev.Player);
            if (flag)
            {
                ev.IsAllowed = false;
            }
        }
        public void Died(DiedEventArgs ev)
        {
            bool flag = this.MaskEquipped.Contains(ev.Player);
            if (flag)
            {
                this.MaskEquipped.Remove(ev.Player);
                SchematicManager.DespawnForPlayer(ev.Player);

            }
        }



        public void ChangedItem(ChangedItemEventArgs ev)
        {

            if (Serials.Contains(ev.Item.Serial))
            {
                ev.Player.ShowHint("you have picked up");

            }
        }


        public IEnumerator<float> Scp096Corountine(Exiled.API.Features.Player cuffer, Exiled.API.Features.Player scp096)
        {
            int num;
            for (int i = MainPlugin.Instance.Config.SecondsToUse; i >= 0; i = num - 1)
            {
                bool flag = i == 0;
                if (flag)
                {
                    this.MaskEquipped.Add(scp096);
                    scp096.Role.As<Scp096Role>().ClearTargets();
                    scp096.DisableEffect(Exiled.API.Enums.EffectType.Ensnared);
                    cuffer.DisableEffect(Exiled.API.Enums.EffectType.Ensnared);
                    SchematicManager.SpawnForPlayer(scp096);

                    yield break;
                }
                bool flag2 = i > 0;
                if (flag2)
                {
                    cuffer.ShowHint(string.Format(MainPlugin.Instance.Translation.PlayerMaskHint, i), 1f);
                    scp096.ShowHint(string.Format(MainPlugin.Instance.Translation.Scp096MaskHint, i), 1f);



                }
                yield return Timing.WaitForSeconds(1f);
                num = i;
            }
            yield break;
        }

        public static class SchematicManager
        {
            public static readonly Dictionary<string, SchematicObject> SpawnedSchematics = new();

            public static void SpawnForPlayer(Player scp096)
            {
                var spawned = ObjectSpawner.SpawnSchematic("bag", scp096.Position, scp096.Rotation, scp096.Scale);
                SpawnedSchematics[scp096.UserId] = spawned;
                spawned.transform.parent = scp096.GameObject.transform;
                Log.Info($"Spawned schematic for {scp096.Nickname}");
                LabApi.Features.Console.Logger.Info($"Spawned bag for {scp096.Nickname}");
            }

            public static void DespawnForPlayer(Player scp096)
            {
                if (SpawnedSchematics.TryGetValue(scp096.UserId, out var schematic))
                {
                    schematic.Destroy();
                    SpawnedSchematics.Remove(scp096.UserId);
                    LabApi.Features.Console.Logger.Info($"despawned bag for {scp096.Nickname}");
                }
            }
        }

        






        public List<Player> MaskEquipped = new List<Player>();
        public List<SchematicObject> spawnedschematicses = new List<SchematicObject>();
        public List<ushort> Serials = new List<ushort>();
        public Player players = null;
        private readonly Dictionary<Player, SchematicObject> Scp096BagSchematics = new Dictionary<Player, SchematicObject>();






    }

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class bag : CommandSystem.ICommand, IUsageProvider
    {
        public string Command => "getbag";
        public string[] Aliases => new[] { "bag" };
        public string Description => "gives you the 096 mask";
        public string[] Usage => new[] { "" };
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);


            if (player == null)
            {
                response = $"BE ALIVE FIRST";
                return false;
            }

            Exiled.API.Features.Items.Item item = player.AddItem(ItemType.SCP268);
            MainPlugin.Instance.eventHandlers.Serials.Add(item.Serial);
            player.ShowHint(MainPlugin.Instance.Translation.PlayerSpawnHint);


            response = $"this is a message";
            return false;

        }


    }

}