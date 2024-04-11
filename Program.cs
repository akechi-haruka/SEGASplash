using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

public class Program {
    static void Main(string[] args) {
        Parser parser = new Parser(with => with.HelpWriter = null);
        ParserResult<Options> parserResult = parser.ParseArguments<Options>(args);

        parserResult
            .WithParsed(opts => RunOptions(opts))
            .WithNotParsed(errs => HandleParseError(parserResult, errs));
    }
    static void RunOptions(Options opts) {
        opts.Volume = Math.Clamp(opts.Volume, 0, 100);
        opts.MinimumWait = Math.Max(opts.MinimumWait, 0);
        new SEGA.SEGA(opts).Run();
    }
    static void HandleParseError(ParserResult<Options> parserResult, IEnumerable<Error> errs) {
        MessageBox.Show(HelpText.AutoBuild(parserResult, h => {
            return HelpText.DefaultParsingErrorsHandler(parserResult, h);
        }, e => e).ToString());
    }

    public class Options {
        [Option("volume", Default = 50, HelpText = "Changes video volume")]
        public int Volume { get; set; }

        [Option("min-wait", Default = 3, HelpText = "Minimum seconds to wait")]
        public int MinimumWait { get; set; }

        [Option("windowed", Default = false, HelpText = "Run windowed")]
        public bool Windowed { get; set; }

        [Option("skip-on-window", Default = false, HelpText = "If the wait-for window is found, skip the min-wait timer")]
        public bool SkipOnWindow { get; set; }

        [Option("wait-for", Default = null, HelpText = "Show video until a window with this title is detected")]
        public string WaitForWindow { get; set; }
    }
}