using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EWK_Server.TeamGehem.Test
{
    public class Entity
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Timestamp { get; set; }
    }

    public class MongoDBTest
    {
        public MongoDBTest() { }
        public void TestTutoril()
        {
            var connectionString = "mongodb://[test:1234]@localhost/test";
            var client = new MongoClient( connectionString );
            var server = client.GetServer();
            var database = server.GetDatabase( "test" );
            var collection = database.GetCollection<Entity>( "entities" );

            var entity = new Entity { Name = "Tom", Age= 25};
            collection.Insert( entity );
            //var id = entity.Id;

            //var query = Query<Entity>.EQ( e => e.Id, id );
            //entity = collection.FindOne( query );

            //entity.Name = "Dick";

            System.DateTime dateTime = new System.DateTime( 1970, 1, 1, 0, 0, 0, 0 );

            // Add the number of seconds in UNIX timestamp to be converted.
            dateTime = dateTime.AddSeconds( entity.Id.Timestamp );

            // The dateTime now contains the right date/time so to format the string,
            // use the standard formatting methods of the DateTime object.
            string printDate = dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();

            // Print the date and time
            System.Console.WriteLine( printDate );
            entity.Timestamp = printDate;


            collection.Save( entity );

            //var update = Update<Entity>.Set( e => e.Name, "Harry" );
            //collection.Update( query, update );

            //collection.Remove( query );
        }
    }
}
