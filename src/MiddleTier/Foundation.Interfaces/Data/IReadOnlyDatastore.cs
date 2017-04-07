//-----------------------------------------------------------------------
// <copyright file="IDatastore.cs" company="Genesys Source">
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
using Genesys.Foundation.Entity;
using System;
using System.Linq;

namespace Genesys.Foundation.Data
{
    /// <summary>
    /// Data, documents, information persistent storage repository
    /// </summary>
    [CLSCompliant(true)]
    public interface IReadOnlyDatastore<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Gets one or more items from the datastore
        ///  Usually implemented with a built-in Take/Top cap to avoid accidental huge selects
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// All data in this datastore subset, except records with default ID/Key
        ///  Criteria: Where ID != TypeExtension.DefaultInteger And Also Key != TypeExtension.DefaultGuid
        ///  Goal: To exclude "Not Selected" records from lookup tables
        /// </summary>
        IQueryable<TEntity> GetAllExcludeDefault();

        /// <summary>
        /// Gets one or no items based on exact ID match
        /// </summary>
        /// <returns>One or no TEntity based on exact ID match</returns>
        TEntity GetByID(int id);

        /// <summary>
        /// Gets one or no items based on exact Key match
        /// </summary>
        /// <returns>One or no TEntity based on exact Key match</returns>
        TEntity GetByKey(Guid key);
    }
}
