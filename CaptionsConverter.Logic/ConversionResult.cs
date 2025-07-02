using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptionsConverter.Logic
{
    public class ConversionResult
    {
        public ConversionStatus Status { get; set; }
        public string Message { get; set; } = "";
        public List<string> ProcessedFilesSuccessfully { get; set; } = new List<string>();

        public List<string> FailedFiles { get; set; } = new List<string>();
    }


    public enum ConversionStatus
    {
        Success,
        NoFilesFound,
        SkippedAllFiles,
        PartialSuccess,
        Failed
    }

}
