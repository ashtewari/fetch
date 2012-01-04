/*
 
 * Author : Ash Tewari (http://www.tewari.info)
 * Date : January 3rd 2012
 
 */


using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Fetch.Puzzle;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fetch.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void test_transfer_when_source_can_be_transferred_completely()
        {
            Bucket b1 = new Bucket(7);
            Bucket b2 = new Bucket(13);

            b1.Level = 5;

            b1.Transfer(b2);

            Assert.AreEqual(0, b1.Level);
            Assert.AreEqual(5, b2.Level);           

        }

        [TestMethod]
        public void test_transfer_when_target_level_will_exceed_capacity()
        {
            Bucket b1 = new Bucket(7);
            Bucket b2 = new Bucket(13);

            b1.Level = 5;
            b2.Level = 12;
            b1.Transfer(b2);

            Assert.AreEqual(4, b1.Level);
            Assert.AreEqual(b2.Capacity, b2.Level);

        }


        [TestMethod]
        public void TestSolver_7_13_2()
        {

            int expected = 2;

            Bucket b1 = new Bucket(7);
            Bucket b2 = new Bucket(13);

            Solver solver = new Solver(expected);
            CommandBase current = solver.Solve(b1, b2);

            Assert.IsTrue(current.B1.Level == expected || current.B2.Level == expected);
            
        }

        [TestMethod]
        public void TestSolver_3_5_1()
        {

            int expected = 1;

            Bucket b1 = new Bucket(3);
            Bucket b2 = new Bucket(5);

            Solver solver = new Solver(expected);
            CommandBase current = solver.Solve(b1, b2);

            Assert.IsTrue(current.B1.Level == expected || current.B2.Level == expected);

        }

        [TestMethod]
        public void TestSolver_2_5_11()
        {

            int expected = 2;

            Bucket b1 = new Bucket(5);
            Bucket b2 = new Bucket(11);

            Solver solver = new Solver(expected);
            CommandBase current = solver.Solve(b1, b2);

            Assert.IsTrue(current.B1.Level == expected || current.B2.Level == expected);

        }


    }



}
