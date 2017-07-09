//-----------------------------------------------------------------------
// <copyright file="ExceptionLogFull.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
// 
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using Genesys.Extensions;
using Genesys.Extras.Configuration;
using Genesys.Foundation.Data;

namespace Genesys.Foundation.Activity
{
    /// <summary>
    /// Code-first class that records exceptions to a 100% uptime database
    /// Default connection string is: DefaultConnection. 
    /// Can be changed via passing new ConnectionString name to the constructor
    /// </summary>
    /// <remarks></remarks>
    [CLSCompliant(true)]    
    public class ExceptionLogger : ExceptionLog
    {
        private string connectStringName = ConnectionStringName.DefaultValue;
        private string databaseSchemaName = DatabaseSchemaName.DefaultActivityValue;
        private Uri endpointUrl = TypeExtension.DefaultUri;
        private Exception currentException = new System.Exception("No Exception");

        /// <summary>
        /// The Activity record that was processing when this exception occurred
        /// </summary>
        public int CreatedActivityID { get; set; } = TypeExtension.DefaultInteger;

        /// <summary>
        /// MachineName
        /// </summary>
        public string MachineName { get { return Environment.MachineName; } }

        /// <summary>
        /// ADDomainName
        /// </summary>
        public string ADDomainName { get { return Environment.UserDomainName; } protected set { } }

        /// <summary>
        /// ADUserName
        /// </summary>
        public string ADUserName { get { return Environment.UserName; } protected set { } }

        /// <summary>
        /// DirectoryWorking
        /// </summary>
        public string DirectoryWorking { get { return Environment.CurrentDirectory; } protected set { } }

        /// <summary>
        /// DirectoryAssembly
        /// </summary>
        public string DirectoryAssembly { get { return Assembly.GetExecutingAssembly().Location; } protected set { } }

        /// <summary>
        /// ApplicationName
        /// </summary>
        public string AssemblyName { get { return Assembly.GetExecutingAssembly().FullName; } protected set { } }

        /// <summary>
        /// URL
        /// </summary>
        public string URL { get { return endpointUrl.ToString(); } protected set { } }

        /// <summary>
        /// This protected constructor should not be called. Factory methods should be used instead.
        /// </summary>
        protected ExceptionLogger() : base() { CreatedDate = DateTime.UtcNow; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <remarks></remarks>
        public ExceptionLogger(string connectionStringName) : this()
        {
            connectStringName = connectionStringName;
        }

        /// <summary>
        /// Creates Exception object
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <param name="databaseSchema"></param>
        public ExceptionLogger(string connectionStringName, string databaseSchema) : this(connectionStringName) { databaseSchemaName = databaseSchema; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="concreteType"></param>
        /// <param name="customMessage"></param>
        public ExceptionLogger(Exception exception, Type concreteType, string customMessage) : base(exception, concreteType, customMessage) { }


        /// <summary>
        /// Hydrates object and saves the log record
        /// </summary>
        /// <param name="exception">System.Exception object to log</param>
        /// <param name="concreteType">Type that is logging the exception</param>
        /// <param name="customMessage">Custom message to append to the exception log</param>
        /// <returns>ID if successfull, -1 if not.</returns>
        public static int Create(Exception exception, Type concreteType, string customMessage)
        {
            ExceptionLogger log = new ExceptionLogger(exception, concreteType, customMessage) { };
            return log.Save();
        }

        /// <summary>
        /// Loads an existing object MyBased on ID.
        /// </summary>
        /// <param name="connectStringName">Key of the config value for this actions connection string</param>
        /// <param name="databaseSchema">Database Schema that owns the Activity table</param>
        public static IQueryable<ExceptionLog> GetAll(string connectStringName, string databaseSchema)
        {
            var returnValue = default(IQueryable<ExceptionLog>);
            var dbContext = new DatabaseContext(connectStringName, databaseSchema);

            try
            {
                returnValue = dbContext.EntityData;
            }
            catch (Exception ex)
            {
                ExceptionLogger.Create(ex, typeof(ExceptionLog), String.Format("ActivityLogger.GetByID({0})"));
            }

            return returnValue;
        }

        /// <summary>
        /// Loads an existing object MyBased on ID.
        /// </summary>
        /// <param name="id">The unique ID of the object</param>
        /// <param name="connectStringName">Key of the config value for this actions connection string</param>
        /// <param name="databaseSchema">Database Schema that owns the Activity table</param>
        public static ExceptionLog GetByID(int id, string connectStringName, string databaseSchema)
        {
            ExceptionLog returnValue = new ExceptionLog();
            var dbContext = new DatabaseContext(connectStringName, databaseSchema);

            try
            {
                if (id != TypeExtension.DefaultInteger)
                {
                    returnValue = dbContext.EntityData.Where(x => x.ExceptionLogID == id).FirstOrDefaultSafe();
                }
            }
            catch(Exception ex)
            {
                ExceptionLogger.Create(ex, typeof(ExceptionLog), String.Format("ExceptionLogger.GetByID({0})", id.ToString()));
            }

            return returnValue;
        }

        /// <summary>
        /// Saves object to database
        /// </summary>
        public virtual int Save()
        {
            using (DatabaseContext dbContext = new DatabaseContext(connectStringName, databaseSchemaName))
            {
                try
                {
                    if (ExceptionLogID == TypeExtension.DefaultInteger)
                    {
                        dbContext.EntityData.Add(this);
                        dbContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    CurrentException = ex; // Never let save errors propagate, else endless loop
                }
            }
                
            return ExceptionLogID;
        }
        
        /// <summary>
        /// DB Context - Entity Framework uses this to connect to the database
        /// </summary>
        protected class DatabaseContext : System.Data.Entity.DbContext
        {
            private string databaseSchemaField = "dbo";
            
            /// <summary>
            /// BusinessEntity - Determines table and columns
            /// </summary>
            public System.Data.Entity.DbSet<ExceptionLog> EntityData { get; set; }
            
            /// <summary>
            /// Constructor. Explicitly set database connection.
            /// </summary>
            /// <remarks></remarks>
            public DatabaseContext(string connectStringName, string databaseSchema)
                : base(new ConfigurationManagerFull().ConnectionStrings.GetValue(connectStringName))
            {
                databaseSchemaField = databaseSchema;
            }

            /// <summary>
            /// SaveChanges - Saves the object to the database
            /// </summary>
            public override int SaveChanges()
            {
                return base.SaveChanges();
            }
            
            /// <summary>
            /// Set values when creating a model in the database
            /// </summary>
            /// <param name="modelBuilder"></param>
            /// <remarks></remarks>
            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.HasDefaultSchema(this.databaseSchemaField);
                base.OnModelCreating(modelBuilder);
                modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
            }
        }
         
        /// <summary>
        /// Initializes the database
        /// </summary>
        public void Initialize()
        {
            System.Data.Entity.Database.SetInitializer<DatabaseContext>((global::System.Data.Entity.IDatabaseInitializer<ExceptionLogger.DatabaseContext>)new DatabaseInitializer());
        }

        /// <summary>
        /// Class that initializes and handles seed/identity
        /// </summary>
        protected class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
        {
            /// <summary>
            /// Sets default data
            /// </summary>
            /// <param name="context"></param>
            /// <remarks></remarks>
            protected override void Seed(DatabaseContext context)
            {
                base.Seed(context);
            }            
        }        
    }
}