# DataManagement
Save Data to JSON and Encryption/Decryption Data
## About the Script
- This script using [`LitJSON`](https://github.com/LitJSON/litjson).
- To use this script, add [`LitJSON`](https://github.com/LitJSON/litjson) on Asset folder
## How to Use
```csharp
class DataClass
{
    int properties;
};

private DataClass Data;

public void SaveData()
{
    DataManagement.SaveData<DataClass>("PlayerData", Data);
}

public bool LoadData()
{
    if(!DataManagement.LoadData<DataClass>("PlayerData", ref Data))
    {
        // Check if Initialized Before
        if()
        {
            // Initialize Data

            SaveData();

            return true;
        }
        else
        {
            return false;
        }
    }
    return true;
}
```
