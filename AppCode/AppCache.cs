using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace USATodayBookList.AppCode
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class AppCache
    {

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="UserName">Name of the user.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static USATodayUser GetUser(string UserName)
        {
            if (HttpContext.Current.Cache["User" + UserName] == null)
            {

                List<string> groups = GetGroupMembership();

                String readWriteGroup = System.Configuration.ConfigurationManager.AppSettings["ReadWriteGroup"];
                String readOnlyGroup = System.Configuration.ConfigurationManager.AppSettings["ReadOnlyGroup"];

                USATodayUser u = new USATodayUser();
                u.UserName = UserName;

                if (groups.Contains(readWriteGroup))
                {
                    u.CanWrite = true;
                    u.HasAccess = true;
                }
                else if (groups.Contains(readOnlyGroup))
                {
                    u.CanWrite = false;
                    u.HasAccess = true;
                }
                else
                {
                    u.CanWrite = false;
                    u.HasAccess = false;
                }

                HttpContext.Current.Cache["User" + UserName] = u;

                return u;
            }
            else
            {
                return (USATodayUser)HttpContext.Current.Cache["User" + UserName];
            }
        }

        /// <summary>
        /// Gets the group membership.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private static List<string> GetGroupMembership()
        {
            List<string> groups = new List<string>();
            foreach (System.Security.Principal.IdentityReference group in
            System.Web.HttpContext.Current.Request.LogonUserIdentity.Groups)
            {
                groups.Add(group.Translate(typeof
                (System.Security.Principal.NTAccount)).ToString());
            }

            return groups;
        }

    }
}