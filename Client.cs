using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System;
using System.Collections.Generic;

namespace Client {
    public class Main : BaseScript {
        private async void LoadConfings() {
            await Delay(50);
            Commands();
        }
        private void Commands() {
            API.RegisterKeyMapping("stopcar", "Turn off your car", "keyboard", "u");
            API.RegisterKeyMapping("startcar", "Turn on your car", "keyboard", "y");
        }

        public Main() {
            // Loads
            LoadConfings();

            // Commands
            API.RegisterCommand("startcar", new Action<int, List<object>, string>((src, args, raw) => startCar()), false);
            API.RegisterCommand("stopcar", new Action<int, List<object>, string>((src, args, raw) => stopCar()), false);
        }

        private void startCar() {
            if (Game.PlayerPed.IsInVehicle()) {
                var playerVehicle = Game.PlayerPed.CurrentVehicle.Handle;

                // https://github.com/Switty6/swt_notifications 
                TriggerEvent("simpe_commands:Notifications", "swt_notifications:Icon", "Turning on Vehicle", "top-right", 1000, "dark", "grey-1", true, "mdi-engine");

                API.SetVehicleEngineOn(playerVehicle, true, false, true);
            }
            else {
                Screen.ShowNotification("Du skal være i et køretøj!");
            }
        }
        private void stopCar() {
            if (Game.PlayerPed.IsInVehicle()) {
                var playerVehicle = Game.PlayerPed.CurrentVehicle.Handle;

                // https://github.com/Switty6/swt_notifications 
                TriggerEvent("simpe_commands:Notifications", "swt_notifications:Icon", "Turning off Vehicle", "top-right", 1000, "dark", "grey-1", true, "mdi-engine-off");

                API.SetVehicleEngineOn(playerVehicle, false, false, true);
            }
            else {
                Screen.ShowNotification("Du skal være i et køretøj!");
            }
        }
    }
}
