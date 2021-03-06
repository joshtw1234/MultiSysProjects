﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioControlLib.Structures
{
    /// <summary>
    /// This has license issue
    /// </summary>
    struct Blob
    {
        /// <summary>
        /// Length of binary object.
        /// </summary>
        public int Length;
        /// <summary>
        /// Pointer to buffer storing data.
        /// </summary>
        public IntPtr Data;
    }
}
