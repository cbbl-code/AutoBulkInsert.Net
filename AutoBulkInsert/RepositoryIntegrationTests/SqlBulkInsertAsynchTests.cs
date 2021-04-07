using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Repository;
using System;
using System.Collections.Generic;


namespace RepositoryIntegrationTests
{
    [TestFixture]
    public class SqlBulkInsertAsynchTests
    {

        private string _targetDbTable = "SampleData";

        [Rollback]
        [Test]
        public void BulkInsertAsync_WhenGivenANull_ThrowsInvalidOperationException()
        {
            //arrange 
            var configuration = GetConfigurationStub();
            var sut = new SqlBulkInsert(configuration);

            //Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => sut.BulkInsertAsync<SampleData>(
                null, 
                _targetDbTable));
        }


        [Rollback]
        [Test]
        public void BulkInsertAsync_WhenGivenEmptyEnumeration_TheReturnsWithNoInsert()
        {
            //arrange 
            var configuration = GetConfigurationStub();

            var sut = new SqlBulkInsert(configuration);
            IList<SampleData> sampleDataList = new List<SampleData>();

            //Act and Assert
            Assert.DoesNotThrowAsync(() => sut.BulkInsertAsync<SampleData>(
                sampleDataList, 
                _targetDbTable));
        }

        [Rollback]
        [Test]
        public void BulkInsertAsync_WhenGivenAnEnumerationWithNewData_TheInsertsAllData()
        {
            //arrange 
            var configuration = GetConfigurationStub();
            var sut = new SqlBulkInsert(configuration);
          

            //Act and Assert
            Assert.DoesNotThrowAsync(() => sut.BulkInsertAsync<SampleData>(
                CreateMutipleRecords(1000000), 
                _targetDbTable));
        }

        private IConfiguration GetConfigurationStub()
        {
            return new ConfigurationBuilder()
                .AddUserSecrets<SampleData>()
                .Build();
        }


        private IEnumerable<SampleData> CreateMutipleRecords(long recordCount)
        {
            for (long i = 1; i <= recordCount; i++ )
            {
                var gender = GetRandomisedGender();
                var isMarried = GetRandomisedBoolean();

                SampleData sampleData = new SampleData
                {
                    Id = i,
                    Identifier = Guid.NewGuid(),
                    Gender = gender,
                    FirtName = GetRandomisedName(new string[] { 
                        "Alex", 
                        "Hilary", 
                        "Terry", 
                        "Jay", 
                        "Luis" }),
                    Surname = GetRandomisedName(new string[] { 
                        "Patel", 
                        "Bello", 
                        "Suarez", 
                        "Osman", 
                        "Carrow" }),
                    DateOfBirth = GetRandomisedDate(),
                    IsMarried = isMarried,
                    MaidenName = gender == 'F' && isMarried? GetRandomisedName(new string[] { "Beech", "Wicklow", "Potter", "Marsh", "Cora" }) : null

                };
                yield return sampleData;
            }
        }

        private static bool GetRandomisedBoolean()
        {
            return  new Random().Next(0, 2) != 0;
        }

        private static DateTime GetRandomisedDate()
        {
            return DateTime.Now.AddDays(-1 * new Random().Next(18 * 365, 100 * 365));
        }

        private static char GetRandomisedGender()
        {
                char[] genders = { 'F', 'M' };
                int index = new Random().Next(genders.Length);
                return genders[index];
        }

        private static string GetRandomisedName(string[] nameList)
        {
            int index = new Random().Next(nameList.Length);
            return nameList[index];
        }
    }
}
