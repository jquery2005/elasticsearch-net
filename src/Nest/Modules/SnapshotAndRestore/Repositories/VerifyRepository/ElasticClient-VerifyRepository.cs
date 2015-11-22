﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;

namespace Nest
{
	public partial interface IElasticClient
	{
		/// <inheritdoc/>
		IVerifyRepositoryResponse VerifyRepository(Name repository, Func<VerifyRepositoryDescriptor, IVerifyRepositoryRequest> selector = null);

		/// <inheritdoc/>
		IVerifyRepositoryResponse VerifyRepository(IVerifyRepositoryRequest verifyRepositoryRequest);

		/// <inheritdoc/>
		Task<IVerifyRepositoryResponse> VerifyRepositoryAsync(Name repository, Func<VerifyRepositoryDescriptor, IVerifyRepositoryRequest> selector = null);

		/// <inheritdoc/>
		Task<IVerifyRepositoryResponse> VerifyRepositoryAsync(IVerifyRepositoryRequest verifyRepositoryRequest);
	}

	public partial class ElasticClient
	{
		/// <inheritdoc/>
		public IVerifyRepositoryResponse VerifyRepository(Name repository, Func<VerifyRepositoryDescriptor, IVerifyRepositoryRequest> selector = null) =>
			this.VerifyRepository(selector.InvokeOrDefault(new VerifyRepositoryDescriptor(repository)));

		/// <inheritdoc/>
		public IVerifyRepositoryResponse VerifyRepository(IVerifyRepositoryRequest verifyRepositoryRequest) => 
			this.Dispatcher.Dispatch<IVerifyRepositoryRequest, VerifyRepositoryRequestParameters, VerifyRepositoryResponse>(
				verifyRepositoryRequest,
				(p, d) => this.LowLevelDispatch.SnapshotVerifyRepositoryDispatch<VerifyRepositoryResponse>(p)
			);

		/// <inheritdoc/>
		public Task<IVerifyRepositoryResponse> VerifyRepositoryAsync(Name repository, Func<VerifyRepositoryDescriptor, IVerifyRepositoryRequest> selector = null) => 
			this.VerifyRepositoryAsync(selector.InvokeOrDefault(new VerifyRepositoryDescriptor(repository)));

		/// <inheritdoc/>
		public Task<IVerifyRepositoryResponse> VerifyRepositoryAsync(IVerifyRepositoryRequest verifyRepositoryRequest) => 
			this.Dispatcher.DispatchAsync<IVerifyRepositoryRequest, VerifyRepositoryRequestParameters, VerifyRepositoryResponse, IVerifyRepositoryResponse>(
				verifyRepositoryRequest,
				(p, d) => this.LowLevelDispatch.SnapshotVerifyRepositoryDispatchAsync<VerifyRepositoryResponse>(p)
			);
	}
}