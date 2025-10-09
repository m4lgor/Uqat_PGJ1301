using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

public class RaySensor_Example : MonoBehaviour, ISensor 
{
    public ISensor.SensorOutput Sense(in ISensor.SensorInput sensorInput)
    {
        // Write your code here
        // Proceed with any type of query, and populate the SensorOutput object
        ISensor.SensorOutput output = new ISensor.SensorOutput();
        return output;
    }

    void OnDrawGizmos()
    {
        // Draw your gizmos here
        // You can use the data from the last Sense call to visualize the results
        // Draw the query and any hit points
    }
}
