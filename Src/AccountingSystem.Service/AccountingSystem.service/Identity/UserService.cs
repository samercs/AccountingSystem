using AccountingSystem.Data;
using AccountingSystem.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace AccountingSystem.Service.Identity
{
    public class UserService : ServiceBase
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;

        public UserService(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            _userManager = new UserManager(dataContextFactory);
            _roleManager = new RoleManager(dataContextFactory);
        }

        public async Task<User> GetUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        //public async Task<User> GetUserByEmail(string email)
        //{
        //    return await _userManager.FindByEmailAsync(email);
        //}

        public async Task<User> GetUserByEmail(string emailOrPhone)
        {
            return await _userManager.FindByEmailAsync(emailOrPhone);
        }

        public async Task<IReadOnlyCollection<User>> GetUsers(string search = "", string role = null)
        {
            return await GetUsersQuery(search, role).ToListAsync();
        }

        public T GetUsersByQuery<T>(string search, string role, Func<IQueryable<User>, T> processQueryable)
        {
            var users = GetUsersQuery(search, role);
            return processQueryable(users);
        }

        private IQueryable<User> GetUsersQuery(string search, string role)
        {
            var users = _userManager.Users;

            if (!string.IsNullOrWhiteSpace(search))
            {
                users = users.Where(i =>
                    i.FirstName.Contains(search) ||
                    i.LastName.Contains(search) ||
                    i.UserName.Contains(search) ||
                    i.Email.Contains(search));
            }

            if (!string.IsNullOrEmpty(role))
            {
                var identityRole = _roleManager.FindByName(role);
                if (identityRole == null)
                {
                    throw new ArgumentException("Cannot find role '" + role + "'.");
                }

                users = users.Where(i => i.Roles.Select(r => r.RoleId).Contains(identityRole.Id));
            }

            return users.OrderByDescending(i => i.LastName)
                .ThenBy(i => i.FirstName);
        }

        public async Task<IdentityResult> CreateUser(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> UpdateUser(User user)
        {
            if (!(await ValidateUserIdentity(user.Email, user.PhoneNumber, user.Id)))
            {
                return new IdentityResult("Email or phone number already exist in database");
            }

            using (var dc = DataContext())
            {
                dc.SetModified(user);
                await dc.SaveChangeAsyn();
                return IdentityResult.Success;
            }
        }

        public async Task<IdentityResult> ChangePassword(IIdentity identity, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(identity.GetUserId(), oldPassword, newPassword);
        }

        public string HashPassword(string password)
        {
            return _userManager.PasswordHasher.HashPassword(password);
        }

        public async Task<IReadOnlyCollection<IdentityRole>> GetAllRoles()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<IReadOnlyCollection<string>> GetRolesForUser(string userId)
        {
            return (await _userManager.GetRolesAsync(userId)).ToList();
        }

        public async Task<IEnumerable<User>> GetUsersInRole(string role)
        {
            var userRole = await _roleManager.FindByNameAsync(role);
            var userIds = userRole.Users.Select(i => i.UserId).ToList();
            var users = _userManager.Users.Where(i => userIds.Contains(i.Id));
            return await users.ToListAsync();
        }

        public async Task<bool> UserIsInRole(User user, string role)
        {
            return await _userManager.IsInRoleAsync(user.Id, role);
        }

        public async Task AddUserToRole(string userId, string roleName)
        {
            await _userManager.AddToRoleAsync(userId, roleName);
        }

        public async Task RemoveUserFromRole(string userId, string roleName)
        {
            await _userManager.RemoveFromRoleAsync(userId, roleName);
        }

        public async Task<bool> ValidateUserIdentity(string email, string phoneNumber, string userId = null)
        {
            var validEmail = true;
            var validPhone = true;
            var users = _userManager.Users;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                users = users.Where(i => i.Id != userId);
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                var user = await users.FirstOrDefaultAsync(i => i.Email == email);
                validEmail = user == null;
            }

            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                var user = await users.FirstOrDefaultAsync(i => i.PhoneNumber == phoneNumber);
                validPhone = user == null;
            }

            return validEmail && validPhone;

        }

    }
}
