/* 
 * Qdrant API
 *
 * API description for Qdrant vector search engine.  This document describes CRUD and search operations on collections of points (vectors with payload).  Qdrant supports any combinations of `should`, `must` and `must_not` conditions, which makes it possible to use in applications when object could not be described solely by vector. It could be location features, availability flags, and other custom properties businesses should take into account. ## Examples This examples cover the most basic use-cases - collection creation and basic vector search. ### Create collection First - let's create a collection with dot-production metric. ``` curl -X PUT 'http://localhost:6333/collections/test_collection' \\   -H 'Content-Type: application/json' \\   - -data-raw '{     \"vectors\": {       \"size\": 4,       \"distance\": \"Dot\"     }   }'  ``` Expected response: ``` {     \"result\": true,     \"status\": \"ok\",     \"time\": 0.031095451 } ``` We can ensure that collection was created: ``` curl 'http://localhost:6333/collections/test_collection' ``` Expected response: ``` {   \"result\": {     \"status\": \"green\",     \"vectors_count\": 0,     \"segments_count\": 5,     \"disk_data_size\": 0,     \"ram_data_size\": 0,     \"config\": {       \"params\": {         \"vectors\": {           \"size\": 4,           \"distance\": \"Dot\"         }       },       \"hnsw_config\": {         \"m\": 16,         \"ef_construct\": 100,         \"full_scan_threshold\": 10000       },       \"optimizer_config\": {         \"deleted_threshold\": 0.2,         \"vacuum_min_vector_number\": 1000,         \"max_segment_number\": 5,         \"memmap_threshold\": 50000,         \"indexing_threshold\": 20000,         \"flush_interval_sec\": 1       },       \"wal_config\": {         \"wal_capacity_mb\": 32,         \"wal_segments_ahead\": 0       }     }   },   \"status\": \"ok\",   \"time\": 2.1199e-05 } ```  ### Add points Let's now add vectors with some payload: ``` curl -L -X PUT 'http://localhost:6333/collections/test_collection/points?wait=true' \\ -H 'Content-Type: application/json' \\ - -data-raw '{   \"points\": [     {\"id\": 1, \"vector\": [0.05, 0.61, 0.76, 0.74], \"payload\": {\"city\": \"Berlin\"}},     {\"id\": 2, \"vector\": [0.19, 0.81, 0.75, 0.11], \"payload\": {\"city\": [\"Berlin\", \"London\"] }},     {\"id\": 3, \"vector\": [0.36, 0.55, 0.47, 0.94], \"payload\": {\"city\": [\"Berlin\", \"Moscow\"] }},     {\"id\": 4, \"vector\": [0.18, 0.01, 0.85, 0.80], \"payload\": {\"city\": [\"London\", \"Moscow\"] }},     {\"id\": 5, \"vector\": [0.24, 0.18, 0.22, 0.44], \"payload\": {\"count\": [0]}},     {\"id\": 6, \"vector\": [0.35, 0.08, 0.11, 0.44]}   ] }' ``` Expected response: ``` {     \"result\": {         \"operation_id\": 0,         \"status\": \"completed\"     },     \"status\": \"ok\",     \"time\": 0.000206061 } ``` ### Search with filtering Let's start with a basic request: ``` curl -L -X POST 'http://localhost:6333/collections/test_collection/points/search' \\ -H 'Content-Type: application/json' \\ - -data-raw '{     \"vector\": [0.2,0.1,0.9,0.7],     \"top\": 3 }' ``` Expected response: ``` {     \"result\": [         { \"id\": 4, \"score\": 1.362, \"payload\": null, \"version\": 0 },         { \"id\": 1, \"score\": 1.273, \"payload\": null, \"version\": 0 },         { \"id\": 3, \"score\": 1.208, \"payload\": null, \"version\": 0 }     ],     \"status\": \"ok\",     \"time\": 0.000055785 } ``` But result is different if we add a filter: ``` curl -L -X POST 'http://localhost:6333/collections/test_collection/points/search' \\ -H 'Content-Type: application/json' \\ - -data-raw '{     \"filter\": {         \"should\": [             {                 \"key\": \"city\",                 \"match\": {                     \"value\": \"London\"                 }             }         ]     },     \"vector\": [0.2, 0.1, 0.9, 0.7],     \"top\": 3 }' ``` Expected response: ``` {     \"result\": [         { \"id\": 4, \"score\": 1.362, \"payload\": null, \"version\": 0 },         { \"id\": 2, \"score\": 0.871, \"payload\": null, \"version\": 0 }     ],     \"status\": \"ok\",     \"time\": 0.000093972 } ``` 
 *
 * OpenAPI spec version: v1.1.3
 * Contact: andrey@vasnetsov.com
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using SwaggerDateConverter = IO.Swagger.Client.SwaggerDateConverter;
namespace IO.Swagger.Model
{
    /// <summary>
    /// CollectionParams
    /// </summary>
    [DataContract]
        public partial class CollectionParams :  IEquatable<CollectionParams>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionParams" /> class.
        /// </summary>
        /// <param name="vectors">vectors (required).</param>
        /// <param name="shardNumber">Number of shards the collection has (default to 1).</param>
        /// <param name="replicationFactor">Number of replicas for each shard (default to 1).</param>
        /// <param name="writeConsistencyFactor">Defines how many replicas should apply the operation for us to consider it successful. Increasing this number will make the collection more resilient to inconsistencies, but will also make it fail if not enough replicas are available. Does not have any performance impact. (default to 1).</param>
        /// <param name="onDiskPayload">If true - point&#x27;s payload will not be stored in memory. It will be read from the disk every time it is requested. This setting saves RAM by (slightly) increasing the response time. Note: those payload values that are involved in filtering and are indexed - remain in RAM. (default to false).</param>
        public CollectionParams(VectorsConfig vectors = default(VectorsConfig), int? shardNumber = 1, int? replicationFactor = 1, int? writeConsistencyFactor = 1, bool? onDiskPayload = false)
        {
            // to ensure "vectors" is required (not null)
            if (vectors == null)
            {
                throw new InvalidDataException("vectors is a required property for CollectionParams and cannot be null");
            }
            else
            {
                this.Vectors = vectors;
            }
            // use default value if no "shardNumber" provided
            if (shardNumber == null)
            {
                this.ShardNumber = 1;
            }
            else
            {
                this.ShardNumber = shardNumber;
            }
            // use default value if no "replicationFactor" provided
            if (replicationFactor == null)
            {
                this.ReplicationFactor = 1;
            }
            else
            {
                this.ReplicationFactor = replicationFactor;
            }
            // use default value if no "writeConsistencyFactor" provided
            if (writeConsistencyFactor == null)
            {
                this.WriteConsistencyFactor = 1;
            }
            else
            {
                this.WriteConsistencyFactor = writeConsistencyFactor;
            }
            // use default value if no "onDiskPayload" provided
            if (onDiskPayload == null)
            {
                this.OnDiskPayload = false;
            }
            else
            {
                this.OnDiskPayload = onDiskPayload;
            }
        }
        
        /// <summary>
        /// Gets or Sets Vectors
        /// </summary>
        [DataMember(Name="vectors", EmitDefaultValue=false)]
        public VectorsConfig Vectors { get; set; }

        /// <summary>
        /// Number of shards the collection has
        /// </summary>
        /// <value>Number of shards the collection has</value>
        [DataMember(Name="shard_number", EmitDefaultValue=false)]
        public int? ShardNumber { get; set; }

        /// <summary>
        /// Number of replicas for each shard
        /// </summary>
        /// <value>Number of replicas for each shard</value>
        [DataMember(Name="replication_factor", EmitDefaultValue=false)]
        public int? ReplicationFactor { get; set; }

        /// <summary>
        /// Defines how many replicas should apply the operation for us to consider it successful. Increasing this number will make the collection more resilient to inconsistencies, but will also make it fail if not enough replicas are available. Does not have any performance impact.
        /// </summary>
        /// <value>Defines how many replicas should apply the operation for us to consider it successful. Increasing this number will make the collection more resilient to inconsistencies, but will also make it fail if not enough replicas are available. Does not have any performance impact.</value>
        [DataMember(Name="write_consistency_factor", EmitDefaultValue=false)]
        public int? WriteConsistencyFactor { get; set; }

        /// <summary>
        /// If true - point&#x27;s payload will not be stored in memory. It will be read from the disk every time it is requested. This setting saves RAM by (slightly) increasing the response time. Note: those payload values that are involved in filtering and are indexed - remain in RAM.
        /// </summary>
        /// <value>If true - point&#x27;s payload will not be stored in memory. It will be read from the disk every time it is requested. This setting saves RAM by (slightly) increasing the response time. Note: those payload values that are involved in filtering and are indexed - remain in RAM.</value>
        [DataMember(Name="on_disk_payload", EmitDefaultValue=false)]
        public bool? OnDiskPayload { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class CollectionParams {\n");
            sb.Append("  Vectors: ").Append(Vectors).Append("\n");
            sb.Append("  ShardNumber: ").Append(ShardNumber).Append("\n");
            sb.Append("  ReplicationFactor: ").Append(ReplicationFactor).Append("\n");
            sb.Append("  WriteConsistencyFactor: ").Append(WriteConsistencyFactor).Append("\n");
            sb.Append("  OnDiskPayload: ").Append(OnDiskPayload).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as CollectionParams);
        }

        /// <summary>
        /// Returns true if CollectionParams instances are equal
        /// </summary>
        /// <param name="input">Instance of CollectionParams to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(CollectionParams input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Vectors == input.Vectors ||
                    (this.Vectors != null &&
                    this.Vectors.Equals(input.Vectors))
                ) && 
                (
                    this.ShardNumber == input.ShardNumber ||
                    (this.ShardNumber != null &&
                    this.ShardNumber.Equals(input.ShardNumber))
                ) && 
                (
                    this.ReplicationFactor == input.ReplicationFactor ||
                    (this.ReplicationFactor != null &&
                    this.ReplicationFactor.Equals(input.ReplicationFactor))
                ) && 
                (
                    this.WriteConsistencyFactor == input.WriteConsistencyFactor ||
                    (this.WriteConsistencyFactor != null &&
                    this.WriteConsistencyFactor.Equals(input.WriteConsistencyFactor))
                ) && 
                (
                    this.OnDiskPayload == input.OnDiskPayload ||
                    (this.OnDiskPayload != null &&
                    this.OnDiskPayload.Equals(input.OnDiskPayload))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.Vectors != null)
                    hashCode = hashCode * 59 + this.Vectors.GetHashCode();
                if (this.ShardNumber != null)
                    hashCode = hashCode * 59 + this.ShardNumber.GetHashCode();
                if (this.ReplicationFactor != null)
                    hashCode = hashCode * 59 + this.ReplicationFactor.GetHashCode();
                if (this.WriteConsistencyFactor != null)
                    hashCode = hashCode * 59 + this.WriteConsistencyFactor.GetHashCode();
                if (this.OnDiskPayload != null)
                    hashCode = hashCode * 59 + this.OnDiskPayload.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
