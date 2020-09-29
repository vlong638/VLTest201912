using Dapper;
using Dapper.Contrib.Extensions;
using FrameworkTest.Common.DALSolution;
using FrameworkTest.Common.DBSolution;
using System;
using System.Collections.Generic;

namespace FrameworkTest.Business.Research
{
    [Table(TableName)]
    public class Delivery
    {
        public const string TableName = "R_Delivery";

        public long Id { set; get; }
        public string Idcard { set; get; }
        public string WeightOnBirth { set; get; }
        public string xingbiedm { set; get; }
        public string taifangwdm { set; get; }
        public string fenmianfsdm { set; get; }
        public string apgar5 { set; get; }
        public string apgar10 { set; get; }
        public DateTime? DeliveryDate { set; get; }
        public string chuxuel { set; get; }
        public string huiyinqkdm { set; get; }
    }

    public class DeliveryRepository : RepositoryBase<Delivery>
    {
        public DeliveryRepository(DbContext context) : base(context)
        {
        }

        //public IEnumerable<LabCheck> GetAll()
        //{
        //    return context.DbGroup.Connection.Query<LabCheck>($"select * from [{TableName}] order by Id desc;", transaction: _transaction);
        //}
    }
}