using System.Text;
using Ude;

namespace CaptionsConverter.Logic
{
    public class CCLogic
    {

        public static string changeCharacters(string contents)
        {
            var replacements = ConfigurationManager.GetActiveCharacterReplacements();

            string result = replacements.Aggregate(contents, (current, pair) => current.Replace(pair.Key, pair.Value));

            return result;
        }

        public static ConversionResult SingleFileConversion(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return new ConversionResult
                    {
                        Status = ConversionStatus.NoFilesFound,
                        Message = "The specified file does not exist."
                    };
                }

                Encoding sourceEncoding = DetectEncoding(filePath);
                string contents = File.ReadAllText(filePath, sourceEncoding);
                string result = changeCharacters(contents);

                if (contents == result)
                {
                    return new ConversionResult
                    {
                        Status = ConversionStatus.SkippedAllFiles,
                        Message = "No changes detected in the file."
                    };
                }

                File.WriteAllText(filePath, result, Encoding.UTF8);

                return new ConversionResult
                {
                    Status = ConversionStatus.Success,
                    Message = $"File converted successfully: {Path.GetFileName(filePath)}"
                };
            }
            catch (Exception ex)
            {
                return new ConversionResult
                {
                    Status = ConversionStatus.Failed,
                    Message = $"Unexpected error: {ex.Message}"
                };
            }
        }

        public static ConversionResult FileReading(string folderPath, string? fileExtension = null)
        {
            if (string.IsNullOrWhiteSpace(fileExtension))
            {
                fileExtension = ConfigurationManager.Settings.DefaultFileExtension;
            }
            else
            {
                fileExtension = fileExtension.TrimStart('*');
                if (!fileExtension.StartsWith('.'))
                {
                    fileExtension = "." + fileExtension;
                }
            }

            try
            {
                var files = Directory.EnumerateFiles(folderPath, "*" + fileExtension);

                if (!files.Any())
                {
                    return new ConversionResult
                    {
                        Status = ConversionStatus.NoFilesFound,
                        Message = $"No files found with extension '{fileExtension}' in folder: {folderPath}"
                    };
                }

                int successCount = 0;

                foreach (string file in files)
                {
                    Encoding sourceEncoding = DetectEncoding(file);
                    string contents = File.ReadAllText(file, sourceEncoding);
                    string result = CCLogic.changeCharacters(contents);
                    if(contents == result)
                    {                       
                        continue;
                    }
                    File.WriteAllText(file, result, Encoding.UTF8);
                    successCount++;

                }

                if (successCount == 0)
                {
                    return new ConversionResult
                    {
                        Status = ConversionStatus.SkippedAllFiles,
                        Message = "All files were skipped (no changes detected)."
                    };
                }


                if (successCount < files.Count())
                {
                    return new ConversionResult
                    {
                        Status = ConversionStatus.PartialSuccess,
                        Message = $"Changes were made in {successCount} of {files.Count()} files. Some were skipped or failed."
                    };
                }

                return new ConversionResult
                {
                    Status = ConversionStatus.Success,
                    Message = "All files converted successfully."
                };

            }
            catch (Exception ex)
            {
                return new ConversionResult
                {
                    Status = ConversionStatus.Failed,
                    Message = $"Unexpected error: {ex.Message}"
                };
            }
        }


        static Encoding DetectEncoding(string filePath)
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using var fileStream = File.OpenRead(filePath);
            var detector = new CharsetDetector();
            detector.Feed(fileStream);
            detector.DataEnd();

            if (detector.Charset != null)
            {
                Console.WriteLine($"Detected encoding: {detector.Charset} for file: {Path.GetFileName(filePath)}");

                try
                {
                    return Encoding.GetEncoding(detector.Charset);
                }
                catch
                {
                    Console.WriteLine($"Unsupported encoding detected, falling back to {ConfigurationManager.Settings.DefaultFallbackEncoding}.");
                }
            }

            return Encoding.GetEncoding(ConfigurationManager.Settings.DefaultFallbackEncoding);
        }



    }
}
