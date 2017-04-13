using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestInterafaces : MonoBehaviour
{
    Car carA = new Car("Citroen", "C4", 2013);
    Car carB = new Car("Citroen", "C5", 2015);

    private List<Car> m_ListCars;

    /// <summary>
    /// Filter method
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public List<Car> FilterByMake(string make)
    {
        List<Car> lCarsMatched = m_ListCars.FindAll(x => x.Make.Equals(make));
        return lCarsMatched;
    }

    void Start ()
    {     

        if (carA.Equals(carB))
        {
            Debug.Log("carA == carB");
        }else
        {
            Debug.Log("carA != carB");
        }

        Debug.Log("CAR A " + carA.Print());
        Debug.Log("CAR B " + carB.Print());

        m_ListCars = new List<Car>();
        m_ListCars.Add(carA);
        m_ListCars.Add(carB);
        m_ListCars.Add(new Car("Citroen", "C5", 2012));
        m_ListCars.Add(new Car("Toyota", "Auris", 2012));

        List<Car> lCarsMatched = FilterByMake("Citroen");
        for (int i=0; i<lCarsMatched.Count; i++)
        {
            Debug.LogFormat(" Car {0} - {1} ", i, lCarsMatched[i].Print());
        }

    }
	
}
