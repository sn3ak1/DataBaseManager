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
            context.Users.Add(new User(){Name = name, PasswdHash = Encrypt(password)});
            context.SaveChanges();
            return true;
        }

        public static bool Login(string name, string password)
        {
            using var context = new Context();
            var user = context.Users
                .Include(x=>x.Role)
                .ThenInclude(x=>x.Permissions)
                .ThenInclude(x=>x.ModifiableCategories)
                .FirstOrDefault(x=>x.Name == name);
            if (user == null) return false;
            var verified = CheckHash(password, user?.PasswdHash);
            if (verified)
                LoggedAs = user;
            return verified;
        }

        public static IEnumerable<Role> GetRoles()
        {
            using var context = new Context();
            return context.Roles
                .Include(x => x.Users)
                .Include(x => x.Permissions)
                .ThenInclude(x => x.ModifiableCategories);
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