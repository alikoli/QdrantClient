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
    /// Current statistics and configuration of the collection
    /// </summary>
    [DataContract]
        public partial class CollectionInfo :  IEquatable<CollectionInfo>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionInfo" /> class.
        /// </summary>
        /// <param name="status">status (required).</param>
        /// <param name="optimizerStatus">optimizerStatus (required).</param>
        /// <param name="vectorsCount">Number of vectors in collection All vectors in collection are available for querying Calculated as &#x60;points_count x vectors_per_point&#x60; Where &#x60;vectors_per_point&#x60; is a number of named vectors in schema (required).</param>
        /// <param name="indexedVectorsCount">Number of indexed vectors in the collection. Indexed vectors in large segments are faster to query, as it is stored in vector index (HNSW) (required).</param>
        /// <param name="pointsCount">Number of points (vectors + payloads) in collection Each point could be accessed by unique id (required).</param>
        /// <param name="segmentsCount">Number of segments in collection. Each segment has independent vector as payload indexes (required).</param>
        /// <param name="config">config (required).</param>
        /// <param name="payloadSchema">Types of stored payload (required).</param>
        public CollectionInfo(CollectionStatus status = default(CollectionStatus), OptimizersStatus optimizerStatus = default(OptimizersStatus), int? vectorsCount = default(int?), int? indexedVectorsCount = default(int?), int? pointsCount = default(int?), int? segmentsCount = default(int?), CollectionConfig config = default(CollectionConfig), Dictionary<string, PayloadIndexInfo> payloadSchema = default(Dictionary<string, PayloadIndexInfo>))
        {
            // to ensure "status" is required (not null)
            if (status == null)
            {
                throw new InvalidDataException("status is a required property for CollectionInfo and cannot be null");
            }
            else
            {
                this.Status = status;
            }
            // to ensure "optimizerStatus" is required (not null)
            if (optimizerStatus == null)
            {
                throw new InvalidDataException("optimizerStatus is a required property for CollectionInfo and cannot be null");
            }
            else
            {
                this.OptimizerStatus = optimizerStatus;
            }
            // to ensure "vectorsCount" is required (not null)
            if (vectorsCount == null)
            {
                throw new InvalidDataException("vectorsCount is a required property for CollectionInfo and cannot be null");
            }
            else
            {
                this.VectorsCount = vectorsCount;
            }
            // to ensure "indexedVectorsCount" is required (not null)
            if (indexedVectorsCount == null)
            {
                throw new InvalidDataException("indexedVectorsCount is a required property for CollectionInfo and cannot be null");
            }
            else
            {
                this.IndexedVectorsCount = indexedVectorsCount;
            }
            // to ensure "pointsCount" is required (not null)
            if (pointsCount == null)
            {
                throw new InvalidDataException("pointsCount is a required property for CollectionInfo and cannot be null");
            }
            else
            {
                this.PointsCount = pointsCount;
            }
            // to ensure "segmentsCount" is required (not null)
            if (segmentsCount == null)
            {
                throw new InvalidDataException("segmentsCount is a required property for CollectionInfo and cannot be null");
            }
            else
            {
                this.SegmentsCount = segmentsCount;
            }
            // to ensure "config" is required (not null)
            if (config == null)
            {
                throw new InvalidDataException("config is a required property for CollectionInfo and cannot be null");
            }
            else
            {
                this.Config = config;
            }
            // to ensure "payloadSchema" is required (not null)
            if (payloadSchema == null)
            {
                throw new InvalidDataException("payloadSchema is a required property for CollectionInfo and cannot be null");
            }
            else
            {
                this.PayloadSchema = payloadSchema;
            }
        }
        
        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [DataMember(Name="status", EmitDefaultValue=false)]
        public CollectionStatus Status { get; set; }

        /// <summary>
        /// Gets or Sets OptimizerStatus
        /// </summary>
        [DataMember(Name="optimizer_status", EmitDefaultValue=false)]
        public OptimizersStatus OptimizerStatus { get; set; }

        /// <summary>
        /// Number of vectors in collection All vectors in collection are available for querying Calculated as &#x60;points_count x vectors_per_point&#x60; Where &#x60;vectors_per_point&#x60; is a number of named vectors in schema
        /// </summary>
        /// <value>Number of vectors in collection All vectors in collection are available for querying Calculated as &#x60;points_count x vectors_per_point&#x60; Where &#x60;vectors_per_point&#x60; is a number of named vectors in schema</value>
        [DataMember(Name="vectors_count", EmitDefaultValue=false)]
        public int? VectorsCount { get; set; }

        /// <summary>
        /// Number of indexed vectors in the collection. Indexed vectors in large segments are faster to query, as it is stored in vector index (HNSW)
        /// </summary>
        /// <value>Number of indexed vectors in the collection. Indexed vectors in large segments are faster to query, as it is stored in vector index (HNSW)</value>
        [DataMember(Name="indexed_vectors_count", EmitDefaultValue=false)]
        public int? IndexedVectorsCount { get; set; }

        /// <summary>
        /// Number of points (vectors + payloads) in collection Each point could be accessed by unique id
        /// </summary>
        /// <value>Number of points (vectors + payloads) in collection Each point could be accessed by unique id</value>
        [DataMember(Name="points_count", EmitDefaultValue=false)]
        public int? PointsCount { get; set; }

        /// <summary>
        /// Number of segments in collection. Each segment has independent vector as payload indexes
        /// </summary>
        /// <value>Number of segments in collection. Each segment has independent vector as payload indexes</value>
        [DataMember(Name="segments_count", EmitDefaultValue=false)]
        public int? SegmentsCount { get; set; }

        /// <summary>
        /// Gets or Sets Config
        /// </summary>
        [DataMember(Name="config", EmitDefaultValue=false)]
        public CollectionConfig Config { get; set; }

        /// <summary>
        /// Types of stored payload
        /// </summary>
        /// <value>Types of stored payload</value>
        [DataMember(Name="payload_schema", EmitDefaultValue=false)]
        public Dictionary<string, PayloadIndexInfo> PayloadSchema { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class CollectionInfo {\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  OptimizerStatus: ").Append(OptimizerStatus).Append("\n");
            sb.Append("  VectorsCount: ").Append(VectorsCount).Append("\n");
            sb.Append("  IndexedVectorsCount: ").Append(IndexedVectorsCount).Append("\n");
            sb.Append("  PointsCount: ").Append(PointsCount).Append("\n");
            sb.Append("  SegmentsCount: ").Append(SegmentsCount).Append("\n");
            sb.Append("  Config: ").Append(Config).Append("\n");
            sb.Append("  PayloadSchema: ").Append(PayloadSchema).Append("\n");
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
            return this.Equals(input as CollectionInfo);
        }

        /// <summary>
        /// Returns true if CollectionInfo instances are equal
        /// </summary>
        /// <param name="input">Instance of CollectionInfo to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(CollectionInfo input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Status == input.Status ||
                    (this.Status != null &&
                    this.Status.Equals(input.Status))
                ) && 
                (
                    this.OptimizerStatus == input.OptimizerStatus ||
                    (this.OptimizerStatus != null &&
                    this.OptimizerStatus.Equals(input.OptimizerStatus))
                ) && 
                (
                    this.VectorsCount == input.VectorsCount ||
                    (this.VectorsCount != null &&
                    this.VectorsCount.Equals(input.VectorsCount))
                ) && 
                (
                    this.IndexedVectorsCount == input.IndexedVectorsCount ||
                    (this.IndexedVectorsCount != null &&
                    this.IndexedVectorsCount.Equals(input.IndexedVectorsCount))
                ) && 
                (
                    this.PointsCount == input.PointsCount ||
                    (this.PointsCount != null &&
                    this.PointsCount.Equals(input.PointsCount))
                ) && 
                (
                    this.SegmentsCount == input.SegmentsCount ||
                    (this.SegmentsCount != null &&
                    this.SegmentsCount.Equals(input.SegmentsCount))
                ) && 
                (
                    this.Config == input.Config ||
                    (this.Config != null &&
                    this.Config.Equals(input.Config))
                ) && 
                (
                    this.PayloadSchema == input.PayloadSchema ||
                    this.PayloadSchema != null &&
                    input.PayloadSchema != null &&
                    this.PayloadSchema.SequenceEqual(input.PayloadSchema)
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
                if (this.Status != null)
                    hashCode = hashCode * 59 + this.Status.GetHashCode();
                if (this.OptimizerStatus != null)
                    hashCode = hashCode * 59 + this.OptimizerStatus.GetHashCode();
                if (this.VectorsCount != null)
                    hashCode = hashCode * 59 + this.VectorsCount.GetHashCode();
                if (this.IndexedVectorsCount != null)
                    hashCode = hashCode * 59 + this.IndexedVectorsCount.GetHashCode();
                if (this.PointsCount != null)
                    hashCode = hashCode * 59 + this.PointsCount.GetHashCode();
                if (this.SegmentsCount != null)
                    hashCode = hashCode * 59 + this.SegmentsCount.GetHashCode();
                if (this.Config != null)
                    hashCode = hashCode * 59 + this.Config.GetHashCode();
                if (this.PayloadSchema != null)
                    hashCode = hashCode * 59 + this.PayloadSchema.GetHashCode();
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