//-----------------------------------------------------------------------
// <copyright file="NameIDModel.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using Genesys.Extensions;
using Genesys.Foundation.Entity;

namespace Genesys.Foundation.Name
{
	/// <summary>
	/// Common object across models and business entity
	/// </summary>
	/// <remarks></remarks>
	[CLSCompliant(true)]
	public class NameIDModel : ModelEntity<NameIDModel>, INameID
	{
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = TypeExtension.DefaultString;
		
		/// <summary>
		/// Constructor
		/// </summary>
		public NameIDModel() : base()
		{
		}
	}
}
