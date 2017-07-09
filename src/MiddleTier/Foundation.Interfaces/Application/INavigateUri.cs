//-----------------------------------------------------------------------
// <copyright file="INavigate.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Genesys.Foundation.Application
{
    /// <summary>
    /// Application frame/page navigation
    /// </summary>
    public interface INavigateUri
    {
        /// <summary>
        /// Navigates to the specified type, typically a Wpf or Universal page
        /// </summary>
        /// <param name="destinationPageUrl">Type to navigate, typically a Wpf or Universal page</param>
        /// <returns>False if navigation fails</returns>
        bool Navigate(Uri destinationPageUrl);

        /// <summary>
        /// Navigates to the specified type, typically a Wpf or Universal page
        /// </summary>
        /// <param name="destinationPageUrl">Type to navigate, typically a Wpf or Universal page</param>
        /// <param name="dataToPass">Object to pass to the destination</param>
        /// <returns>False if navigation fails</returns>
        bool Navigate<TModel>(Uri destinationPageUrl, TModel dataToPass);
    }

    /// <summary>
    /// Application frame/page navigation
    /// </summary>
    public interface INavigateUri<TModel> : INavigateUri
    {
        /// <summary>
        /// Navigates to the specified type, typically a Wpf or Universal page
        /// </summary>
        /// <param name="destinationPageUrl">Type to navigate, typically a Wpf or Universal page</param>
        /// <param name="dataToPass">Object to pass to the destination</param>
        /// <returns>False if navigation fails</returns>
        bool Navigate(Uri destinationPageUrl, TModel dataToPass);
    }
}
