﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NI2S.Node.Data.Mapping
{
    /// <summary>
    /// Backward compatibility of table naming
    /// </summary>
    public partial interface INameCompatibility
    {
        /// <summary>
        /// Gets table name for mapping with the type
        /// </summary>
        Dictionary<Type, string> TableNames { get; }

        /// <summary>
        ///  Gets column name for mapping with the entity's property and type
        /// </summary>
        Dictionary<(Type, string), string> ColumnName { get; }
    }
}
