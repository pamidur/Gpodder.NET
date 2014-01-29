Gpodder.NET
===========

Async Gpodder library for .NET

USAGE:

	var storage = new FileStream("gpodder.data", FileMode.OpenOrCreate, FileAccess.ReadWrite); //file to store some cache and settings
	using (var client = await GpodderClient.Init(storage, "MyApp", "login", "password"))
	{
		var devices = await client.DevicesService.QueryDevices(); //get devices
		var top = await client.DirectoryService.QueryTopPodcasts(10); //get top 10 podcasts
		var toptags = await client.DirectoryService.QueryTopTags(10); //get top 10 tags/keywords
		var pft = await client.DirectoryService.QueryPodcastsForTag("1Arts", 5); //get podcasts by tags, here you can get podcast's ids
		var podc = await client.DirectoryService.QueryPodcastData(new Uri("http://ypp.rpod.ru/rss.xml")); //get podcsat by uri (id) 
		var epis = await client.DirectoryService.QueryEpisodeData(new Uri("http://rpod.ru/get/300774/268258/original/ypp712.mp3"), podc.Url); //get episode by uri(id)
	}