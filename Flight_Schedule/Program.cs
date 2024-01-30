// See https://aka.ms/new-console-template for more information

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Security.Principal;
using static Flights;

class Flights
{

    public static List<Flight> flightList = new List<Flight>();

    // Main Method 
    static public void Main(String[] args)
    {

        //User Story 1 -  Get Flight Schedule 
        FlightSchedules();

        //User Story 2 - Generate flight itineraries by scheduling a batch of orders.
        FlightItineraries();

    }

    /* User Story 1 -  Get Flight Schedule 
     * Read the Flight Schedule Json
     * Set the Flight list
     * */
    public static void FlightSchedules()
    {


        try
        {
            var orderpath = @"D:/JSONDATA/Flight-schedule.json";
            if (!File.Exists(orderpath)) return;

            using StreamReader reader = new StreamReader(orderpath);
            var json = reader.ReadToEnd();

            //JSON Parse
            FlightSchedule[] flightSchedules = JsonConvert.DeserializeObject<FlightSchedule[]>(json);

            Console.WriteLine(" ######### User Story 1");
            if (flightSchedules != null)
            {
                foreach (var flightSchedule in flightSchedules)
                {
                    var day = flightSchedule.day;
                    if (flightSchedule.flights != null)
                    {
                        foreach (var flight in flightSchedule.flights)
                        {
                            Console.WriteLine($"Flight:{ flight.flightNo}, departure: { flight.origin}, arrival: { flight.destination}, day: {day}");
                            flight.day = day;
                            flightList.Add(flight);
                        }
                    }
                }
            }
           

        }

        catch (Exception ex)
        {
            Console.WriteLine("Unexpected Error: "+ex.Message);
        }
        finally { }

    }
    /** User Story 2 -  Get Flight Schedule 
    * Iterate through Order data, compare with Flight destination
    * 
    **/
    public static void FlightItineraries()
    {
        try
        {
            var maxFlightOrder = 20;
            var flightNumber = "";
            var flightOrigin = "";
            var flightDest = "" ;
            var flightDay = "";
            var orderdataJson = @"D:/JSONDATA/Orders.json";
            if (!File.Exists(orderdataJson)) return;

            using StreamReader reader = new StreamReader(orderdataJson);
            var json = reader.ReadToEnd();
            //Console.WriteLine(json);
            OrderData[] orderData = JsonConvert.DeserializeObject<OrderData[]>(json);
            Dictionary<String, List<String>> flightOrderMapping = new Dictionary<String, List<String>>();
            Console.WriteLine("\n ######### User Story 2");
            foreach (var order in orderData)
            {
                Boolean isScheduled = false;
                foreach (var flight in flightList)
                {
                   
                    if (flight.destination.Equals(order.destination))
                    {
                        
                        List<String> flightOrderlist = new List<String>();
                       
                        if (flightOrderMapping.Count > 0 && flightOrderMapping.ContainsKey(flight.flightNo)) { 
                           
                            flightOrderlist = flightOrderMapping[flight.flightNo];
                        }
                        

                        if (flightOrderlist != null && flightOrderlist.Count <= maxFlightOrder)
                        {
                           
                            flightOrderlist.Add(order.orderNumber);
                            flightOrderMapping.Remove(flight.flightNo);
                            flightOrderMapping.Add(flight.flightNo, flightOrderlist);
                            flightNumber = flight.flightNo;
                            flightOrigin = flight.origin;
                            flightDest = flight.destination;
                            flightDay = flight.day;
                            isScheduled = true;
                            break;
                        }
                    }
                }

                if (!isScheduled)
                {
                    Console.WriteLine($"Örder:{order.orderNumber}, flightNumber: not scheduled");
                }
                else
                {
                    Console.WriteLine($"Örder:{order.orderNumber},FlightNumber:{ flightNumber}, departure: { flightOrigin }, arrival: { flightDest }, day:  { flightDay}");
                }
                
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected Error: " + ex.Message);
        }
        finally
        {

        }

    }



    public class FlightSchedule
    {
        [JsonProperty("day")]
        public string day { get; set; }

        [JsonProperty("flights")]
        public Flight[] flights { get; set; }
    }

    public class Flight
    {
        [JsonProperty("flightNo")]
        public string flightNo { get; set; }

        [JsonProperty("origin")]
        public string origin { get; set; }

        [JsonProperty("destination")]
        public string destination { get; set; }
        public string day { get; set; }
    }


    
    public class OrderData
    {
        [JsonProperty("orderNo")]
        public string orderNumber { get; set; }

        [JsonProperty("destination")]
        public string destination { get; set; }

    }


    public class Destination
    {
        public string destination { get; set; }
    }
}



    

