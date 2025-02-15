﻿using Domain.Commands.Contracts;
using Domain.Enums;
using Flunt.Notifications;
using System.Text.Json.Serialization;

namespace Domain.Commands.Packages;
public class ChangePackageTypeCommand : Notifiable<Notification>, ICommand
{
    [JsonConstructor]
    public ChangePackageTypeCommand(Guid guid, EPackageType type)
    {
        Guid = guid;
        Type = type;

        Validate();
    }

    public Guid Guid { get; set; }
    public EPackageType Type { get; set; }
    public void Validate()
    {
        if (Equals(Guid, Guid.Empty))
            AddNotification("Guid", "Empty Guid");
    }
}