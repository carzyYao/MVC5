using System;
using System.Linq;
using System.Collections.Generic;

namespace MVC5.Models
{
	public  class EnrollmentRepository : EFRepository<Enrollment>, IEnrollmentRepository
	{

	}

	public  interface IEnrollmentRepository : IRepository<Enrollment>
	{

	}
}