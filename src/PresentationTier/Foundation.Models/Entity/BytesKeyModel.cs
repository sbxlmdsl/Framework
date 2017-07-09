//-----------------------------------------------------------------------
// <copyright file="KVPModel.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using Genesys.Extensions;

namespace Genesys.Foundation.Entity
{
	/// <summary>
	/// Common object across models and business entity
	/// </summary>
	/// <remarks></remarks>
	[CLSCompliant(true)]
	public class BytesKeyModel : ModelEntity<BytesKeyModel>, IBytesKey
	{
        /// <summary>
        /// Bytes of BLOB
        /// </summary>
        public byte[] Bytes { get; set; } = TypeExtension.DefaultBytes;
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <remarks></remarks>
		public BytesKeyModel() : base()
		{
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks></remarks>
        public BytesKeyModel(byte[] bytes)
            : this()
        {
            Bytes = bytes;
        }
		
	}
}
