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


        public static void FileReading(string folderPath, string fileExtension)
        {
            try
            {
                foreach (string file in Directory.EnumerateFiles(folderPath, "*.srt"))
                {
                    //Console.WriteLine(file);                
                    Encoding sourceEncoding = DetectEncoding(file);
                    string contents = File.ReadAllText(file, sourceEncoding);
                    string result = CCLogic.changeCharacters(contents);
                    File.WriteAllText(file, result, Encoding.UTF8);

                }
            }
            catch (Exception)
            {               
                throw;
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
