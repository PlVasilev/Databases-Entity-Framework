using System;
using System.Linq;
using System.Reflection;

namespace FestivalManager.Entities.Factories
{
	using Contracts;
	using Entities.Contracts;
	using Instruments;

	public class InstrumentFactory : IInstrumentFactory
	{
		public IInstrument CreateInstrument(string type)
		{
		    Assembly assembly = Assembly.GetCallingAssembly();

		    Type typeOf = assembly.GetTypes().FirstOrDefault(t => t.Name == type);

		    var instance = (IInstrument)Activator.CreateInstance(typeOf, true);

		    return instance;

            //if (type == "Drums")
            //{
            //	return new Drums();
            //}
            //else if (type == "Guitar")
            //{
            //	return new Guitar();
            //}
            //else if (type == "Microphone")
            //{
            //	return new Microphone();
            //}
            //else
            //{
            //    return null;
            //}
        }
	}
}