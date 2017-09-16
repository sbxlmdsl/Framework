//-----------------------------------------------------------------------
// <copyright file="CrudOperation.cs" company="Genesys Source">
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
using Genesys.Framework.Activity;
using Genesys.Framework.Data;
using Genesys.Framework.Operation;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Genesys.Framework.Entity
{
    /// <summary>
    ///  ReadOperation - Read-only operation of an entity against a connection to a datastore
    /// </summary>
    [CLSCompliant(true)]
    public abstract class CrudEntity<TEntity> : EntityInfo<TEntity>, ICrudOperation<TEntity> where TEntity : CrudEntity<TEntity>, new()
    {
        /// <summary>
        /// Class will throw exception if encountered
        /// </summary>
        public bool ThrowException { get; set; } = TypeExtension.DefaultBoolean;

        /// <summary>
        /// Forces initialization of EF-generated properties (PropertyValue = TypeExtension.Default{Type})
        /// </summary>
        public CrudEntity() : base()
        {
#if (DEBUG)
            ThrowException = true;
#endif
        }

        /// <summary>
        /// Creates this object in the database
        ///  ActivityContext record is auto-generated
        /// </summary>s
        /// <returns></returns>
        public virtual TEntity Create()
        {
            var returnValue = new TEntity();
            var db = SaveableDatabase<TEntity>.Construct();

            returnValue = db.Save(this.ToEntity(), forceInsert: true);

            return returnValue;
        }

        /// <summary>
        /// Creates this object in the database
        ///  ActivityContext record is supplied as parameter
        /// </summary>
        /// <param name="activity">Activity Context of this operation, used to track DB commits</param>
        /// <returns>Fresh TEntity pulled from the database, as DB might change some data automatically</returns>
        public virtual TEntity Create(IActivityContext activity)
        {
            var returnValue = new TEntity();
            var db = SaveableDatabase<TEntity>.Construct();

            this.ActivityContextID = activity.ActivityContextID;
            returnValue = db.Save(this.ToEntity(), forceInsert: true);

            return returnValue;
        }

        /// <summary>
        /// Reads this object from the database, using the passed predicate
        /// </summary>
        /// <returns>Objects found</returns>
        public virtual IQueryable<TEntity> Read()
        {
            var db = ReadOnlyDatabase<TEntity>.Construct();
            var returnValue = default(IQueryable<TEntity>);

            returnValue = db.Data;

            return returnValue;
        }

        /// <summary>
        /// Reads this object from the database, using the passed predicate
        /// </summary>
        /// <param name="whereClause">Expression to be used as where clause</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Read(Expression<Func<TEntity, bool>> whereClause)
        {
            return Read().Where(whereClause);
        }

        /// <summary>
        /// Updates this object in the database
        /// </summary>
        /// <returns>Object updated and all current values as of the save</returns>
        public virtual TEntity Update()
        {
            var db = SaveableDatabase<TEntity>.Construct();
            var returnValue = new TEntity();

            returnValue = db.Save(this.ToEntity(), forceInsert: false);

            return returnValue;
        }

        /// <summary>
        /// Updates this object in the database
        ///  ActivityContext record is supplied as parameter
        /// </summary>
        /// <param name="activity">Activity Context of this operation, used to track DB commits</param>
        /// <returns>Fresh TEntity pulled from the database, as DB might change some data automatically</returns>
        public virtual TEntity Update(IActivityContext activity)
        {           
            var db = SaveableDatabase<TEntity>.Construct();
            var returnValue = new TEntity();

            this.ActivityContextID = activity.ActivityContextID;
            returnValue = db.Save(this.ToEntity(), forceInsert: false);

            return returnValue;
        }

        /// <summary>
        /// Deletes this object with automatic tracking
        /// </summary>
        /// <returns>Object deleted and all current values. Returned values should be empty</returns>
        public virtual bool Delete()
        {
            var db = SaveableDatabase<TEntity>.Construct();
            var returnValue = TypeExtension.DefaultBoolean;

            returnValue = db.Delete(this.ToEntity());

            return returnValue;
        }

        /// <summary>
        /// Deletes this object from the database
        ///  ActivityContext record is supplied as parameter
        /// </summary>
        /// <param name="activity">Activity Context of this operation, used to track DB commits</param>
        /// <returns>Fresh TEntity pulled from the database, as DB might change some data automatically</returns>
        public virtual bool Delete(IActivityContext activity)
        {
            var db = SaveableDatabase<TEntity>.Construct();
            var returnValue = TypeExtension.DefaultBoolean;

            this.ActivityContextID = activity.ActivityContextID;
            returnValue = db.Delete(this.ToEntity());

            return returnValue;
        }
        
        /// <summary>
        /// Gets database record with exact ID match
        /// </summary>
        /// <param name="id">Database ID of the record to pull</param>
        /// <returns>Single entity that matches by id, or an empty entity for not found</returns>
        public static TEntity GetByID(int id)
        {
            var db = ReadOnlyDatabase<TEntity>.Construct();
            return db.GetByID(id);
        }

        /// <summary>
        /// Gets database record with exact Key match
        /// </summary>
        /// <param name="key">Database Key of the record to pull</param>
        /// <returns>Single entity that matches by Key, or an empty entity for not found</returns>
        public static TEntity GetByKey(Guid key)
        {
            var db = ReadOnlyDatabase<TEntity>.Construct();
            return db.GetByKey(key);
        }
    }
}
