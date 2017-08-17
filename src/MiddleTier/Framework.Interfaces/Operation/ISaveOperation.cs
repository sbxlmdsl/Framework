//-----------------------------------------------------------------------
// <copyright file="ISaveAsync.cs" company="Genesys Source">
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
using Genesys.Framework.Activity;
using Genesys.Framework.Entity;
using System;

namespace Genesys.Framework.Operation
{
    /// <summary>
    /// Write operation to a non-thread-safe datastore such as EF data context
    /// Includes all Save() and Delete() overloads, as well as Get..() methods
    /// </summary>
    [CLSCompliant(true)]
    public interface ISaveOperation<TEntity> : IGetOperation<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Saves the entity to the datastore
        /// </summary>
        /// <returns>Returns TEntity exactly as the data has been stored</returns>
        TEntity Save(TEntity entity, bool forceInsert);

        /// <summary>
        /// Inserts or Updates this object in the database
        /// </summary>
        /// <returns>Object updated and all current values as of the save</returns>
        TEntity Save(TEntity entity);

        /// <summary>
        /// Inserts or Updates this object with Workflow-based tracking.
        /// </summary>
        /// <param name="entity">Entity to be committed to the datastore</param>        
        /// <param name="activity">Activity record owning this process</param>        
        TEntity Save(TEntity entity, IActivityContext activity);

        /// <summary>
        /// Deletes the item from the datastore, using one or more identifiers in TEntity
        /// </summary>
        /// <returns></returns>
        bool Delete(TEntity entity);
    }
}
