﻿using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Framework;
using Tests.Framework.MockData;
using static Tests.Framework.UrlTester;

namespace Tests.Cluster.ClusterReroute
{
	public class ClusterRerouteUrlTests : IUrlTests
	{
		[U] public async Task Urls()
		{
			await POST("/_cluster/reroute")
				.Fluent(c => c.ClusterReroute(r=>r))
				.Request(c => c.ClusterReroute(new ClusterRerouteRequest()))
				.FluentAsync(c => c.ClusterRerouteAsync(r=>r))
				.RequestAsync(c => c.ClusterRerouteAsync(new ClusterRerouteRequest()))
				;
		}
	}
}
