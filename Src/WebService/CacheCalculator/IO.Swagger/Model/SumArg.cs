/* 
 * Calculator API (plain)
 *
 * The (plain) Calculator calculates.
 *
 * OpenAPI spec version: v2
 * 
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
    /// Jadajada.
    /// </summary>
    [DataContract]
    public partial class SumArg :  IEquatable<SumArg>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SumArg" /> class.
        /// </summary>
        /// <param name="Term1">Term1.</param>
        /// <param name="Term2">Term2.</param>
        public SumArg(int? Term1 = default(int?), int? Term2 = default(int?))
        {
            this.Term1 = Term1;
            this.Term2 = Term2;
        }
        
        /// <summary>
        /// Gets or Sets Term1
        /// </summary>
        [DataMember(Name="term1", EmitDefaultValue=false)]
        public int? Term1 { get; set; }

        /// <summary>
        /// Gets or Sets Term2
        /// </summary>
        [DataMember(Name="term2", EmitDefaultValue=false)]
        public int? Term2 { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class SumArg {\n");
            sb.Append("  Term1: ").Append(Term1).Append("\n");
            sb.Append("  Term2: ").Append(Term2).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            return this.Equals(obj as SumArg);
        }

        /// <summary>
        /// Returns true if SumArg instances are equal
        /// </summary>
        /// <param name="other">Instance of SumArg to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(SumArg other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Term1 == other.Term1 ||
                    this.Term1 != null &&
                    this.Term1.Equals(other.Term1)
                ) && 
                (
                    this.Term2 == other.Term2 ||
                    this.Term2 != null &&
                    this.Term2.Equals(other.Term2)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;
                // Suitable nullity checks etc, of course :)
                if (this.Term1 != null)
                    hash = hash * 59 + this.Term1.GetHashCode();
                if (this.Term2 != null)
                    hash = hash * 59 + this.Term2.GetHashCode();
                return hash;
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
