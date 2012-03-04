//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq.Dynamic;
using System.Windows.Forms;
#if SQLDEMO
using NorthwindMapping;
#endif

namespace Dynamic
{
    public class Program
    {
#if SQLDEMO
        static void Main(string[] args)
        {
            // For this sample to work, you need an active database server or SqlExpress.
            // Here is a connection to the Data sample project that ships with Microsoft Visual Studio 2008.
            string dbPath = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\..\..\Data\NORTHWND.MDF"));
            string sqlServerInstance = @".\SQLEXPRESS";
            string connString = "AttachDBFileName='" + dbPath + "';Server='" + sqlServerInstance + "';user instance=true;Integrated Security=SSPI;Connection Timeout=60";
            
            // Here is an alternate connect string that you can modify for your own purposes.
            // string connString = "server=test;database=northwind;user id=test;password=test";

            Northwind db = new Northwind(connString); 
            db.Log = Console.Out;

            var query =
                db.Customers.Where("City == @0 and Orders.Count >= @1", "London", 10).
                OrderBy("CompanyName").
                Select("New(CompanyName as Name, Phone)");

            Console.WriteLine(query);
            Console.ReadLine();
        }
#else	
		public class valAndDoubled
		{
			public int val { get; set; }
			public int doubled { get; set; }
		}
		
		public class indexed 
		{
			public object this[int index] 
			{ 
				get 
				{
					if (index == 1)
						return "foo";
					else
						return 10;
				}
			}
		}
		
		class Pet
        {
        	public string Name { get; set; }
            public int Age { get; set; }
        }
		
		class Multiplier
		{
			public int Factor {get; set;}
			
			public int Multiply(int x)
			{
				return x * Factor;
			}
		}
		
		static void Main(string[] args)
        {
			var query = (new int [] { 10, 20, 40, 5, 3, 5, 7, 2, 9 }).AsQueryable().Select(t => new { val = t })
				.Where("val >= 7").Select<object>("new (val, new Dynamic.Program.valAndDoubled(val, (val * 2) as doubled) as cool)");
			
			var arr = query.ToArray();
			
		    Expression< Func<int> > ten = () => 10;
			
			var doubler = new Multiplier { Factor = 2 }; 
			Func<int, int> doubler_func = doubler.Multiply;
			
			var q2 = (new int [] { 10, 20, 40, 5, 3, 5, 7, 2, 9 }).AsQueryable()
				.Select<valAndDoubled>("new @out (it as val, @1(it * @0) as doubled)", ten, doubler_func);
			
			var arr2 = q2.ToArray();
			
			var q3 = (new indexed[] { new indexed (), new indexed () }).AsQueryable().Where("Int32([0]) == 10");
			var arr3 = q3.ToArray();
			
			List<Pet> pets =
                    new List<Pet>{ new Pet { Name="Barley", Age=8 },
                                   new Pet { Name="Boots", Age=4 },
                                   new Pet { Name="Whiskers", Age=1 },
                                   new Pet { Name="Daisy", Age=4 } };
			
			var pet_query = pets.AsQueryable().Where("Age >= 2").GroupBy<int, string>("Age", "Name");
			var pet_arr = pet_query.ToArray();
			
			var pet_query2 = pets.AsQueryable().GroupBy<int, Pet>("Age");
			var pet_arr2 = pet_query2.ToArray();
			
            Console.WriteLine(query);
			Console.WriteLine(q2);
            Console.ReadLine();
        }
#endif
    }
}
