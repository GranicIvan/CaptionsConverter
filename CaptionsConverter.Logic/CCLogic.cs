using System.Text;
using Ude;

namespace CaptionsConverter.Logic
{
    public class CCLogic
    {

        public static string changeCharacters(string contents)
        {
            // TODO: Add more language specific characters if needed
            var replacements = new Dictionary<char, char>
            {
                ['è'] = 'č',
                ['ð'] = 'đ',
                ['æ'] = 'ć',
                ['È'] = 'Č',
                ['Ð'] = 'Đ',
                ['Æ'] = 'Ć'
            };

            string result = replacements.Aggregate(contents, (current, pair) => current.Replace(pair.Key, pair.Value));

            return result;
        }


        public static ConversionResult FileReading(string folderPath, string fileExtension)
        {
            if ( string.IsNullOrWhiteSpace(fileExtension))
            {
                fileExtension = "*.str"; // Default file extension
            }
            try
            {
                var files = Directory.EnumerateFiles(folderPath, "*"+fileExtension);

                if (!files.Any())
                {
                    //TODO: Find if there are files with different extensions, 
                    return new ConversionResult
                    {
                        Status = ConversionStatus.NoFilesFound,
                        Message = "No files found with the specified extension."
                    };
                }

                int successCount = 0;

                foreach (string file in files)
                {
                    //Console.WriteLine(file);                
                    Encoding sourceEncoding = DetectEncoding(file);
                    string contents = File.ReadAllText(file, sourceEncoding);
                    string result = CCLogic.changeCharacters(contents);
                    if(contents == result)
                    {                       
                        continue; // To not overwrite the file if no changes were made
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
                //throw;
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

                // Try to return the corresponding Encoding object
                try
                {
                    return Encoding.GetEncoding(detector.Charset);
                }
                catch
                {
                    //TODO: Rethrow to be handled by the caller
                    Console.WriteLine("Unsupported encoding detected, falling back to Windows-1252.");
                }
            }

            // Fallback
            return Encoding.GetEncoding("windows-1252");
        }



    }
}
