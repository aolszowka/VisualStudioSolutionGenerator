// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2015-2020. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace VisualStudioSolutionGenerator
{
    using System;

    /// <summary>
    /// Toy program to generate a Visual Studio Solution File (*.sln)
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Generates a Visual Studio Solution File (*.sln) for all projects found in the specified directory.
        /// </summary>
        /// <param name="args">None are used right now.</param>
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                throw new NotSupportedException();
            }

            SolutionGenerator.FromProjectListing(args[0], args[1]);
        }
    }
}
