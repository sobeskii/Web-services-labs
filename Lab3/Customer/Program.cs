﻿using System;
using System.Threading;


namespace Customer
{
    /// <summary>
    /// Customer client
    /// </summary>
    class Program
    {
        /// <summary>
        /// Generates a random number of items that customer wants to buy.
        /// </summary>
        /// <returns>Number of items customer is buying</returns>
        static int GetNumberOfItems()
        {
            return new Random().Next(1, 11);
        }
        /// <summary>
        /// Generates a random time to delay customer from entering the queue.
        /// </summary>
        /// <returns>Amount of time customer waits before entering queue</returns>
        static int GetDelay()
        {
            return new Random().Next(1, 10);
        }
        /// <summary>
        /// Property indicating if user is in queue or not
        /// </summary>
        static bool IsInQueue { get; set; } = false;
        /// <summary>
        /// Starts the customer client
        /// </summary>
        static void Main(string[] args)
        {

            int count, delay;

            var counter = new Counter();

            CustomerRequest customerRequest = null;

            while (true)
            {
                //Customer initial values are generated. Customer walks into market
                if (!IsInQueue && customerRequest == null)
                {
                    count = GetNumberOfItems();
                    delay = GetDelay();
                    customerRequest = new CustomerRequest { itemCount = count, delay = delay };
                    Console.WriteLine($"Customer has entered the market ");
                    Console.WriteLine($"Customer is buying {count} item(s) and waiting {delay}s before entering the queue");
                    Thread.Sleep(delay * 1000);
                }
                //Customer is removed from queue. Refill is needed
                if (counter.NeedRefill() && IsInQueue && customerRequest != null)
                {
                    IsInQueue = false;
                    var leave = counter.LeaveQueue(customerRequest);
                    Console.WriteLine(leave ? "Customer has left the queue sucessfully" : "Customer couldn't leave the queue");
                    Thread.Sleep(2000); // Wait 2 secs to prevent console spam
                }
                //Customer is added to queue.
                if (!IsInQueue && customerRequest != null)
                {
                    IsInQueue = true;
                    var enter = counter.EnterQueue(customerRequest);
                    customerRequest.id = enter;

                    Console.WriteLine("Customer has entered the queue sucessfully");
                }
                // Customer is removed from queue. Has bought items
                if (counter.IsServed(customerRequest) && IsInQueue && customerRequest != null)
                {
                    IsInQueue = false;
                    var leave = counter.LeaveQueue(customerRequest);
                    Console.WriteLine(leave ? "Customer has left the queue sucessfully" : "Customer couldn't leave the queue");
                    customerRequest = null;
                }
            }
        }
    }
}

