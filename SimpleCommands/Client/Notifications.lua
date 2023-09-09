RegisterNetEvent('simpe_commands:Notifications') -- This Event only works with swt notification type Icon...
AddEventHandler('simpe_commands:Notifications', function(swt_notificationsType, message,position,timeout,color,textColor,progress,icon)

    TriggerEvent(swt_notificationsType, message,position,timeout,color,textColor,progress,icon);

end)

