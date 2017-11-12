//-----------------------------------------------------------------------
// <copyright file="Customer.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      Licensed to the Apache Software Foundation (ASF) under one or more 
//      contributor license agreements.  See the NOTICE file distributed with 
//      this work for additional information regarding copyright ownership.
//      The ASF licenses this file to You under the Apache License, Version 2.0 
//      (the 'License'); you may not use this file except in compliance with 
//      the License.  You may obtain a copy of the License at 
//       
//        http://www.apache.org/licenses/LICENSE-2.0 
//       
//       Unless required by applicable law or agreed to in writing, software  
//       distributed under the License is distributed on an 'AS IS' BASIS, 
//       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
//       See the License for the specific language governing permissions and  
//       limitations under the License. 
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Extensions;
using System;

namespace Genesys.Framework.Test
{
    /// <summary>
    /// Simulates a customer business object for passing over Http and binding to screens
    /// </summary>
    public class Customer
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
        public Customer()
        {
        }
    }
}