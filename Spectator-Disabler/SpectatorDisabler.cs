﻿using System;
using System.Reflection;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using HarmonyLib;
using Player = Exiled.Events.Handlers.Player;
using Scp049 = Exiled.Events.Handlers.Scp049;
using Server = Exiled.Events.Handlers.Server;
using Item = Exiled.Events.Handlers.Item;

namespace SpectatorDisabler
{
    public class SpectatorDisabler : Plugin<Config>
    {
        private static int _harmonyCounter;

        private static Harmony HarmonyInstance { get; set; }

        public override string Author => "zochris";
        public override string Name => "SpectatorDisabler";
        public override Version RequiredExiledVersion { get; } = new Version(8, 8, 0);
        public override Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;

        public override void OnDisabled()
        {
            if (HarmonyInstance != null || HarmonyInstance != default)
            {
                HarmonyInstance.UnpatchAll();
            }

            UnregisterEvents();

            Log.Info("SpectatorDisabler unloaded");
        }

        public override void OnEnabled()
        {
            HarmonyInstance = new Harmony($"{Name}{_harmonyCounter++}");
            HarmonyInstance.PatchAll();

            RegisterEvents();

            Log.Info($"SpectatorDisabler {Version} loaded");
        }

        private static void RegisterEvents()
        {
            Log.Debug("Setting up event handler");

            Player.Spawned += EventHandler.OnPlayerSpawning;
            Scp049.FinishingRecall += EventHandler.OnFinishingRecall;
            Server.RoundStarted += EventHandler.OnRoundStarted;
            Player.PickingUpItem += EventHandler.OnPickingUpItem;
            Item.ChangingAttachments += EventHandler.OnAttachmentChange;
            Player.DroppingItem += EventHandler.OnDroppingItem;
        }

        private static void UnregisterEvents()
        {
            Player.Spawned -= EventHandler.OnPlayerSpawning;
            Scp049.FinishingRecall -= EventHandler.OnFinishingRecall;
            Server.RoundStarted -= EventHandler.OnRoundStarted;
            Player.PickingUpItem -= EventHandler.OnPickingUpItem;
            Item.ChangingAttachments -= EventHandler.OnAttachmentChange;
            Player.DroppingItem -= EventHandler.OnDroppingItem;
        }
    }
}
