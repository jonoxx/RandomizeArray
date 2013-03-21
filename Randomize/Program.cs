﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Randomize
{
    public class Program
    {
        private static int n = 10000;
        private static Random rand;

        /// <summary>
        /// Main entry point to randomize an array of n
        /// </summary>
        /// <param name="args">n</param>
        static void Main(String[] args)
        {
            if (args.Length > 0)
                n = Convert.ToInt32(args[0]);

            Stopwatch watch = Stopwatch.StartNew();

            int[] main = BuildInitialSequence(n);

            rand = new Random();

            if (args.Length > 1 && args[1] == "shufflepile")
                ShufflePile(ref main);
            else
                RandomizeBySort(ref main);

            watch.Stop();

            PrintOutput(main, watch.Elapsed);

            Console.Read();
        }

        /// <summary>
        /// Randomize an array by sorting a hashtable with random values (slow, more random)
        /// </summary>
        /// <param name="main"></param>
        private static void RandomizeBySort(ref int[] main)
        {
            int n = main.Length;

            var unsorted = new Dictionary<int, int>(n);

            Array.ForEach(main, (m) =>
            {
                unsorted.Add(m, rand.Next(1, n));
            });

            var sorted = unsorted
                .OrderBy(u => u.Value)
                .ToDictionary(d => d.Key, d => d.Value);

            main = sorted.Keys.ToArray();
        }
        
        /// <summary>
        /// Shuffle array values from a depleting base reference (fast, less random)
        /// </summary>
        /// <param name="main">Main array to work with</param>
        private static void ShufflePile(ref int[] main)
        {
            // copy the main values into a generic pile to pick random values from
            var pile = new List<int>(main);

            int index = 1,          // the current main array index
                pileCount = n,      // descending counter of values left in the pile
                emptyVisited = 0,   // counter for visited empty slots, flush empty slots when b > 5
                nextMin = 0,        // floating minValue calculated by c & f to enforce more randomization
                factor = 2;         // factor varying between 2 & 4 used to calculate s

            while (pileCount > 0)
            {
                nextMin = NextMinValue(pileCount, factor);

                IncrementFactor(ref factor, 2, 4);

                int k = rand.Next(nextMin, pileCount);

                if (pile[k] == 0)
                {
                    emptyVisited++;
                    if (emptyVisited > 5)
                        pile.RemoveAll(p => p == 0);
                    continue;
                }

                main[index] = pile[k];

                pile[k] = 0;

                index++;
                pileCount--;
                emptyVisited--;
            }
        }

        /// <summary>
        /// Print results to console, including calculation time
        /// </summary>
        /// <param name="main">Main array</param>
        /// <param name="took">Time taken to execute</param>
        private static void PrintOutput(int[] main, TimeSpan took)
        {
            //StringBuilder sb = new StringBuilder(n);

            String result = String.Join(",", main);

            //Array.ForEach(main, (m) =>
            //{
            //    sb.AppendLine(m.ToString());
            //});

            Console.WriteLine(result);

            Console.WriteLine("Calculation time: {0}s {1}ms", took.Seconds, took.Milliseconds);
        }

        /// <summary>
        /// Build the main array with values from 1 to n
        /// </summary>
        /// <param name="n">Total amount of values</param>
        /// <returns>Complete array</returns>
        private static int[] BuildInitialSequence(int n)
        {
            int[] main = new int[n + 1];

            for (int i = 1; i <= n; i++)
            {
                main[i] = i;
            }
            return main;
        }

        /// <summary>
        /// Cycle a factor between min and max
        /// </summary>
        /// <param name="f">Referenced factor</param>
        /// <param name="min">Minimum factor</param>
        /// <param name="max">Maximum factor</param>
        private static void IncrementFactor(ref int f, int min, int max)
        {
            f = (f == max) ? min : f + 1;
        }

        /// <summary>
        /// Calculate a minvalue based on total / factor (providing modulus = factor)
        /// </summary>
        /// <param name="count"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        private static int NextMinValue(int count, int factor)
        {
            return (count / factor) % factor == 1 ? (count / factor) : 0;            
        }

        
    }
}
