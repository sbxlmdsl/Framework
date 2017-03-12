//-----------------------------------------------------------------------
// <copyright file="CrudEntity.cs" company="Genesys Source">
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
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Reflection;
using Genesys.Foundation.Validation;
using Genesys.Extensions;
using Genesys.Foundation.Data;
using Genesys.Foundation.Activity;
using System.Collections.Generic;
using Genesys.Extras.Configuration;
using Genesys.Extras.Serialization;

namespace Genesys.Foundation.Entity
{
    /// <summary>
    /// CrudEntity
    /// </summary>
    /// <remarks>CrudEntity</remarks>
    [CLSCompliant(true)]
    public abstract class CrudEntity<TEntity> : Validator<CrudEntity<TEntity>>, ICrudEntity<TEntity>
        where TEntity : CrudEntity<TEntity>, new()
    {
        private const string connectionStringValue = "DefaultConnection";
        private const string activitySchemaValue = "Activity";

        /// <summary>
        /// ID of record
        /// </summary>
        public virtual int ID { get; set; } = TypeExtension.DefaultInteger;

        /// <summary>
        /// Guid of record
        /// </summary>
        public virtual Guid Key { get; set; } = TypeExtension.DefaultGuid;

        /// <summary>
        /// Workflow activity that created this record
        /// </summary>
        public virtual int ActivityID { get; set; } = TypeExtension.DefaultInteger;

        /// <summary>
        /// Date record was created
        /// </summary>
        public virtual DateTime CreatedDate { get; set; } = TypeExtension.DefaultDate;

        /// <summary>
        /// Date record was modified
        /// </summary>
        public virtual DateTime ModifiedDate { get; set; } = TypeExtension.DefaultDate;

        /// <summary>
        /// Status of this record
        /// </summary>
        public virtual RecordStatusValues Status { get; set; } = RecordStatusValues.Default;

        /// <summary>
        /// Class will throw exception if encountered
        /// </summary>
        public bool ThrowException { get; set; } = TypeExtension.DefaultBoolean;

        /// <summary>
        /// Is this a new object not yet committed to the database
        /// </summary>
        public virtual bool IsNew
        {
            get
            {
                return (this.ID == TypeExtension.DefaultInteger ? true : false);
            }
        }

        /// <summary>
        /// Forces initialization of EF-generated properties (PropertyValue = TypeExtension.Default{Type})
        /// </summary>
        public CrudEntity() : base()
        {
            this.Initialize<CrudEntity<TEntity>>();
#if (DEBUG)
            ThrowException = true;
#endif
        }

        /// <summary>
        /// Constructor with initialization
        /// </summary>
        protected CrudEntity(int newID)
            : this()
        {
            ID = newID;
        }

        /// <summary>
        /// Loads an existing object MyBased on ID.
        /// </summary>
        /// <param name="id">The unique ID of the object</param>
        public static TEntity GetByID(int id)
        {
            var dbContext = new DatabaseContext();
            var returnValue = new TEntity();

            try
            {
                if (id != TypeExtension.DefaultInteger) // Do not lookup on "no record" IDs
                {
                    returnValue = dbContext.EntityData.Where(x => x.ID == id).FirstOrDefaultSafe();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.Create(ex, typeof(TEntity), String.Format("CrudEntity.GetByID({0})", id.ToString()), connectionStringValue, activitySchemaValue);
#if (DEBUG)
                throw;
#endif
            }

            return returnValue;
        }

        /// <summary>
        /// Loads an existing object MyBased on ID.
        /// </summary>
        /// <param name="Key">The unique ID of the object</param>
        public static TEntity GetByKey(Guid Key)
        {
            var dbContext = new DatabaseContext();
            var returnValue = new TEntity();

            try
            {
                if (Key != TypeExtension.DefaultGuid) // Do not lookup on "no record" IDs
                {
                    returnValue = dbContext.EntityData.Where(x => x.Key == Key).FirstOrDefaultSafe();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.Create(ex, typeof(TEntity), String.Format("CrudEntity.GetByKey({0})", Key.ToString()), connectionStringValue, activitySchemaValue);
            }

            return returnValue;
        }

        /// <summary>
        /// Gets all from database
        /// Can add clauses, such as GetAll().Take(1), GetAll().Where(), etc.
        /// </summary>
        public static IQueryable<TEntity> GetAll()
        {
            var dbContext = new DatabaseContext();
            IQueryable<TEntity> returnValue = default(IQueryable<TEntity>);

            try
            {
                returnValue = dbContext.EntityData.Select(x => x).Take(new TEntity().GetAttributeValue<TakeRows, int>(100));
            }
            catch (Exception ex)
            {
                ExceptionLogger.Create(ex, typeof(TEntity), "CrudEntity.GetAll()", connectionStringValue, activitySchemaValue);
#if (DEBUG)
                throw;
#endif
            }

            return returnValue;
        }

        /// <summary>
        /// Retrieves data with purpose of displaying results over multiple pages (i.e. in Grid/table)
        /// </summary>
        /// <param name="whereClause">Expression for where clause</param>
        /// <param name="orderByClause">Expression for order by clause</param>
        /// <param name="pageSize">Size of each result</param>
        /// <param name="pageNumber">Page number</param>

        public IQueryable<TEntity> GetWithPaging(Expression<Func<TEntity, bool>> whereClause,
            Expression<Func<TEntity, bool>> orderByClause, int pageSize, int pageNumber)
        {
            var dbContext = new DatabaseContext();
            IQueryable<TEntity> result = null;
            result = (dbContext.EntityData.Select(x => x)).AsQueryable();
            if (whereClause != null) { result = result.Where<TEntity>(whereClause).AsQueryable(); }
            if (orderByClause != null) { result = result.OrderBy(orderByClause).AsQueryable(); }
            if (pageNumber > 0 && pageSize > 0)
                result = result.Skip((pageNumber * pageSize)).Take(pageSize).AsQueryable();
            return result;
        }

        /// <summary>
        /// Inserts this object into the database
        /// </summary>
        /// <returns>Object inserted and the newly assigned ID and Key</returns>
        public virtual ICrudEntity<TEntity> Create()
        {
            Save(forceInsert: true);
            return this;
        }

        /// <summary>
        /// Reads this object from the database, using the passed predicate
        /// </summary>
        /// <returns>Objects found</returns>
        public virtual IEnumerable<TEntity> Read(Expression<Func<TEntity, bool>> predicate)
        {
            return CrudEntity<TEntity>.GetAll().Where(predicate.Compile());
        }

        /// <summary>
        /// Updates this object in the database
        /// </summary>
        /// <returns>Object updated and all current values as of the save</returns>
        public virtual ICrudEntity<TEntity> Update()
        {
            Save(forceInsert: false);
            return this;
        }

        /// <summary>
        /// Inserts or Updates this object in the database
        /// </summary>
        /// <returns>Object updated and all current values as of the save</returns>
        public virtual ICrudEntity<TEntity> Save()
        {
            Save(forceInsert: false);
            return this;
        }

        /// <summary>
        /// Inserts or Updates this object with Workflow-based tracking.
        ///  More complicated than Save(), but verbose logged info and ensures full-tiered architecture
        /// </summary>
        /// <param name="activity">Activity record owning this process.</param>        
        public virtual bool Save(IActivityContext activity)
        {
            ActivityID = activity.ActivityContextID;
            
            return Save(forceInsert: false);
        }

        /// <summary>
        /// Worker that saves this object with automatic tracking.
        /// </summary>
        /// <param name="forceInsert">Ability to override insert vs. update and force insert</param>
        public virtual bool Save(bool forceInsert)
        {
            var dbContext = new DatabaseContext();
            var newItem = new TEntity();
            var returnValue = TypeExtension.DefaultBoolean;

            try
            {
                if (Equals(new TEntity()) == false)
                {
                    if (ActivityLogger.GetByID(this.ActivityID, connectionStringValue, activitySchemaValue).ActivityContextID == TypeExtension.DefaultInteger)
                        { ActivityID = ActivityLogger.Create(connectionStringValue, activitySchemaValue); } // All database commits require activity of some sort
                    if (IsNew == true || forceInsert == true || dbContext.DataAccessBehavior() == DataAccessBehaviorValues.InsertOnly)
                    {
                        if (dbContext.DataAccessBehavior() == DataAccessBehaviorValues.SelectOnly)
                        {
                            if (ThrowException) throw new System.Exception(String.Format("{0}: {1}", this.GetType().ToStringSafe(), "Inserts not allowed."));
                        }
                        dbContext.EntityData.Add((TEntity)this);
                    } else
                    {
                        if ((dbContext.DataAccessBehavior() != DataAccessBehaviorValues.AllAccess) | (dbContext.DataAccessBehavior() == DataAccessBehaviorValues.NoUpdate))
                        {
                            if (ThrowException) throw new System.Exception(String.Format("{0}: {1}", this.GetType().ToStringSafe(), "Updates not allowed."));
                        }
                        // Overlay new data onto existing DB record.
                        newItem = dbContext.EntityData.Find(this.ID);
                        newItem?.Fill((TEntity)this);
                    }
                    ValidateAll(this);
                    if (CanSave(this) == true)
                    {
                        dbContext.SaveChanges();
                    }
                    if (dbContext.DataConcurrency() == DataConcurrencyValues.Pessimistic)
                    {
                        newItem = CrudEntity<TEntity>.GetByID(this.ID); // Re-pull to load any data changed by the database (i.e. re-linking IDs, default values.)
                        this.Fill(newItem); // Fill current object from database item to absorb all current values
                    }
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
                returnValue = false;
#if DEBUG
                System.Diagnostics.Debugger.Break();
#endif
                ExceptionLogger.Create(ex, typeof(TEntity), String.Format("{0}.Save() in CrudEntity", this.GetType().ToStringSafe()), connectionStringValue, activitySchemaValue);                
                if (ThrowException) throw;
            }
            finally
            {
                dbContext.Dispose();
            }

            return returnValue;
        }

        /// <summary>
        /// Deletes this object with automatic tracking
        /// </summary>
        /// <returns>Object deleted and all current values. Returned values should be empty</returns>
        public virtual ICrudEntity<TEntity> Delete()
        {
            Delete(okToDelete: true);
            return this;
        }

        /// <summary>
        /// Deletes this object with Workflow-based tracking
        /// More complicated than Delete(), but verbose logged info and ensures full-tiered architecture
        /// </summary>
        /// <param name="activity">Activity record owning this process.</param>        
        public virtual bool Delete(IActivityContext activity)
        {
            ActivityID = activity.ActivityContextID;
            return Delete(okToDelete: true);
        }

        /// <summary>
        /// Worker that deletes this object with automatic tracking
        /// Less complicated than Delete(IFlowClass), but minimal data logged and free-form architecture
        /// </summary>
        /// <param name="okToDelete">Confirmation of the delete</param>
        /// <returns>Resulting Object.GetByID double-check</returns>
        public virtual bool Delete(bool okToDelete)
        {
            var dbContext = new DatabaseContext();
            var returnValue = TypeExtension.DefaultBoolean;

            try
            {
                if ((ID != TypeExtension.DefaultInteger) && okToDelete)
                {
                    if (ActivityLogger.GetByID(this.ActivityID, connectionStringValue, activitySchemaValue).ActivityContextID == TypeExtension.DefaultInteger)
                    {
                        ActivityID = ActivityLogger.Create(connectionStringValue, activitySchemaValue);
                    } // All database commits require activity of some sort
                    if ((dbContext.DataAccessBehavior() == DataAccessBehaviorValues.InsertOnly) & (dbContext.DataAccessBehavior() == DataAccessBehaviorValues.SelectOnly))
                    {
                        if (ThrowException) throw new System.Exception("Deletes not allowed.");
                    }
                    dbContext.EntityData.Remove(dbContext.EntityData.Where(x => x.ID == this.ID).FirstOrDefaultSafe());
                    dbContext.SaveChanges();
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
                ExceptionLogger.Create(ex, typeof(TEntity), String.Format("CrudEntity.Delete() on {0}", this.ToString()), connectionStringValue, activitySchemaValue);
                if (ThrowException) throw;
            }
            finally
            {
                dbContext.Dispose();
            }
            return returnValue;
        }

        /// <summary>
        /// Compares two objects
        /// </summary>
        /// <param name="newItem">Item to compare</param>
        public bool Equals(TEntity newItem)
        {
            bool returnValue = TypeExtension.DefaultBoolean;
            Type newObjectType = newItem.GetType();

            returnValue = true;
            foreach (var newObjectProperty in newObjectType.GetProperties())
            {
                PropertyInfo currentProperty = typeof(TEntity).GetProperty(newObjectProperty.Name);
                if (currentProperty.CanWrite == true)
                {
                    if (object.Equals(currentProperty.GetValue(this, null), newObjectProperty.GetValue(newItem, null)) == false)
                    {
                        returnValue = false;
                        break;
                    }
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Start with ID as string representation
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return ID.ToString();
        }

        /// <summary>
        /// DB Context - Entity Framework uses this to connect to the database
        /// </summary>
        protected class DatabaseContext : DbContext
        {
            /// <summary>
            /// Pulls the connection string from an attribute, used to feed DbContext the full EF connection string
            /// </summary>
            /// <returns></returns>
            internal static string ConnectionString()
            {
                var returnValue = TypeExtension.DefaultString;
                ConnectionStringSafe configConnectString = new ConnectionStringSafe();
                ConfigurationManagerFull configManager = new ConfigurationManagerFull();

                configConnectString = configManager.ConnectionString(connectionStringValue);
                returnValue = configConnectString.ToEF(typeof(TEntity));
                if (returnValue == TypeExtension.DefaultString) { throw new System.Exception("Connection string could not be found. A valid connection string required for data access."); }

                return returnValue;
            }

            /// <summary>
            /// Defines if object can select, insert, update and/or delete.
            /// </summary>
            internal DataAccessBehaviorValues DataAccessBehavior()
            {
                DataAccessBehaviorValues returnValue = DataAccessBehaviorValues.AllAccess;
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
            /// Concurrency model to follow in middle tier, and optionally in the data tier
            /// </summary>
            /// <returns></returns>
            internal DataConcurrencyValues DataConcurrency()
            {
                DataConcurrencyValues returnValue = DataConcurrencyValues.Pessimistic;
                var itemType = new TEntity();

                foreach (var item in itemType.GetType().GetCustomAttributes(false))
                {
                    if ((item is DataConcurrency) == true)
                    {
                        returnValue = ((DataConcurrency)item).Value;
                        break;
                    }
                }

                return returnValue;
            }

            /// <summary>
            /// Worker DbSet class that saves the entity
            /// </summary>
            public DbSet<TEntity> EntityData { get; set; }

            /// <summary>
            /// Class will throw exception if encountered
            /// </summary>
            public bool ThrowException { get; set; } = TypeExtension.DefaultBoolean;

            /// <summary>
            /// Constructor. Explicitly set database connection.
            /// </summary>

            public DatabaseContext()
                : base(ConnectionString())
            {
                // Throw exception always in debug mode
#if (DEBUG)
                ThrowException = true;
#endif
            }

            /// <summary>
            /// SaveChanges - Saves the object to the database
            /// </summary>

            public override int SaveChanges()
            {
                int returnValue = TypeExtension.DefaultInteger;

                try
                {
                    returnValue = base.SaveChanges();
                }
                catch (Exception ex)
                {
                    ExceptionLogger.Create(ex, typeof(TEntity), String.Format("CrudEntity.SaveChanges() on {0}", this.ToString()), connectionStringValue, activitySchemaValue);
                    if (ThrowException == true)
                    {
                        throw;
                    }
                }

                return returnValue;
            }
        }

        /// <summary>
        /// Initializes database for Code First classes
        /// </summary>
        protected class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
        {
            /// <summary>
            /// Sets default data
            /// </summary>
            /// <param name="context">User, device and app context</param>

            protected override void Seed(DatabaseContext context)
            {
                base.Seed(context);
            }
        }

        /// <summary>
        /// Serializes this object into a Json string
        /// </summary>

        public string Serialize()
        {
            JsonSerializer<TEntity> serializer = new JsonSerializer<TEntity>();
            return serializer.Serialize(this);
        }

        /// <summary>
        /// De-serializes a string into this object
        /// </summary>

        public static CrudEntity<TEntity> Deserialize(string jsonString)
        {
            JsonSerializer<TEntity> serializer = new JsonSerializer<TEntity>();
            return serializer.Deserialize(jsonString);
        }
    }
}
