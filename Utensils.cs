using Newtonsoft.Json;
using System.Text;


public static class Utensils
{

    public static T? ReadJson<T>(string filePath)
    {
        try
        {
            using StreamReader reader = new StreamReader(filePath);
            string json = reader.ReadToEnd();
            T? obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"File not found: {ex.Message}");
            return default;
        }
        catch (JsonReaderException ex)
        {
            Console.WriteLine($"Invalid JSON format: {ex.Message}");
            return default;
        }
        catch (JsonSerializationException ex)
        {
            Console.WriteLine($"Deserialization failed: {ex.Message}");
            return default;
        }
    }

    public static void WriteJson<T>(string filePath, T obj)
    {
        try
        {
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            using StreamWriter writer = new StreamWriter(filePath);
            writer.Write(json);
        }
        catch (IOException ex)
        {
            Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
        }
        catch (JsonSerializationException ex)
        {
            Console.WriteLine($"Serialization failed: {ex.Message}");
        }
    }

    public static StringBuilder HashPassword(string password)
    {
        using (System.Security.Cryptography.SHA256 sha256Hash = System.Security.Cryptography.SHA256.Create()) // using SHA256 to hash
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder;
        }

    }
}
