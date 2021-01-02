using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using task_scheduler_entities;
using task_scheduler_application;
using task_scheduler_application.UseCases;
using task_scheduler_application.Frequencies;

namespace task_scheduler {

    class Prototyping {

        private static void TaskHarness() {
            INotificationManager manager = new BasicNotificationManager();

            manager.NotificationAdded += (object s, Notification n) => { Console.WriteLine($"{n.Time} : {n.Producer.Title}"); };

            DateTime fake = DateTime.Now.Add(new TimeSpan(0, 0, 0, 10));

            ITaskItem newTask =
                new TaskItem(
                    "New task",
                    "New task",
                    new Colour(255, 255, 255),
                    DateTime.Now,
                    manager,
                    new ConstantFrequency(new TimeSpan(0, 0, 5)),
                    new RealTimeClock(),
                    fake.AddSeconds(80),
                    Guid.NewGuid()
                );

            string input = "";
            while (input != "x") {
                Console.WriteLine("Waiting . . .");
                input = Console.ReadLine();
                break;
            }
        }


        enum Directions {
            RIGHT, LEFT, UP, DOWN
        }

        //static INotificationPeriod MapPeriod(string periodStr) {
        //    switch (periodStr) {
        //        default:
        //            break;
        //    }
        //}

        static void MapHarness() {

        }


        static void Main(string[] args) {
            //TaskHarness();
            //GuidHarness();
        }
    }
}
