//-----------------------------------------------------------------------
// <copyright file="CustomerSearch.cs" company="Genesys Source">
//      Copyright (c) 2017 Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Extensions;
using System;
using System.Collections.Generic;

namespace Genesys.Framework.Test
{
    /// <summary>
    /// Simulates a customer business object search class, for passing over Http and binding to screens
    /// </summary>
    public class CustomerSearch
    {
        public int ID { get; set; } = TypeExtension.DefaultInteger;
        public Guid Key { get; set; } = TypeExtension.DefaultGuid;
        public string FirstName { get; set; } = TypeExtension.DefaultString;
        public string MiddleName { get; set; } = TypeExtension.DefaultString;
        public string LastName { get; set; } = TypeExtension.DefaultString;
        public DateTime BirthDate { get; set; } = TypeExtension.DefaultDate;
        public int GenderID { get; set; } = TypeExtension.DefaultInteger;
        public Guid CustomerTypeKey { get; set; } = TypeExtension.DefaultGuid;
        public DateTime CreatedDate { get; set; } = TypeExtension.DefaultDate;
        public DateTime ModifiedDate { get; set; } = TypeExtension.DefaultDate;
        public int ActivityContextID { get; set; } = TypeExtension.DefaultInteger;
        public List<Customer> Results { get; set; } = new List<Customer>();
        public CustomerSearch()
                : base()
        {
        }
    }
}
