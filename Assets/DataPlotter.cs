using System.Collections;
using System.Collections.Generic;
using System; 
using UnityEngine;
using System.Linq; 

public class DataPlotter : MonoBehaviour
{

    // Name of the input file, no extension
    public string inputfile;


    // List for holding data from CSV reader
    private List<Dictionary<string, object>> pointList;

    // sets up the expected column numbers so that the CSV is read properly 
    public int columnX1 = 0;
    public int columnY1 = 1;
    public int columnZ1 = 2;
    public int columnX2 = 3;
    public int columnY2 = 4;
    public int columnZ2 = 5; 

    // Full column names
    public string xName;
    public string yName;
    public string zName;

    public string x2Name;
    public string y2Name;
    public string z2Name; 

    public GameObject PointPrefab;

    public GameObject PointHolder;

    public GameObject LineHolder;

    public GameObject LineHolder2; 

    public float plotScale = 20;


    // Use this for initialization
    void Start()
    {
        StartCoroutine("filler"); 
    }

    IEnumerator filler()
    {
        // Set pointlist to results of function Reader with argument inputfile
        pointList = CSVReader.Read(inputfile);

        //Log to console
        Debug.Log(pointList);

        List<string> columnList = new List<string>(pointList[1].Keys);

        Debug.Log("There are " + columnList.Count + " columns in CSV");

        foreach (string key in columnList)
            Debug.Log("Column name is " + key);

        xName = columnList[columnX1];
        yName = columnList[columnY1];
        zName = columnList[columnZ1];

        x2Name = columnList[columnX2];
        y2Name = columnList[columnY2];
        z2Name = columnList[columnZ2];

        // Get maxes of each axis
        float xMax = FindMaxValue(xName);
        float yMax = FindMaxValue(yName);
        float zMax = FindMaxValue(zName);
        float x2Max = FindMaxValue(x2Name);
        float y2Max = FindMaxValue(y2Name);
        float z2Max = FindMaxValue(z2Name);

        // Get minimums of each axis
        float xMin = FindMinValue(xName);
        float yMin = FindMinValue(yName);
        float zMin = FindMinValue(zName);
        float x2Min = FindMinValue(x2Name);
        float y2Min = FindMinValue(y2Name);
        float z2Min = FindMinValue(z2Name);

        //draws the lines that connects each dot (positional data)   
        LineRenderer line = LineHolder.GetComponent<LineRenderer>();
        LineRenderer line2 = LineHolder2.GetComponent<LineRenderer>();

        //creates a list of points to input into the line later
        var points = new Vector3[pointList.Count];
        line.positionCount = pointList.Count;

        var points2 = new Vector3[pointList.Count];
        line2.positionCount = pointList.Count;

        //modifies the two different lines to be different colors
        line.startWidth = .05f;
        line.endWidth = .05f;
        line.useWorldSpace = true;
        line.startColor = Color.red;
        line.endColor = Color.yellow;

        line2.startWidth = .05f;
        line2.endWidth = .05f;
        line2.useWorldSpace = true;
        line2.startColor = Color.cyan;
        line2.endColor = Color.green;

        //iterates over each time slot(row) and creates two dots - one for each hand 
        for (var i = 0; i < pointList.Count; i++)
        {
            //take in the input data for each axis for a single time slot 
            float x = System.Convert.ToSingle(pointList[i][xName]);
            float y = System.Convert.ToSingle(pointList[i][yName]);
            float z = System.Convert.ToSingle(pointList[i][zName]);
            float x2 = System.Convert.ToSingle(pointList[i][x2Name]);
            float y2 = System.Convert.ToSingle(pointList[i][y2Name]);
            float z2 = System.Convert.ToSingle(pointList[i][z2Name]);

            //modifies the data points so that they're not incredibly small
            points[i] = new Vector3(x, y, z) * plotScale;
            points2[i] = new Vector3(x2, y2, z2) * plotScale;

            //instantiate each of the dot gameobjects
            GameObject dataPoint = Instantiate(PointPrefab, new Vector3(x, y, z) * plotScale, Quaternion.identity);
            GameObject dataPoint2 = Instantiate(PointPrefab, new Vector3(x2, y2, z2) * plotScale, Quaternion.identity);

            dataPoint.name = "Controller 1 - " + i;
            dataPoint2.name = "Controller 2 - " + i;

            dataPoint.transform.parent = PointHolder.transform;
            dataPoint2.transform.parent = PointHolder.transform;

            string dataPointName = pointList[i][xName] + " " + pointList[i][yName] + " " + pointList[i][zName];       
            string dataPointName2 = pointList[i][xName] + " " + pointList[i][yName] + " " + pointList[i][zName];

            dataPoint.transform.name = dataPointName;
            dataPoint2.transform.name = dataPointName2;

            //set the color of the dot gameobjects accordingly between red/yellow or cyan/green depending on 
            dataPoint.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.yellow, (float)i / pointList.Count);
            dataPoint2.GetComponent<Renderer>().material.color = Color.Lerp(Color.cyan, Color.green, (float)i / pointList.Count);

            line.SetPositions(points.Take(i + 1).ToArray());
            line2.SetPositions(points2.Take(i + 1).ToArray()); 

            //sets the time delay so that we can see it animate in real time 
            yield return new WaitForSecondsRealtime(0.0085f); 
        }

        //visualizes the line
        line.SetPositions(points);   
        line2.SetPositions(points2);
        
    }

   


    //these two following functions help normalize the graph 
    private float FindMaxValue(string columnName)
    {
        //set initial value to first value
        float maxValue = Convert.ToSingle(pointList[0][columnName]);

        //Loop through Dictionary, overwrite existing maxValue if new value is larger
        for (var i = 0; i < pointList.Count; i++)
        {
            if (maxValue < Convert.ToSingle(pointList[i][columnName]))
                maxValue = Convert.ToSingle(pointList[i][columnName]);
        }

        //Spit out the max value
        return maxValue;
    }


    private float FindMinValue(string columnName)
    {

        float minValue = Convert.ToSingle(pointList[0][columnName]);

        //Loop through Dictionary, overwrite existing minValue if new value is smaller
        for (var i = 0; i < pointList.Count; i++)
        {
            if (Convert.ToSingle(pointList[i][columnName]) < minValue)
                minValue = Convert.ToSingle(pointList[i][columnName]);
        }

        return minValue;
    }

}
