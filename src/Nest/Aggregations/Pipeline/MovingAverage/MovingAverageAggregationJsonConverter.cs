﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nest
{
	internal class MovingAverageAggregationJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType) => true;

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var o = JObject.Load(reader);
			var ps = o.Properties().ToDictionary(p => p.Name, p => p.Value);
			return new MovingAverageAggregation
			{
				BucketsPath = GetOrDefault<SingleBucketsPath>("buckets_path", ps),
				Format = GetOrDefault<string>("format", ps),
				GapPolicy = GetGapPolicy(ps),
				Minimize = GetOrDefault<bool?>("minimize", ps),
				Predict = GetOrDefault<int?>("predict", ps),
				Window = GetOrDefault<int?>("window", ps),
				Model = GetModel(ps)
			};
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var movingAvg = value as IMovingAverageAggregation;
			if (movingAvg == null) return;
			writer.WriteStartObject();
			writer.WritePropertyName("buckets_path");
			serializer.Serialize(writer, movingAvg.BucketsPath);
			if (movingAvg.GapPolicy != null)
			{
				writer.WritePropertyName("gap_policy");
				writer.WriteValue(movingAvg.GapPolicy.GetStringValue());
			}
			if (!movingAvg.Format.IsNullOrEmpty())
			{
				writer.WritePropertyName(movingAvg.Format);
				writer.WriteValue(movingAvg.Format);
			}
			if (movingAvg.Window != null)
			{
				writer.WritePropertyName("window");
				writer.WriteValue(movingAvg.Window);
			}
			if (movingAvg.Minimize != null)
			{
				writer.WritePropertyName("minimize");
				writer.WriteValue(movingAvg.Minimize);
			}
			if (movingAvg.Predict != null)
			{
				writer.WritePropertyName("predict");
				writer.WriteValue(movingAvg.Predict);
			}
			if (movingAvg.Model != null)
			{
				writer.WritePropertyName("model");
				writer.WriteValue(movingAvg.Model.Name);
				writer.WritePropertyName("settings");
				serializer.Serialize(writer, movingAvg.Model);
			}
			writer.WriteEndObject();
		}

		private T GetOrDefault<T>(string key, Dictionary<string, JToken> properties)
		{
			if (!properties.ContainsKey(key)) return default(T);
			return (T)Convert.ChangeType(properties[key], typeof(T));
		}

		private GapPolicy? GetGapPolicy(Dictionary<string, JToken> properties)
		{
			var value = GetOrDefault<string>("gap_policy", properties);
			if (value.IsNullOrEmpty()) return null;
			if (value == "insert_zeros") return GapPolicy.InsertZeros;
			else if (value == "skip") return GapPolicy.Skip;
			else return null;
		}

		private IMovingAverageModel GetModel(Dictionary<string, JToken> properties)
		{
			var settings = GetOrDefault<JObject>("settings", properties);
			if (settings == null) return null;

			var name = GetOrDefault<string>("model", properties);
			if (name.IsNullOrEmpty()) return null;

			if (name == "linear")
				return settings.ToObject<LinearModel>();
			else if (name == "simple")
				return settings.ToObject<SimpleModel>();
			else if (name == "ewma")
				return settings.ToObject<EwmaModel>();
			else if (name == "holt")
				return settings.ToObject<HoltLinearModel>();
			else if (name == "holt_winters")
				return settings.ToObject<HoltWintersModel>();
			return null;
		}
	}
}
