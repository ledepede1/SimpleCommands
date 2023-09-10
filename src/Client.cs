using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using Config.Reader;
using System.ComponentModel;
using System.Threading.Tasks;
using CitizenFX.Core.UI;
using System.Linq;

namespace Client {
    public class Main : BaseScript {
        
        private bool IsSpeedSet = false;
        private bool AlreadyStopped = false;
        private bool alreadyOutlined = false;

        private async void LoadCommands() {
            await Delay(50);
            Commands();
        }
        private void Commands() {
            // Loading the config.ini //
            iniconfig config = new iniconfig("SimpleCommands", "config.ini");
            string engineKey = config.GetStringValue("Keybindings", "engineKey", "fallback");
            string clearmaxspeedKey = config.GetStringValue("Keybindings", "clearmaxspeedKey", "fallback");
            string engineText = config.GetStringValue("Keybindings", "engineText", "fallback");
            string clearmaxspeedText = config.GetStringValue("Keybindings", "clearmaxspeedText", "fallback");

            API.RegisterKeyMapping("togglecar", engineText, "keyboard", engineKey);
            API.RegisterKeyMapping("+clearCC", clearmaxspeedText, "keyboard", clearmaxspeedKey);
        }





        public Main() {

            // Loads
            iniconfig config = new iniconfig("SimpleCommands", "Config.ini");
            LoadCommands();


            // Commands
            API.RegisterCommand("engine", new Action<int, List<object>, string>((src, args, raw) => toggleCar()), false);


            API.RegisterCommand("cc", new Action<int, List<object>, string>((source, args, rawCommand) =>
            {
                string yournotinaVehicle = config.GetStringValue("NotificationString", "yournotinaVehicle", "fallback");
                if (!Game.PlayerPed.IsInVehicle()) {
                    Screen.ShowNotification(yournotinaVehicle, false);
                }
                else {
                    if (args.ToList().Count() > 0) {
                        int playerVehicle = API.GetVehiclePedIsIn(API.GetPlayerPed(-1), false);
                        // strings:
                        string turnonmaxSpeed = config.GetStringValue("NotificationString", "turnonmaxSpeed", "fallback");
                        string turnonmaxSpeed2 = config.GetStringValue("NotificationString", "turnonmaxSpeed2", "fallback");

                        // Need to figure out why it wont work with booleans!
                        string usingOldNotificationSystem = config.GetStringValue("usingOldNotificationSystem", "usingOldNotificationSystem", "fallback");


                        double convertedArg = Convert.ToInt32(args[0]);
                        float v = Convert.ToSingle(convertedArg / 3.6);
                        int speed = Convert.ToInt32(v * 3.6);


                        if (Game.PlayerPed.IsInVehicle()) {


                            // Not a good solution but it works for now. 
                            // I will figure out soon why boolean wont work!
                            if (usingOldNotificationSystem == "false") {
                                // https://github.com/Switty6/swt_notifications 
                                TriggerEvent("simpe_commands:Notifications", "swt_notifications:Icon", turnonmaxSpeed + " " + speed + " " + turnonmaxSpeed2, "top-right", 1000, "dark", "grey-1", true, "mdi-engine");
                            }
                            else if (usingOldNotificationSystem == "true") {
                                //Screen.ShowNotification(turnonmaxSpeed + " " + speed + " " + turnonmaxSpeed2, false);
                                shownoti(turnonmaxSpeed + "" + speed + " " + turnonmaxSpeed2, true, -70);
                            }
                            IsSpeedSet = true;
                            API.SetVehicleMaxSpeed(playerVehicle, v);

                        }
                    }
                }
            }), false);

            API.RegisterCommand("+clearCC", new Action<int, List<object>, string>((source, args, rawCommand) => {
                int playerVehicle = API.GetVehiclePedIsIn(API.GetPlayerPed(-1), false);
                // Strings:
                string turnoffmaxSpeed = config.GetStringValue("NotificationString", "turnoffmaxSpeed", "fallback");

                // Need to figure out why it wont work with booleans!
                string usingOldNotificationSystem = config.GetStringValue("usingOldNotificationSystem", "usingOldNotificationSystem", "fallback");
                if (IsSpeedSet == true) {
                    if (Game.PlayerPed.IsInVehicle()) {
                        // Not a good solution but it works for now. 
                        // I will figure out soon why boolean wont work!
                        if (usingOldNotificationSystem == "false") {
                            // https://github.com/Switty6/swt_notifications 
                            TriggerEvent("simpe_commands:Notifications", "swt_notifications:Icon", turnoffmaxSpeed, true, "mdi-engine");
                        }
                        else if (usingOldNotificationSystem == "true") {
                            shownoti(turnoffmaxSpeed, true, -70);
                        }
                        IsSpeedSet = false;
                        API.SetVehicleMaxSpeed(playerVehicle, 0); // 0 means no maxspeed : In the future i wil make it so when you type /cc without any argument it will also disable the maxspeed
                    }
                }
            }), false);
        }

        private void toggleCar() {
            int playerVehicle = API.GetVehiclePedIsIn(API.GetPlayerPed(-1), false);
            if (Game.PlayerPed.IsInVehicle()) {
                if (AlreadyStopped == false) {

                    // https://github.com/Switty6/swt_notifications 
                    TriggerEvent("simpe_commands:Notifications", "swt_notifications:Icon", "Turning off Vehicle", "top-right", 1000, "dark", "grey-1", true, "mdi-engine-off");

                    AlreadyStopped = true;
                    API.SetVehicleEngineOn(playerVehicle, false, false, true);
                }
                else if (AlreadyStopped == true) {
                    TriggerEvent("simpe_commands:Notifications", "swt_notifications:Icon", "Turning on Vehicle", "top-right", 1000, "dark", "grey-1", true, "mdi-engine");

                    AlreadyStopped = false;
                    API.SetVehicleEngineOn(playerVehicle, true, false, true);
                }
            }
        }
        public static void shownoti(string message, bool beep, int duration) {
            API.AddTextEntry("CH_ALERT", message);
            API.BeginTextCommandDisplayHelp("CH_ALERT");
            API.EndTextCommandDisplayHelp(0, false, beep, duration);
            }
        }
    }
