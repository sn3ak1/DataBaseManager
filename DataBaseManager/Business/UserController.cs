using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DataBaseManager.Data;
using Microsoft.EntityFrameworkCore;

namespace DataBaseManager.Business
{
    public static class UserController
    {
        public static User LoggedAs { get; set; }

        public static bool AddUser(string name, string password)
        {
            using var context = new Context();
            context.Database.EnsureCreated();
            if (context.Users.FirstOrDefault(x => x.Name == name) != null) return false;
            var role = context.Roles.FirstOrDefault(x => x.Name == "user");
            if (role == null){
                role = new Role() {Name = "user"};
                context.Roles.Add(role);
            }
            context.Users.Add(new User(){Name = name, PasswdHash = Encrypt(password), Role = role});
            context.SaveChanges();
            return true;
        }

        public static bool Login(string name, string password)
        {
            using var context = new Context();
            var user = context.Users
                .Include(x=>x.Role)
                .FirstOrDefault(x=>x.Name == name);
            if (user == null) return false;
            var verified = CheckHash(password, user?.PasswdHash);
            if (verified)
                LoggedAs = user;
            return verified;
        }

        public static void DeleteUser(User user)
        {
            using (var context = new Context())
            {
                context.Users.Attach(user);
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }
        
        public static void EditUser(User user)
        {
            using (var context = new Context())
            {
                context.Users.Update(user);
                context.SaveChanges();
            }

            if (user.Id == LoggedAs.Id)
                LoggedAs = user;
        }
        
        public static void EditRole(Role role)
        {
            using (var context = new Context())
            {
                context.Roles.Update(role);
                context.SaveChanges();
            }
        }

        public static void AddRole(Role role)
        {
            using var context = new Context();
            context.Database.EnsureCreated();
            context.Roles.Add(role);
            context.SaveChanges();
        }
        
        public static void DeleteRole(Role role)
        {
            using (var context = new Context())
            {
                context.Roles.Attach(role);
                context.Roles.Remove(role);
                context.SaveChanges();
            }
        }

        public static User[] GetUsers()
        {
            using var context = new Context();
            return context.Users
                .Include(x=>x.Role)
                .ToArray();
        }

        public static Role[] GetRoles()
        {
            using var context = new Context();
            return context.Roles
                .Include(x => x.Users).ToArray();
        }

        public static string Encrypt(string password)
        {   
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            var hash = pbkdf2.GetBytes(20);
            
            var hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            
            var savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
        }

        public static bool CheckHash(string password, string savedPasswordHash)
        {
            var hashBytes = Convert.FromBase64String(savedPasswordHash);
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            var hash = pbkdf2.GetBytes(20);
            for (var i=0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }
    }
}