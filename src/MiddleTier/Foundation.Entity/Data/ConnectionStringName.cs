//-----------------------------------------------------------------------
// <copyright file="ConnectionString.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using Genesys.Extensions;

namespace Genesys.Foundation.Data
{
    /// <summary>
    /// Class attribute decoration that holds the ConnectionStringName 
    /// Name is the key used to lookup connection string from config file.
    /// </summary>    
    [AttributeUsage(AttributeTargets.All)]
    public class ConnectionStringName : Attribute, IAttributeValue<string>
    {
        public static readonly string DefaultValue = "DefaultConnection";

        /// <summary>
        /// Name supplied by attribute.
        /// Default is DefaultConnection
        /// </summary>
        public string Value { get; set; } = DefaultValue;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionStringValue">Connection string name</param>
        public ConnectionStringName(string connectionStringValue)
        {
            Value = connectionStringValue;
        }
    }
}
