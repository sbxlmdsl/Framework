//-----------------------------------------------------------------------
// <copyright file="IEntity.cs" company="Genesys Source">
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
using Genesys.Extras.Serialization;
using System;

namespace Genesys.Framework.Entity
{
    /// <summary>
    /// Base used for all entity classes
    /// </summary>    
    [CLSCompliant(true)]
    public interface IEntity : IID, IKey, ICreatedDate, IModifiedDate, ISerialize<IEntity>
    {
        /// <summary>
        /// Is a new object, and most likely not yet committed to the database
        /// </summary>
        bool IsNew { get; }

        /// <summary>
        /// ActivityFlowID
        /// </summary>
        int ActivityContextID { get; set; }

        /// <summary>
        /// Status of this record with static values: 0x0 - Default, 0x1 - ReadOnly, 0x2 - Historical, 0x4 - Deleted
        /// </summary>
        EntityStates Status { get; set; }
    }
}
