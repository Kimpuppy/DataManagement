# DataManagement
Save Data to JSON and Encryption/Decryption Data.
## About the Script
- This script supports [`UnityEngine`](https://unity3d.com/).
- This script using [`LitJSON`](https://github.com/LitJSON/litjson).
- To use this script, add [`LitJSON`](https://github.com/LitJSON/litjson) on Asset folder.
## How to Use
```csharp
class DataClass
{
    int properties;
};
```
> **Add Custom Data Class.**
<br>

```csharp
private DataClass Data;
```
> **Create Data Properties.**
<br>

```csharp
public void SaveData()
{
    DataManagement.SaveData<DataClass>("CustomData", Data);
}
```
> Using `DataManagement.SaveData<T>()` to **Create Custom Save Method.**
<br>

```csharp
public bool LoadData()
{
    if(!DataManagement.LoadData<DataClass>("CustomData", ref Data))
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
> Using `DataManagement.LoadData<T>()` to **Create Custom Load Method.** 
> Checking if File Exists/Initialized and Create Data File.
