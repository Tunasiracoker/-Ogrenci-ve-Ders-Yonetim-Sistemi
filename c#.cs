using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

// Base Class
public abstract class Person
{
	public int Id { get; set; }
	public string Name { get; set; }

	public abstract void DisplayInfo();
}

// Interface
public interface IPerson
{
	void SaveToFile(string filePath);
}

// Student Class
public class Ogrenci : Person, IPerson
{
	public string Department { get; set; }

	public override void DisplayInfo()
	{
		Console.WriteLine($"Ogrenci: {Name}, Id: {Id}, Department: {Department}");
	}

	public void SaveToFile(string filePath)
	{
		var jsonData = JsonSerializer.Serialize(this);
		File.WriteAllText(filePath, jsonData);
	}

	public static Ogrenci LoadFromFile(string filePath)
	{
		var jsonData = File.ReadAllText(filePath);
		return JsonSerializer.Deserialize<Ogrenci>(jsonData);
	}
}

// Instructor Class
public class OgretimGorevlisi : Person, IPerson
{
	public string Title { get; set; }

	public override void DisplayInfo()
	{
		Console.WriteLine($"OgretimGorevlisi: {Name}, Id: {Id}, Title: {Title}");
	}

	public void SaveToFile(string filePath)
	{
		var jsonData = JsonSerializer.Serialize(this);
		File.WriteAllText(filePath, jsonData);
	}

	public static OgretimGorevlisi LoadFromFile(string filePath)
	{
		var jsonData = File.ReadAllText(filePath);
		return JsonSerializer.Deserialize<OgretimGorevlisi>(jsonData);
	}
}

// Course Class
public class Ders
{
	public string Name { get; set; }
	public int Credits { get; set; }
	public OgretimGorevlisi Instructor { get; set; }
	public List<Ogrenci> Students { get; set; } = new List<Ogrenci>();

	public void DisplayCourseInfo()
	{
		Console.WriteLine($"Ders: {Name}, Credits: {Credits}, Instructor: {Instructor.Name}");
		Console.WriteLine("Students:");
		foreach (var student in Students)
		{
			Console.WriteLine($"- {student.Name}");
		}
	}

	public void SaveToFile(string filePath)
	{
		var jsonData = JsonSerializer.Serialize(this);
		File.WriteAllText(filePath, jsonData);
	}

	public static Ders LoadFromFile(string filePath)
	{
		var jsonData = File.ReadAllText(filePath);
		return JsonSerializer.Deserialize<Ders>(jsonData);
	}
}

class Program
{
	static void Main(string[] args)
	{
		// Create instances
		var ogrenci1 = new Ogrenci { Id = 1, Name = "Ali", Department = "Bilgisayar Muhendisligi" };
		var ogretimGorevlisi = new OgretimGorevlisi { Id = 101, Name = "Dr. Ahmet", Title = "Doçent" };
		var ders = new Ders { Name = "Programlama", Credits = 3, Instructor = ogretimGorevlisi };

		// Add student to course
		ders.Students.Add(ogrenci1);

		// Save to files
		ogrenci1.SaveToFile("ogrenci1.json");
		ogretimGorevlisi.SaveToFile("ogretimGorevlisi.json");
		ders.SaveToFile("ders.json");

		// Load from files
		var loadedOgrenci = Ogrenci.LoadFromFile("ogrenci1.json");
		var loadedOgretimGorevlisi = OgretimGorevlisi.LoadFromFile("ogretimGorevlisi.json");
		var loadedDers = Ders.LoadFromFile("ders.json");

		// Display loaded data
		loadedOgrenci.DisplayInfo();
		loadedOgretimGorevlisi.DisplayInfo();
		loadedDers.DisplayCourseInfo();
	}
}
