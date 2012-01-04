/*
 
 * Author : Ash Tewari (http://www.tewari.info)
 * Date : January 3rd 2012
 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fetch.Puzzle;


namespace Fetch.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            int s1, s2, f;

            if (args.Length == 3)
            {
                s1 = Convert.ToInt32(args[0]);
                s2 = Convert.ToInt32(args[1]);
                f = Convert.ToInt32(args[2]);
            }
            else
            {
                System.Console.Write("bucket 1 size : ");
                string input = System.Console.ReadLine();
                s1 = Convert.ToInt32(input);

                System.Console.Write("bucket 2 size : ");
                input = System.Console.ReadLine();
                s2 = Convert.ToInt32(input);

                System.Console.Write("fetch : ");
                input = System.Console.ReadLine();
                f = Convert.ToInt32(input);
            }

            Solver solver = new Solver(f);
            Bucket b1 = new Bucket(s1);
            Bucket b2 = new Bucket(s2);

            try
            {
                CommandBase solution = solver.Solve(b1, b2);

                System.Console.WriteLine(string.Format("\n\nsteps : \n"));
                foreach (string step in solution.Steps)
                {
                    System.Console.WriteLine(step);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }            

            System.Console.WriteLine(string.Format("\n\npress any key to exit."));
            System.Console.ReadKey();
        }
    }
}
