using System;
using System.IO;
using System.Linq;

using VatsimLibrary.VatsimClient;
using VatsimLibrary.VatsimDb;

namespace hw
{
    class Program
    {
        static void Main(string[] args)
        {

            
            // 1. Which pilot has been logged on the longest? Done - Matches SQL
            // Select Realname, TimeLogon from Pilots ORDER by TimeLogon ASC;

            // 2. Which controller has been logged on the longest? Done - Matches SQL
            // Select Realname, TimeLogon from Controllers ORDER by TimeLogon ASC;

            // 3. Which airport has the most departures? Done - Matches SQL
            // Select PlannedDepairport, Count(*) AS CountOf from Flights GROUP by PlannedDepairport ORDER by CountOf DESC;

            // 4. Which airport has the most arrivals? Done - Matches SQL
            // Select PlannedDestairport, Count(*) AS CountOf from Flights GROUP by PlannedDestairport ORDER by CountOf DESC;

            // 5. Who is flying at the highest altitude and what kind of plane are they flying? Done - Matches SQL
            // Select Positions.Cid, Positions.Realname, Positions.Altitude, Flights.PlannedAircraft from Positions Inner Join Flights on Flights.Cid=Positions.Cid ORDER by Positions.Altitude DESC;

            // 6. Who is flying the slowest (hint: they can't be on the ground) Done - Matches SQL
            // Select Realname, Groundspeed from Positions WHERE Altitude != 0 And Groundspeed != 0 ORDER by Groundspeed ASC

            // 7. Which aircraft type is being used the most? Done - Matches SQL
            // Select PlannedAircraft, Count(*) AS CountOf from Flights GROUP by PlannedAircraft ORDER by CountOf DESC;

            // 8. Who is flying the fastest? Done - Matches SQL
            // Select Realname, Groundspeed from Positions WHERE Altitude != 0 And Groundspeed != 0 ORDER by Groundspeed DESC

            // 9. How many pilots are flying North? (270 degrees to 90 degrees) Done - Matches SQL
            // Select Cid, Realname, Heading from Positions Where (Heading >= 0 and Heading < 90) or (Heading > 270 and heading <= 360) ORDER by Heading ASC

            // 10. Which pilot has the longest remarks section of their flight? Done - Matches SQL
            // Select Realname, PlannedRemarks from Flights ORDER by length(PlannedRemarks) DESC;
           
            
           //using(var db = new VatsimDbContext())
           using(var db = new VatsimDbContext())
           {
                var _pilots = db.Pilots.Select(p => p).ToList(); // Cid - Call - Full Name
                var _controllers = db.Controllers.Select(c => c).ToList(); // Cid - Call - FacType - Frequency
                var _flights = db.Flights.Select(f => f).ToList(); // Cid - Callsign - Dep - Dest
                var _positions = db.Positions.Select(l => l).ToList();  //Cid - Call - Lat - Long

                //Query 1 - Which pilot has been logged on the longest?
                var pilotLongestLogin = 
                    from p in _pilots
                    orderby _pilots.Max(p => p.TimeLogon)
                    select p.Realname;
                Console.WriteLine($"1) The player who has been logged on the longest is: {pilotLongestLogin.First()}");
                
                //Query 2 - Which controller has been logged on the longest?
                var controllerLongestLogon = 
                    from c in _controllers
                    orderby _controllers.Max(c => c.TimeLogon)
                    select c.Realname;
                Console.WriteLine($"2) The controller who has been logged on the longest is: {controllerLongestLogon.First()}");

                //Query 3 - Which airport has the most departures? 
                var mostDepartures = _flights.GroupBy(_flights => _flights.PlannedDepairport)
                                    .OrderByDescending(group => group.Count())
                                    .First()
                                    .Key;
                Console.WriteLine($"3) The airport with the most departures is: {mostDepartures}");

                //Query 4 - Which airport has the most arrivals?
                var mostArrivals = _flights.GroupBy(_flights => _flights.PlannedDestairport)
                                    .OrderByDescending(group => group.Count())
                                    .First()
                                    .Key;
                Console.WriteLine($"4) The airport with the most departures is: {mostArrivals}");

                //Query 5 - Who is flying at the highest altitude and what kind of plane are they flying?
                var highestPlane = from p in _positions
                                   join f in _flights
                                   on p.Cid equals f.Cid
                                   orderby Convert.ToInt32(p.Altitude) descending
                                   select new 
                                    {
                                        Realname = p.Realname,
                                        Altitude = Convert.ToInt32(p.Altitude),
                                        Plane = f.PlannedAircraft
                                    };
                Console.WriteLine($"5) The player with the highest altitude is: {highestPlane.First()}");
    
                //Query 6 - Who is flying the slowest (hint: they can't be on the ground)
                var slowPilot = 
                    from s in _positions
                    where Convert.ToInt32(s.Altitude) != 0 && Convert.ToInt32(s.Groundspeed) !=0
                    orderby Convert.ToInt32(s.Groundspeed) ascending
                    select s.Realname;
                Console.WriteLine($"6) The slowest pilot is: {slowPilot.First()}");
        
                //Query 7 - Which aircraft type is being used the most?
                var air = _flights.GroupBy(_flights => _flights.PlannedAircraft)
                          .OrderByDescending(group => group.Count())
                          .First()
                          .Key;
                Console.WriteLine($"7) The most used Aircradt is: {air}");
        
                //Query 8 - Who is flying the fastest?
                var fastPilot = 
                    from s in _positions
                    where Convert.ToInt32(s.Altitude) != 0 && Convert.ToInt32(s.Groundspeed) !=0
                    orderby Convert.ToInt32(s.Groundspeed) descending
                    select s.Realname;
                Console.WriteLine($"8) The fastest pilot is: {fastPilot.First()}");
        
                //Query 9 - How many pilots are flying North? (270 degrees to 90 degrees)
                var pilotsNorth = 
                    from n in _positions
                    where (Convert.ToInt32(n.Heading) > 270 && Convert.ToInt32(n.Heading) <= 360 || Convert.ToInt32(n.Heading) >= 0 && Convert.ToInt32(n.Heading) < 90)
                    select n;
                var northPilots = pilotsNorth.Count();
                Console.WriteLine($"9) There are this many players flying north: {northPilots}");
                
                //Query 10 - Which pilot has the longest remarks section of their flight?
                var remarks = 
                    from r in _flights
                    orderby r.PlannedRemarks.Length descending
                    select r.Realname;

                Console.WriteLine($"10) The player with the longest remarks is: {remarks.First()}");
                
                
            }

            
        }


    }
}
