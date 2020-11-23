// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2015-2020. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace VisualStudioSolutionGenerator
{
    using System;
    using System.IO;

    using NDesk.Options;

    using VisualStudioSolutionGenerator.Properties;

    /// <summary>
    /// Toy program to generate a Visual Studio Solution File (*.sln)
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Generates a Visual Studio Solution File (*.sln) for all projects found in the specified directory.
        /// </summary>
        /// <param name="args">See <see cref="ShowUsage"/></param>
        public static void Main(string[] args)
        {
            // Modes
            bool forFolder = false;
            bool fromProjectListing = false;
            bool fromProjectListingRelative = false;
            bool fromSolutionListing = false;
            bool showHelp = false;

            // Arguments
            string solutionPath = string.Empty;
            string listingPath = string.Empty;
            string directory = string.Empty;
            string ignoreFile = string.Empty;

            OptionSet p = new OptionSet()
            {
                { "FromProjectListing|fpl", Strings.FromProjectListingDescription, v => fromProjectListing = v != null },
                { "FromProjectListingRelative|fplr", Strings.FromProjectListingRelativeDescription, v => fromProjectListingRelative = v != null },
                { "FromSolutionListing|fsl", Strings.FromSolutionListingDescription, v => fromSolutionListing = v != null },
                { "ForFolder|ff", Strings.ForFolderDescription, v => forFolder = v != null },
                { "solution|sln=", Strings.SolutionDescription, v => solutionPath = v },
                { "listing|l=", Strings.ListingDescription, v => listingPath = v },
                { "directory|d=", Strings.DirectoryDescription, v => directory = v },
                { "ignorefile|i=", Strings.IgnoreFileDescription, v => ignoreFile = v },
                { "?|h|help", Strings.HelpDescription, v => showHelp = v != null },
            };

            // Parse the Options
            try
            {
                p.Parse(args);
            }
            catch (OptionException)
            {
                Console.WriteLine(Strings.ShortUsageMessage);
                Console.WriteLine($"Try `--help` for more information.");
                Environment.ExitCode = 160;
                return;
            }


            if (showHelp)
            {
                Environment.ExitCode = ShowUsage(p);
            }
            else if (fromProjectListing)
            {
                if (ValidateArgumentWasPassed("solution", solutionPath) && ValidateArgumentPathIsValid("listing", listingPath))
                {
                    // We need the full path to the solution file
                    solutionPath = new FileInfo(solutionPath).FullName;
                    SolutionGenerator.FromProjectListing(solutionPath, listingPath);
                }
                else
                {
                    Environment.ExitCode = InvalidArguments("FromProjectListing");
                }
            }
            else if (fromProjectListingRelative)
            {
                if (ValidateArgumentWasPassed("solution", solutionPath) && ValidateArgumentPathIsValid("listing", listingPath))
                {
                    // We need the full path to the solution file
                    solutionPath = new FileInfo(solutionPath).FullName;
                    SolutionGenerator.FromProjectListingRelative(solutionPath, listingPath);
                }
                else
                {
                    Environment.ExitCode = InvalidArguments("FromProjectListingRelative");
                }
            }
            else if (fromSolutionListing)
            {
                if (ValidateArgumentWasPassed("solution", solutionPath) && ValidateArgumentPathIsValid("listing", listingPath))
                {
                    // We need the full path to the solution file
                    solutionPath = new FileInfo(solutionPath).FullName;
                    SolutionGenerator.FromSolutionListing(solutionPath, listingPath);
                }
                else
                {
                    Environment.ExitCode = InvalidArguments("FromSolutionListing");
                }
            }
            else if (forFolder)
            {
                if (ValidateArgumentWasPassed("solution", solutionPath) && ValidateArgumentPathIsValid("directory", directory))
                {
                    // We need the full path to the solution file
                    solutionPath = new FileInfo(solutionPath).FullName;
                    SolutionGenerator.ForFolder(directory, solutionPath, ignoreFile);
                }
                else
                {
                    Environment.ExitCode = InvalidArguments("ForFolder");
                }
            }
            else
            {
                Environment.ExitCode = ShowUsage(p);
            }
        }

        /// <summary>
        /// Provides common logic to indicate that invalid arguments were passed to a mode.
        /// </summary>
        /// <param name="mode">The mode in which this tool is being ran.</param>
        /// <returns>Always 160 - ERROR_BAD_ARGUMENTS</returns>
        private static int InvalidArguments(string mode)
        {
            Console.WriteLine($"One or more arguments were invalid for mode {mode}. See previous messages.");
            return 160;
        }

        /// <summary>
        /// Provides common logic to validate that an argument was passed and exists.
        /// </summary>
        /// <param name="argumentName">The name of the argument/switch passed.</param>
        /// <param name="argument">The value of the argument.</param>
        /// <returns><c>true</c> if the argument is valid; otherwise, <c>false</c>.</returns>
        private static bool ValidateArgumentPathIsValid(string argumentName, string argument)
        {
            bool isValid = true;

            isValid = ValidateArgumentWasPassed(argumentName, argument);

            if (!File.Exists(argument))
            {
                Console.WriteLine($"You must provide a path that exists to -{argumentName}; Received `{argument}`.");
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// Provides common logic to validate that an argument was passed.
        /// </summary>
        /// <param name="argumentName">The name of the argument/switch passed.</param>
        /// <param name="argument">The value of the argument.</param>
        /// <returns><c>true</c> if the argument is valid; otherwise, <c>false</c>.</returns>
        private static bool ValidateArgumentWasPassed(string argumentName, string argument)
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(argument))
            {
                Console.WriteLine($"You must provide an argument to -{argumentName}");
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// Provides common logic to display the usage for this tool.
        /// </summary>
        /// <param name="p">The <see cref="OptionSet"/> that was parsed.</param>
        /// <returns>Always 160 - ERROR_BAD_ARGUMENTS</returns>
        private static int ShowUsage(OptionSet p)
        {
            Console.WriteLine(Strings.ShortUsageMessage);
            Console.WriteLine();
            Console.WriteLine(Strings.LongDescription);
            Console.WriteLine();
            p.WriteOptionDescriptions(Console.Out);
            return 160;
        }
    }
}
