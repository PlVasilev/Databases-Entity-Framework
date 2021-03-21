// Use this file for your unit tests.
// When you are ready to submit, REMOVE all using statements to your project (entities/controllers/etc)

using System;
using System.Collections;
using FestivalManager.Core.Controllers;
using FestivalManager.Entities;
using FestivalManager.Entities.Instruments;
using FestivalManager.Entities.Sets;

namespace FestivalManager.Tests
{
	using NUnit.Framework;

	[TestFixture]
	public class SetControllerTests
    {
		[Test]
	    public void Test()
	    {
            Stage stage = new Stage();            
            Set set = new Medium("Set1");
            Performer performer = new Performer("Gosho", 24);
            Drums drums = new Drums();
            performer.AddInstrument(drums);
	        Performer performer2 = new Performer("Pesho", 19);
	        performer2.AddInstrument(new Guitar());
            set.AddPerformer(performer);
            set.AddPerformer(performer2);
            Song song = new Song("Song1", new TimeSpan(0,1,2));
            set.AddSong(song);
	        Set set2 = new Medium("Long");
            stage.AddSet(set);
            stage.AddSet(set2);
	        SetController setController = new SetController(stage);

	        var  befforValue =  drums.Wear;
           
	        string actual = setController.PerformSets();

	        var aftervalue = drums.Wear;

	        string expected = "1. Set1:\r\n-- 1. Song1 (01:02)\r\n-- Set Successful\r\n2. Long:\r\n-- Did not perform";

            Assert.AreEqual(actual,expected);
            Assert.AreNotEqual(befforValue,aftervalue);

	    }
	}
}