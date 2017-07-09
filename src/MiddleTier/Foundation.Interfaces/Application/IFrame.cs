//-----------------------------------------------------------------------
// <copyright file="IFrame.cs" company="Genesys Source">
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
    /// Exposing Microsoft Internal frame interface to force consistency cross-platform
    /// </summary>
    public interface IFrame
    {
        /// <summary>
        /// CanGoBack
        /// </summary>
        bool CanGoBack { get; }

        /// <summary>
        /// CanGoForward
        /// </summary>
        bool CanGoForward { get; }

        /// <summary>
        /// GoBack
        /// </summary>
        void GoBack();

        /// <summary>
        /// GoForward
        /// </summary>
        void GoForward();
    }
}
