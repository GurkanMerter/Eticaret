using eticaret_staj2.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace eticaret_staj2.Security
{
    public class PersonelRol : RoleProvider
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }
         
        public override string[] GetRolesForUser(string username)
        {

            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select Yetki from Personel where Eposta='" + username + "'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                var kullanici = dr["Yetki"];
                con.Close();
                return new string[] { kullanici.ToString() };
            }
            else
            {
                con.Close();
                return new string[] { }; 
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        void connectionString()
        {

            con.ConnectionString = "Data Source = MONSTERPC; database = ProjeDB; Integrated Security = False; User Id = MyUser; Password = MyUser123;";
        }
    }
}