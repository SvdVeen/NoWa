﻿using NoWa.Common;
using NoWa.Common.Logging;
using NoWa.Converter;
using NoWa.Parser;
using Con = System.Console;

namespace NoWa.Console;

/// <summary>
/// Main program class, contains the application entry point.
/// </summary>
class Program
{
    private const string HELPINFO =
@"Converts the given grammer to Chomsky Normal Form.
The first rule in the grammar is assumed to be the start rule.

Usage:
NoWa.exe -help
Displays this message.

NoWa.exe <input path> [-cfg] [-out <output path>] [-log <log level>]
Converts a grammar to Chomsky Normal Form.
input path: the path to the grammar file to parse.
-cfg parses a normal context-free grammar instead of a context-free weighted attribute grammar.
output path: the path to write the output to.
log level: The log level to use for messages about parsing and conversion. Possible levels are:
    - none/-1: show no log messages.
    - error/0: show error messages only.
    - warning/1: show warnings and errors.
    - info/2: show informational messages, warnings, and errors. This is the default level.
    - debug/3: show debug messages and all other types of messages. When this level is used, errors are ignored where possible.

Return codes:
    - 0: success.
    - 1: invalid arguments.
    - 2: parsing failed.
    - 3: conversion failed.
    - 4: write failure.
";

    /// <summary>
    /// Main entry point for NoWa.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    /// <returns>Exit code: 0 for success, 1 for invalid arguments, 2 for parse failure, 3 for conversion failure, 4 for file write failure.</returns>
    static int Main(string[] args)
    {
        WriteAppInfo();

        if (!ValidateArguments(args, out bool help, out string? inPath, out bool cfg, out string? outPath, out LogLevel logLevel))
        {
            return 1;
        }

        if (help)
        {
            return 0;
        }

        ConsoleLogger logger = new() { DisplayLevel = logLevel };
        Grammar grammar;
        if (cfg)
        {
            grammar = NoWaCFGParser.Parse(inPath!);
        }
        else
        {
            grammar = NoWaWAGParser.Parse(inPath!);
        }
        NoWaConverter converter = new(logger);
        Grammar? result = converter.Convert(grammar);

        if (result == null)
        {
            return 3;
        }

        if (!string.IsNullOrWhiteSpace(outPath))
        {
            if (!SaveResultToFile(outPath, result))
            {
                return 4;
            }
        }
        else
        {
            Con.WriteLine("Result:");
            Con.WriteLine(result!.ToString());
        }

        return 0;
    }

    /// <summary>
    /// Write basic information about the application.
    /// </summary>
    static void WriteAppInfo()
    {
        Con.WriteLine("NoWa: a Chomsky Normal Form converter for Weighted Attrubute Grammars.");
        Con.WriteLine("01-2024, Suzanne van der Veen, University of Twente");
        Con.WriteLine();
    }

    /// <summary>
    /// Validate the command-line arguments. Prints error and help messages when appropriate.
    /// </summary>
    /// <param name="args">The arguments to validate.</param>
    /// <param name="help"><see langword="true"/> if the "-help" argument was passed; <see langword="false"/> otherwise.</param>
    /// <param name="inPath">The given input path if it was valid; <see langword="null"/> otherwise.</param>
    /// <param name="cfg"><see langword="true"/> if "-cfg" was passed, <see langword="false"/> otherwise.</param>
    /// <param name="outPath">The given output path if it was valid; <see langword="null"/> if it was invalid or not specified.</param>
    /// <param name="logLevel">The given log level if it was valid; <see cref="LogLevel.Info"/> if it was invalid or not specified.</param>
    /// <returns><see langword="true"/> if the arguments were successfully validated; <see langword="false"/> otherwise.</returns>
    static bool ValidateArguments(string[] args, out bool help, out string? inPath, out bool cfg, out string? outPath, out LogLevel logLevel)
    {
        help = false;
        inPath = null;
        cfg = false;
        outPath = null;
        logLevel = LogLevel.Info;

        if (args.Length == 0)
        {
            Con.Error.WriteLine("No arguments given!");
            Con.WriteLine(HELPINFO);
            return false;
        }

        if (string.Equals(args[0], "-help", StringComparison.OrdinalIgnoreCase))
        {
            help = true;
            Con.WriteLine(HELPINFO);
            return true;
        }

        if (!Path.Exists(args[0]))
        {
            Con.Error.WriteLine($"Could not find the input file {args[0]}.");
            return false;
        }
        else
        {
            inPath = args[0];
        }

        for (int i = 1; i < args.Length; i++)
        {
            if (string.Equals(args[i], "-cfg", StringComparison.OrdinalIgnoreCase))
            {
                cfg = true;
            }
            else if (string.Equals(args[i], "-out", StringComparison.OrdinalIgnoreCase))
            {
                if (++i == args.Length)
                {
                    Con.Error.WriteLine("Missing output path.");
                    return false;
                }
                if (!Path.Exists(Path.GetDirectoryName(args[i])))
                {
                    Con.Error.WriteLine($"Invalid output path {args[i]}");
                    return false;
                }
                outPath = args[i];
            }
            else if (string.Equals(args[i], "-log", StringComparison.OrdinalIgnoreCase))
            {
                if (++i == args.Length)
                {
                    Con.Error.WriteLine("Missing log level.");
                    return false;
                }
                if (!Enum.TryParse(args[i], true, out logLevel))
                {
                    Con.Error.WriteLine($"Invalid log level '{args[i]}'");
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Save a result to a file.
    /// </summary>
    /// <param name="outPath">The path to save the result to.</param>
    /// <param name="result">The result to save.</param>
    /// <returns><see langword="true"/> if the file was saved successfully; <see langword="false"/> otherwise.</returns>
    private static bool SaveResultToFile(string outPath, Grammar result)
    {
        try
        {
            File.WriteAllText(outPath, result.ToString() + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Con.Error.WriteLine("Failed to save result:");
            Con.Error.WriteLine($"{ex.GetType} - {ex.Message}");
            return false;
        }

        Con.WriteLine($"Result saved to {Path.GetFullPath(outPath)}");
        return true;
    }
}
