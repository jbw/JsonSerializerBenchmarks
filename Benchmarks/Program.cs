using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using JsonSerializer;

namespace Benchmarks
{
    /// <summary>
    /// Simple benchmark runner using Stopwatch. Measuring using ElapsedTicks.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var testObj = TestObject;

            var newtonsoft = new JsonSerializerManager(new NewtonsoftSerializer());
            var jil = new JsonSerializerManager(new JilSerializer());

            // Warm up with a throw away serialization. 
            //newtonsoft.SerializeToString(testObj);
            //jil.SerializeToString(testObj);

            // 1 million
            int iterations = 1000000;

            // Experiment
            var jilResults = RunSerializer(jil, iterations, testObj);
            var newtonsoftResults = RunSerializer(newtonsoft, iterations, testObj);

            // Report results
            long jilTotal = jilResults.Sum();
            long newtonsoftTotal = newtonsoftResults.Sum();
            Debug.WriteLine("Newton: " + newtonsoftTotal);
            Debug.WriteLine("Jil: " + jilTotal);

            // Log results
            var sb = new StringBuilder();
            foreach (var jilResult in jilResults) sb.AppendFormat(string.Format("{0},", jilResult));
            File.WriteAllText("jil.txt", sb.ToString());
            sb.Clear();
            foreach (var newtonsoftResult in newtonsoftResults) sb.AppendFormat(string.Format("{0},", newtonsoftResult));
            File.WriteAllText("newtonsoft.txt", sb.ToString());

        }

        static List<long> RunSerializer(IJsonSerializer jsonSerializer, int iterations, TestObj testObj)
        {
            var timing = new List<long>();
            var sw = new Stopwatch();

            for (int i = 0; i < iterations; i++)
            {
                sw.Start();
                var serialised = jsonSerializer.SerializeToString(testObj);
                sw.Stop();
                timing.Add(sw.ElapsedTicks);
                sw.Reset();
            }

            return timing;
        }


        // Sample test object to run experiment on. 
        static TestObj TestObject
        {
            get
            {
                return new TestObj
                {
                    Name = "JsonNewton",
                    Entries = new List<TestObj>
                {
                    new TestObj
                    {
                        Name = "AA",
                        Entries = new List<TestObj>
                        {
                            new TestObj
                            {
                                Name = "AA",
                                Entries = new List<TestObj>
                                {
                                    new TestObj
                                    {
                                        Name = "AA",
                                        Entries = new List<TestObj>
                                        {

                                            new TestObj
                                            {
                                                Name = "AA",
                                                Entries = new List<TestObj>
                                                {
                                                    new TestObj
                                                    {
                                                        Name = "AA"

                                                    },
                                                            new TestObj("B"),
                                                            new TestObj("C")
                                                }
                                            },
                                            new TestObj("B"),
                                            new TestObj("C")
                                        }
                                   },
                                new TestObj("B"),
                                new TestObj("C")
                               }
                            },
                                new TestObj("B"),
                                new TestObj("C")
                            }
                                },
                                new TestObj("B"),
                                new TestObj("C")
                            }
                };


            }
        }

        /// <summary>
        /// Nested structure in the context of the experiment. 
        /// </summary>
        public class TestObj
        {
            public string Name { get; set; }
            public List<TestObj> Entries { get; set; }

            public TestObj()
            {
                Entries = new List<TestObj>();
            }

            public TestObj(string name) : this()
            {
                Name = name;
            }
        }

    }
}
