using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

using VatsimLibrary.VatsimClientV1;
using VatsimLibrary.VatsimDb;

namespace api
{
    public class PilotsEndpoint
    {
        public static async Task CallsignEndpoint(HttpContext context)
        {
            string responseText = null;
            string callsign = context.Request.RouteValues["callsign"] as string;
            switch((callsign ?? "").ToLower())
            {
                case "aal1":
                    responseText = "Callsign: AAL1";
                    break;
                default:
                    responseText = "Callsign: INVALID";
                    break;
            }

            if(callsign != null)
            {
                await context.Response.WriteAsync($"{responseText} is the callsign");
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
        }


        /* NOTE: All of these require that you first obtain a pilot and then search in Positions */

        public static async Task AltitudeEndpoint(HttpContext context)
        {
            //TO DO
            //await context.Response.WriteAsync($"Altitude");
            string responseText = null;
            string callsign = context.Request.RouteValues["callsign"] as string;
            string altitude = context.Request.RouteValues["altitude"] as string;

            using(var db = new VatsimDbContext())
            {
                if(altitude != null && callsign != null)
                {
                    var _altitude = await db.Positions.Where(f => f.Altitude == (altitude ?? "") && f.Callsign == (callsign ?? "")).Select(f => f.Realname).Distinct().ToListAsync();
                        
                    if ( _altitude.Any() ){
                        Console.WriteLine($"{altitude}");

                        responseText = $"Pilot {_altitude.First()} is flying at an altitude of {altitude}";                    
                        await context.Response.WriteAsync($"RESPONSE: {responseText}");
                    }
                    else{
                        await context.Response.WriteAsync("Terminal Middleware Reached");
                    }
                }
                else
                {
                    await context.Response.WriteAsync("Terminal Middleware Reached");
                }
            }
        }

        public static async Task GroundspeedEndpoint(HttpContext context)
        {
            //TO DO
            string responseText = null;
            string callsign = context.Request.RouteValues["callsign"] as string;
            string groundspeed = context.Request.RouteValues["groundspeed"] as string;

            using(var db = new VatsimDbContext())
            {
                if(groundspeed != null && callsign != null)
                {
                    var _groundspeed = await db.Positions.Where(f => f.Groundspeed == (groundspeed ?? "") && f.Callsign == (callsign ?? "")).Select(f => f.Realname).Distinct().ToListAsync();
                        
                    if ( _groundspeed.Any() ){
                        Console.WriteLine($"{groundspeed}");

                        responseText = $"Pilot {_groundspeed.First()} is flying at an groundspeed of {groundspeed}";                    
                        await context.Response.WriteAsync($"RESPONSE: {responseText}");
                    }
                    else{
                        await context.Response.WriteAsync("Terminal Middleware Reached");
                    }
                }
                else
                {
                    await context.Response.WriteAsync("Terminal Middleware Reached");
                }
            }
        }

        public static async Task LatitudeEndpoint(HttpContext context)        
        {
            //TO DO
            string responseText = null;
            string callsign = context.Request.RouteValues["callsign"] as string;
            string latitude = context.Request.RouteValues["latitude"] as string;

            using(var db = new VatsimDbContext())
            {
                if(latitude != null && callsign != null)
                {
                    var _latitude = await db.Positions.Where(f => f.Latitude == (latitude ?? "") && f.Callsign == (callsign ?? "")).Select(f => f.Realname).Distinct().ToListAsync();
                        
                    if ( _latitude.Any() ){
                        Console.WriteLine($"{latitude}");

                        responseText = $"Pilot {_latitude.First()} is flying at an latitude of {latitude}";                    
                        await context.Response.WriteAsync($"RESPONSE: {responseText}");
                    }
                    else{
                        await context.Response.WriteAsync("Terminal Middleware Reached");
                    }
                }
                else
                {
                    await context.Response.WriteAsync("Terminal Middleware Reached");
                }
            }
        }

        public static async Task LongitudeEndpoint(HttpContext context)
        {
            //TO DO
            string responseText = null;
            string callsign = context.Request.RouteValues["callsign"] as string;
            string longitude = context.Request.RouteValues["longitude"] as string;

            using(var db = new VatsimDbContext())
            {
                if(longitude != null && callsign != null)
                {
                    var _longitude = await db.Positions.Where(f => f.Longitude == (longitude ?? "") && f.Callsign == (callsign ?? "")).Select(f => f.Realname).Distinct().ToListAsync();
                        
                    if ( _longitude.Any() ){
                        Console.WriteLine($"{longitude}");

                        responseText = $"Pilot {_longitude.First()} is flying at an longitude of {longitude}";                    
                        await context.Response.WriteAsync($"RESPONSE: {responseText}");
                    }
                    else{
                        await context.Response.WriteAsync("Terminal Middleware Reached");
                    }
                }
                else
                {
                    await context.Response.WriteAsync("Terminal Middleware Reached");
                }
            }
        }
    }
}