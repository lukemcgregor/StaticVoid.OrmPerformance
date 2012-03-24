using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticVoid.OrmPerformance.Harness.Models;

namespace StaticVoid.OrmPerformance.Harness.Util
{
    public static class TestEntityHelpers
    {
        public static List<TestEntity> GenerateRandomTestEntities(int sampleSize)
        {
            Random random = new Random();
            List<TestEntity> testEntities = new List<TestEntity>();
            for (int i = 0; i < sampleSize; i++)
            {
                testEntities.Add(new TestEntity
                {
                    TestDate = random.NextDateTime(new DateTime(2000, 1, 1), new DateTime(2012, 1, 1)),
                    TestInt = random.Next(),
                    TestString = random.NextString(1000)
                });
            }
            return testEntities;
        }
    }
}
