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
		
		static void Main(string[] args)
        {
			var query = (new int [] { 10, 20, 40, 5, 3, 5, 7, 2, 9 }).AsQueryable().Select(t => new { val = t })
				.Where("val >= 7").Select("new (val, new Dynamic.Program.valAndDoubled(val, (val * 2) as doubled) as cool)");
			
			var arr = query.OfType<object>().ToArray();
			
			IQueryable<valAndDoubled> q2 = (new int [] { 10, 20, 40, 5, 3, 5, 7, 2, 9 }).AsQueryable()
				.Select("new Dynamic.Program.valAndDoubled(it as val, it * 2 as doubled)") as IQueryable<valAndDoubled>;
			
			var arr2 = q2.ToArray();
			
            Console.WriteLine(query);
			Console.WriteLine(q2);
            Console.ReadLine();
        }
#endif
    }
}
