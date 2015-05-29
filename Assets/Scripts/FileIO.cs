using UnityEngine;
using System.Collections;

// For Unity UI Inputfieldds
using UnityEngine.UI;

// For Serialization / Saving
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// For compression the file
using Ionic.Zip;

public class FileIO : MonoBehaviour
{
    // Some text fields for our input, and for reloading
    public Text[] textFields;
    public Text[] inputFields;

    void Start()
    {
        // These are the paths you can save files into, you can append strings to add company or game folders, etc.
        // Check the console to see where these go.
        //
        // (Not specifying a path and just using a filename will save to the directory from which the program was run)
        Debug.Log("persistentDataPath: " + Application.persistentDataPath); // Preferred location
        Debug.Log("dataPath: " + Application.dataPath); // Read only
        // Windows file locations
        Debug.Log("Roaming AppData Folder: " + System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData));
        Debug.Log("Local AppData Folder: " + System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData));
        Debug.Log("My Documents Folder: " + System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    // Basic loading of uncompressed file
    public void Load()
    {
        // Check if our file exists
        if (File.Exists("savefile.dat"))
        {
            // Create a new binaryformatter
            BinaryFormatter bf = new BinaryFormatter();

            // Open our file
            FileStream file = File.Open("savefile.dat", FileMode.Open);
            // Deserialize the binary file, and cast it to our playerdata
            LoadTextFields data = (LoadTextFields)bf.Deserialize(file);
            // Close our file since we're done with it
            file.Close();

            // Run through all of the input fields
            for (int i = 0; i < inputFields.Length; i++)
            {
                // Bring out inputfield data we saved and load it into our text fields
                textFields[i].text = data.inputFieldStrings[i].ToString();
            }

            Debug.Log("Savefile has been loaded.");
        }
        else
            Debug.Log("No savefile found, can't load!");
    }

    // Use DotNetZip to load and uncompress the file
    public void LoadCompressed()
    {
        // Check if our file exists
        if (File.Exists("savefile.datz"))
        {
            // Load up a new memorystream
            using (MemoryStream m = new MemoryStream())
            {
                // Open our compressed file into a ZipFile
                using (ZipFile zipFile = new ZipFile("savefile.datz"))
                {
                    // This is the file inside the zipfile
                    ZipEntry zipEntry = zipFile["savefile"];
                    // This is the password we used when we saved it
                    zipEntry.Password = "test";
                    // Extract it into our memorystream
                    zipEntry.Extract(m);
                    // Set our memory stream position back to 0
                    m.Position = 0;

                    //now serialize it back to the correct type
                    BinaryFormatter formatter = new BinaryFormatter();
                    // Deserialize the binary file, and cast it to our playerdata
                    LoadTextFields data = (LoadTextFields)formatter.Deserialize(m);

                    // Run through all of the input fields
                    for (int i = 0; i < inputFields.Length; i++)
                    {
                        // Bring out inputfield data we saved and load it into our text fields
                        textFields[i].text = data.inputFieldStrings[i].ToString();
                    }

                    Debug.Log("Compressed savefile has been loaded.");
                }
            }
        }
        else
            Debug.Log("No compressed savefile found, can't load!");
    }

    // Basic saving of uncompresse file
    public void Save()
    {
        // Create a new binaryformatter
        BinaryFormatter bf = new BinaryFormatter();

        // Create our file
        FileStream file = File.Create("savefile.dat");
        // Create our new serialed data field
        LoadTextFields data = new LoadTextFields();

        // Create our array of inputFieldStrings based on our inputFields length
        data.inputFieldStrings = new String[inputFields.Length];

        // Run through all of the input fields and save them to our serialized data field
        for (int i = 0; i < inputFields.Length; i++)
            data.inputFieldStrings[i] = inputFields[i].text;

        // Serialize all of our data into the binaryformatter
        bf.Serialize(file, data);

        // Close the file
        file.Close();

        Debug.Log("Savefile has been saved.");
    }

    // Use DotNetZip to save and compress the file
    public void SaveCompressed()
    {
        // Create our file
        FileStream file = File.Create("savefile.datz");

        // Create our new ZipFile
        using (ZipFile zipFile = new ZipFile())
        {

            //serialize item to memorystream
            using (MemoryStream m = new MemoryStream())
            {
                // Create a new binaryformatter
                BinaryFormatter bf = new BinaryFormatter();

                // Create our new serialed data field
                LoadTextFields data = new LoadTextFields();

                // Create our array of inputFieldStrings based on our inputFields length
                data.inputFieldStrings = new String[inputFields.Length];

                // Run through all of the input fields and save them to our serialized data field
                for (int i = 0; i < inputFields.Length; i++)
                    data.inputFieldStrings[i] = inputFields[i].text;

                // Serialize all of our data into the binaryformatter
                bf.Serialize(m, data);
                // Set our memory stream position back to 0
                m.Position = 0;

                // Create a new zipfile and compress our data into a file named "savefile" inside
                ZipEntry zipEntry = zipFile.AddEntry("savefile", m);
                // Use UTF8 encoding
                zipEntry.AlternateEncoding = System.Text.Encoding.UTF8;
                zipEntry.AlternateEncodingUsage = ZipOption.AsNecessary;
                // Use the best compression possible, can be set to fast, or custom levels
                zipEntry.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                // Sets the password on the zipfile
                zipEntry.Password = "test";

                // Save the zipfile
                zipFile.Save(file);
            }
        }

        // Close our file
        file.Close();

        Debug.Log("Compressed savefile has been saved.");
    }
}

// This is our serializable class to store our data.
[Serializable]
class LoadTextFields
{
    // String array for holding our stuff
    public string[] inputFieldStrings;
}