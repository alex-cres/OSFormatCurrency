using System;
using System.Data;
using System.Collections;
using System.Runtime.Serialization;
using System.Reflection;
using System.Xml;
using OutSystems.ObjectKeys;
using OutSystems.RuntimeCommon;
using OutSystems.HubEdition.RuntimePlatform;
using OutSystems.HubEdition.RuntimePlatform.Db;
using OutSystems.Internal.Db;
using OutSystems.HubEdition.RuntimePlatform.NewRuntime;

namespace OutSystems.NssFormatCurrency {

	/// <summary>
	/// RecordList type <code>RLLocaleRecordList</code> that represents a record list of
	///  <code>Locale</code>
	/// </summary>
	[Serializable()]
	public partial class RLLocaleRecordList: GenericRecordList<RCLocaleRecord>, IEnumerable, IEnumerator, ISerializable {
		public static void EnsureInitialized() {}

		protected override RCLocaleRecord GetElementDefaultValue() {
			return new RCLocaleRecord("");
		}

		public T[] ToArray<T>(Func<RCLocaleRecord, T> converter) {
			return ToArray(this, converter);
		}

		public static T[] ToArray<T>(RLLocaleRecordList recordlist, Func<RCLocaleRecord, T> converter) {
			return InnerToArray(recordlist, converter);
		}
		public static implicit operator RLLocaleRecordList(RCLocaleRecord[] array) {
			RLLocaleRecordList result = new RLLocaleRecordList();
			result.InnerFromArray(array);
			return result;
		}

		public static RLLocaleRecordList ToList<T>(T[] array, Func <T, RCLocaleRecord> converter) {
			RLLocaleRecordList result = new RLLocaleRecordList();
			result.InnerFromArray(array, converter);
			return result;
		}

		public static RLLocaleRecordList FromRestList<T>(RestList<T> restList, Func <T, RCLocaleRecord> converter) {
			RLLocaleRecordList result = new RLLocaleRecordList();
			result.InnerFromRestList(restList, converter);
			return result;
		}
		/// <summary>
		/// Default Constructor
		/// </summary>
		public RLLocaleRecordList(): base() {
		}

		/// <summary>
		/// Constructor with transaction parameter
		/// </summary>
		/// <param name="trans"> IDbTransaction Parameter</param>
		[Obsolete("Use the Default Constructor and set the Transaction afterwards.")]
		public RLLocaleRecordList(IDbTransaction trans): base(trans) {
		}

		/// <summary>
		/// Constructor with transaction parameter and alternate read method
		/// </summary>
		/// <param name="trans"> IDbTransaction Parameter</param>
		/// <param name="alternateReadDBMethod"> Alternate Read Method</param>
		[Obsolete("Use the Default Constructor and set the Transaction afterwards.")]
		public RLLocaleRecordList(IDbTransaction trans, ReadDBMethodDelegate alternateReadDBMethod): this(trans) {
			this.alternateReadDBMethod = alternateReadDBMethod;
		}

		/// <summary>
		/// Constructor declaration for serialization
		/// </summary>
		/// <param name="info"> SerializationInfo</param>
		/// <param name="context"> StreamingContext</param>
		public RLLocaleRecordList(SerializationInfo info, StreamingContext context): base(info, context) {
		}

		public override BitArray[] GetDefaultOptimizedValues() {
			BitArray[] def = new BitArray[1];
			def[0] = null;
			return def;
		}
		/// <summary>
		/// Create as new list
		/// </summary>
		/// <returns>The new record list</returns>
		protected override OSList<RCLocaleRecord> NewList() {
			return new RLLocaleRecordList();
		}


	} // RLLocaleRecordList
}
