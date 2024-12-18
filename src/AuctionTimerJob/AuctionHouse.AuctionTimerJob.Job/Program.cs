// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


/*Step by step*/
//Receive rabbit with endtime

//Create database entity

//Do cron scheduled job every 1 minute (or faster)...

//Check database for endtimes to exceed current time and then make rabbitmq event back to action management service to tell auction has ended.

//Delete the entity afterwards