using Microsoft.EntityFrameworkCore;
using SharedModels;
using UserApi.Core.Models;
using UserApi.Domain.Repositories;

namespace UserApi.Infrastructure.EfCore.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserApiContext db;
    
    public UserRepository(UserApiContext context)
    {
        db = context;
    }

    public User Add(User entity)
    {
        var newUser = db.Users.Add(entity).Entity;
        db.SaveChanges();
        return newUser;
    }

    public IEnumerable<User> GetAll()
    {
        return db.Users;
    }

    public User Get(int id)
    {
        return db.Users.FirstOrDefault(u => u.Id == id)!;
    }

    public User Edit(User entity)
    {
        db.Entry(entity).State = EntityState.Modified;
        db.SaveChanges();
        return entity;
    }

    public User Remove(User entity)
    {
        var userRemove = db.Users.FirstOrDefault(entity);
        db.Users.Remove(userRemove);
        db.SaveChanges();
        return userRemove;
    }

    public User FindByUsername(string username)
    {
        return db.Users.FirstOrDefault(u => u.Username == username)!;
    }
}