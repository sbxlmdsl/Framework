//-----------------------------------------------------------------------
// <copyright file="ICrudEntity.cs" company="Genesys Source">
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
using System;
using Genesys.Foundation.Activity;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Genesys.Foundation.Entity
{
    /// <summary>
    /// Data access entity that can be saved
    /// </summary>
    /// <typeparam name="TEntity">Type of class supporting CRUD methods</typeparam>
    [CLSCompliant(true)]
    public interface ICrudEntity<TEntity> : ISaveableEntity
    {
        /// <summary>
        /// Create the object
        /// </summary>
        /// <returns></returns>
        ICrudEntity<TEntity> Create();

        /// <summary>
        /// Retrieve the object
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Read(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Update the object
        /// </summary>
        ICrudEntity<TEntity> Update();

        /// <summary>
        /// Deletes this object with automatic tracking.
        /// Less complicated than Delete(IActivity), but minimal data logged and free-form architecture
        /// </summary>
        ICrudEntity<TEntity> Delete();

        /// <summary>
        /// Inserts or updates this object with automatic tracking.
        /// Less complicated than Save(IActivity), but minimal data logged and free-form architecture
        /// </summary>
        ICrudEntity<TEntity> Save();
    }
}
