namespace GpodderLib.LocalServices
{
    class StaticConfigurationService : ServiceBase
    {
        /* Gpodder service related settings */
        public readonly string ClientConfigUri = "https://gpodder.net/clientconfig.json";

        /* Local client related settings */
        public readonly string ConfigurationServiceDataFilename = "configurationService.data";
    }
}
