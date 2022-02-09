﻿using Domain.Commands.Resident;
using Domain.Entities;
using Domain.Handlers.Contracts;
using Domain.Repository;
using Domain.ValueObjects;
using Flunt.Notifications;

namespace Domain.Handlers;
public class ResidentHandler :Notifiable<Notification>,
        IHandler<CreateResidentCommand>,
        IHandler<ChangeNameResidentCommand>,
        IHandler<ChangeEmailResidentCommand>,
        IHandler<ChangePhoneNumberResidentCommand>,
        IHandler<ChangeDocumentResidentCommand>
{
    private readonly IRepository<Resident> repos;
    public ResidentHandler(IRepository<Resident> repos)
    {
        this.repos = repos;
    }

    //
    //question/todo: How to see errors in Handlers and alert the user?
    //

    public IHandlerResult Handle(CreateResidentCommand command)
    {
        //fail fast validation
        if (!command.IsValid)
            return new HandlerResult(true, command.Notifications);

        Resident newResident = new Resident(new Name(command.FirstName, command.LastName), command.Email, command.PhoneNumber, new Document(command.Type, command.DocumentNumber));

        try
        {
            repos.Create(newResident);
        }
        catch
        {
            return new HandlerResult(false, "Internal Error");
        }
        return new HandlerResult(true, newResident);
    }

    public IHandlerResult Handle(ChangeNameResidentCommand command)
    {
        if (!command.IsValid)
            return new HandlerResult(true, command.Notifications);

        //rehydration
        var resident = repos.GetById(command.Id);
        if (resident == null)
            return new HandlerResult(false, "Resident not found");

        resident.ChangeName(new Name(command.FirstName, command.LastName));

        try
        {
            repos.Update(resident);
        }
        catch
        {
            return new HandlerResult(false, "Internal Error");
        }
        return new HandlerResult(true, resident);
    }

    public IHandlerResult Handle(ChangeEmailResidentCommand command)
    {
        if (!command.IsValid)
            return new HandlerResult(true, command.Notifications);

        var resident = repos.GetById(command.Id);
        if (resident == null)
            return new HandlerResult(false, "Resident not found");

        resident.ChangeEmail(command.Email);

        try
        {
            repos.Update(resident);
        }
        catch
        {
            return new HandlerResult(false, "Internal Error");
        }
        return new HandlerResult(true, resident);
    }

    public IHandlerResult Handle(ChangePhoneNumberResidentCommand command)
    {
        if (!command.IsValid)
            return new HandlerResult(true, command.Notifications);

        var resident = repos.GetById(command.Id);
        if (resident == null)
            return new HandlerResult(false, "Resident not found");

        resident.ChangePhoneNumber(command.PhoneNumber);

        try
        {
            repos.Update(resident);
        }
        catch
        {
            return new HandlerResult(false, "Internal Error");
        }
        return new HandlerResult(true, resident);
    }

    public IHandlerResult Handle(ChangeDocumentResidentCommand command)
    {
        if (!command.IsValid)
            return new HandlerResult(true, command.Notifications);

        var resident = repos.GetById(command.Id);
        if (resident == null)
            return new HandlerResult(false, "Resident not found");

        resident.ChangeDocument(new Document(command.Type, command.DocumentNumber));

        try
        {
            repos.Update(resident);
        }
        catch
        {
            return new HandlerResult(false, "Internal Error");
        }
        return new HandlerResult(true, resident);
    }
}