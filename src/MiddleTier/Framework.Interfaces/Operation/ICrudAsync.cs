//-----------------------------------------------------------------------
// <copyright file="ICrudAsync.cs" company="Genesys Source">
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
using Genesys.Framework.Entity;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Genesys.Framework.Operation
{
    /// <summary>
    /// CRUD operations
    /// Create, Read, Update, Delete.
    ///  Purpose is to encapsulate IGetAsync and CRUD methods 
    ///   for asyncronous datastore access, primarily via Http
    /// </summary>
    /// <typeparam name="TEntity">Type of class supporting CRUD methods</typeparam>
    /// <typeparam name="TID">Type of identifier for this record. Typically int ID or Guid Key</typeparam>
    [CLSCompliant(true)]
    public interface ICrudAsync<TEntity, TID> : ISaveAsync<TEntity, TID> where TEntity : IEntity where TID : struct
    {
        /// <summary>
        /// Create operation on the object
        /// </summary>
        /// <returns></returns>
        Task<TEntity> CreateAsync();

        /// <summary>
        /// Retrieve TEntity objects operation
        /// </summary>
        /// <param name="key">Expression to query the datastore</param>
        /// <returns></returns>
        Task<TEntity> ReadAsync(TID key);

        /// <summary>
        /// Update the object
        /// </summary>
        Task<TEntity> UpdateAsync();

        /// <summary>
        /// Deletes operation on this entity
        /// </summary>
        Task<bool> DeleteAsync();      
    }
}
