using System;

namespace Models
{
    internal static class DatabaseContextInitializer
    {
        static DatabaseContextInitializer()
        {

        }

        internal static void Seed(DatabaseContext databaseContext)
        {
            InitialRoles(databaseContext);
        }

        #region Role
        public static void InitialRoles(DatabaseContext databaseContext)
        {
            InsertRole("7c245113-6653-440e-b6a3-beb2caa82bf7", "superadministrator", "راهبر ویژه", databaseContext);
            InsertRole("ddd6e5c9-7417-44b8-b4e5-e9bc796b4609", "administrator", "راهبر", databaseContext);
            InsertRole("bbce3864-b441-4e3d-9ed6-6df036a9d441", "customet", "مشتری", databaseContext);
            InsertRole("d7465bc0-e3e3-42d4-b7a3-d914593ab804", "seller", "فروشنده", databaseContext);
        }

        public static void InsertRole(string roleId, string roleName, string roleTitle, DatabaseContext databaseContext)
        {
            Guid id = new Guid(roleId);
            Role role = new Role();
            role.Id = id;
            role.Title = roleTitle;
            role.Name = roleName;
            role.CreationDate = DateTime.Now;
            role.IsActive = true;
            role.IsDeleted = false;

            databaseContext.Roles.Add(role);
            databaseContext.SaveChanges();
        }
        #endregion

       
    }
}
