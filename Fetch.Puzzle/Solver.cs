/*
 
 * Author : Ash Tewari (http://www.tewari.info)
 * Date : January 3rd 2012
 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fetch.Puzzle
{
    public class CommandBase : Node, ICommand
    {
        public Bucket B1 { get; set; }
        public Bucket B2 { get; set; }

        public CommandBase(Bucket b1, Bucket b2, CommandBase parent)
            : base(parent)
        {
            Pending = true;
            B1 = (Bucket)b1.Clone();
            B2 = (Bucket)b2.Clone();
        }

        public bool Pending { get; set; }

        #region ICommand Members

        public virtual void Execute()
        {
            Pending = false;
        }

        public virtual string GetDescription()
        {
            return string.Format("B1:{0}, B2:{1}", B1.Level, B2.Level);
        }

        #endregion

        private List<string> _steps = new List<string>();
        public List<string> Steps
        {
            get { return _steps; }
            set { _steps = value; }
        }

        #region ICommand Members


        public virtual bool CheckLevel(int level)
        {
            return B1.Level == level || B2.Level == level;
        }

        #endregion
    }

    public class TransferCommand : CommandBase
    {

        public TransferCommand(Bucket to, Bucket from, CommandBase parent)
            : base(to, from, parent)
        {
            foreach (string stp in parent.Steps)
            {
                Steps.Add(stp);
            }
        }

        public override void Execute()
        {
            B1.Transfer(B2);
            base.Execute();
            this.Steps.Add(GetDescription());
        }

        public override string GetDescription()
        {
            return string.Format(" Transfer > {0}", base.GetDescription());
        }

        public override string ToString()
        {
            return GetDescription();
        }
    }

    public class DumpCommand : CommandBase
    {
        public DumpCommand(Bucket b1, Bucket b2, CommandBase parent)
            : base(b1, b2, parent)
        {
            foreach (string stp in parent.Steps)
            {
                Steps.Add(stp);
            }
        }

        public override void Execute()
        {
            B1.Dump();
            base.Execute();
            this.Steps.Add(GetDescription());
        }

        public override string GetDescription()
        {
            return string.Format(" Dump > {0}", base.GetDescription());
        }

        public override string ToString()
        {
            return GetDescription();
        }
    }

    public class FillCommand : CommandBase
    {
        public FillCommand(Bucket b1, Bucket b2, CommandBase parent)
            : base(b1, b2, parent)
        {
            foreach (string stp in parent.Steps)
            {
                Steps.Add(stp);
            }
        }

        public override void Execute()
        {
            B1.Fill();
            base.Execute();
            this.Steps.Add(GetDescription());
        }

        public override string GetDescription()
        {
            return string.Format(" Fill > {0}", base.GetDescription());
        }

        public override string ToString()
        {
            return GetDescription();
        }
    }

    public interface ICommand
    {
        void Execute();
        bool CheckLevel(int level);
        string GetDescription();
    }

    public class Node
    {

        public Node(Node parent)
        {
            _parent = parent;

            if (_parent != null)
            {
                if (_parent.Children.Contains(this) == false)
                {
                    if (_parent.Children.Count > 6)
                    {
                        Console.WriteLine(_parent.Children.Count);
                    }
                    _parent.Children.Add(this);
                }
            }
        }

        private readonly Node _parent;
        public Node Parent
        {
            get { return _parent; }
        }

        private List<Node> _children = new List<Node>();
        public List<Node> Children
        {
            get { return _children; }
            set { _children = value; }
        }

    }

    public class BucketState
    {
        private int _level;
        public int Capacity { get; set; }

        public BucketState(int capacity, int level)
        {
            _level = level;
            Capacity = capacity;
        }

        public int Level
        {
            get { return _level; }
            set
            {
                _level = value;
            }
        }

    }

    public class Bucket : BucketState, ICloneable
    {
        public Bucket(int capacity)
            : base(capacity, 0)
        {

        }

        public Bucket(int capacity, int level)
            : base(capacity, level)
        {

        }

        public void Dump()
        {
            Level = 0;
        }

        public void Fill()
        {
            Level = Capacity;
        }

        public void Transfer(Bucket toBucket)
        {
            if (toBucket.IsFull) return;
            if (this.IsEmpty) return;

            if (toBucket.Level + this.Level <= toBucket.Capacity)
            {
                toBucket.Level += this.Level;
                this.Level = 0;
            }
            else
            {
                this.Level -= (toBucket.Capacity - toBucket.Level);
                toBucket.Fill();
            }

        }

        public bool IsFull
        {
            get { return (Level == Capacity); }
        }

        public bool IsEmpty
        {
            get { return (Level == 0); }
        }

        #region ICloneable Members

        public object Clone()
        {
            return new Bucket(this.Capacity, this.Level);
        }

        #endregion

    }

    public class Solver
    {
        private readonly int _gallonsNeeded;

        public Solver(int gallonsNeeded)
        {
            _gallonsNeeded = gallonsNeeded;
            _result = new List<string>();
        }

        private List<string> _result;
        public List<string> Result
        {
            get { return _result; }
            set { _result = value; }
        }

        public CommandBase Solve(Bucket b1, Bucket b2)
        {
            if (_gallonsNeeded == 0) throw new Exception("Fetching nothing. Give me something to do.");

            if (b1.Capacity == 0 || b2.Capacity == 0) throw new Exception("Invalid bucket capacity. Must be more than zero.");

            if (b1.Capacity > 15 || b2.Capacity > 15) throw new Exception("Bucket is too big. Use a smaller bucket. Must be less than 15 gallons");

            if (_gallonsNeeded > b1.Capacity && _gallonsNeeded > b2.Capacity) throw new Exception("Can not fetch that much in any of the buckets.");

            if (b1.Capacity == b2.Capacity)
                throw new Exception(
                    "It is not possible to create a differential because capacities of two buckets are the same.");



            // more validation here

            Queue<CommandBase> queue = new Queue<CommandBase>();

            CommandBase rootCommand = new CommandBase(b1, b2, null);

            CommandBase current = rootCommand;

            int attempts = 0;
            while (current != null)
            {
                attempts++;

                current.Execute();

                if (current.CheckLevel(_gallonsNeeded)) break;

                if (attempts > 1000000) throw new Exception("mission impossible.");

                if (!current.B1.IsFull)
                    queue.Enqueue(new FillCommand(current.B1, current.B2, current));

                if (!current.B2.IsFull)
                    queue.Enqueue(new FillCommand(current.B2, current.B1, current));

                if (!current.B1.IsEmpty)
                    queue.Enqueue(new TransferCommand(current.B1, current.B2, current));

                if (!current.B2.IsEmpty)
                    queue.Enqueue(new TransferCommand(current.B2, current.B1, current));

                if (!current.B1.IsEmpty)
                    queue.Enqueue(new DumpCommand(current.B1, current.B2, current));

                if (!current.B2.IsEmpty)
                    queue.Enqueue(new DumpCommand(current.B2, current.B1, current));

                current = queue.Dequeue();
            }

            return current;

        }

    }
}
