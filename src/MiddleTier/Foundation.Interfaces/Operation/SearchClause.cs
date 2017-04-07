//-----------------------------------------------------------------------
// <copyright file="SearchClause.cs" company="Genesys Source">
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
using Genesys.Extensions;
using Genesys.Foundation.Entity;
using System.Linq.Expressions;

namespace Genesys.Foundation.Operation
{
    /// <summary>
    /// Search Clause Expression. To allow for exact search preferences.
    /// Entity: Person
    ///  Search Clauses:
    /// - FirstName BeginsWith Rob
    /// - And LastName == Smith
    /// Results:
    ///     FirstName     LastName
    ///     Robby           Smith
    ///     Robert          Smith
    ///     Rob             Smith
    /// </summary>
    /// <typeparam name="TEntity">Entity to Search</typeparam>
    [CLSCompliant(true)]
    public class SearchClause<TEntity> : Tuple<String, Expression<TEntity>> where TEntity : IEntity
    {
        /// <summary>
        /// Type of clause (And, Or)
        /// </summary>
        public ClauseTypes ClauseTypeID { get; set; } = ClauseTypes.And;

        /// <summary>
        /// Operator to apply to condition (Equals, Contains, BeginsWith, EndsWith)
        /// </summary>
        public Operators OperatorID { get; set; } = Operators.Equals;

        /// <summary>
        /// Entity to Search
        /// </summary>
        public string Entity { get { return base.Item1; } }

        /// <summary>
        /// Property Name to Search
        /// </summary>
        public string Property { get { return base.Item1; } }

        /// <summary>
        /// Expression of the validation query
        /// </summary>
        public Expression<TEntity> Criteria { get { return base.Item2; } }

        /// <summary>
        /// Is Valid
        /// </summary>
        public bool IsFound { get; private set; } = TypeExtension.DefaultBoolean;

        /// <summary>
        /// Has been Searchd
        /// </summary>
        public bool HasSearched { get; private set; } = TypeExtension.DefaultBoolean;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyNameToSearch"></param>
        /// <param name="searchExpression"></param>
        public SearchClause(string propertyNameToSearch, Expression<TEntity> searchExpression)
            : base(propertyNameToSearch, searchExpression)
        { }
        

        ///// <summary>
        ///// Searchs per predicate Lambda.
        ///// </summary>
        ///// <param name="entityToSearch"></param>
        //public bool ExecuteSearch(TEntity entityToSearch)
        //{
        //    IsFound = entityToSearch.Where();
        //    HasSearched = true;
        //    return IsFound;
        //}
    }

    /// <summary>
    /// And or Or
    /// </summary>
    public enum ClauseTypes
    {
        /// <summary>
        /// And
        /// </summary>
        And = 0x0,

        /// <summary>
        /// Or
        /// </summary>
        Or = 0x1
    }

    /// <summary>
    /// And or Or
    /// </summary>
    public enum Operators
    {
        /// <summary>
        /// Equals
        /// </summary>
        Equals = 0x0,

        /// <summary>
        /// Contains
        /// </summary>
        Contains = 0x1,

        /// <summary>
        /// BeginsWith
        /// </summary>
        BeginsWith = 0x2,

        /// <summary>
        /// EndsWith
        /// </summary>
        EndsWith = 0x3
    }
}
