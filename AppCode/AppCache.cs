using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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

			// If user's identity does not exist or the user isn't assigned to any groups, just return with empty set.
			var userIdentity = HttpContext.Current.Request.LogonUserIdentity;
			if (userIdentity == null || userIdentity.Groups == null)
			{
				return groups;
			}

			// Return set of groups user belongs to.
			foreach (IdentityReference group in userIdentity.Groups)
			{
				try
				{
					// Only get groups that can be cast to a user or group account.  Ignore other types.
					var ntGroup = @group.Translate(typeof (NTAccount));
					groups.Add(ntGroup.ToString());
				}
				catch (Exception ex)
				{
					// Basically this just eats the exception since this is in a web application, but allows a debugger breakpoint to be set.
					// This can be researched further if it's ever needed.
					Console.WriteLine("Could not translate group: {0}", @group);
				}
			}

	        return groups;
        }

    }
}