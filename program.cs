using System;
using AntsBenchmark;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace AntsBenchmark
{
    [MemoryDiagnoser]
    public class CountAntsBenchmarks
    {
        private string AntStampede;
        [Params(1000)]

        public int N;

        [GlobalSetup]
        public void Setup()
        {
            AntStampede = new string('.', N) + "ant" + new string('.', N);
        }
        
        [Benchmark]
        public void CountAntsOriginal()
        {
            OriginalImplementation.CountAnts(AntStampede);
        }

        [Benchmark]
        public void CountAntsImproved()
        {
            ImprovedImplementation.CountAnts(AntStampede);
        }

        [Benchmark]
        public void CountAntsBaseline()
        {
            BaselineImplementation.CountAnts(AntStampede);
        }
    }
}

public static class OriginalImplementation
{
    public static int CountAnts(string carnage)
    {
        int intactAnt = carnage.Split(new string[] { "ant" }, StringSplitOptions.None).Length - 1;
        int heads = carnage.Split(new char[] { 'a' }).Length - 1;
        int bodies = carnage.Split(new char[] { 'b' }).Length - 1;
        int otherParts = carnage.Length - intactAnt * 3 - heads - bodies;

        int deadAntsFromHeads = Math.Max(heads - intactAnt, 0);

        int deadAntsFromBodyParts = Math.Min(Math.Max(otherParts - deadAntsFromHeads, 0), bodies);

        return deadAntsFromHeads + deadAntsFromBodyParts;

    }
}

public static class ImprovedImplementation
{
    public static int antsCounter = 0;
    public static int CountAnts(string Ants)
    {
        bool head = false;
        bool body = false;
        bool tail = false;


        for (int i = 0; i < Ants.Length; i++)
        {
            // Confirm if the string is valid
            if (Ants[i] != 'a' && Ants[i] != 'n' && Ants[i] != 't' && Ants[i] != '.') return 0;
            // Skip all the dots
            if (Ants[i] == '.') continue;

            // If we find a living ant, we skip it
            if (Ants.Length > i + 2 && Ants[i] == 'a' && Ants[i + 1] == 'n' && Ants[i + 2] == 't')
            {
                i += 2;
                continue;
            }

            //Verify Every Part of the Ants
            switch (Ants[i])
            {
                case 'a':
                    head = VerifyPart(head);
                    break;
                case 'n':
                    body = VerifyPart(body);
                    break;
                case 't':
                    tail = VerifyPart(tail);
                    break;

            }

            //In case that is a whole body of ants, add 1 to the counter
            if (head && body && tail)
            {
                antsCounter++;

                head = false;
                body = false;
                tail = false;
            }


        }

        //In case that a part of the ant is found, add 1 to the counter
        if (head || body || tail)
        {
            antsCounter++;
        }

        return antsCounter;
    }
    private static bool VerifyPart(bool part)
    {
        if (part)
        {
            antsCounter++;
        }
        else
        {
            part = true;
        }

        return part;
    }
}

public class BaselineImplementation
{
    public static int CountAnts (string Ants)
    {
        return Ants.Split(new string[] { "ants" }, StringSplitOptions.None).Length - 1;
    }
}

class program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<CountAntsBenchmarks>();
    }
}
