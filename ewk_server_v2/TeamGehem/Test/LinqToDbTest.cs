using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EWK_Server.TeamGehem.DataModels;
using NUnit.Framework;
using LinqToDB.Mapping;
using LinqToDB;
using LinqToDB.Data;
using EWK_Server.TeamGehem.Utility;

namespace EWK_Server.TeamGehem.Test
{
    [TestFixture]
    public class LinqToDbTest
    {
        [Table]
        public class TestTable
        {
            [PrimaryKey, Identity]
            public int Id;
            [Column, NotNull]
            public string Name;
            [Column, Nullable]
            public string Description;
        }
        //[Test]
        //public void CreateTest([Values(ProviderName.PostgreSQL)] string configString)
        //{
        //    using (var db = new DataConnection(configString))
        //    {
        //        try { db.DropTable<TestTable>(); }
        //        catch { }
        //        db.CreateTable<TestTable>();
        //    }
        //}

        [TestFixtureSetUp]
        public void Init()
        {
            TruncateAccount();
        }

        [TestFixtureTearDown]
        public void Cleanup()
        { }

        [Test]
        public void TruncateAccount()
        {
            using (var db = new ewkDB(ProviderName.PostgreSQL))
            {
                db.Execute("TRUNCATE account CASCADE");
                //GenericCRUD.DeleteEntity<account>(db.accounts);
            }
        }

        [Test]
        public void InsertAccount([Values(ProviderName.PostgreSQL)] string configString)
        {
            using (var db = new DataConnection(configString))
            {
                int id_count = 0;
                string template_email = "test_{0}_@test.com";
                for (int i = id_count; i < 200000; ++i)
                {
                    db.Insert<account>(new account() { email = string.Format(template_email, i), last_signed_date = DateTime.Now });
                }
                //Console.WriteLine("id = {0}", id);
            }
        }

        [Test]
        public void InsertAccountBySP([Values(ProviderName.PostgreSQL)] string configString)
        {
            int id_count = 0;
            string template_email = "test_{0}_@test.com";
            using (var db = new ewkDB(configString))
            {
                for (int i = id_count; i < 200000; ++i)
                {
                    db.ExecuteProc("insert_account", DataParameter.VarChar("email", string.Format(template_email, i)));
                }
            }
        }

        [Test]
        public void IsEqualValue([Values(ProviderName.PostgreSQL)] string configString)
        {
            using (var db = new ewkDB(configString))
            {
                var result = from a in db.accounts where a.email.Equals("test_19999_@test.com") select a;
                Console.WriteLine("result = {0}", result.ToList().Count);
            }
        }

    }
}
