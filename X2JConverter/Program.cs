using CommandLine;
using System;
using System.IO;

namespace X2JConverter
{
    internal class Program
    {
        private enum Targets
        {
            Locations,
            Bin,
        }

        private class Options
        {
            [Option('i', "indent", Default = false, Required = false, HelpText = "Set to true to indent output files.")]
            public bool Indent { get; set; }
            
            [Option('t', "target", Required = true, HelpText = "Target type. Allowed values: Locations, Bin")]
            public Targets Targets { get; set; }

            [Option('s', "source", Required = false, HelpText = "Source file. Defaults to stdin.")]
            public string Source { get; set; }

            [Option('o', "output", Required = false, HelpText = "Output file. Defaults to stdout.")]
            public string Output { get; set; }
        }

        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithNotParsed(errors => errors.Output())
                .WithParsed(options =>
                {
                    var sourceIsStdIn = options.Source == null || options.Source.Length == 0;
                    var outputIsStdOut = options.Output == null || options.Output.Length == 0;
                    Stream source = null;
                    Stream output = null;

                    if (sourceIsStdIn)
                    {
                        var stdin = Console.OpenStandardInput();
                        source = new MemoryStream();
                        stdin.CopyTo(source);
                    }
                    else
                        if (!CatchFileOpenErrors("Source", () => source = File.OpenRead(options.Source)))
                        return;

                    if (outputIsStdOut)
                        output = Console.OpenStandardOutput();
                    else
                    {
                        var opened = CatchFileOpenErrors("Output", () =>
                        {
                            if (File.Exists(options.Output))
                                output = File.Open(options.Output, FileMode.Truncate, FileAccess.Write);
                            else
                                output = File.OpenWrite(options.Output);
                        });

                        if (!opened)
                            return;
                    }

                    string result = "";

                    switch (options.Targets)
                    {
                        case Targets.Locations:
                            result = X2JCore.Converter.ConverLocations(source, options.Indent);
                            break;
                        case Targets.Bin:
                            result = X2JCore.Converter.ConvertBins(source, options.Indent);
                            break;
                        default:
                            Console.WriteLine("Unknown target.");
                            return;
                    }

                    using (var writer = new StreamWriter(output))
                        writer.Write(result);

                    source.Dispose();
                    if (!outputIsStdOut)
                        output.Dispose();
                });
        }

        private static bool CatchFileOpenErrors(string pathName, Action worker)
        {
            try
            {
                worker();
                return true;
            }
            catch (ArgumentException)
            {
                Console.Error.WriteLine($"{pathName} path is invalid.");
                return false;
            }
            catch (PathTooLongException)
            {
                Console.Error.WriteLine($"{pathName} path is too long.");
                return false;
            }
            catch (DirectoryNotFoundException)
            {
                Console.Error.WriteLine($"{pathName} path is invalid: directory not found.");
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                Console.Error.WriteLine($"{pathName} path is a directory or you do not have the required permission.");
                return false;
            }
            catch (FileNotFoundException)
            {
                Console.Error.WriteLine($"{pathName} path is invalid: file was not found.");
                return false;
            }
            catch (NotSupportedException)
            {
                Console.Error.WriteLine($"{pathName} path is in an invalid format.");
                return false;
            }
            catch (IOException)
            {
                Console.Error.WriteLine($"An I/O error occurred while opening the file from {pathName} path.");
                return false;
            }
        }
    }
}
