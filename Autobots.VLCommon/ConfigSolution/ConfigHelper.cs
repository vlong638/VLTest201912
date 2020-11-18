namespace Autobots.Infrastracture.Common.ConfigSolution
{
    public class ConfigHelper
    {
        public static VLConfig GetVLConfig(string text)
        {
            return new VLConfig(text);
        }
    }

}
