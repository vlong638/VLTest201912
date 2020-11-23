using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Autobots.ConsulSample.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {

            //获取所有注册的服务
            using (var consul = new Consul.ConsulClient(c =>
            {
                //c.Address = new Uri($"http://{consulAddress}:{consulPort}");
            }))
            {
                //取在Consul注册的全部服务
                var services = consul.Agent.Services().Result.Response;
                //foreach (var s in services.Values)
                //{
                //    var serviceNodes = services.Values.Where(c => c.Service.ToLower() == serviceName.ToLower()).ToList();
                //    if (serviceNodes.Count() > 0)
                //    {
                //        var serviceNode = serviceNodes[DateTime.Now.Millisecond % serviceNodes.Count()];
                //        return new ServiceConfig()
                //        {
                //            Name = serviceNode.Service,
                //            Address = serviceNode.Address,
                //            Port = serviceNode.Port,
                //        };
                //    }
                //    Console.WriteLine($"ID={s.ID},Service={s.Service},Addr={s.Address},Port={s.Port}");
                //}
            }

            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
