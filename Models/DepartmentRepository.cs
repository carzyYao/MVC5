using System;
using System.Linq;
using System.Collections.Generic;

namespace MVC5.Models
{
	public  class DepartmentRepository : EFRepository<Department>, IDepartmentRepository
	{
		public Department GetOne(int id)
		{
			return this.All().FirstOrDefault(p => p.DepartmentID == id);
		}
	}

	public  interface IDepartmentRepository : IRepository<Department>
	{

	}
}