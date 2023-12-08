using System.Xml.Serialization;

public static class XmlFileHandler
{
    public static void WriteToFile<T>(List<T> data, string fileName)
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            serializer.Serialize(writer, data);
        }
    }

    public static List<T> ReadFromFile<T>(string fileName)
    {
        using (StreamReader reader = new StreamReader(fileName))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            List<T> contents = (List<T>)serializer.Deserialize(reader);
            return contents;
        }
    }
}
