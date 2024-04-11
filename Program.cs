using CommandLine;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

public class Program {
    static void Main(string[] args) {
        CommandLine.Parser.Default.ParseArguments<Options>(args)
          .WithParsed(RunOptions)
          .WithNotParsed(HandleParseError);
    }
    static void RunOptions(Options opts) {
        opts.Volume = Math.Clamp(opts.Volume, 0, 100);
        opts.MinimumWait = Math.Max(opts.MinimumWait, 0);
        new SEGA.SEGA(opts).Run();
    }
    static void HandleParseError(IEnumerable<Error> errs) {
        string str = "";
        foreach (Error e in errs) {
            str += e.ToString();
        }
        MessageBox.Show("Command Line argument errors: \n" + str);
    }

    public class Options {
        [Option("volume", Default = 50)]
        public int Volume { get; set; }

        [Option("min-wait", Default = 3)]
        public int MinimumWait { get; set; }

        [Option("windowed", Default = false)]
        public bool Windowed { get; set; }

        [Option("skip-on-window", Default = false)]
        public bool SkipOnWindow { get; set; }

        [Option("wait-for", Default = null)]
        public string WaitForWindow { get; set; }
    }
}