//-----------------------------------------------------------------------
// <copyright file="SaveableEntity.cs" company="Genesys Source">
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
using Genesys.Extensions;
using Genesys.Extras.Exceptions;
using Genesys.Extras.Serialization;
using Genesys.Foundation.Entity;
using Genesys.Foundation.Validation;
using Genesys.Foundation.Activity;
using Genesys.Extras.Configuration;
using Genesys.Foundation.Data;

namespace Genesys.Foundation.Entity
{
    /// <summary>
    /// SaveableEntity
    /// </summary>
    /// <remarks>SaveableEntity</remarks>
    [CLSCompliant(true)]
    public abstract class SaveableEntity<TEntity> : Validator<SaveableEntity<TEntity>>, ISaveableEntity
        where TEntity : SaveableEntity<TEntity>, new()
    {
        private const string connectionStringDefault = "MyCodeConnection";

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
        public virtual int Status { get; set; }

        /// <summary>
        /// Class will throw exception if encountered
        /// </summary>
        public bool ThrowException { get; set; } = TypeExtension.DefaultBoolean;

        /// <summary>
        /// Is a new object, and most likely not yet committed to the database
        /// </summary>
        public virtual bool IsNew
        {
            get
            {
                return (this.ID == TypeExtension.DefaultInteger ? true : false) && (this.Key == TypeExtension.DefaultGuid ? true : false);
            }
        }

        /// <summary>
        /// Forces initialization of EF-generated properties (PropertyValue = TypeExtension.Default{Type})
        /// </summary>
        public SaveableEntity() : base()
        {
            this.Initialize<SaveableEntity<TEntity>>();
#if (DEBUG)
            this.ThrowException = true;
#endif
        }

        /// <summary>
        /// Constructor with initialization
        /// </summary>
        protected SaveableEntity(int newID)
            : this()
        {
            this.ID = newID;
        }

        /// <summary>
        /// Loads an existing object MyBased on ID.
        /// </summary>
        /// <param name="id">The unique ID of the object</param>
        public static TEntity GetByID(int id)
        {
            DatabaseContext dbContext = new DatabaseContext();
            TEntity returnValue = new TEntity();

            try
            {
                if (id != TypeExtension.DefaultInteger) // Do not lookup on "no record" IDs
                {
                    returnValue = dbContext.EntityData.Where(x => x.ID == id).FirstOrDefaultSafe();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.Create(ex, typeof(TEntity), String.Format("SaveableEntity.GetByID({0})", id.ToString()), "MyLogConnection", "MiddleTier");
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
            DatabaseContext dbContext = new DatabaseContext();
            TEntity returnValue = new TEntity();

            try
            {
                if (Key != TypeExtension.DefaultGuid) // Do not lookup on "no record" IDs
                {
                    returnValue = dbContext.EntityData.Where(x => x.Key == Key).FirstOrDefaultSafe();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.Create(ex, typeof(TEntity), String.Format("SaveableEntity.GetByKey({0})", Key.ToString()), "MyLogConnection", "MiddleTier");
            }

            return returnValue;
        }

        /// <summary>
        /// Gets all from database. Use .ToList() to convert to Generic.List(Of EntityDataType)
        /// </summary>
        public static IQueryable<TEntity> GetAll()
        {
            DatabaseContext dbContext = new DatabaseContext();
            IQueryable<TEntity> returnValue = default(IQueryable<TEntity>);

            try
            {
                returnValue = dbContext.EntityData.Select(x => x);
            }
            catch (Exception ex)
            {
                ExceptionLogger.Create(ex, typeof(TEntity), "SaveableEntity.GetAll()", "MyLogConnection", "MiddleTier");
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

        public IQueryable<TEntity> GetWithPaging(Expression<Func<TEntity, Boolean>> whereClause,
            Expression<Func<TEntity, Boolean>> orderByClause, int pageSize, int pageNumber)
        {
            DatabaseContext dbContext = new DatabaseContext();
            IQueryable<TEntity> result = null;
            result = (dbContext.EntityData.Select(x => x)).AsQueryable();
            if (whereClause != null) { result = result.Where<TEntity>(whereClause).AsQueryable(); }
            if (orderByClause != null) { result = result.OrderBy(orderByClause).AsQueryable(); }
            if (pageNumber > 0 && pageSize > 0)
                result = result.Skip((pageNumber * pageSize)).Take(pageSize).AsQueryable();
            return result;
        }

        /// <summary>
        /// Saves this object with automatic tracking.
        /// Less complicated than Save(IFlowClass), but minimal data logged and free-form architecture
        /// </summary>
        public virtual int Save()
        {
            return this.Save(false);
        }

        /// <summary>
        /// Saves this object with Workflow-based tracking.
        /// More complicated than Save(), but verbose logged info and ensures full-tiered architecture
        /// </summary>
        /// <param name="activity">Activity record owning this process.</param>        
        public virtual int Save(IActivity activity)
        {
            this.ActivityID = activity.ActivityID;
            return this.Save(false);
        }

        /// <summary>
        /// Saves object to database
        /// </summary>
        /// <param name="forceInsert">Ability to override insert vs. update and force insert</param>
        protected virtual int Save(bool forceInsert)
        {
            DatabaseContext dbContext = new DatabaseContext();
            TEntity newItem = new TEntity();

            try
            {
                if (this.Equals(new TEntity()) == false)
                {
                    if (ActivityLogger.GetByID(this.ActivityID, "MyDataConnection", "Activity").ActivityID == TypeExtension.DefaultInteger)
                        { this.ActivityID = ActivityLogger.Create("MyDataConnection", "Activity"); } // All database commits require activity of some sort
                    if (this.IsNew == true || forceInsert == true || dbContext.DataAccessBehavior() == DataAccessBehaviorValues.InsertOnly)
                    {
                        if (dbContext.DataAccessBehavior() == DataAccessBehaviorValues.SelectOnly)
                        {
                            if (this.ThrowException) throw new System.Exception(String.Format("{0}: {1}", this.GetType().ToStringSafe(), "Inserts not allowed."));
                        }
                        dbContext.EntityData.Add((TEntity)this);
                    } else
                    {
                        if ((dbContext.DataAccessBehavior() != DataAccessBehaviorValues.AllAccess) | (dbContext.DataAccessBehavior() == DataAccessBehaviorValues.NoUpdate))
                        {
                            if (this.ThrowException) throw new System.Exception(String.Format("{0}: {1}", this.GetType().ToStringSafe(), "Updates not allowed."));
                        }
                    }
                    this.ValidateAll(this);
                    if (this.CanSave(this) == true)
                    {
                        dbContext.SaveChanges();
                    }
                    if(dbContext.DataConcurrency() == DataConcurrencyValues.Pessimistic)
                    {
                        newItem = SaveableEntity<TEntity>.GetByID(this.ID); // Re-pull to load any data changed by the database (i.e. re-linking IDs, default values.)
                        this.FillByInterface(newItem); // Fill current object from database item to absorb all current values
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debugger.Break();
#endif
                ExceptionLogger.Create(ex, typeof(TEntity), String.Format("{0}.Save() in SaveableEntity", this.GetType().ToStringSafe()), "MyLogConnection", "MiddleTier");
                if (this.ThrowException) throw;
            }
            finally
            {
                dbContext.Dispose();
            }

            return this.ID;
        }

        /// <summary>
        /// Deletes this object with Workflow-based tracking.
        /// More complicated than Delete(), but verbose logged info and ensures full-tiered architecture
        /// </summary>
        /// <param name="activity">Activity record owning this process.</param>        
        public virtual void Delete(IActivity activity)
        {
            this.ActivityID = activity.ActivityID;
            this.Delete();
        }

        /// <summary>
        /// Deletes this object with automatic tracking.
        /// Less complicated than Delete(IFlowClass), but minimal data logged and free-form architecture
        /// </summary>
        public virtual void Delete()
        {
            DatabaseContext dbContext = new DatabaseContext();

            try
            {
                if (this.ID != TypeExtension.DefaultInteger)
                {
                    if (ActivityLogger.GetByID(this.ActivityID, "MyDataConnection", "Activity").ActivityID == TypeExtension.DefaultInteger)
                    {
                        this.ActivityID = ActivityLogger.Create("MyDataConnection", new TEntity().GetAttributeValue<DatabaseSchemaAttribute>("Activity"));
                    } // All database commits require activity of some sort
                    if ((dbContext.DataAccessBehavior() == DataAccessBehaviorValues.InsertOnly) & (dbContext.DataAccessBehavior() == DataAccessBehaviorValues.SelectOnly))
                    {
                        if (this.ThrowException) throw new System.Exception("Deletes not allowed.");
                    }
                    dbContext.EntityData.Remove(dbContext.EntityData.Where(x => x.ID == this.ID).FirstOrDefaultSafe());
                    dbContext.SaveChanges();
                } else
                {
                    this.FillByInterface(new TEntity());
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debugger.Break();
#endif
                ExceptionLogger.Create(ex, typeof(TEntity), String.Format("SaveableEntity.Delete() on {0}", this.ToString()), "MyLogConnection", "MiddleTier");
                if (this.ThrowException) throw;
            }
            finally
            {
                dbContext.Dispose();
            }
        }
        
        /// <summary>
        /// Static ability to fill this class with a class that implements the same Property(s)
        /// This method fully constructs TEntity before filling.
        /// </summary>
        /// <param name="sourceItem">Item to fill TEntity object, must implement at least one common Property.</param>
        /// <returns>Fully-constructed, fully-hydrated TEntity class</returns>
        public static TEntity Fill(object sourceItem)
        {
            TEntity returnValue = new TEntity();
            returnValue.FillByProperty<TEntity>(sourceItem);
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
            foreach (PropertyInfo newObjectProperty in newObjectType.GetProperties())
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
            return this.ID.ToString();
        }

        /// <summary>
        /// Pulls the connection string from an attribute, used to feed DbContext the full EF connection string
        /// </summary>
        /// <returns></returns>
        public static string ConnectionString()
        {
            string returnValue = TypeExtension.DefaultString;
            ConnectionStringSafe configConnectString = new ConnectionStringSafe();
            ConfigurationManagerFull configManager = new ConfigurationManagerFull();

            configConnectString = configManager.ConnectionString(new TEntity().GetAttributeValue<ConnectionStringAttribute>(connectionStringDefault));
            returnValue = configConnectString.ToEF(typeof(TEntity));
            if (returnValue == TypeExtension.DefaultString) { throw new System.Exception("Connection string could not be found. A valid connection string required for data access."); }

            return returnValue;
        }

        /// <summary>
        /// DB Context - Entity Framework uses this to connect to the database
        /// </summary>
        protected class DatabaseContext : System.Data.Entity.DbContext
        {
            /// <summary>
            /// Defines if object can select, insert, update and/or delete.
            /// </summary>
            internal DataAccessBehaviorValues DataAccessBehavior()
            {
                DataAccessBehaviorValues returnValue = DataAccessBehaviorValues.AllAccess;
                TEntity itemType = new TEntity();

                foreach (object item in itemType.GetType().GetCustomAttributes(false))
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
                TEntity itemType = new TEntity();

                foreach (object item in itemType.GetType().GetCustomAttributes(false))
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
            public System.Data.Entity.DbSet<TEntity> EntityData { get; set; }

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
                this.ThrowException = true;
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
                    ExceptionLogger.Create(ex, typeof(TEntity), String.Format("SaveableEntity.SaveChanges() on {0}", this.ToString()), "MyLogConnection", "MiddleTier");
                    if (this.ThrowException == true)
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

        public static SaveableEntity<TEntity> Deserialize(string jsonString)
        {
            JsonSerializer<TEntity> serializer = new JsonSerializer<TEntity>();
            return serializer.Deserialize(jsonString);
        }
    }
}
