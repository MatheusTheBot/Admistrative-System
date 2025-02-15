﻿using Domain.Commands.Resident;
using Domain.Entities;
using Domain.Handlers.Contracts;
using Domain.Repository;
using Domain.ValueObjects;
using Flunt.Notifications;

namespace Domain.Handlers;
public class ResidentHandler : Notifiable<Notification>,
        IHandler<CreateResidentCommand>,
        IHandler<ChangeNameResidentCommand>,
        IHandler<ChangeEmailResidentCommand>,
        IHandler<ChangePhoneNumberResidentCommand>,
        IHandler<ChangeDocumentResidentCommand>,
        IHandler<GenerateNewPasswordResidentCommand>,
        IHandler<ChangePasswordResidentCommand>
{
    private readonly IRepository<Resident> repos;
    public ResidentHandler(IRepository<Resident> repos)
    {
        this.repos = repos;
    }
    public IHandlerResult Handle(CreateResidentCommand command)
    {
        //fail fast validation
        if (!command.IsValid)
            return new HandlerResult(false, command.Notifications);

        Resident newResident = new(new Name(command.FirstName, command.LastName), command.Email, command.PhoneNumber, new Document(command.Type, command.DocumentNumber), command.Number, command.Block, command.Password);

        try
        {
            repos.Create(newResident);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }
        return new HandlerResult(true, newResident);
    }

    public IHandlerResult Handle(ChangeNameResidentCommand command)
    {
        if (!command.IsValid)
            return new HandlerResult(false, command.Notifications);

        //rehydration
        Resident? resident;
        try
        {
            resident = repos.GetById(command.Number, command.Block, command.Id);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        };
        if (resident == null)
            return new HandlerResult(false, "Resident not found");

        resident.ChangeName(new Name(command.FirstName, command.LastName));

        try
        {
            repos.Update(resident);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }
        return new HandlerResult(true, resident);
    }

    public IHandlerResult Handle(ChangeEmailResidentCommand command)
    {
        if (!command.IsValid)
            return new HandlerResult(false, command.Notifications);

        Resident? resident;
        try
        {
            resident = repos.GetById(command.Number, command.Block, command.Id);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }
        if (resident == null)
            return new HandlerResult(false, "Resident not found");

        resident.ChangeEmail(command.Email);

        try
        {
            repos.Update(resident);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }
        return new HandlerResult(true, resident);
    }

    public IHandlerResult Handle(ChangePhoneNumberResidentCommand command)
    {
        if (!command.IsValid)
            return new HandlerResult(false, command.Notifications);

        Resident? resident;
        try
        {
            resident = repos.GetById(command.Number, command.Block, command.Id);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }
        if (resident == null)
            return new HandlerResult(false, "Resident not found");

        resident.ChangePhoneNumber(command.PhoneNumber);

        try
        {
            repos.Update(resident);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }
        return new HandlerResult(true, resident);
    }

    public IHandlerResult Handle(ChangeDocumentResidentCommand command)
    {
        if (!command.IsValid)
            return new HandlerResult(false, command.Notifications);

        Resident? resident;
        try
        {
            resident = repos.GetById(command.Number, command.Block, command.Id);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }
        if (resident == null)
            return new HandlerResult(false, "Resident not found");

        resident.ChangeDocument(new Document(command.Type, command.DocumentNumber));

        try
        {
            repos.Update(resident);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }
        return new HandlerResult(true, resident);
    }
    public IHandlerResult Handle(ChangePasswordResidentCommand command)
    {
        if (!command.IsValid)
            return new HandlerResult(false, command.Notifications);

        Resident? Resident;
        try
        {
            Resident = repos.GetById(command.Id);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }
        if (Resident == null)
            return new HandlerResult(false, "Resident not found");

        Resident.ChangePassword(command.NewPassword);

        try
        {
            repos.Update(Resident);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }

        return new HandlerResult(true, Resident);
    }

    public IHandlerResult Handle(GenerateNewPasswordResidentCommand comm)
    {
        if (!comm.IsValid)
            return new HandlerResult(false, comm.Notifications);

        Resident? Resident;
        try
        {
            Resident = repos.GetById(comm.Id);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }
        if (Resident == null)
            return new HandlerResult(false, "Resident not found");

        Resident.ChangePassword(comm.NewPassword);

        try
        {
            repos.Update(Resident);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }

        return new HandlerResult(true, Resident);
    }
}