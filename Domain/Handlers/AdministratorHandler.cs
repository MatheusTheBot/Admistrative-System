﻿using Domain.Commands.Administrator;
using Domain.Entities;
using Domain.Handlers.Contracts;
using Domain.Repository;
using Domain.ValueObjects;
using Flunt.Notifications;

namespace Domain.Handlers;
public class AdministratorHandler : Notifiable<Notification>,
        IHandler<CreateAdministratorCommand>,
        IHandler<ChangeNameAdministratorCommand>,
        IHandler<ChangeEmailAdministratorCommand>,
        IHandler<ChangePhoneNumberAdministratorCommand>,
        IHandler<ChangeDocumentAdministratorCommand>,
        IHandler<ChangePasswordAdministratorCommand>,
        IHandler<GenerateNewPasswordAdminCommand>
{
    private readonly IRepository<Administrator> repos;
    public AdministratorHandler(IRepository<Administrator> repos)
    {
        this.repos = repos;
    }
    public IHandlerResult Handle(CreateAdministratorCommand command)
    {
        //fail fast validation
        if (!command.IsValid)
            return new HandlerResult(false, command.Notifications);

        Administrator newAdministrator = new(new Name(command.FirstName, command.LastName), new Document(command.Type, command.DocumentNumber), command.Email, command.PhoneNumber, command.Password);

        try
        {
            repos.Create(newAdministrator);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }
        return new HandlerResult(true, newAdministrator);
    }

    public IHandlerResult Handle(ChangeNameAdministratorCommand command)
    {
        if (!command.IsValid)
            return new HandlerResult(false, command.Notifications);

        //rehydration
        Administrator? Administrator;
        try
        {
            Administrator = repos.GetById(command.Id);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        };
        if (Administrator == null)
            return new HandlerResult(false, "Administrator not found");

        Administrator.ChangeName(new Name(command.FirstName, command.LastName));

        try
        {
            repos.Update(Administrator);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to update data, unable to perform requested operation");
        }

        try
        {
            Administrator = repos.GetById(command.Id);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        };
        if (Administrator == null)
            return new HandlerResult(false, "Administrator was updated, but now we can't search for him");

        return new HandlerResult(true, Administrator);
    }

    public IHandlerResult Handle(ChangeEmailAdministratorCommand command)
    {
        if (!command.IsValid)
            return new HandlerResult(false, command.Notifications);

        Administrator? Administrator;
        try
        {
            Administrator = repos.GetById(command.Id);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }
        if (Administrator == null)
            return new HandlerResult(false, "Administrator not found");

        Administrator.ChangeEmail(command.Email);

        try
        {
            repos.Update(Administrator);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }

        try
        {
            Administrator = repos.GetById(command.Id);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        };
        if (Administrator == null)
            return new HandlerResult(false, "Administrator was updated, but now we can't search for him");

        return new HandlerResult(true, Administrator);
    }

    public IHandlerResult Handle(ChangePhoneNumberAdministratorCommand command)
    {
        if (!command.IsValid)
            return new HandlerResult(false, command.Notifications);

        Administrator? Administrator;
        try
        {
            Administrator = repos.GetById(command.Id);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }
        if (Administrator == null)
            return new HandlerResult(false, "Administrator not found");

        Administrator.ChangePhoneNumber(command.PhoneNumber);

        try
        {
            repos.Update(Administrator);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }

        try
        {
            Administrator = repos.GetById(command.Id);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        };
        if (Administrator == null)
            return new HandlerResult(false, "Administrator was updated, but now we can't search for him");
        return new HandlerResult(true, Administrator);
    }

    public IHandlerResult Handle(ChangeDocumentAdministratorCommand command)
    {
        if (!command.IsValid)
            return new HandlerResult(false, command.Notifications);

        Administrator? Administrator;
        try
        {
            Administrator = repos.GetById(command.Id);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }
        if (Administrator == null)
            return new HandlerResult(false, "Administrator not found");

        Administrator.ChangeDocument(new Document(command.Type, command.DocumentNumber));

        try
        {
            repos.Update(Administrator);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }

        try
        {
            Administrator = repos.GetById(command.Id);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        };
        if (Administrator == null)
            return new HandlerResult(false, "Administrator was updated, but now we can't search for him");
        return new HandlerResult(true, Administrator);
    }

    public IHandlerResult Handle(ChangePasswordAdministratorCommand command)
    {
        if (!command.IsValid)
            return new HandlerResult(false, command.Notifications);

        Administrator? Administrator;
        try
        {
            Administrator = repos.GetById(command.Id);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }
        if (Administrator == null)
            return new HandlerResult(false, "Administrator not found");

        Administrator.ChangePassword(command.NewPassword);

        try
        {
            repos.Update(Administrator);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }

        return new HandlerResult(true, Administrator);
    }

    public IHandlerResult Handle(GenerateNewPasswordAdminCommand comm)
    {
        if (!comm.IsValid)
            return new HandlerResult(false, comm.Notifications);

        Administrator? Administrator;
        try
        {
            Administrator = repos.GetById(comm.Id);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }
        if (Administrator == null)
            return new HandlerResult(false, "Administrator not found");

        Administrator.ChangePassword(comm.NewPassword);

        try
        {
            repos.Update(Administrator);
        }
        catch (Exception)
        {
            return new HandlerResult(false, "Unable to access database, unable to perform requested operation");
        }

        return new HandlerResult(true, Administrator);
    }
}