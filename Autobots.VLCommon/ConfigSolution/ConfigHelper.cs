using VLAutobots.Infrastracture.Common.FileSolution;

namespace Autobots.Infrastracture.Common.ConfigSolution
{
    public class ConfigHelper
    {
        public static VLConfig GetVLConfig(string directory,string file)
        {
            var currentDirectory = FileHelper.GetDirectory(directory);
            var configText = FileHelper.ReadAllText(currentDirectory, file);
            return GetVLConfig(configText);
        }

        public static VLConfig GetVLConfig(string text)
        {
            return new VLConfig(text);
        }
    }

}
