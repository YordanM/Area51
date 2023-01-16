using Area51;

class Program
{
    static void Main()
    {
        Array values = Enum.GetValues(typeof(Agent.Security));
        Random random = new Random();
        Agent.Security randomSecurity = (Agent.Security)values.GetValue(random.Next(values.Length));

        Elevator elevator = new Elevator();

        var agentThreads = new List<Thread>();

        for (int i = 0; i < 1; i++)
        {
            Agent agent = new Agent(elevator, randomSecurity)
            {
                Id = i.ToString()
            };
            var t = new Thread(agent.DoWork);
            t.Start();
            agentThreads.Add(t);
        }

        Thread elevatorThread = new Thread(elevator.Work);
        elevatorThread.Start();

        foreach (var t in agentThreads)
        {
            t.Join();
        }
        Console.WriteLine("Work is done.");
    }
}