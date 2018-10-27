
public static void SetRoomDataByLocationJSON()
{
    string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    string filePath = dirPath + "\\RoomData.json";

    using (StreamReader reader = File.OpenText(filePath))
    {
        // Read JSON file, and get a JToken from it for iterating
        JObject jObject = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
        JToken roomObject = jObject["Rooms"];

        //foreach (JToken thing in roomObject.Values())
        //{
        //    Console.WriteLine(thing["ElementIdInteger"]);
        //    Console.WriteLine(("________"));
        //}

        // Sample JSON query
        IEnumerable<JToken> jsonQueryEnumerable = from JToken thing in roomObject.Values()
                                                  where thing["RoomNumber"].ToString() == "206"
                                                  select thing;

        JToken jsonQuerySingle = jsonQueryEnumerable.First();

        Console.WriteLine(jsonQuerySingle.ToString());
    }

    Console.Read();

}
