//-----------------------------------------------------------------------
// <copyright file="ModelEntity.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Genesys.Foundation.Entity
{
	/// <summary>
	/// ModelBase
	/// </summary>
	/// <remarks>ModelBase</remarks>
	[CLSCompliant(true)]
	public abstract class ModelEntity<TModel> : EntityInfo<TModel>, INotifyPropertyChanged where TModel : ModelEntity<TModel>, new()
	{
        private int idField = TypeExtension.DefaultInteger;
        private Guid keyField = TypeExtension.DefaultGuid;

        /// <summary>
        /// Primary key of this record. Internal use only, use Key when exposing data externally.
        /// </summary>
        public override int ID { get { return idField; } set { SetField(ref idField, value); } }

        /// <summary>
        /// Primary key of this record. To be used for external use.
        /// </summary>
        public override Guid Key { get { return keyField; } set { SetField(ref keyField, value); } }

        /// <summary>
        /// Constructor
        /// </summary>
        public ModelEntity() : base() { this.Initialize<ModelEntity<TModel>>(); }

        /// <summary>
        /// Property changed event handler for INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Property changed event for INotifyPropertyChanged
        /// </summary>
        /// <param name="propertyName">String name of property</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the property data as well as fired OnPropertyChanged() for INotifyPropertyChanged
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetField<TField>(ref TField field, TField value,
        [CallerMemberName] string propertyName = null)
        {
            var returnValue = TypeExtension.DefaultBoolean;
            if (EqualityComparer<TField>.Default.Equals(field, value) == false)
            {
                field = value;
                OnPropertyChanged(propertyName);
                returnValue = true;
            }                
            return returnValue;
        }
    }
}
