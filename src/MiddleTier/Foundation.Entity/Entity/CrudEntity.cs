//-----------------------------------------------------------------------
// <copyright file="CrudOperation.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Extensions;
using Genesys.Foundation.Activity;
using Genesys.Foundation.Data;
using Genesys.Foundation.Operation;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Genesys.Foundation.Entity
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
        /// </summary>
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
    }
}