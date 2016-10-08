﻿using System;
using SQLite;

namespace novemob 
{
	/// <summary>
	/// Gender.
	/// </summary>
	public enum Gender
	{
		Homem,
		Mulher
	}
	/// <summary>
	/// Person.
	/// </summary>
	public class Person : BaseItem
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int Age { get; set; }
		public Gender Gender { get; set; }

		public override string ToString()
		{
			return $"{ID}, {FirstName}, {LastName}, {Age}, {Gender.ToString()}";
		}
		public Person()
		{
		}
	}
}
