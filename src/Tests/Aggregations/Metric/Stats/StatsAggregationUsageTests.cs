﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Tests.Framework;
using Tests.Framework.Integration;
using Tests.Framework.MockData;
using static Nest.Static;
using FluentAssertions;

namespace Tests.Aggregations.Metric.Stats
{
	public class StatsAggregationUsageTests : AggregationUsageTestBase
	{
		public StatsAggregationUsageTests(ReadOnlyCluster i, EndpointUsage usage) : base(i, usage) { }

		protected override object ExpectJson => new
		{
			aggs = new
			{
				commit_stats = new
				{
					stats = new
					{
						field = Field<Project>(p => p.NumberOfCommits)
					}
				}
			}
		};

		protected override Func<SearchDescriptor<Project>, ISearchRequest> Fluent => s => s
			.Aggregations(a => a
				.Stats("commit_stats", st => st
					.Field(p => p.NumberOfCommits)
				)
			);

		protected override SearchRequest<Project> Initializer =>
			new SearchRequest<Project>
			{
				Aggregations = new StatsAggregation("commit_stats", Field<Project>(p => p.NumberOfCommits))
			};

		protected override void ExpectResponse(ISearchResponse<Project> response)
		{
			response.IsValid.Should().BeTrue();
			var commitStats = response.Aggs.Stats("commit_stats");
			commitStats.Should().NotBeNull();
			commitStats.Average.Should().BeGreaterThan(0);
			commitStats.Max.Should().BeGreaterThan(0);
			commitStats.Min.Should().BeGreaterThan(0);
			commitStats.Count.Should().BeGreaterThan(0);
			commitStats.Sum.Should().BeGreaterThan(0);
		}
	}
}
