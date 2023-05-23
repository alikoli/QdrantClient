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
    /// HnswConfigDiff
    /// </summary>
    [DataContract]
        public partial class HnswConfigDiff :  IEquatable<HnswConfigDiff>, IValidatableObject, AnyOfCreateCollectionHnswConfig, AnyOfVectorParamsHnswConfig 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HnswConfigDiff" /> class.
        /// </summary>
        /// <param name="m">Number of edges per node in the index graph. Larger the value - more accurate the search, more space required..</param>
        /// <param name="efConstruct">Number of neighbours to consider during the index building. Larger the value - more accurate the search, more time required to build the index..</param>
        /// <param name="fullScanThreshold">Minimal size (in KiloBytes) of vectors for additional payload-based indexing. If payload chunk is smaller than &#x60;full_scan_threshold_kb&#x60; additional indexing won&#x27;t be used - in this case full-scan search should be preferred by query planner and additional indexing is not required. Note: 1Kb &#x3D; 1 vector of size 256.</param>
        /// <param name="maxIndexingThreads">Number of parallel threads used for background index building. If 0 - auto selection..</param>
        /// <param name="onDisk">Store HNSW index on disk. If set to false, the index will be stored in RAM. Default: false.</param>
        /// <param name="payloadM">Custom M param for additional payload-aware HNSW links. If not set, default M will be used..</param>
        public HnswConfigDiff(int? m = default(int?), int? efConstruct = default(int?), int? fullScanThreshold = default(int?), int? maxIndexingThreads = default(int?), bool? onDisk = default(bool?), int? payloadM = default(int?))
        {
            this.M = m;
            this.EfConstruct = efConstruct;
            this.FullScanThreshold = fullScanThreshold;
            this.MaxIndexingThreads = maxIndexingThreads;
            this.OnDisk = onDisk;
            this.PayloadM = payloadM;
        }
        
        /// <summary>
        /// Number of edges per node in the index graph. Larger the value - more accurate the search, more space required.
        /// </summary>
        /// <value>Number of edges per node in the index graph. Larger the value - more accurate the search, more space required.</value>
        [DataMember(Name="m", EmitDefaultValue=false)]
        public int? M { get; set; }

        /// <summary>
        /// Number of neighbours to consider during the index building. Larger the value - more accurate the search, more time required to build the index.
        /// </summary>
        /// <value>Number of neighbours to consider during the index building. Larger the value - more accurate the search, more time required to build the index.</value>
        [DataMember(Name="ef_construct", EmitDefaultValue=false)]
        public int? EfConstruct { get; set; }

        /// <summary>
        /// Minimal size (in KiloBytes) of vectors for additional payload-based indexing. If payload chunk is smaller than &#x60;full_scan_threshold_kb&#x60; additional indexing won&#x27;t be used - in this case full-scan search should be preferred by query planner and additional indexing is not required. Note: 1Kb &#x3D; 1 vector of size 256
        /// </summary>
        /// <value>Minimal size (in KiloBytes) of vectors for additional payload-based indexing. If payload chunk is smaller than &#x60;full_scan_threshold_kb&#x60; additional indexing won&#x27;t be used - in this case full-scan search should be preferred by query planner and additional indexing is not required. Note: 1Kb &#x3D; 1 vector of size 256</value>
        [DataMember(Name="full_scan_threshold", EmitDefaultValue=false)]
        public int? FullScanThreshold { get; set; }

        /// <summary>
        /// Number of parallel threads used for background index building. If 0 - auto selection.
        /// </summary>
        /// <value>Number of parallel threads used for background index building. If 0 - auto selection.</value>
        [DataMember(Name="max_indexing_threads", EmitDefaultValue=false)]
        public int? MaxIndexingThreads { get; set; }

        /// <summary>
        /// Store HNSW index on disk. If set to false, the index will be stored in RAM. Default: false
        /// </summary>
        /// <value>Store HNSW index on disk. If set to false, the index will be stored in RAM. Default: false</value>
        [DataMember(Name="on_disk", EmitDefaultValue=false)]
        public bool? OnDisk { get; set; }

        /// <summary>
        /// Custom M param for additional payload-aware HNSW links. If not set, default M will be used.
        /// </summary>
        /// <value>Custom M param for additional payload-aware HNSW links. If not set, default M will be used.</value>
        [DataMember(Name="payload_m", EmitDefaultValue=false)]
        public int? PayloadM { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class HnswConfigDiff {\n");
            sb.Append("  M: ").Append(M).Append("\n");
            sb.Append("  EfConstruct: ").Append(EfConstruct).Append("\n");
            sb.Append("  FullScanThreshold: ").Append(FullScanThreshold).Append("\n");
            sb.Append("  MaxIndexingThreads: ").Append(MaxIndexingThreads).Append("\n");
            sb.Append("  OnDisk: ").Append(OnDisk).Append("\n");
            sb.Append("  PayloadM: ").Append(PayloadM).Append("\n");
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
            return this.Equals(input as HnswConfigDiff);
        }

        /// <summary>
        /// Returns true if HnswConfigDiff instances are equal
        /// </summary>
        /// <param name="input">Instance of HnswConfigDiff to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(HnswConfigDiff input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.M == input.M ||
                    (this.M != null &&
                    this.M.Equals(input.M))
                ) && 
                (
                    this.EfConstruct == input.EfConstruct ||
                    (this.EfConstruct != null &&
                    this.EfConstruct.Equals(input.EfConstruct))
                ) && 
                (
                    this.FullScanThreshold == input.FullScanThreshold ||
                    (this.FullScanThreshold != null &&
                    this.FullScanThreshold.Equals(input.FullScanThreshold))
                ) && 
                (
                    this.MaxIndexingThreads == input.MaxIndexingThreads ||
                    (this.MaxIndexingThreads != null &&
                    this.MaxIndexingThreads.Equals(input.MaxIndexingThreads))
                ) && 
                (
                    this.OnDisk == input.OnDisk ||
                    (this.OnDisk != null &&
                    this.OnDisk.Equals(input.OnDisk))
                ) && 
                (
                    this.PayloadM == input.PayloadM ||
                    (this.PayloadM != null &&
                    this.PayloadM.Equals(input.PayloadM))
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
                if (this.M != null)
                    hashCode = hashCode * 59 + this.M.GetHashCode();
                if (this.EfConstruct != null)
                    hashCode = hashCode * 59 + this.EfConstruct.GetHashCode();
                if (this.FullScanThreshold != null)
                    hashCode = hashCode * 59 + this.FullScanThreshold.GetHashCode();
                if (this.MaxIndexingThreads != null)
                    hashCode = hashCode * 59 + this.MaxIndexingThreads.GetHashCode();
                if (this.OnDisk != null)
                    hashCode = hashCode * 59 + this.OnDisk.GetHashCode();
                if (this.PayloadM != null)
                    hashCode = hashCode * 59 + this.PayloadM.GetHashCode();
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
