# DataManagement
Save Data to JSON and Encryption/Decryption Data.

## About the Script
- This script supports [`UnityEngine`](https://unity3d.com/).
- This script using [`.json`] file.

## License
- This project is distributed under the [DO WHAT THE FUCK YOU WANT TO PUBLIC LICENSE](https://en.wikipedia.org/wiki/WTFPL).

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
> Using `DataManagement.SaveData<T>(dataname, data)` to **Create Custom Save Method.**
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
> Using `DataManagement.LoadData<T>(dataname, data)` to **Create Custom Load Method.** 
> Checking if File Exists/Initialized and Create Data File.
