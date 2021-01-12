using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using task_scheduler_data_access_standard;
using System;

namespace task_scheduler_data_access_tests {
    [TestClass]
    public class RepositoryTests {
        static string connStr;

        [ClassInitialize]
        public static void Initialize(){
            
            //create file name for test db
            string dir = Directory.GetCurrentDirectory();
            string dbFileName = dir + "\\testDb.db";

            //create test db file
            File.Create(dbFileName);

            connStr = $"Data Source={dbFileName}";
        }

        [TestMethod]
        public void TestMethod1() {

        }
    }
}
