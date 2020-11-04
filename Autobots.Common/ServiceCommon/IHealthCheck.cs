namespace Autobots.Common.ServiceBase
{
    interface IHealthCheck
    {
        HealthCheckReport GetHealthCheckReport();
    }

    public class HealthCheckReport
    { 
    }
}
