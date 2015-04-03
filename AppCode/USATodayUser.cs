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
    public class USATodayUser
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        /// <remarks></remarks>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance can write.
        /// </summary>
        /// <value><c>true</c> if this instance can write; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool CanWrite { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance has access.
        /// </summary>
        /// <value><c>true</c> if this instance has access; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool HasAccess { get; set; }
    }
}