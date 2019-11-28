// *****************************************************************************************************
// 
//   BigBinObject.cs
// 
// 
//   Description:
//      The Big Bin Object
//
// *****************************************************************************************************
using System;

namespace AudioControlLib.Structures
{
    /// <summary>
    /// For Big binary Object
    /// </summary>
    struct BigBinObject
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
