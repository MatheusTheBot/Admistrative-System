﻿using Domain.Entities;
using Domain.Queries;
using Domain.Repository;
using Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repository;
public class PackageRepository : IRepository<Packages>
{
    public DataContext Context { get; set; }
    public PackageRepository(DataContext context)
    {
        Context = context;
    }

    public void Create(Packages entity)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var sc = Context.Find<Apart>(entity.Number, entity.Block);
        if (sc != null)
        {
            Context.Packages.Add(entity);
            Context.SaveChanges();
        }
        watch.Stop();
        Console.WriteLine($"Create sync run in {watch.ElapsedMilliseconds}");
    }

    public void Delete(Packages entity)
    {
        var sc = Context.Find<Apart>(entity.Number, entity.Block);
        if (sc != null)
        {
            Context.Entry(entity).State = EntityState.Deleted;
            Context.Packages.Remove(entity);
            Context.SaveChanges();
        }
    }

    public Packages? GetById(Guid id)
    {
        return Context.Packages.FirstOrDefault(Queries<Packages>.GetById(id));
    }

    public Packages? GetById(int id, int id2)
    {
        return null;
    }

    public Packages? GetById(int Number, int Block, Guid id)
    {
        return Context.Packages.Where(x => x.Number == Number && x.Block == Block).FirstOrDefault(Queries<Packages>.GetById(id));
    }

    public void Update(Packages entity)
    {
        var sc = Context.Find<Apart>(entity.Number, entity.Block);
        if (sc != null)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.Packages.Update(entity);
            Context.SaveChanges();
        }
    }
}