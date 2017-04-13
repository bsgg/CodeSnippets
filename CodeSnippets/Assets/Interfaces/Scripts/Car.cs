using System.Collections;
using System;

interface IEquatable<T>
{
    bool Equals(T obj);
}
interface IPrint
{
    string Print();
}

public class Car : IEquatable<Car>, IPrint
{
    private string m_Make;
    public string Make
    {
        get { return m_Make; }
        set { m_Make = value; }
    }

    private string m_Model;
    public string Model
    {
        get { return m_Model; }
        set { m_Model = value; }
    }
    private int m_Year;
    public int Year
    {
        get { return m_Year; }
        set { m_Year = value; }
    }

    public Car(string make, string model, int Year)
    {
        m_Make = make;
        m_Model = model;
        m_Year = Year;
    }

    public bool Equals(Car other)
    {
        if (Make == other.Make &&
            Model == other.Model &&
            Year == other.Year)
        {
            return true;
        }
        else
            return false;
    }

    public string Print()
    {
        return "Make: " + Make + " Model: " + Model + " Year: " + Year;
    }
}
