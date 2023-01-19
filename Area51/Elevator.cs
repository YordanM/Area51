using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area51
{
    internal class Elevator
    {
        const int capacity = 1;
        Semaphore semaphore;
        List<Agent> agents;

        public ManualResetEvent GoHomeSignal { get; private set; }

        public Elevator()
        {
            semaphore = new Semaphore(capacity, capacity);
            agents = new List<Agent>();
            GoHomeSignal = new ManualResetEvent(false);
        }

        public bool TryEnter(Agent agent)
        {
            if (semaphore.WaitOne())
            {
                lock (agents) agents.Add(agent);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryExit(Agent agent, string floor)
        {
            if (agent.securityLevel.ToString() == "Confidential" && floor == "G")
            {
                lock (agents) agents.Remove(agent);
                semaphore.Release();
                return true;
            }
            else if (agent.securityLevel.ToString() == "Secret" && (floor == "G" || floor == "S"))
            {
                lock (agents) agents.Remove(agent);
                semaphore.Release();
                return true;
            }
            else if (agent.securityLevel.ToString() == "TopSecret")
            {
                lock (agents) agents.Remove(agent);
                semaphore.Release();
                return true;
            }
            else 
                return false;
        }

        public void GoHome(Agent agent)
        {
            lock (agents) agents.Remove(agent);
            semaphore.Release();
        }

        public void Work()
        {
            Console.WriteLine("Base is open.");
            Thread.Sleep(10000);
            Console.WriteLine("Base is closing.");
            GoHomeSignal.Set();
        }
    }
}
