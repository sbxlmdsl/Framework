//-----------------------------------------------------------------------
// <copyright file="IViewModel.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Extras.Net;
using System.ComponentModel;

namespace Genesys.Foundation.Application
{
    /// <summary>
    /// Interface to enforce ViewModel division of responsibilities
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IViewModel<TModel> : INotifyPropertyChanged
    {
        /// <summary>
        /// Configuration data
        ///  Data must be constructed in the application tier
        /// </summary>
        IApplication MyApplication { get; }

        /// <summary>
        /// Model data
        /// </summary>
        TModel MyModel { get; }

        /// <summary>
        /// Sender of main Http Verbs
        /// </summary>
        HttpVerbSender Sender { get; }
    }
}
