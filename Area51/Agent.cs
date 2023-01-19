using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area51
{
    internal class Agent
    {
        public enum Place { Home, G, S, T1, T2 }

        public enum Security { Confidential, Secret, TopSecret }

        static Random random = new Random();

        private bool Throw(int chance)
        {
            int dice = random.Next(100);
            return dice < chance;
        }

        public Place currentPlace;
        public Security securityLevel;
        Elevator elevator;

        public string Id { get; set; }

        public void DoWork()
        {
            while (true)
            {
                Thread.Sleep(1000);
                //Going to base
                if (Throw(70))
                {
                    if (elevator.GoHomeSignal.WaitOne(0))
                    {
                        Console.WriteLine($"{Id} is going home.");
                        break;
                    }
                    //Try to enter in the elevator
                    elevator.TryEnter(this);
                    //Choose to which floor to go
                    while (true)
                    {
                        //Go to Ground floor
                        if (Throw(25))
                        {
                            //Agent could enter
                            if (elevator.TryExit(this, Place.G.ToString()))
                            {
                                currentPlace = Place.G;
                                Console.WriteLine($"{Id} is on the ground floor.");
                                break;
                            }
                            //Agent couldn't enter
                            else
                            {
                                Console.WriteLine($"{Id} don't have needed credentials to enter the Ground floor.");
                            }
                        }
                        //Go to Secret floor
                        else if (Throw(25))
                        {
                            //Agent could enter
                            if (elevator.TryExit(this, Place.S.ToString()))
                            {
                                currentPlace = Place.S;
                                Console.WriteLine($"{Id} is on the secret floor.");
                                break;
                            }
                            //Agent couldn't enter
                            else
                            {
                                Console.WriteLine($"{Id} don't have needed credentials to enter the T1 floor.");
                            }
                        }
                        //Go to T1 floor
                        else if (Throw(25))
                        {
                            //Agent could enter
                            if (elevator.TryExit(this, Place.T1.ToString()))
                            {
                                currentPlace = Place.T1;
                                Console.WriteLine($"{Id} is on the T1 floor.");
                                break;
                            }
                            //Agent couldn't enter
                            else
                            {
                                Console.WriteLine($"{Id} don't have needed credentials to enter the T1 floor.");
                            }
                        }
                        //Go to T2 floor
                        else if (Throw(25))
                        {
                            //Agent could enter
                            if (elevator.TryExit(this, Place.T2.ToString()))
                            {
                                currentPlace = Place.T2;
                                Console.WriteLine($"{Id} is on the T2 floor.");
                                break;
                            }
                            //Agent couldn't enter
                            else
                            {
                                Console.WriteLine($"{Id} don't have needed credentials to enter the T1 floor.");
                            }
                        }
                        Thread.Sleep(200);
                    }
                }
                //Should go home?
                else if (Throw(30))
                {
                    if (this.currentPlace != Place.Home)
                    {
                        //Try to enter in the elevator
                        elevator.TryEnter(this);
                        elevator.GoHome(this);
                        Console.WriteLine($"{Id} is going home.");
                    }
                    else
                    {
                        Console.WriteLine($"{Id} is staying home.");
                    }
                    break;
                }
            }
        }

        public Agent(Elevator elevator ,Security securityLevel)
        {
            this.securityLevel = securityLevel;
            this.elevator = elevator;
        }
    }
}
