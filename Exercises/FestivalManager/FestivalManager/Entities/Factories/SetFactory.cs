using System;
using System.Linq;
using System.Reflection;

namespace FestivalManager.Entities.Factories
{
	using Contracts;
	using Entities.Contracts;
	using Sets;

	public class SetFactory : ISetFactory
	{
		public ISet CreateSet(string name, string type)
		{
		   Assembly assembly =  Assembly.GetCallingAssembly();

		    Type typeOf = assembly.GetTypes().FirstOrDefault(t => t.Name == type);

           var instance = (ISet)Activator.CreateInstance(typeOf,new object[]{name});

		    return instance;
		    //if (type == "Short")
		    //{
		    //	return new Short(name);
		    //}
		    //else if (type == "Medium")
		    //{
		    //	return new Medium(name);
		    //}
		    //else if (type == "Long")
		    //{
		    //	return new Long(name);
		    //}
		    //else
		    //{
		    //    return null;
		    //}

		}
	}
}
