//-----------------------------------------------------------------------
// <copyright file="SaveableDatabase.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Extensions;
using Genesys.Foundation.Activity;
using Genesys.Foundation.Entity;
using Genesys.Foundation.Operation;
using System;
using System.Data.Entity;
using System.Linq;

namespace Genesys.Foundation.Data
{
    /// <summary>
    /// EF DbContext for read-only GetBy* operations
    /// </summary>
    public class SaveableDatabase<TEntity> : ReadOnlyDatabase<TEntity>, ISaveOperation<TEntity> where TEntity : EntityInfo<TEntity>, new()
    {
        /// <summary>
        /// Defines if object can select, insert, update and/or delete.
        /// </summary>
        internal DataAccessBehaviors DataAccessBehavior()
        {
            DataAccessBehaviors returnValue = DataAccessBehaviors.AllAccess;
            var itemType = new TEntity();

            foreach (var item in itemType.GetType().GetCustomAttributes(false))
            {
                if ((item is DataAccessBehavior) == true)
                {
                    returnValue = ((DataAccessBehavior)item).Value;
                    break;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Constructs the class statically.
        ///  Cannot use new(), since we need Attribute decorations during base() construction
        /// </summary>
        /// <returns>Fully constructed SaveableDatabase</returns>
        public new static SaveableDatabase<TEntity> Construct()
        {
            return new SaveableDatabase<TEntity>();
        }

        /// <summary>
        /// Constructor. Explicitly set database connection.
        ///   Use .Construct() instead. Cannot use new() 
        ///   due to needing ConnectionStringName() attribute data before construction
        /// </summary>
        protected SaveableDatabase()
            : base(ReadOnlyDatabase<TEntity>.Construct().ConnectionString)
        {

#if (DEBUG)
            ThrowException = true;
#endif
        }

        /// <summary>
        /// Inserts or Updates this object in the database
        /// </summary>
        /// <returns>Object updated and all current values as of the save</returns>
        public virtual TEntity Save(TEntity entity)
        {
            return Save(entity, forceInsert: false);
        }

        /// <summary>
        /// Inserts or Updates this object with Workflow-based tracking.
        /// </summary>
        /// <param name="entity">Entity to be committed to the datastore</param>        
        /// <param name="activity">Activity record owning this process</param>        
        public virtual TEntity Save(TEntity entity, IActivityContext activity)
        {
            entity.ActivityContextID = activity.ActivityContextID;
            return Save(entity, forceInsert: false);
        }

        /// <summary>
        /// Worker that saves this object with automatic tracking.
        /// </summary>
        /// <param name="entity">Entity to be committed to the datastore</param>        
        /// <param name="forceInsert">Ability to override insert vs. update and force insert</param>
        public virtual TEntity Save(TEntity entity, bool forceInsert)
        {
            var returnValue = new TEntity();
            var connectionName = this.GetAttributeValue<ConnectionStringName>(ConnectionStringName.DefaultValue);
            var activitySchema = this.GetAttributeValue<DatabaseSchemaName>(DatabaseSchemaName.DefaultValue);
            var activity = new ActivityContext();

            try
            {
                activity = ActivityLogger.GetByID(entity.ActivityContextID, connectionName, activitySchema);

                if (entity.Equals(returnValue) == false)
                {
                    if (activity.ActivityContextID == TypeExtension.DefaultInteger)
                    {
                        entity.ActivityContextID = ActivityLogger.Create(connectionName, activitySchema);
                    }
                    if (entity.IsNew == true || forceInsert == true || this.DataAccessBehavior() == DataAccessBehaviors.InsertOnly)
                    {
                        if (this.DataAccessBehavior() == DataAccessBehaviors.SelectOnly)
                        {
                            if (ThrowException) throw new System.Exception(String.Format("{0}: {1}", this.GetType().ToStringSafe(), "Inserts not allowed."));
                        }
                        this.Data.Add((TEntity)entity);
                    } else
                    {
                        if ((this.DataAccessBehavior() != DataAccessBehaviors.AllAccess) | (this.DataAccessBehavior() == DataAccessBehaviors.NoUpdate))
                        {
                            if (ThrowException) throw new System.Exception(String.Format("{0}: {1}", this.GetType().ToStringSafe(), "Updates not allowed."));
                        }                        
                        returnValue = this.Data.Find(entity.ID); // Overlay new data onto existing DB record to establish correct context
                        returnValue?.Fill((TEntity)entity);
                    }
                    entity.ValidateAll(entity);
                    if (entity.CanSave(entity) == true)
                    {
                        this.SaveChanges();
                    }                    
                    returnValue = GetByID(entity.ID); // Re-pull clean object, exactly as the DB has stored
                    entity.Key = returnValue.Key; // ID auto updates object from SP, Key is not updated. Sync Key between return object and passed object.
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debugger.Break();
#endif
                ExceptionLogger.Create(ex, typeof(TEntity), String.Format("{0} SaveableDatabase.Save()", this.GetType().ToStringSafe()));
                if (ThrowException) throw;
            }
            finally
            {
                returnValue = returnValue ?? entity;
            }

            return returnValue;
        }

        /// <summary>
        /// EF DbContext.SaveChanges() - Saves the object to the database
        /// </summary>
        public override int SaveChanges()
        {
            var returnValue = TypeExtension.DefaultInteger;

            try
            {
                // Save
                returnValue = base.SaveChanges();
                // Clear DbSet tracking
                foreach (var x in this.Set<TEntity>())
                {
                    Entry(x).State = System.Data.Entity.EntityState.Detached;
                }
                this.Set<TEntity>().Load();
            }
            catch (Exception ex)
            {
                ExceptionLogger.Create(ex, typeof(TEntity), String.Format("SaveableDatabase.SaveChanges() on {0}", this.ToString()));
                if (ThrowException == true)
                {
                    throw;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Worker that deletes this object with automatic tracking
        /// </summary>
        /// <param name="entity">Entity to be removed from the datastore</param>        
        /// <returns>True if record deleted, false if not</returns>
        public virtual bool Delete(TEntity entity)
        {
            var returnValue = TypeExtension.DefaultBoolean;

            try
            {
                if (entity.ID != TypeExtension.DefaultInteger)
                {
                    if (ActivityLogger.GetByID(entity.ActivityContextID, ConnectionStringName.DefaultValue, DatabaseSchemaName.DefaultActivityValue).ActivityContextID == TypeExtension.DefaultInteger)
                    {
                        entity.ActivityContextID = ActivityLogger.Create(ConnectionStringName.DefaultValue, DatabaseSchemaName.DefaultActivityValue);
                    } // All database commits require activity of some sort
                    if ((this.DataAccessBehavior() == DataAccessBehaviors.InsertOnly) & (this.DataAccessBehavior() == DataAccessBehaviors.SelectOnly))
                    {
                        if (ThrowException) throw new System.Exception("Deletes not allowed.");
                    }
                    this.Data.Remove(this.Data.Where(x => x.ID == entity.ID).FirstOrDefaultSafe());
                    this.SaveChanges();
                } else
                {
                    this.Fill(new TEntity());
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                returnValue = false;
#if DEBUG
                System.Diagnostics.Debugger.Break();
#endif
                ExceptionLogger.Create(ex, typeof(TEntity), String.Format("WriteConnection.Delete() on {0}", this.ToString()));
                if (ThrowException) throw;
            }

            return returnValue;
        }

        /// <summary>
        /// Initializes database for Code First classes
        /// </summary>
        private class DatabaseInitializer : DropCreateDatabaseIfModelChanges<SaveableDatabase<TEntity>>
        {
            /// <summary>
            /// Sets default data
            /// </summary>
            /// <param name="context">User, device and app context</param>
            protected override void Seed(SaveableDatabase<TEntity> context)
            {
                base.Seed(context);
            }
        }
    }
}
